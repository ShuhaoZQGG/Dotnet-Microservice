using Microsoft.AspNetCore.Mvc;

namespace PlatformService.Controllers
{
  [ApiController]
  [Route("")]
  public class HomePageController : ControllerBase
  {
    [HttpGet]
    public string ShowHomePage()
    {
      return "Hello, Welcome to Dotnet Microservices";
    }
  }
}
