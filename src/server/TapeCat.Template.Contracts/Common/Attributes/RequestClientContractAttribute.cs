namespace TapeCat.Template.Contracts.Common.Attributes;

using MassTransit;

[AttributeUsage ( AttributeTargets.All , AllowMultiple = false )]
public sealed class RequestClientContractAttribute : Attribute
{
	public RequestTimeout? RequestTimeout { get; }

	public RequestClientContractAttribute ()
	{ }

	public RequestClientContractAttribute ( int requestTimeoutInMilliseconds )
	{
		RequestTimeout = requestTimeoutInMilliseconds;
	}
}