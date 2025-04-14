using Npgsql;
using WorderAPI.Classes;
using WorderAPI.Interfaces;

namespace WorderAPI.Repositories
{
    public class WordRepositoryAsync : IWordRepositoryAsync
    {
        private readonly string _connectionString;
        public WordRepositoryAsync(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("WORD_DB");
        }

        public async Task<List<WordType>> GetWordTypes()
        {
            var wordTypes = new List<WordType>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand("CALL GetWordTypes()", conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            while (await reader.ReadAsync())
            {
                wordTypes.Add(new WordType
                {
                    ID = reader.GetInt32(0),
                    Type = reader.GetString(1),
                }); 
            }

            return wordTypes;
        }


    }
}
