using System.Collections.Generic;

namespace Functors.Maybe.Example
{
    public class Client
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class MockClientRepository
    {
        List<Client> clients = new List<Client>{
                    new  Client{Id=1, Name="Jim"},
                    new  Client{Id=2, Name="John"}
                };

        public Maybe<Client> GetById(int id) => clients.FirstOrNone(x => x.Id == id);
    }

    public class Demo
    {
        public void Run()
        {
            var repository = new MockClientRepository();

            var result =
                repository.GetById(6)
                  .MatchWith(pattern: (
                        None: () => "Not Found",
                        Some: (client) => client.Name
                    ));
        }
    }
}
