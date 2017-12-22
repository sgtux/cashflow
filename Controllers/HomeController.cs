using Microsoft.AspNetCore.Mvc;

namespace Controllers
{
  public class HomeController : Controller
  {
    public IActionResult Index()
    {
      return View();
    }

    public IActionResult Detail()
    {
      return View();
    }

    public IActionResult List()
    {
      return View();
    }
  }
}