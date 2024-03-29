﻿using Microsoft.Graph;
using Microsoft.Identity.Client;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
namespace OPDCLAIMFORM.Controllers
{
    public class HomeController : Controller
    {

        public ActionResult Index()
        {
            if (Request.IsAuthenticated)
            {           
               
                string userName = ClaimsPrincipal.Current.FindFirst("name").Value;
            
                string userId = ClaimsPrincipal.Current.FindFirst(ClaimTypes.NameIdentifier).Value;

                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userId))
                {
                    // Invalid principal, sign out
                    return RedirectToAction("SignOut", "OFFICEAPIMANAGER", null); 
                }

                // Since we cache tokens in the session, if the server restarts
                // but the browser still has a cached cookie, we may be
                // authenticated but not have a valid token cache. Check for this
                // and force signout.
                SessionTokenCache tokenCache = new SessionTokenCache(userId, HttpContext);
                if (!tokenCache.HasData())
                {
                    // Cache is empty, sign out
                    return RedirectToAction("SignOut","OFFICEAPIMANAGER",null);
                }         



                string emailAddress = GetEmailAddress();

                List<string> HRList = ConfigurationManager.AppSettings["HR:List"].Split(',').ToList<string>();

                List<string> FINList = ConfigurationManager.AppSettings["FIN:List"].Split(',').ToList<string>();

                List<string> MANList = ConfigurationManager.AppSettings["MAN:List"].Split(',').ToList<string>();

                List<string> GENList = ConfigurationManager.AppSettings["GEN:List"].Split(',').ToList<string>();

                if (HRList.Contains(emailAddress))
                {
                    ViewBag.RollType = "HR";
                }
                else if (FINList.Contains(emailAddress))
                {
                    ViewBag.RollType = "FIN";
                }
                else if (MANList.Contains(emailAddress))
                {
                    ViewBag.RollType = "MAN";
                }
                else if (GENList.Contains(emailAddress))
                {
                    ViewBag.RollType = "GEN";
                }

                ViewBag.UserName = userName;
            }


            //OFFICEAPIMANAGERController managerController = new OFFICEAPIMANAGERController();
            //var emailAddressrrr = managerController.GetMemberGroup(HttpContext);
                       
            return View();
        }

        private string GetEmailAddress()
        {
            OFFICEAPIMANAGERController managerController = new OFFICEAPIMANAGERController();
            string emailAddress = managerController.GetEmailAddress();

            return emailAddress;
        }


        public ActionResult About()
        {
            return View();
        }

    }



}