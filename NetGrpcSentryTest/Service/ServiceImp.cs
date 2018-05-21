using System;
using System.Threading.Tasks;
using Grpc.Core;

namespace NetGrpcSentryTest.Service
{
    public class ServiceImp : TestService.TestServiceBase
    {
        public override Task<Response> ThrowsExcpetion(Request request, ServerCallContext context)
        {
            throw new Exception();
        }

        public override Task<Response> ThrowsRpc(Request request, ServerCallContext context)
        {
            throw new RpcException(new Status(StatusCode.Internal, "message"));
        }
    }
}
