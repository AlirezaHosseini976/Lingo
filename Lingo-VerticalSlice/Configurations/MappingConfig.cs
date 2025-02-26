using System.Reflection;
using Lingo_VerticalSlice.Contracts.CardSet;
using Lingo_VerticalSlice.Entities;
using Mapster;

namespace Lingo_VerticalSlice.Configurations;

public class MappingConfig
{
    public static void MappingConfiguration(IServiceCollection service)
    {
        TypeAdapterConfig<CardSet, CardSetResponse>.NewConfig().Map(d => d.Name, src => src.Name);


        TypeAdapterConfig.GlobalSettings.Scan(Assembly.GetExecutingAssembly());
        service.AddSingleton(TypeAdapterConfig.GlobalSettings);
    }
}