using Microsoft.AspNetCore.Mvc;

namespace CommandService.Controllers
{
  [ApiController]
  [Route("")]
  public class HomeController : ControllerBase
  {
    [HttpGet]
    public string Welcome()
    {
      return "Welcome to Command Service";
    }
  }
}
