using WorderAPI.Classes;

namespace WorderAPI.Interfaces
{
    public interface IWordRepositoryAsync 
    {
        Task<List<WordType>> GetWordTypes();
    }
}
