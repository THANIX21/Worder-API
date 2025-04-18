using Npgsql;
using WorderAPI.Classes.Base;
using WorderAPI.Repositories.Interfaces;

namespace WorderAPI.Repositories.Base
{
    public class SentenceRepositoryAsync : ISentenceRepositoryAsync
    {
        private readonly string _connectionString;
        private readonly TimeZoneInfo _timeZone;
        public SentenceRepositoryAsync(
            IConfiguration configuration,
            TimeZoneInfo timeZone)
        {
            _connectionString = configuration.GetConnectionString("WORD_DB");
            _timeZone = timeZone;
        }

        public async Task<List<Sentence>> GetAllSentences()
        {
            List<Sentence> sentences = new();

            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();

            string sql = @"
                    SELECT s.""id"", s.""dtCreated"", s.""dtAltered"",
                           sw.""id"", sw.""word"", sw.""position""
                    FROM public.""Sentence"" s
                    LEFT JOIN public.""SentenceWords"" sw ON s.""id"" = sw.""idSentence""
                    ORDER BY s.""id"", sw.""position"";";

            await using var cmd = new NpgsqlCommand(sql, conn);
            await using var reader = await cmd.ExecuteReaderAsync();

            Dictionary<int, Sentence> sentenceMap = new();

            while (await reader.ReadAsync())
            {
                int sentenceId = reader.GetInt32(0);

                if (!sentenceMap.ContainsKey(sentenceId))
                {
                    sentenceMap[sentenceId] = new Sentence
                    {
                        ID = sentenceId,
                        DTCreated = reader.GetDateTime(1),
                        DTAltered = reader.GetDateTime(2),
                        Words = new List<SentenceWord>()
                    };
                }

                if (!reader.IsDBNull(3)) // If there is a word row
                {
                    sentenceMap[sentenceId].Words.Add(new SentenceWord
                    {
                        ID = reader.GetInt32(3),
                        Word = reader.GetString(4),
                        Position = reader.GetInt32(5)
                    });
                }
            }

            return sentenceMap.Values.ToList();
        }

        public async Task<long> CreateSentence(List<SentenceWord> sentence)
        {
            await using var conn = new NpgsqlConnection(_connectionString);
            await conn.OpenAsync();
            await using var tran = await conn.BeginTransactionAsync();

            try
            {
                // Sentence table
                var insertSentenceSql = @"
                        INSERT INTO public.""Sentence"" (""dtCreated"", ""dtAltered"",""collection"")
                        VALUES (@Created, @Altered, @Collection)
                        RETURNING ""id"";";

                var now = TimeZoneInfo.ConvertTime(DateTime.UtcNow, _timeZone);
                string collection = string.Join(" ", sentence
                                          .OrderBy(w => w.Position)
                                          .Select(w => w.Word));

                long sentenceId;
                await using (var cmd = new NpgsqlCommand(insertSentenceSql, conn, tran))
                {
                    cmd.Parameters.AddWithValue("@Created", now);
                    cmd.Parameters.AddWithValue("@Altered", now);
                    cmd.Parameters.AddWithValue("@Collection", collection);
                    sentenceId = (long)(await cmd.ExecuteScalarAsync())!;
                }

                // SentenceWord table
                foreach (var word in sentence)
                {
                    var insertWordSql = @"
                        INSERT INTO public.""SentenceWords"" 
                        (""idSentence"", ""word"", ""position"")
                        VALUES (@SentenceId, @Word, @Position);";

                    await using var cmd = new NpgsqlCommand(insertWordSql, conn, tran);
                    cmd.Parameters.AddWithValue("@SentenceId", sentenceId);
                    cmd.Parameters.AddWithValue("@Word", word.Word);
                    cmd.Parameters.AddWithValue("@Position", word.Position);

                    await cmd.ExecuteNonQueryAsync();
                }

                await tran.CommitAsync();
                return sentenceId;
            }
            catch (Exception e)
            {
                await tran.RollbackAsync();
                return 0;
            }
        }
    }
}
