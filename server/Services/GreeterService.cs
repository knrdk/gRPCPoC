using Grpc.Core;
using gRPCpoc;

namespace gRPCpoc.Services;

public class GreeterService : Greeter.GreeterBase
{
    private readonly ILogger<GreeterService> _logger;
    public GreeterService(ILogger<GreeterService> logger)
    {
        _logger = logger;
    }

    public override Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
    {
        _logger.LogInformation("Processing request");
        return Task.FromResult(new HelloReply
        {
            Message = "Hello " + request.Name
        });
    }

    public override async Task<HelloReply> SayHelloStream(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
    {
        await foreach (HelloRequest? request in requestStream.ReadAllAsync())
        {
            // _logger.LogInformation("Processing request: {requestName}", request.Name);
        }

        return new HelloReply
        {
            Message = "Processed all messages"
        };
    }
}
