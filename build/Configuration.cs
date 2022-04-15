using Nuke.Common.Tooling;
using System.ComponentModel;

[TypeConverter ( typeof ( TypeConverter<Configuration> ) )]
public class Configuration : Enumeration
{
	public static Configuration Debug { set; get; } = new () { Value = nameof ( Debug ) };

	public static Configuration Release { set; get; } = new () { Value = nameof ( Release ) };

	public static implicit operator string ( Configuration configuration )
		=> configuration.Value;
}