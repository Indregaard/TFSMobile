using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using System.Web.Mvc;
using TfsMobile.Repositories.v1;
using TfsWebClient.Models;

namespace TfsWebClient.Controllers
{
    public class BuildsController : Controller
    {
        public ActionResult Index()
        {
            var rep = new BuildsRepository();
            var result = rep.GetBuilds();
            return View(new BuildsVm(result));
        }

        //public ActionResult About()
        //{
        //    ViewBag.Message = "Your application description page.";

        //    return View();
        //}

        //public ActionResult Contact()
        //{
        //    ViewBag.Message = "Your contact page.";

        //    return View();
        //}
    }
}