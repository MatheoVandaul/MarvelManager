using MarvelManager.Models;
using Npgsql;

namespace MarvelManager.Services;

public class TeamService
{
    private readonly string _connectionString;

    public TeamService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public List<Team> GetAll()
    {
        var teams = new List<Team>();

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = """
                                  SELECT id, name, description
                                  FROM teams
                                  ORDER BY name
                              """;

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            teams.Add(new Team
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                Description = reader.GetString(2)
            });
        }

        return teams;
    }
}