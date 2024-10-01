using Microsoft.AspNetCore.Mvc;

namespace LinkTic_Test_Back.Presentation.Controllers
{
    public class ProductController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
