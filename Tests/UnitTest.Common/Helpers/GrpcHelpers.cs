using Grpc.Core;
using Grpc.Core.Testing;

namespace UnitTest.Common.Helpers;

public static class GrpcHelpers
{
    public static AsyncUnaryCall<T> BuildAsyncUnaryCall<T>(T response)
    {
        var call = TestCalls.AsyncUnaryCall(Task.FromResult(response), Task.FromResult(new Metadata()),
            () => Status.DefaultSuccess, () => [], () => { });

        return call;
    }
}