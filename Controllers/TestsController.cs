using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TestsController : ControllerBase
    {
        private readonly IStringLocalizer<TestsController> _localizer;

        public TestsController(IStringLocalizer<TestsController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet("{lang}")]
        public IActionResult Test(string lang)
        {
            var message = _localizer["TestMessage"];
            return Ok(new { message });
        }
    }
}
