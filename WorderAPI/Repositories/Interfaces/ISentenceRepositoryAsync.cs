using WorderAPI.Classes.Base;

namespace WorderAPI.Repositories.Interfaces
{
    public interface ISentenceRepositoryAsync
    {
        Task<List<Sentence>> GetAllSentences();
        Task<long> CreateSentence(List<SentenceWord> sentence);
    }
}
