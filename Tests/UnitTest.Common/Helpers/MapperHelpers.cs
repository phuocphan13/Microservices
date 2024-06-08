using AutoMapper;

namespace UnitTest.Common.Helpers;

public static class MapperHelpers
{
    public static void ConfigMapper<TSource, TDestination>(this Mock<IMapper> mapper, TDestination destination)
    {
        mapper.Setup(x => x.Map<TDestination>(It.IsAny<TSource>())).Returns(destination);
    }
}