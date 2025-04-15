using WorderAPI.Classes.Base;
using WorderAPI.Classes.Interfaces;

namespace WorderAPI.Interfaces
{
    public interface IWordRepositoryAsync 
    {
        Task<int> CreateWord(IWord word);
        Task<List<WordType>> GetWordTypes();
    }
}
