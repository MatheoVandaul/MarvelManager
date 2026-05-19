using MarvelManager.Extensions;
using MarvelManager.Models;
using Npgsql;

namespace MarvelManager.Services;

public class CharacterService
{
    private readonly string _connectionString;

    public CharacterService(IConfiguration configuration)
    {
        _connectionString = configuration.GetConnectionString("DefaultConnection")!;
    }

    public List<MarvelCharacter> GetAll()
    {
        var characters = new List<MarvelCharacter>();

        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = """
                                  SELECT c.id,
                                         c.name,
                                         c.hero_name,
                                         c.power,
                                         c.power_level,
                                         c.image_url,
                                         c.team_id,
                                         t.name
                                  FROM characters c
                                  JOIN teams t ON c.team_id = t.id
                                  ORDER BY c.hero_name
                              """;

        using var reader = command.ExecuteReader();

        while (reader.Read())
        {
            characters.Add(new MarvelCharacter
            {
                Id = reader.GetInt32(0),
                Name = reader.GetString(1),
                HeroName = reader.GetString(2),
                Power = reader.GetString(3),
                PowerLevel = reader.GetInt32(4),
                ImageUrl = reader.IsDBNull(5) ? null : reader.GetString(5),
                TeamId = reader.GetInt32(6),
                TeamName = reader.GetString(7)
            });
        }

        return characters;
    }
    public void Create(MarvelCharacter character)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = """
                                  INSERT INTO characters
                                  (name, hero_name, power, power_level, image_url, team_id)
                                  VALUES
                                  (@name, @heroName, @power, @powerLevel, @imageUrl, @teamId)
                              """;

        command.AddParameter("@name", character.Name);
        command.AddParameter("@heroName", character.HeroName);
        command.AddParameter("@power", character.Power);
        command.AddParameter("@powerLevel", character.PowerLevel);
        command.AddParameter("@imageUrl",
            character.ImageUrl ?? (object)DBNull.Value);
        command.AddParameter("@teamId", character.TeamId);

        command.ExecuteNonQuery();
    }
    
    public MarvelCharacter? GetById(int id)
    {
        using var connection = new NpgsqlConnection(_connectionString);
        connection.Open();

        var command = connection.CreateCommand();

        command.CommandText = """
                                  SELECT c.id, c.name, c.hero_name, c.power, c.power_level,
                                         c.image_url, c.team_id, t.name
                                  FROM characters c
                                  JOIN teams t ON c.team_id = t.id
                                  WHERE c.id = @id
                              """;

        command.AddParameter("@id", id);

        using var reader = command.ExecuteReader();

        if (!reader.Read())
            return null;

        return new MarvelCharacter
        {
            Id = reader.GetInt32(0),
            Name = reader.GetString(1),
            HeroName = reader.GetString(2),
            Power = reader.GetString(3),
            PowerLevel = reader.GetInt32(4),
            ImageUrl = reader.IsDBNull(5) ? null : reader.GetString(5),
            TeamId = reader.GetInt32(6),
            TeamName = reader.GetString(7)
        };
    }
}