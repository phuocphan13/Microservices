namespace Catalog.API.Tests.UnitTests.Repositories;

public class ProductRepositoryTests
{
    // [Fact]
    // public async Task GetEntitiesAsync_ExpectedResult()
    // {
        //Todo Update Unit Test later after figure out the the way
        // Mock<IConfigurationSection> mockSection = new Mock<IConfigurationSection>();
        // mockSection.Setup(x => x.Key).Returns("DatabaseSettings:ConnectionString");
        // mockSection.Setup(x => x.Value).Returns("DatabaseSettings:ConnectionString");
        //
        // var configurationMock = new Mock<IConfiguration>();
        // configurationMock.Setup(x => x.GetValue<string>("DatabaseSettings:ConnectionString")).Returns("");

        // var products = ModelHelpers.Product.GenerateProductEntities();

        // var mockIMongoCollection = new Mock<IMongoCollection<Person>>();
        // var asyncCursor = new Mock<IAsyncCursor<Person>>();
        //
        // var expectedResult = fixture.CreateMany<Person>(5);
        //
        // mockIMongoCollection.Setup(_collection => _collection.FindSync(
        //         Builders<Person>.Filter.Empty,
        //         It.IsAny<FindOptions<Person>>(),
        //         default))
        //     .Returns(asyncCursor.Object);

        // asyncCursor.SetupSequence(_async => _async.MoveNext(default)).Returns(true).Returns(false);
        // asyncCursor.SetupGet(_async => _async.Current).Returns(expectedResult);




        // var entities = ModelHelpers.Product.GenerateProductEntities();
        //
        // var products = new Mock<IFindFluent<Entities.Product, Entities.Product>>();
        //
        // var asyncCursor = new Mock<IAsyncCursor<Entities.Product>>();
        //
        // var configuration = new Mock<IConfiguration>();
        // var configurationSection = new Mock<IConfigurationSection>();
        // var mongoClient = new Mock<MongoClient>();
        // var mongoDB = new Mock<IMongoDatabase>();
        // var mongoCollection = new Mock<IMongoCollection<Entities.Product>>();
        //
        // configurationSection.Setup(a => a.Value).Returns("mongodb://localhost:27017");
        // configuration.Setup(a => a.GetSection(It.IsAny<string>())).Returns(configurationSection.Object);
        //
        // mongoClient.Setup(x => x.GetDatabase(It.IsAny<string>(), It.IsAny<MongoDatabaseSettings>())).Returns(mongoDB.Object);
        // mongoDB.Setup(x => x.GetCollection<Entities.Product>(It.IsAny<string>(), It.IsAny<MongoCollectionSettings>())).Returns(mongoCollection.Object);
        // mongoCollection.Setup(x => x.FindAsync(Builders<Entities.Product>.Filter.Empty, It.IsAny<FindOptions<Entities.Product>>(), default))
        //     .ReturnsAsync(asyncCursor.Object);
        //
        // asyncCursor.SetupSequence(_async => _async.MoveNext(default)).Returns(true).Returns(false);
        // asyncCursor.SetupGet(_async => _async.Current).Returns(entities);
        //
        // var repository = new Repository<Entities.Product>(configuration.Object);
        //
        // var result = await repository.GetEntitiesAsync(default);
    // }
}