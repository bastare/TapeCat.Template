namespace TapeCat.Template.Domain.Shared.Configurations.Options;

using Attributes;

[Option ( sectionName: nameof ( RabbitMqOption ) )]
public sealed class RabbitMqOption
{
	public string? Host { get; set; }

	public ushort Port { get; set; }

	public string? VirtualHost { get; set; }

	public string? Username { get; set; }

	public string? Password { get; set; }
}