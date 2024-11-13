using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using TSchedule.Web.Models;

namespace TSchedule.Web.Controllers;

public class HomeController : Controller
{
    public IActionResult Index() => View();

    [Route("/install")]
    public IActionResult Install() => View();

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
        => View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
}
