# Sentry logging using gRPC interceptors
This repository implements [gRPC](https://grpc.io/) inteceptors for logging unwanted exceptions to the [Sentry](https://sentry.io).

## Usage

### Client
```C#
var channel = new Channel(hostname, port, ChannelCredentials.Insecure);
var client = new TestService.TestServiceClient(channel.Intercept(new SentryInterceptor("sentry DSN")));
```

### Server
```C#
var server = new Server()
{
  Services =
  {
    TestService.BindService(new ServiceImp()).Intercept(new SentryInterceptor(action))
  },
  Ports = { new ServerPort("localhost", Port, ServerCredentials.Insecure) }
};
```

### Own action
```C#
var interceptor = new SentryInterceptor(exception =>
{
  // DO STUFF
});
```

## LICENSE
License can be found [here](https://github.com/michalderdak/csharp-grpc-sentry/blob/master/LICENSE)
