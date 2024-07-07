namespace TapeCat.Template.Infrastructure.loC.Injectors;

using Domain.Shared.Configurations.Options;
using Domain.Shared.Configurations.Options.Attributes;
using InjectorBuilder.Common.Interfaces;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using MoreLinq;

public sealed class OptionsInjector : IInjectable
{
    private readonly Assembly[] _assembliesWithOptions = [
        typeof ( RabbitMqOption ).Assembly
    ];

    public void Inject(IServiceCollection _, IConfiguration configuration)
    {
        _assembliesWithOptions
            .SelectMany(ResolveAllTypes)
            .Where(IsOptionType)
            .ForEach(optionType =>
                InjectOptionType(configuration, optionType));

        static IEnumerable<Type> ResolveAllTypes(Assembly assembly)
            => assembly.GetTypes();

        static bool IsOptionType(Type type)
            => type.GetCustomAttribute<OptionAttribute>() is not null;

        static void InjectOptionType(IConfiguration configuration, Type optionType)
        {
            configuration.GetSection(key: optionType.GetCustomAttribute<OptionAttribute>()!.SectionName)
                .Bind(Activator.CreateInstance(optionType));
        }
    }
}