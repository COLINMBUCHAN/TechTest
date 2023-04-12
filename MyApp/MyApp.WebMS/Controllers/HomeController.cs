using System.Web.Mvc;
using MyApp.Services.Factories.Interfaces;
using MyApp.WebMS.Controllers.Base;

namespace MyApp.WebMS.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IServiceFactory serviceFactory) : base(serviceFactory) { }

        [Route("", Name = "Home")]
        public ActionResult Index()
        {
            return View();
        }

        //[Route("2", Name = "LogEntriesList")]
        //public ActionResult Index2()
        //{
        //    return View("Index");
        //}
    }
}