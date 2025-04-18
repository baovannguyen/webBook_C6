using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Microsoft.AspNetCore.Http;

namespace AsmC6_API.Services
{
	public class VnPayLibrary
	{
		private readonly Dictionary<string, string> _requestData = new Dictionary<string, string>();

		// Thêm dữ liệu vào request
		public void AddRequestData(string key, string value)
		{
			if (!string.IsNullOrEmpty(key) && !string.IsNullOrEmpty(value))
			{
				_requestData[key] = value;
			}
		}

		// Tạo URL thanh toán từ dữ liệu đã thêm
		public string CreateRequestUrl(string paymentUrl, string hashSecret)
		{
			// Sắp xếp dữ liệu theo thứ tự từ điển (alphabet)
			var sortedData = _requestData.OrderBy(x => x.Key);

			// Tạo rawData KHÔNG bao gồm vnp_SecureHash và vnp_SecureHashType
			var rawData = string.Join("&", sortedData.Select(kv => $"{kv.Key}={kv.Value}"));

			// Tạo chữ ký từ rawData
			var secureHash = GetSecureHash(rawData, hashSecret);

			// Tạo full query string có thêm SecureHash và HashType
			var query = rawData + $"&vnp_SecureHashType=SHA256&vnp_SecureHash={secureHash}";
			return $"{paymentUrl}?{query}";
		}

		// Xác thực chữ ký trả về từ VNPay
		public bool ValidateSignature(IQueryCollection vnpayData, string hashSecret)
		{
			// Lọc ra các key KHÔNG phải là chữ ký
			var sorted = vnpayData
				.Where(kvp => kvp.Key != "vnp_SecureHash" && kvp.Key != "vnp_SecureHashType")
				.OrderBy(kvp => kvp.Key)
				.ToDictionary(kvp => kvp.Key, kvp => kvp.Value.ToString());

			// Ghép lại rawData từ dữ liệu nhận được
			var rawData = string.Join("&", sorted.Select(kv => $"{kv.Key}={kv.Value}"));

			// Hash rawData
			var computedHash = GetSecureHash(rawData, hashSecret);

			// Lấy hash từ VNPay trả về
			var receivedHash = vnpayData["vnp_SecureHash"];
			return string.Equals(computedHash, receivedHash, StringComparison.OrdinalIgnoreCase);
		}

		// Tạo chữ ký SHA256 HMAC
		private string GetSecureHash(string inputData, string key)
		{
			var keyBytes = Encoding.UTF8.GetBytes(key);
			var inputBytes = Encoding.UTF8.GetBytes(inputData);

			using (var hmac = new HMACSHA256(keyBytes))
			{
				var hashBytes = hmac.ComputeHash(inputBytes);
				return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
			}
		}

		// Cho phép truy cập dữ liệu request từ bên ngoài
		public IReadOnlyDictionary<string, string> RequestData => _requestData;
	}
}
