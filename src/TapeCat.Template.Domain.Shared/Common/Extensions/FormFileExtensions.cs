namespace TapeCat.Template.Domain.Shared.Common.Extensions;

using Microsoft.AspNetCore.Http;

public static class FormFileExtensions
{
	public static async Task<byte[]> ToByteArrayAsync ( this IFormFile formFile , CancellationToken cancellationToken = default )
	{
		using var memoryStream = new MemoryStream ();

		await formFile.CopyToAsync ( memoryStream , cancellationToken );

		return memoryStream.ToArray ();
	}

	public static byte[] ToByteArray ( this IFormFile formFile )
	{
		using var memoryStream = new MemoryStream ();

		formFile.CopyTo ( memoryStream );

		return memoryStream.ToArray ();
	}
}