using AutoMapper;
using AsmC6_API.Models;
using AsmC6_API.DTOs.NewFolder;
using AsmC6_API.DTOs.category;
using AsmC6_API.DTOs.order;
using AsmC6_API.DTOs.user;
using AsmC6_API.DTOs.book;

namespace AsmC6_API.DTOs
{
	public class MappingProfile : Profile
	{
		public MappingProfile()
		{
			CreateMap<BookModel, BookDto>().ReverseMap();
			CreateMap<BookCreateDto, BookModel>();
			CreateMap<BookUpdateDto, BookModel>();
			CreateMap<BookModel, BookGetUpdateDto>();
			CreateMap<BookModel, BookDto>()
	.ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));

			CreateMap<CategoryModel, CategoryDto>().ReverseMap();

			CreateMap<OrderModel, OrderDto>()
		 .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.Username));
			CreateMap<OrderItemModel, OrderItemDto>()
				.ForMember(dest => dest.BookTitle, opt => opt.MapFrom(src => src.Book.Title));


			//User
			CreateMap<UserModel, UserDto>();
			CreateMap<UserCreateDto, UserModel>()
				.ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => src.Password));
			CreateMap<UserUpdateDto, UserModel>()
				.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));


			CreateMap<UserUpdateAdminDto, UserModel>()
	.ForAllMembers(opt => opt.Condition((src, dest, srcMember) => srcMember != null));

		}
	}
}
