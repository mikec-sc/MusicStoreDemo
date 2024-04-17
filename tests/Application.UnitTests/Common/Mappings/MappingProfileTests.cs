using System.Runtime.Serialization;
using AutoMapper;
using MusicStore.Application.Common.Mappings;
using MusicStore.Application.Common.Models;

namespace MusicStore.Application.UnitTests.Common.Mappings;

public class MappingProfileTests
{
    private readonly IConfigurationProvider _configuration;
    private readonly IMapper _mapper;

    public MappingProfileTests()
    {
        _configuration = new MapperConfiguration(config =>
            config.AddProfile<MappingProfile>());

        _mapper = _configuration.CreateMapper();
    }

    [Test]
    public void ShouldHaveValidConfiguration()
    {
        _configuration.AssertConfigurationIsValid();
    }

    [Test]
    [TestCase(typeof(Domain.Entities.InventoryItem), typeof(InventoryItem))]
    public void ShouldSupportMappingFromSourceToDestination(Type source, Type destination)
    {
        var instance = GetInstanceOf(source);

        _mapper.Map(instance, source, destination);
    }

    private static object GetInstanceOf(Type type)
    {
        return type.GetConstructor(Type.EmptyTypes) != null ? Activator.CreateInstance(type)! :
            // Type without parameterless constructor
            FormatterServices.GetUninitializedObject(type);
    }
}
