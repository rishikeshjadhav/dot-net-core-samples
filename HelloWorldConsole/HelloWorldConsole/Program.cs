using Marten;
using Marten.Schema;
using System;
using System.Linq;

namespace HelloWorldConsole
{
    [DocumentAlias("johndeere")]
    public class User
    {
        public Guid Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public bool Internal { get; set; }
        public string UserName { get; set; }
    }


    class Program
    {
        static void Main(string[] args)
        {
            try
            {
                Console.WriteLine("Hello World!");

                //var store = DocumentStore.For("Server=127.0.0.1;Port=5432;Database=AdventureWorks;User Id=postgres;Password=Pass@1234;");
                var store = DocumentStore.For(_ =>
                {
                    _.Connection("Server=127.0.0.1;Port=5432;Database=AdventureWorks;User Id=postgres;Password=Pass@1234;");

                    _.AutoCreateSchemaObjects = AutoCreate.All;
                    //_.AutoCreateSchemaObjects = AutoCreate.CreateOrUpdate;
                    //_.AutoCreateSchemaObjects = AutoCreate.CreateOnly;
                    //_.AutoCreateSchemaObjects = AutoCreate.None;

                    _.Schema.For<User>().DocumentAlias("folks");
                    _.Storage.MappingFor(typeof(User)).DatabaseSchemaName = "other";

                    // this will tell marten to use the default 'public' schema name.
                    _.DatabaseSchemaName = Marten.StoreOptions.DefaultDatabaseSchemaName;
                });

                using (var session = store.LightweightSession())
                {
                    var user = new User() { FirstName = "Rishikesh", LastName = "Jadhav" };
                    session.Store(user);
                    session.SaveChanges();
                    var existing = session.Query<User>().Where(x => x.FirstName == "Rishikesh" && x.LastName == "Jadhav").FirstOrDefault();
                }

                using (var session = store.OpenSession())
                {
                    var existing = session.Query<User>().Where(x => x.FirstName == "Rishikesh" && x.LastName == "Jadhav").FirstOrDefault();
                }

                using (var session = store.QuerySession())
                {
                    var existing = session.Query<User>().Where(x => x.FirstName == "Rishikesh" && x.LastName == "Jadhav").FirstOrDefault();
                }

                using (var session = store.DirtyTrackedSession())
                {
                    var existing = session.Query<User>().Where(x => x.FirstName == "Rishikesh" && x.LastName == "Jadhav").FirstOrDefault();
                }

                Console.ReadKey();
            }
            catch (Exception exception)
            {
                Console.WriteLine("Exception occurred: " + exception.Message);
            }
        }
    }
}
