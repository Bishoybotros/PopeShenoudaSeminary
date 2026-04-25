using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    public IActionResult Index()
    {
        return View();
    }

    public IActionResult About()
    {
        return View();
    }
    public IActionResult Teachers()
    {
        return View();
    }
    public IActionResult Student()
    {
        return View();
    }
}
