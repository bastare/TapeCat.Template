namespace TapeCat.Template.Api.Pipes.SecurityPipes;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

public static class SecurityPipe
{
	public static void UseSecurityHeaders ( this IApplicationBuilder applicationBuilder , IWebHostEnvironment _ )
	{
		applicationBuilder.UseHttpsRedirection ();

		applicationBuilder.UseHsts ( hsts =>
		  {
			  hsts.MaxAge ( days: 365 )
				  .IncludeSubdomains ();
		  } );

		applicationBuilder.UseXContentTypeOptions ();

		applicationBuilder.UsePermissionsPolicy ();

		applicationBuilder.UseXfo ( xfo => { xfo.SameOrigin (); } );

		applicationBuilder.UseReferrerPolicy ( options => { options.NoReferrer (); } );

		applicationBuilder.UseXXssProtection ( options => { options.EnabledWithBlockMode (); } );

		applicationBuilder.UseCsp ( options =>
		  {
			  options
				  .StyleSources ( configurer =>
					  {
						  configurer.Self ()
							 .CustomSources (
								 "www.google.com" ,
								 "platform.twitter.com" ,
								 "cdn.syndication.twimg.com" ,
								 "fonts.googleapis.com" )
							 .UnsafeInline ();
					  } )
				  .ScriptSources ( configurer =>
					  {
						  configurer.Self ()
							 .CustomSources (
								 "www.google.com" ,
								 "cse.google.com" ,
								 "cdn.syndication.twimg.com" ,
								 "platform.twitter.com" ,
								 "https://script.hotjar.com" ,
								 "https://static.hotjar.com" ,
								 "https://www.google-analytics.com" ,
								 "https://connect.facebook.net" ,
								 "https://www.youtube.com" )
						   .UnsafeInline ()
						   .UnsafeEval ();
					  } );
		  } );
	}
}