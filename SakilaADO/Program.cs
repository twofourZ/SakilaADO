using Microsoft.Data.SqlClient;

namespace SakilaADO
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var connection = new SqlConnection("Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=Sakila;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False");
            var query =
                "select f.title, f.description, f.release_year " +
                "from film f " +
                "inner join film_actor fa on fa.film_id = f.film_id " +
                "inner join actor a on a.actor_id = fa.actor_id " +
                "where a.first_name = @actorFirstName and a.last_name = @actorLastName;";

            while (true)
            {
                Console.Clear();
                Console.Write("Actor first name: ");
                var actorFirstName = Console.ReadLine().ToUpper();
                Console.Write("Actor last name: ");
                var actorLastName = Console.ReadLine().ToUpper();

                var command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@actorFirstName", actorFirstName);
                command.Parameters.AddWithValue("@actorLastName", actorLastName);

                connection.Open();
                using (var reader = command.ExecuteReader())
                {
                    if (reader.HasRows)
                    {
                        while (reader.Read())
                        {
                            Console.WriteLine($"\n{reader.GetString(0)} {reader.GetString(2)}\n{reader.GetString(1)}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"\n{actorFirstName} {actorLastName} does not exist");
                    }
                }
                connection.Close();

                Console.ReadKey();
            }
        }
    }
}
