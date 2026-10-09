using EventMVC.Attributes;
using Microsoft.AspNetCore.Mvc;

namespace EventMVC.Controllers
{
    [UserOrAnonymousOnly]
    public class PagesController : Controller
    {
        // GET: About Us Page
        public IActionResult About()
        {
            return View();
        }
    }
}
