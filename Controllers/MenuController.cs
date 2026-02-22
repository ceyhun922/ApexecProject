

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Localization;

namespace ApexWebAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MenuController : ControllerBase
    {
        private readonly IStringLocalizer<MenuController> _localizer;

        public MenuController(IStringLocalizer<MenuController> localizer)
        {
            _localizer = localizer;
        }

        [HttpGet("getmenu/{lang}")]
        public IActionResult GetMenu(string lang)
        {
            var supportedLanguages = new[] { "az", "tr" };
            if (!supportedLanguages.Contains(lang))
            {
                return BadRequest("Unsupported language");
            }

            var cultureInfo = new System.Globalization.CultureInfo(lang);
            System.Threading.Thread.CurrentThread.CurrentCulture = cultureInfo;
            System.Threading.Thread.CurrentThread.CurrentUICulture = cultureInfo;

            var menu = new
            {
                Home = _localizer["Home"],
                About = _localizer["About"],
                Contact = _localizer["Contact"]
            };

            return Ok(menu);
        }
    }
}

