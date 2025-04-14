using Microsoft.AspNetCore.Mvc;
using WorderAPI.Classes;
using WorderAPI.Interfaces;
using WorderAPI.Repositories;

namespace WorderAPI.Controllers
{
    [Route("api/words")]
    [ApiController]
    public class WordController : ControllerBase
    {
        private readonly IWordRepositoryAsync _wordRepositoryAsync;

        public WordController(IWordRepositoryAsync wordRepositoryAsync)
        {
            _wordRepositoryAsync = wordRepositoryAsync;
        }

        //[Route("getwords")]
        //public async Task<List<Word>> GetAllWords()
        //{            
        //    return await _wordRepositoryAsync.GetWordTypes();
        //}

        [Route("getwordtypes")]
        public async Task<List<WordType>> GetWordTypes()
        {
            return await _wordRepositoryAsync.GetWordTypes();
        }
    }
}
