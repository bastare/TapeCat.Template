namespace TapeCat.Template.Infrastructure.InjectorBuilder.Common.Interfaces;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

public interface IInjectable
{
	void Inject ( IServiceCollection serviceCollection , IConfiguration configuration );

	public bool IsInjectable ( IServiceCollection _ , IConfiguration _1 )
		=> true;
}