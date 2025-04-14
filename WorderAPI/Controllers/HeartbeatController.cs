using Microsoft.AspNetCore.Mvc;

namespace WorderAPI.Controllers
{
    [Route("api")]
    public class HeartbeatController : ControllerBase
    {
        [Route("heartbeat")]
        public async Task<IActionResult> Heartbeat()
        {
            return Ok("Worder API - Heartbeat");
        }
    }
}
