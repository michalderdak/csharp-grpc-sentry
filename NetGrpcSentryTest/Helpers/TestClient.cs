using System;
using Grpc.Core;
using Grpc.Core.Interceptors;
using NetGrpcSentry;
using NetGrpcSentryTest.Service;

namespace NetGrpcSentryTest.Helpers
{
    public static class TestClient
    {
        public static TestService.TestServiceClient Create(int port, Action<Exception> action)
        {
            var channel = new Channel("localhost", port, ChannelCredentials.Insecure);
            return new TestService.TestServiceClient(channel.Intercept(new SentryInterceptor(action)));
        }
    }
}
