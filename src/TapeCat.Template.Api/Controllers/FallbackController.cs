namespace TapeCat.Template.Api.Controllers;

using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Net.Mime;

public sealed class FallbackController : ControllerBase
{
	private const string IndexFileName = "index.html";

	private string PathToIndexFile => Path.Combine ( _webHostingEnvironment.WebRootPath , IndexFileName );

	private readonly IWebHostEnvironment _webHostingEnvironment;

	public FallbackController ( IWebHostEnvironment webHostingEnvironment )
	{
		_webHostingEnvironment = webHostingEnvironment;
	}

	public IActionResult Index ()
		=> System.IO.File.Exists ( PathToIndexFile )
			? PhysicalFile ( PathToIndexFile , MediaTypeNames.Text.Html )
			: NotFound ( new { Message = $"No '{IndexFileName}' file in '{_webHostingEnvironment.WebRootPath}'" } );
}