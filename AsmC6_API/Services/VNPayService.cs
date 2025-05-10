using System;
using System.Text;
using System.Security.Cryptography;
using System.Web;

public class VNPayService
{
	private readonly string _vnpUrl = "https://sandbox.vnpayment.vn/paymentv2/vpcpay.html";
	private readonly string _hashSecret = "91HL7DAJ45BEYYF9HGNDAJFHEUSQHL03";
	private readonly string _vnpTmnCode = "9U4CW4IM";

	public string CreatePaymentUrl(string amount, string orderInfo)
	{
		var createDate = DateTime.Now.ToString("yyyyMMddHHmmss");
		var returnUrl = "https://4965-171-250-162-59.ngrok-free.app/api/payment/vnpay-return";

		// Cấu hình các tham số thanh toán
		var requestData = new SortedDictionary<string, string>
		{
			{ "vnp_Version", "2.1.0" },
			{ "vnp_Command", "pay" },
			{ "vnp_TmnCode", _vnpTmnCode },
			{ "vnp_Amount", amount },
			{ "vnp_CurrCode", "VND" },
			{ "vnp_CreateDate", createDate },
			{ "vnp_OrderInfo", orderInfo },
			{ "vnp_Locale", "vn" },
			{ "vnp_ReturnUrl", returnUrl },
			{ "vnp_IpAddr", "127.0.0.1" } // Sử dụng IP của máy để gửi yêu cầu
        };

		// Tạo chuỗi dữ liệu
		var queryString = string.Join("&", requestData.Select(kv => $"{kv.Key}={kv.Value}"));

		// Tạo chữ ký cho yêu cầu
		var secureHash = GetSecureHash(queryString, _hashSecret);

		// Tạo URL yêu cầu thanh toán với chữ ký
		var paymentUrl = $"{_vnpUrl}?{queryString}&vnp_SecureHash={secureHash}";

		return paymentUrl;
	}

	private string GetSecureHash(string stringData, string secret)
	{
		var keyByte = Encoding.UTF8.GetBytes(secret);
		var dataByte = Encoding.UTF8.GetBytes(stringData);

		using (var sha256 = new HMACSHA256(keyByte))
		{
			var hashBytes = sha256.ComputeHash(dataByte);
			return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
		}
	}
}
