using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using HCBPCoreUI_Backend.Models;

namespace HCBPCoreUI_Backend.Controllers;

public class HomeController : Controller
{
  private readonly ILogger<HomeController> _logger;

  public HomeController(ILogger<HomeController> logger)
  {
    _logger = logger;
  }

  public IActionResult Index()
  {
    return View();
  }

  public IActionResult Budget()
  {
    return View();
  }

  public IActionResult BudgetPlanning()
  {
    return View();
  }
  public IActionResult Privacy()
  {
    return View();
  }

  [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
  public IActionResult Error()
  {
    return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
  }
}
