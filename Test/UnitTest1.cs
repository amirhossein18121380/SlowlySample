using CrossCuttingConcerns.Protos;
using Grpc.Net.Client;

namespace Test
{
    public class UnitTest1
    {
        [Fact]
        public async Task Test1()
        {
            var channel = GrpcChannel.ForAddress("https://localhost:7184");
            var client = new GetTopicByIdGrpcService.GetTopicByIdGrpcServiceClient(channel);

            var request = new GetTopicQueryByIdGrpcRequest { Id = "b6c51d8b-beea-427d-ac03-08dc727aae63" };
            var response = await client.HandleAsync(request);

            Console.WriteLine($"Status Code: {response.StatusCode}, Message: {response.Message}");

        }
    }
}