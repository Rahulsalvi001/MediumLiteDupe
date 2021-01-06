using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using MediumLiteDupe.Models;
using Microsoft.AspNetCore.Authorization;
using DataAccessLayer.Repository.IRepository;
using EntityModels;
using Microsoft.AspNetCore.Identity;

namespace MediumLiteDupe.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly IUnitOfWork _unitOfWork;
        private readonly UserManager<AppUser> _userManager;

        public HomeController(ILogger<HomeController> logger, IUnitOfWork unitOfWork, UserManager<AppUser> userManager)
        {
            _logger = logger;
            _unitOfWork = unitOfWork;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Dashoard()
        {
            List<StoryDashboard> model = new List<StoryDashboard>();
            model = _unitOfWork.Story.GetStoryForDashboard();
            
            return View(model);            
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
