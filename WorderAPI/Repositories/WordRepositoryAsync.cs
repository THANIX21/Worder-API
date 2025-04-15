using Npgsql;
using WorderAPI.Classes.Base;
using WorderAPI.Classes.Interfaces;
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

        public async Task<int> CreateWord(IWord word)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"INSERT INTO public.""Word"" (""term"", ""dt_created"", ""dt_altered"", ""type"") 
                   VALUES (@Term, @DTCreated, @DTAltered, @Type) 
                   RETURNING ""id""";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@Term", word.Term);
            cmd.Parameters.AddWithValue("@DTCreated", word.DTCreated);
            cmd.Parameters.AddWithValue("@DTAltered", word.DTAltered);
            cmd.Parameters.AddWithValue("@Type", word.Type);

            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task<List<WordType>> GetWordTypes()
        {
            var wordTypes = new List<WordType>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            await using var cmd = new NpgsqlCommand("SELECT * FROM public.\"WordType\" ORDER BY ID ASC", conn);
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
