using Grpc.Net.Client;
using GrpcService;
using GrpcService.Protos;
using System;
using System.Threading.Tasks;

namespace GrpcClient
{
    class Program
    {
        static async Task Main(string[] args)
        {
            //var input = new HelloRequest { Name = "Jane Bond" };

            //var channel = GrpcChannel.ForAddress("https://localhost:5001");
            //var client = new Greeter.GreeterClient(channel);

            //var reply = await client.SayHelloAsync(input);

            //Console.WriteLine(reply.Message);

            var channel = GrpcChannel.ForAddress("https://localhost:5001");
            var studentClient = new RemoteStudent.RemoteStudentClient(channel);
            var studentInput = new StudentLookupModel { StudentId = 33 };
            var studentReply = await studentClient.GetStudentInfoAsync(studentInput);
            Console.WriteLine($"{studentReply.FirstName} {studentReply.LastName}");

            Console.ReadLine();
        }

    }
}
