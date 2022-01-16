namespace TapeCat.Template.Api.Controllers;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Mime;

public sealed class FallbackController : ControllerBase
{
	private const string IndexFileName = "index.html";

	private readonly IWebHostEnvironment _webHostingEnvironment;

	public FallbackController ( IWebHostEnvironment webHostingEnvironment )
	{
		_webHostingEnvironment = webHostingEnvironment;
	}

	public IActionResult Index ()
	{
		var pathToIndexFile = Path.Combine ( _webHostingEnvironment.WebRootPath , IndexFileName );

		return System.IO.File.Exists ( pathToIndexFile )
			? PhysicalFile ( pathToIndexFile , MediaTypeNames.Text.Html )
			: NotFound ( new { Message = $"No '{IndexFileName}' file in '{_webHostingEnvironment.WebRootPath}'" } );
	}
}