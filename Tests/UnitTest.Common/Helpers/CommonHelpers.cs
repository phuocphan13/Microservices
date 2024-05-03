using ApiClient.Common;
using Platform.ApiBuilder;

namespace UnitTest.Common.Helpers;

public static class CommonHelpers
{
    private static readonly Random _random = new Random();

    public static string GenerateBsonId()
    {
        return Guid.NewGuid().ToString().Replace("-", "")[..24];
    }

    public static string GenerateRandomString()
    {
        return Guid.NewGuid().ToString().Replace("-", "");
    }

    public static int GenerateRandomInteger()
    {
        int firstBits = _random.Next(0, 1 << 4) << 28;
        int lastBits = _random.Next(0, 1 << 28);
        return firstBits | lastBits;
    }

    public static decimal GenerateRandomDecimal()
    {
        byte scale = (byte)_random.Next(29);
        bool sign = _random.Next(2) == 1;

        return new decimal(GenerateRandomInteger(), GenerateRandomInteger(), GenerateRandomInteger(), sign, scale);
    }
    
    public static class ApiResult
    {
        public static ApiDataResult<T> Ok<T>(T data)
            where T: class, new()
        {
            return new ApiDataResult<T>(data);
        }

        public static ApiDataResult<T> NotFound<T>(T data)
            where T : class, new()
        {
            return new ApiDataResult<T>(data)
            {
                Message = "not existed"
            };
        }

        public static ApiDataResult<T> Problem<T>(T data)
            where T : class, new()
        {
            return new ApiDataResult<T>(data)
            {
                Message = "Problem"
            };
        }
    }
}