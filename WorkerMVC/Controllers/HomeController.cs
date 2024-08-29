using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using WorkerMVC.Models;
using WorkerService.Models;
using WorkerService.Services;

namespace WorkerMVC.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly mvcService _mvcService;
        public HomeController(ILogger<HomeController> logger, mvcService mvcService)
        {
            _logger = logger;
            _mvcService = mvcService;
        }

        public IActionResult Index()
        {
            List<User> users = _mvcService.GetUsers();
            UserViewModel userViewModel = new UserViewModel() { Users = users};
            return View(userViewModel);
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
}
