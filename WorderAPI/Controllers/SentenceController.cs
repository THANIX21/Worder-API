using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using WorderAPI.Classes.Base;
using WorderAPI.Repositories.Interfaces;

namespace WorderAPI.Controllers
{
    [Route("api/sentences")]
    [ApiController]
    public class SentenceController : ControllerBase
    {
        private readonly ISentenceRepositoryAsync _sentenceRepositoryAsync;
        public SentenceController(ISentenceRepositoryAsync sentenceRepositoryAsync)
        {
            _sentenceRepositoryAsync = sentenceRepositoryAsync;
        }
        // GET api/sentences/all
        [HttpGet("all")]
        public async Task<ActionResult<List<Sentence>>> GetAllSentences()
        {
            var sentences = await _sentenceRepositoryAsync.GetAllSentences();
            if (sentences == null || sentences.Count() == 0)
                return NotFound("No sentences found.");
            return Ok(sentences);
        }
        //// GET api/sentences/id
        //[HttpGet("{id}")]
        //public async Task<ActionResult<List<Word>>> GetSentence()
        //{
        //    var words = await _wordRepositoryAsync.GetAllWords();
        //    if (words == null || words.Count() == 0)
        //        return NotFound("No words found.");
        //    return Ok(words);
        //}

        // POST api/sentences
        [HttpPost]
        public async Task<ActionResult> CreateSentence([FromBody] List<SentenceWord> sentence)
        {
            var result = await _sentenceRepositoryAsync.CreateSentence(sentence);
            return Ok(result);
        }
        //// PATCH api/sentences/{id}
        //[HttpPatch("{id}")]
        //public async Task<ActionResult> EditSentence(int id, [FromBody] Word word)
        //{
        //    var affectedRows = await _wordRepositoryAsync.EditWord(word);
        //    if (affectedRows <= 0)
        //        return NotFound($"Word with id {word.ID} not found.");
        //    return NoContent();
        //}
        //// DELETE api/sentences/{id}
        //[HttpDelete("{id}")]
        //public async Task<ActionResult> DeleteSentence(int id)
        //{
        //    var affectedRows = await _wordRepositoryAsync.DeleteWord(id);
        //    if (affectedRows <= 0)
        //        return NotFound($"Word with id {id} not found.");
        //    return NoContent();
        //}
    }
}
