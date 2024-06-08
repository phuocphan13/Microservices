using Grpc.Core;
using Grpc.Core.Testing;

namespace UnitTest.Common.Helpers.Grpc;

public static class TestServerCallContextHelpers
{
    public static ServerCallContext Create()
    {
        return TestServerCallContext.Create(null, null, default, null, default, null, null, null, null, null, null);
    }
}