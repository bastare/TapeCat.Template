namespace TapeCat.Template.Api.Controllers.Interfaces;

using Domain.Shared.Authorization.Session;
using MapsterMapper;
using MassTransit;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.DependencyInjection;

[ApiController]
[Route ( "api/v{apiVersion:apiVersion}/[controller]" )]
public abstract class ExtendedController : ControllerBase
{
	private IMapper? _mapper;

	private UserSession? _userSession;

	protected IMapper Mapper =>
		_mapper ??= ResolveService<IMapper> ();

	protected UserSession UserSession =>
		_userSession ??= ResolveService<UserSession> ();

	protected IRequestClient<TContract> ResolveRequestClient<TContract> ()
		where TContract : class
			=> ResolveService<IRequestClient<TContract>> ();

	protected IHubContext<THub , IHubClient> ResolveHubContext<THub, IHubClient> ()
		where THub : Hub<IHubClient>
		where IHubClient : class
			=> ResolveService<IHubContext<THub , IHubClient>> ();

	private TService ResolveService<TService> ()
		where TService : notnull
	{
		using var scopeService = HttpContext.RequestServices.CreateScope ();

		return scopeService.ServiceProvider.GetRequiredService<TService> ();
	}
}