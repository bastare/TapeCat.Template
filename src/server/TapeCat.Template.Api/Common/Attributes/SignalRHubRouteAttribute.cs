namespace TapeCat.Template.Api.Common.Attributes;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = false)]
public sealed class SignalRHubRouteAttribute : Attribute
{
    public string Path { get; }

    public SignalRHubRouteAttribute(string? path)
    {
        NotNullOrEmpty(path);

        Path = path!;
    }
}