using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Functors.Task
{
    public static class FunctionalExtensions
    {
        public static Task<T1> Select<T, T1>(this Task<T> task, Func<T, T1> mapping)
        {
            var value = task.GetAwaiter().GetResult();
            return Task<T1>.Run(() => mapping(value));
        }

    }
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class MockClientRepository
    {
        public Task<Client> GetById(int id) =>
            Task<Client>.Run(() =>
                new List<Client>{
                    new  Client{Id=1, Name="Jim"},
                    new  Client{Id=2, Name="John"}
                }
                .FirstOrDefault(x => x.Id == id)
            );

    }

    public class Demo
    {
        public static async System.Threading.Tasks.Task RunAsync()
        {
            var repository = new MockClientRepository();

            var clientName = await (from client in repository.GetById(1)
                                    select client != null ?
                                    client.Name :
                                    "Nothing found");

        }
    }

}