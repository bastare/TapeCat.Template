namespace TapeCat.Template.Api.Common.Extensions;

using Pipes.SecurityPipes;

public static class ApplicationBuilderExtensions
{
	public static IApplicationBuilder UseSecureHeaders ( this IApplicationBuilder applicationBuilder )
		=> applicationBuilder
			.UseHttpsRedirection ()
			.UseHsts ( hsts =>
			{
				hsts.MaxAge ( days: 365 ).IncludeSubdomains ();
			} )
			.UseXContentTypeOptions ()
			.UsePermissionsPolicy (
				siteUrl => [
					$"fullscreen=(self {siteUrl})",
					$"geolocation=(self {siteUrl})",
					$"payment=(self {siteUrl})",
					"camera=()",
					"microphone=()",
					"usb=()"
				]
			)
			.UseXfo ( xfo =>
			{
				xfo.SameOrigin ();
			} )
			.UseReferrerPolicy ( options =>
			{
				options.NoReferrer ();
			} )
			.UseXXssProtection ( options =>
			{
				options.EnabledWithBlockMode ();
			} )
			.UseCsp ( options =>
			{
				options
					.StyleSources ( configure =>
					{
						configure
							.Self ()
							.CustomSources (
								"www.google.com" ,
								"platform.twitter.com" ,
								"cdn.syndication.twimg.com" ,
								"fonts.googleapis.com"
							)
							.UnsafeInline ();
					} )
					.ScriptSources ( configure =>
					{
						configure
							.Self ()
							.CustomSources (
								"www.google.com" ,
								"cse.google.com" ,
								"cdn.syndication.twimg.com" ,
								"platform.twitter.com" ,
								"https://www.google-analytics.com" ,
								"https://connect.facebook.net" ,
								"https://www.youtube.com"
							)
							.UnsafeInline ()
							.UnsafeEval ();
					} );
			} );
}