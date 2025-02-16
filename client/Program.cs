using Grpc.Net.Client;
using client;

// The port number must match the port of the gRPC server.
using var channel = GrpcChannel.ForAddress("https://localhost:7127");
var client = new Greeter.GreeterClient(channel);
var reply = await client.SayHelloAsync(
    new HelloRequest { Name = "Single message - GreeterClient" });
Console.WriteLine("Greeting: " + reply.Message);

Grpc.Core.AsyncClientStreamingCall<HelloRequest, HelloReply> call = client.SayHelloStream();
Grpc.Core.IClientStreamWriter<HelloRequest> stream = call.RequestStream;
stream.WriteOptions = new Grpc.Core.WriteOptions(Grpc.Core.WriteFlags.BufferHint);
for(int i=0;i<50_000;i++){
    HelloRequest request = new HelloRequest { Name = $"Multi messages - GreeterClient {i}" };
    await call.RequestStream.WriteAsync(request);
}
await call.RequestStream.CompleteAsync();
var response = await call;
Console.WriteLine("Streaming result: " + response.Message);