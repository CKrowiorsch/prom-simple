using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using Prom_Simple.StandardWeb.Models;

namespace Prom_Simple.StandardWeb.Controllers
{
    public class HomeController : Controller
    {
        Random _random = new Random();
        SimplePrometheusTextSerializer<RuntimeState.State> _serializer;

        public HomeController()
        {
            _serializer = new SimplePrometheusTextSerializer<RuntimeState.State>().Initialize("test_project", new Dictionary<string, string>()
                {
                    {"environment", "staging" }
                }
            );
        }


        public ActionResult Index()
        {
            RuntimeState.Current.RequestCounterTotal += 1;
            RuntimeState.Current.RunningRequests = _random.Next(0, 100);
            return View();
        }

        public ActionResult About()
        {
            RuntimeState.Current.RequestCounterTotal += 1;
            RuntimeState.Current.RunningRequests = _random.Next(0, 100);
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            RuntimeState.Current.RequestCounterTotal += 1;
            RuntimeState.Current.RunningRequests = _random.Next(0, 100);
            ViewBag.Message = "Your contact page.";

            return View();
        }

        [HttpGet, Route("metrics")]
        public string Metrics()
        {
            return _serializer.Serialize(RuntimeState.Current);
        }
    }
}