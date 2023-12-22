using Catalog.API.Repositories;
using Microsoft.Extensions.Configuration;

namespace Catalog.API.Tests.UnitTests.Repositories;

public class ProductRepositoryTests
{
    [Fact]
    public async Task GetEntitiesAsync_ExpectedResult()
    {
        Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
        mockSection.Setup(x => x.Value).Returns("DatabaseSettings:ConnectionString");
        
        var configurationMock = new Mock<IConfiguration>();
        configurationMock.Setup(x => x.GetSection("DatabaseSettings:ConnectionString")).Returns(mockSection.Object);

        var repository = new Repository<Entities.Product>(configurationMock.Object);

        var entities = await repository.GetEntitiesAsync(default);
    }
}