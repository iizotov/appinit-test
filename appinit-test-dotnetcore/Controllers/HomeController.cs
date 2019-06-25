using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using appinit_test_dotnetcore.Models;
using System.Runtime.Caching;

namespace appinit_test_dotnetcore.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            ViewBag.Title = "Home Page.";
            ObjectCache cache = MemoryCache.Default;
            string initialisationDt = cache["initialisation_dt"] as String;
            if (initialisationDt == null)
            {
                ViewBag.Message = "Initialisation has not been completed";
            }
            else
            {
                ViewBag.Message = "Initialisation completed at " + initialisationDt + " UTC";
            }
            ViewBag.ComputerName = Environment.GetEnvironmentVariable("COMPUTERNAME");
            ViewBag.HostName = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
            ViewBag.StampName = Environment.GetEnvironmentVariable("WEBSITE_CURRENT_STAMPNAME");

            return View();
        }

        public IActionResult Init()
        {
            ViewBag.Title = "Init Page.";
            System.Threading.Thread.Sleep(5 * 1000);
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddYears(10)
            };
            cache.Set("initialisation_dt", DateTime.UtcNow.ToString(), policy);
            ViewBag.Message = "Caches initialised";
            ViewBag.ComputerName = Environment.GetEnvironmentVariable("COMPUTERNAME");
            ViewBag.HostName = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
            ViewBag.StampName = Environment.GetEnvironmentVariable("WEBSITE_CURRENT_STAMPNAME");

            return View();
        }
        public IActionResult Uninit()
        {
            ViewBag.Title = "Uninit Page.";
            ObjectCache cache = MemoryCache.Default;
            cache.Remove("initialisation_dt");
            ViewBag.Message = "Caches invalidated";
            ViewBag.ComputerName = Environment.GetEnvironmentVariable("COMPUTERNAME");
            ViewBag.HostName = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
            ViewBag.StampName = Environment.GetEnvironmentVariable("WEBSITE_CURRENT_STAMPNAME");

            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
