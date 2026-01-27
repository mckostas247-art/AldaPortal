using Microsoft.AspNetCore.Mvc;

namespace PortalBase.Web.Controllers;

public class ServicesController : Controller
{
    public IActionResult Index()
    {
        return View();
    }
}


