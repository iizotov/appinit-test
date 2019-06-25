using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.IO;
using System.Runtime.Caching;

namespace app_init_test.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
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

        public ActionResult Init()
        {
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

        public ActionResult Uninit()
        {
            ObjectCache cache = MemoryCache.Default;
            CacheItemPolicy policy = new CacheItemPolicy
            {
                AbsoluteExpiration = DateTimeOffset.Now.AddYears(10)
            };
            cache.Remove("initialisation_dt");
            ViewBag.Message = "Caches invalidated";
            ViewBag.ComputerName = Environment.GetEnvironmentVariable("COMPUTERNAME");
            ViewBag.HostName = Environment.GetEnvironmentVariable("WEBSITE_HOSTNAME");
            ViewBag.StampName = Environment.GetEnvironmentVariable("WEBSITE_CURRENT_STAMPNAME");

            return View();
        }
    }
}