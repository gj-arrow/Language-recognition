using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.AspNet.Identity;
using Work.BLL.DTO;
using Work.BLL.Interfaces;
using Work.Models;
using Work.App_Start;
using AutoMapper;

namespace Work.Controllers
{
    [Authorize]
    public class HomeController : Controller
    {
        IMapper _mapper;

        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private ILanguageService LanguageService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<ILanguageService>();
            }
        }

        private IGenerateData GenerateDataService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IGenerateData>();
            }
        }

        public HomeController()
        {
            _mapper = AutoMapperConfig.MapperConfiguration.CreateMapper();
        }

        public ActionResult Index()
        {
            return View();
        }

        public PartialViewResult GetTopUsers()
        {
            List<TopUserViewModel> topUsers = _mapper.Map<List<TopUserDTO>, List<TopUserViewModel>>(UserService.TopUsers());
            return PartialView("_TopUsers", topUsers);
        }


        [HttpGet]
        public JsonResult Translator(string word)
        {
            Dictionary<string, string> dict = LanguageService.DetectedLanguage(word, User.Identity.GetUserId());
            return Json(dict, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Success()
        {
            GenerateDataService.GeneratingDataDatabase();
            return View();
        }
    }
}