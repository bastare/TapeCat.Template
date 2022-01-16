namespace TapeCat.Template.Domain.Shared.Common.Extensions;

using Microsoft.AspNetCore.Http;
using System.IO;
using System.Threading.Tasks;

public static class FormFileExtensions
{
	public static async Task<byte[]> ToByteArrayAsync ( this IFormFile formFile )
	{
		using var memoryStream = new MemoryStream ();

		await formFile.CopyToAsync ( memoryStream );

		return memoryStream.ToArray ();
	}

	public static byte[] ToByteArray ( this IFormFile formFile )
	{
		using var memoryStream = new MemoryStream ();

		formFile.CopyTo ( memoryStream );

		return memoryStream.ToArray ();
	}
}
