using Microsoft.AspNetCore.Mvc;

namespace WorderAPI.Controllers
{
    [Route("api/sentences")]
    public class SentenceController : Controller
    {
        [Route("getsentences")]
        public async Task<IActionResult> GetPastSentences()
        {
            return Ok("Worder API - Sentences");
        }
    }
}
