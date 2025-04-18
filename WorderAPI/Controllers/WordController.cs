using Microsoft.AspNetCore.Mvc;
using WorderAPI.Classes.Base;
using WorderAPI.Classes.Interfaces;
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
        // GET api/words/types
        [HttpGet("types")]
        public async Task<ActionResult<List<WordType>>> GetWordTypes()
        {
            var wordTypes = await _wordRepositoryAsync.GetWordTypes();
            if (wordTypes == null || wordTypes.Count == 0)
                return NotFound("No word types found.");
            return Ok(wordTypes);
        }
        // GET api/words/all
        [HttpGet("all")]
        public async Task<ActionResult<List<Word>>> GetAllWords()
        {
            var words = await _wordRepositoryAsync.GetAllWords();
            if(words == null || words.Count() == 0)
                return NotFound("No words found.");
            return Ok(words);
        }
        // GET api/words/id
        [HttpGet("{id}")]
        public async Task<ActionResult<Word>> GetWord(int id)
        {
            var word = await _wordRepositoryAsync.GetWord(id);
            if (word == null)
                return NotFound();
            return word;
        }
        // POST api/words
        [HttpPost]
        public async Task<ActionResult<Word>> CreateWord([FromBody] Word word)
        {
            var createdWord = await _wordRepositoryAsync.CreateWord(word);

            return CreatedAtAction(
                nameof(GetWord),
                new { id = createdWord.ID },
                createdWord
            );
        }
        // PATCH api/words/{id}
        [HttpPatch("{id}")]
        public async Task<ActionResult> EditWord(int id, [FromBody] Word word)
        {
            var affectedRows = await _wordRepositoryAsync.EditWord(word);
            if (affectedRows <= 0)
                return NotFound($"Word with id {word.ID} not found.");
            return NoContent();
        }
        // DELETE api/words/{id}
        [HttpDelete("{id}")]
        public async Task<ActionResult> DeleteWord(int id)
        {
            var affectedRows = await _wordRepositoryAsync.DeleteWord(id);
            if (affectedRows <= 0)
                return NotFound($"Word with id {id} not found.");
            return NoContent();
        }
    }
}
