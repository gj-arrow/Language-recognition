using System.Collections.Generic;
using System.Web;
using System.Web.Mvc;
using Microsoft.Owin.Security;
using Microsoft.AspNet.Identity.Owin;
using System.Threading.Tasks;
using Work.Models;
using Work.BLL.DTO;
using System.Security.Claims;
using Work.BLL.Interfaces;
using Work.BLL.Infrastructure;

namespace Work.Controllers
{
    public class AccountController : Controller
    {
        private IUserService UserService
        {
            get
            {
                return HttpContext.GetOwinContext().GetUserManager<IUserService>();
            }
        }

        private IAuthenticationManager AuthenticationManager
        {
            get
            {
                return HttpContext.GetOwinContext().Authentication;
            }
        }

        public ActionResult Login()
        {
            return View();
        }

        //[HttpPost]
        //[ValidateAntiForgeryToken]
        //public async Task<ActionResult> Login(LoginViewModel model)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
        //        ClaimsIdentity claim = await UserService.Authenticate(userDto);
        //        var user = UserService.FindUserByName(userDto.Email);
        //        if (claim == null && user == false)
        //        {
        //            OperationDetails operationDetails = await UserService.Create(userDto);
        //            if (operationDetails.Succedeed)
        //            {
        //                TempData["success"] = "Success!You were registered";
        //                ClaimsIdentity newclaim = await UserService.Authenticate(userDto);
        //                AuthenticationManager.SignOut();
        //                AuthenticationManager.SignIn(new AuthenticationProperties
        //                {
        //                    IsPersistent = true
        //                }, newclaim);
        //                return RedirectToAction("Index", "Home");
        //            }
        //            else ModelState.AddModelError("", operationDetails.Message);
        //        }
        //        else
        //        {
        //            AuthenticationManager.SignOut();
        //            AuthenticationManager.SignIn(new AuthenticationProperties
        //            {
        //                IsPersistent = true
        //            }, claim);
        //            return RedirectToAction("Index", "Home");
        //        }
        //    }
        //    return View(model);
        //}



        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO { Email = model.Email, Password = model.Password };
                ClaimsIdentity claim = await UserService.Authenticate(userDto);
                var user = UserService.FindUserByName(userDto.Email);
                if (claim == null && user == false)
                {
                    RegisterModel registerUser = new RegisterModel { Email = model.Email, Password = model.Password};
                    return RedirectToAction("Register", "Account", registerUser);
                }
                else if (claim == null) ModelState.AddModelError("", "Invalid Password.");
                else
                {
                    AuthenticationManager.SignOut();
                    AuthenticationManager.SignIn(new AuthenticationProperties
                    {
                        IsPersistent = true
                    }, claim);
                    return RedirectToAction("Index", "Home");
                }
            }
            return View(model);
        }




        public ActionResult Register(RegisterModel model,bool param = true)
        {
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> Register(RegisterModel model)
        {
            if (ModelState.IsValid)
            {
                UserDTO userDto = new UserDTO
                {
                    Email = model.Email,
                    Password = model.Password
                };
                OperationDetails operationDetails = await UserService.Create(userDto);
                TempData["success"] = "Success!You were registered";
                ClaimsIdentity newclaim = await UserService.Authenticate(userDto);
                AuthenticationManager.SignOut();
                AuthenticationManager.SignIn(new AuthenticationProperties
                {
                    IsPersistent = true
                }, newclaim);
                return RedirectToAction("Index", "Home");
            }
            return View(model);
        }




        public ActionResult LogOff()
        {
            AuthenticationManager.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}