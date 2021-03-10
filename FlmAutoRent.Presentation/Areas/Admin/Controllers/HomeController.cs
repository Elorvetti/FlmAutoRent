using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
using FlmAutoRent.Presentation.Areas.Admin.Models;
using FlmAutoRent.Services;

namespace FlmAutoRent.Presentation.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize]
    public class HomeController : Controller
    {
        private readonly IProfilingGroupServices _profilingGroupServices;

        public HomeController(IProfilingGroupServices profilingGroupServices){
            this._profilingGroupServices = profilingGroupServices;
        }

        public IActionResult Index()
        {
            return View();
        }
    }
}