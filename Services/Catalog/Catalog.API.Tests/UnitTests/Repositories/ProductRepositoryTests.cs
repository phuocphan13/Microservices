using System.Linq.Expressions;
using Catalog.API.Repositories;
using Catalog.API.Tests.Common;
using Microsoft.Extensions.Configuration;
using MongoDB.Driver;

namespace Catalog.API.Tests.UnitTests.Repositories;

public class ProductRepositoryTests
{
    [Fact]
    public async Task GetEntitiesAsync_ExpectedResult()
    {
        // Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
        // mockSection.Setup(x => x.Key).Returns("DatabaseSettings:ConnectionString");
        // mockSection.Setup(x => x.Value).Returns("DatabaseSettings:ConnectionString");
        //
        // var configurationMock = new Mock<IConfiguration>();
        // configurationMock.Setup(x => x.GetValue<string>("DatabaseSettings:ConnectionString")).Returns("");

        // var products = ModelHelpers.Product.GenerateProductEntities();
        var products = new Mock<IFindFluent<Entities.Product, Entities.Product>>();

        var configuration = new Mock<IConfiguration>();
        var configurationSection = new Mock<IConfigurationSection>();
        var mongoClient = new Mock<MongoClient>();
        var mongoDB = new Mock<IMongoDatabase>();
        var mongoCollection = new Mock<IMongoCollection<Entities.Product>>();
        
        configurationSection.Setup(a => a.Value).Returns("testvalue");
        configuration.Setup(a => a.GetSection(It.IsAny<string>())).Returns(configurationSection.Object);
        mongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(mongoDB.Object);
        mongoDB.Setup(x => x.GetCollection<Entities.Product>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(mongoCollection.Object);
        mongoCollection.Setup(x => x.Find(It.IsAny<Expression<Func<Entities.Product, bool>>>(), It.IsAny<FindOptions>()))
            .Returns(products.Object);

        var repository = new Repository<Entities.Product>(configuration.Object);

        var entities = await repository.GetEntitiesAsync(default);
    }
}