using AsmC6_API.Data;
using AsmC6_API.Models;
using AsmC6_API.Services;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);

// C?u hình CORS cho phép client t? Blazor g?i yêu c?u
builder.Services.AddCors(options =>
{
	options.AddPolicy("AllowBlazorClient",
		policy => policy
			.WithOrigins("https://localhost:7107") // URL c?a Blazor client
			.AllowAnyHeader()
			.AllowAnyMethod()
			.AllowCredentials()
	);
});

// Thêm d?ch v? các controller
builder.Services.AddControllers();

// C?u hình Swagger/OpenAPI cho vi?c ki?m tra API
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// C?u hình k?t n?i v?i c? s? d? li?u (SQL Server)
builder.Services.AddDbContext<ApplicationDbContext>(options =>
	options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// C?u hình b?o m?t v?i JWT Authentication
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
	.AddJwtBearer(options =>
	{
		options.TokenValidationParameters = new TokenValidationParameters
		{
			ValidateIssuer = true,
			ValidateAudience = true,
			ValidateLifetime = true,
			ValidateIssuerSigningKey = true,
			ValidIssuer = builder.Configuration["Jwt:Issuer"],
			ValidAudience = builder.Configuration["Jwt:Audience"],
			IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]))
		};
	});


// C?u hình cache và session
builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession(options =>
{
	options.IdleTimeout = TimeSpan.FromHours(1); // Th?i gian h?t h?n c?a session
	options.Cookie.HttpOnly = true;
	options.Cookie.IsEssential = true;
});

builder.Services.AddHttpContextAccessor();

// C?u hình các d?ch v? c?a ?ng d?ng
builder.Services.AddAuthorization();
builder.Services.AddScoped<TokenService>();
builder.Services.AddAutoMapper(typeof(Program));
builder.Services.AddScoped<ICartService, CartService>();

// C?u hình JSON Serializer ?? tránh l?i khi x? lý các chu k? tham chi?u
builder.Services.AddControllers()
	.AddJsonOptions(options =>
	{
		options.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles;
		options.JsonSerializerOptions.DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull;
	});

// C?u hình các d?ch v? VNPay
builder.Services.Configure<VnPayConfig>(builder.Configuration.GetSection("VNPay"));


var app = builder.Build();

// S? d?ng CORS cho phép truy c?p t? client Blazor
app.UseCors("AllowBlazorClient");

// C?u hình Swagger ch? xu?t hi?n trong môi tr??ng phát tri?n
if (app.Environment.IsDevelopment())
{
	app.UseSwagger();
	app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// C?u hình middleware ?? xác th?c và ?y quy?n ng??i dùng
app.UseAuthentication();
app.UseAuthorization();

// C?u hình Session
app.UseSession();

// Map các controller
app.MapControllers();

// Ch?y ?ng d?ng
app.Run();
