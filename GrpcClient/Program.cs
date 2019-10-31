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

            //StudentModel newStudent = new StudentModel()
            //{
            //    FirstName = "AAAA",
            //    LastName = "BBBB",
            //    School = "Tourism",
            //};
            //await insertStudent(channel, newStudent);


            //StudentModel updStudent = new StudentModel()
            //{
            //    StudentId = 56,
            //    FirstName = "AAAA",
            //    LastName = "ZZZ",
            //    School = "Medicine",
            //};
            //await updateStudent(channel, updStudent);

            //await deleteStudent(channel, 56);

            await displayAllStudents(channel);

            Console.ReadLine();
        }

        static async Task findStudentById(GrpcChannel channel, int id)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var input = new StudentLookupModel { StudentId = id };
            var reply = await client.GetStudentInfoAsync(input);
            Console.WriteLine($"{reply.FirstName} {reply.LastName}");
        }

        static async Task insertStudent(GrpcChannel channel, StudentModel student)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var reply = await client.InsertStudentAsync(student);
            Console.WriteLine(reply.Result);
        }

        static async Task updateStudent(GrpcChannel channel, StudentModel student)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var reply = await client.UpdateStudentAsync(student);
            Console.WriteLine(reply.Result);
        }

        static async Task deleteStudent(GrpcChannel channel, int id)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var input = new StudentLookupModel { StudentId = id };
            var reply = await client.DeleteStudentAsync(input);
            Console.WriteLine(reply.Result);
        }

        static async Task displayAllStudents(GrpcChannel channel)
        {
            var client = new RemoteStudent.RemoteStudentClient(channel);

            var empty = new Empty();
            var list = await client.RetrieveAllStudentsAsync(empty);

            Console.WriteLine(">>>>>>>>>>>>>>>>>>++++++++++++<<<<<<<<<<<<<<<<<<<<<<<<<<<<");

            foreach (var item in list.Items)
            {
                Console.WriteLine($"{item.StudentId}: {item.FirstName} {item.LastName}");
            }
        }


    }
}
