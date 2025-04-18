using Npgsql;
using WorderAPI.Classes.Base;
using WorderAPI.Classes.Interfaces;

namespace WorderAPI.Repositories
{
    public class WordRepositoryAsync : IWordRepositoryAsync
    {
        private readonly string _connectionString;
        private readonly TimeZoneInfo _timeZone;
        public WordRepositoryAsync(
            IConfiguration configuration,
            TimeZoneInfo timeZone)
        {
            _connectionString = configuration.GetConnectionString("WORD_DB");
            _timeZone = timeZone;
        }

        public async Task<List<Word>> GetAllWords()
        {
            List<Word> words = new List<Word>();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                    SELECT * FROM public.""Word""";

            await using var cmd = new NpgsqlCommand(sql, conn);

            await using var reader = await cmd.ExecuteReaderAsync();

            while(await reader.ReadAsync())
            {
                words.Add(new Word
                {
                    ID = reader.GetInt32(0),
                    Term = reader.GetString(1),
                    DTCreated = reader.GetDateTime(2),
                    DTAltered = reader.GetDateTime(3),
                    Type = reader.GetInt32(4)
                });
            }

            return words;
        }

        public async Task<Word> GetWord(int id)
        {
            Word word = null;

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                    SELECT * FROM public.""Word"" 
                    WHERE ""id"" = @ID;";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@ID", id);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                word = new Word
                {
                    ID = reader.GetInt32(0),
                    Term = reader.GetString(1),
                    DTCreated = reader.GetDateTime(2),
                    DTAltered = reader.GetDateTime(3),
                    Type = reader.GetInt32(4)
                };
            }

            return word;
        }

        public async Task<Word> CreateWord(IWord word)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                    INSERT INTO public.""Word"" (""term"", ""dt_created"", ""dt_altered"", ""type"") 
                    VALUES (@Term, @DTCreated, @DTAltered, @Type) 
                    RETURNING ""id"", ""term"", ""dt_created"", ""dt_altered"", ""type"";";

            await using var cmd = new NpgsqlCommand(sql, conn);

            var now = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone);

            cmd.Parameters.AddWithValue("@Term", word.Term);
            cmd.Parameters.AddWithValue("@DTCreated", now);
            cmd.Parameters.AddWithValue("@DTAltered", now);
            cmd.Parameters.AddWithValue("@Type", word.Type);

            await using var reader = await cmd.ExecuteReaderAsync();

            if (await reader.ReadAsync())
            {
                return new Word
                {
                    ID = reader.GetInt32(0),
                    Term = reader.GetString(1),
                    DTCreated = reader.GetDateTime(2),
                    DTAltered = reader.GetDateTime(3),
                    Type = reader.GetInt32(4)
                };
            }

            return null;
        }

        public async Task<int> EditWord(IWord word)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                    UPDATE public.""Word""  
                    SET ""term"" = @Term, 
                        ""dt_altered"" = @DTAltered, 
                        ""type"" = @Type
                    WHERE ""id"" = @ID
                    RETURNING ""id"";";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@ID", word.ID);
            cmd.Parameters.AddWithValue("@Term", word.Term);
            cmd.Parameters.AddWithValue("@DTAltered", TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, _timeZone));
            cmd.Parameters.AddWithValue("@Type", word.Type);

            var result = await cmd.ExecuteScalarAsync();

            return Convert.ToInt32(result);
        }

        public async Task<int> DeleteWord(int id)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                    DELETE FROM public.""Word"" 
                    WHERE ""id"" = @ID;";

            await using var cmd = new NpgsqlCommand(sql, conn);

            cmd.Parameters.AddWithValue("@ID", id);

            var result = await cmd.ExecuteNonQueryAsync(); // to show rows affected

            return result;
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
