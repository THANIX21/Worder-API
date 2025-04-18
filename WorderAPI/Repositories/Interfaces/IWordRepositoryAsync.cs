using WorderAPI.Classes.Base;
using WorderAPI.Classes.Interfaces;

namespace WorderAPI.Repositories
{
    public interface IWordRepositoryAsync 
    {
        Task<List<Word>> GetAllWords();
        Task<Word> GetWord(int id);
        Task<Word> CreateWord(IWord word);
        Task<int> EditWord(IWord word);
        Task<int> DeleteWord(int id);
        Task<List<WordType>> GetWordTypes();
    }
}
