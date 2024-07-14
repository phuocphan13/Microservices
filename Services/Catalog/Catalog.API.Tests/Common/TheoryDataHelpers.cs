namespace Catalog.API.Tests.Common;

public class TheoryDataHelpers<T> : TheoryData<List<T>>
    where T: class
{
    public TheoryDataHelpers()
    {
        Add(null!);
        Add(new List<T>());
    }
}