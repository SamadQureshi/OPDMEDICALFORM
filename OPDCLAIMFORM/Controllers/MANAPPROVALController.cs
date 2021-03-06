﻿using OPDCLAIMFORM.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using PagedList;
using NLog;
using System.Configuration;

namespace OPDCLAIMFORM.Controllers
{
    public class MANAPPROVALController : Controller
    {
        private readonly MedicalInfoEntities db = new MedicalInfoEntities();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        // GET: OPDEXPENSEs
        public ActionResult Index(string sortOrder, string currentFilter, string searchString, int? page)
        {
            try
            {

                if (Request.IsAuthenticated)
                {
                    AuthenticateUser();
                    ViewBag.CurrentSort = sortOrder;
                    ViewBag.EmployeeNameSortParm = String.IsNullOrEmpty(sortOrder) ? "EmployeeName_desc" : "";
                    ViewBag.ClaimForMonthSortParm = String.IsNullOrEmpty(sortOrder) ? "ClaimForMonth_desc" : "";
                    ViewBag.StatusSortParm = String.IsNullOrEmpty(sortOrder) ? "Status_desc" : "";
                    ViewBag.OPDTypeSortParm = String.IsNullOrEmpty(sortOrder) ? "OPDType_desc" : "";
                    ViewBag.ExpenseNumberSortParm = String.IsNullOrEmpty(sortOrder) ? "ExpenseNumber_desc" : "";
                    if (searchString != null)
                    {
                        page = 1;
                    }
                    else
                    {
                        searchString = currentFilter;
                    }
                    ViewBag.CurrentFilter = searchString;

                    string emailAddress = GetEmailAddress();
                    //&& e.MANAGEMENT_EMAILADDRESS == emailAddress

                    var opdExp = db.OPDEXPENSEs.Where(e => e.STATUS == "FINApproved" || e.STATUS == "MANApproved" || e.STATUS == "MANRejected" || e.STATUS == "MANInProcess");
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        opdExp = opdExp.Where(s => s.EXPENSE_NUMBER.Contains(searchString));
                    }
                    switch (sortOrder)
                    {
                        case "EmployeeName_desc":
                            opdExp = opdExp.OrderBy(s => s.EMPLOYEE_NAME);
                            ViewBag.EmployeeNameSortParm = "EmployeeName_asc";
                            break;
                        case "ClaimForMonth_desc":
                            opdExp = opdExp.OrderBy(s => s.CLAIM_MONTH);
                            ViewBag.ClaimForMonthSortParm = "ClaimForMonth_asc";
                            break;
                        case "Status_desc":
                            opdExp = opdExp.OrderBy(s => s.STATUS);
                            ViewBag.StatusSortParm = "Status_asc";
                            break;
                        case "OPDType_desc":
                            opdExp = opdExp.OrderBy(s => s.OPDTYPE);
                            ViewBag.OPDTypeSortParm = "OPDType_asc";
                            break;
                        case "ExpenseNumber_desc":
                            opdExp = opdExp.OrderBy(s => s.EXPENSE_NUMBER);
                            ViewBag.ExpenseNumberSortParm = "ExpenseNumber_asc";
                            break;
                        case "EmployeeName_asc":
                            opdExp = opdExp.OrderByDescending(s => s.EMPLOYEE_NAME);
                            break;
                        case "ClaimForMonth_asc":
                            opdExp = opdExp.OrderByDescending(s => s.CLAIM_MONTH);
                            break;
                        case "Status_asc":
                            opdExp = opdExp.OrderByDescending(s => s.STATUS);
                            break;
                        case "OPDType_asc":
                            opdExp = opdExp.OrderByDescending(s => s.OPDTYPE);
                            break;
                        case "ExpenseNumber_asc":
                            opdExp = opdExp.OrderByDescending(s => s.EXPENSE_NUMBER);
                            break;
                        default:  // Name ascending 
                            opdExp = opdExp.OrderBy(s => s.EXPENSE_NUMBER);
                            break;
                    }

                    int pageSize = Convert.ToInt32(ConfigurationManager.AppSettings["PageSize"]);
                    int pageNumber = (page ?? 1);
                    return View(opdExp.ToPagedList(pageNumber, pageSize));

                    //return View(opdExp);

                    // return View(db.OPDEXPENSEs.Where(e => e.STATUS == "FINApproved" || e.STATUS == "MANApproved" || e.STATUS == "MANRejected").ToList());
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }

            }
            catch (Exception ex)
            {

                logger.Error("MANAPPROVAL : Index()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
        }

        // GET: OPDEXPENSEs/Details/5
        public ActionResult DetailsForOPDExpense(int? id)
        {
            MedicalInfoEntities entities = new MedicalInfoEntities();
            try
            {
                if (Request.IsAuthenticated)
                {
                    AuthenticateUser();
                    if (id == null)
                    {
                        return RedirectToAction("Index", "MANAPPROVAL");
                    }

                    var result2 = new OPDEXPENSE_MASTERDETAIL()
                    {
                        listOPDEXPENSEPATIENT = entities.OPDEXPENSE_PATIENT.Where(e => e.OPDEXPENSE_ID == id).ToList(),
                        listOPDEXPENSEIMAGE = entities.OPDEXPENSE_IMAGE.Where(e => e.OPDEXPENSE_ID == id).ToList(),
                        opdEXPENSE = entities.OPDEXPENSEs.Where(e => e.OPDEXPENSE_ID == id).FirstOrDefault()

                    };
                    //RedirectToAction("Index",);
                    return View(result2);
                }

                else
                {
                    return RedirectToAction("Index", "MANAPPROVAL");
                }
            }
            catch (Exception ex)
            {

                logger.Error("MANAPPROVAL : DetailsForOPDExpense()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
        }



        public ActionResult DetailsForHospitalExpense(int? id)
        {
            MedicalInfoEntities entities = new MedicalInfoEntities();
            try
            {
                if (Request.IsAuthenticated)
                {
                    AuthenticateUser();
                    if (id == null)
                    {
                        return RedirectToAction("Index", "MANAPPROVAL");
                    }

                    OPDEXPENSE oPDEXPENSE = db.OPDEXPENSEs.Find(id);

                    var result2 = new HOSPITALEXPENSE_MASTERDETAIL()
                    {
                        listOPDEXPENSEPATIENT = entities.OPDEXPENSE_PATIENT.Where(e => e.OPDEXPENSE_ID == id).ToList(),
                        listOPDEXPENSEIMAGE = entities.OPDEXPENSE_IMAGE.Where(e => e.OPDEXPENSE_ID == id).ToList(),
                        OPDEXPENSE_ID = oPDEXPENSE.OPDEXPENSE_ID,
                        CLAIMANT_SUFFERED_ILLNESS = oPDEXPENSE.CLAIMANT_SUFFERED_ILLNESS,
                        CLAIMANT_SUFFERED_ILLNESS_DETAILS = oPDEXPENSE.CLAIMANT_SUFFERED_ILLNESS_DETAILS,
                        CLAIMANT_SUFFERED_ILLNESS_DATE = oPDEXPENSE.CLAIMANT_SUFFERED_ILLNESS_DATE,
                        DATE_ILLNESS_NOTICED = oPDEXPENSE.DATE_ILLNESS_NOTICED,
                        DATE_RECOVERY = oPDEXPENSE.DATE_RECOVERY,
                        DIAGNOSIS = oPDEXPENSE.DIAGNOSIS,
                        DOCTOR_NAME = oPDEXPENSE.DOCTOR_NAME,
                        DRUGS_PRESCRIBED_BOOL = oPDEXPENSE.DRUGS_PRESCRIBED_BOOL,
                        DRUGS_PRESCRIBED_DESCRIPTION = oPDEXPENSE.DRUGS_PRESCRIBED_DESCRIPTION,

                        EMPLOYEE_DEPARTMENT = oPDEXPENSE.EMPLOYEE_DEPARTMENT,
                        EMPLOYEE_NAME = oPDEXPENSE.EMPLOYEE_NAME,
                        EMPLOYEE_EMAILADDRESS = oPDEXPENSE.EMPLOYEE_EMAILADDRESS,

                        FINANCE_APPROVAL = oPDEXPENSE.FINANCE_APPROVAL,
                        FINANCE_COMMENT = oPDEXPENSE.FINANCE_COMMENT,
                        FINANCE_NAME = oPDEXPENSE.FINANCE_NAME,
                        FINANCE_APPROVAL_DATE = oPDEXPENSE.FINANCE_APPROVAL_DATE,
                        FINANCE_EMAILADDRESS = oPDEXPENSE.FINANCE_EMAILADDRESS,


                        HOSPITAL_NAME = oPDEXPENSE.HOSPITAL_NAME,

                        HR_APPROVAL = oPDEXPENSE.HR_APPROVAL,
                        HR_COMMENT = oPDEXPENSE.HR_COMMENT,
                        HR_NAME = oPDEXPENSE.HR_NAME,
                        HR_APPROVAL_DATE = oPDEXPENSE.HR_APPROVAL_DATE,
                        HR_EMAILADDRESS = oPDEXPENSE.HR_EMAILADDRESS,


                        MANAGEMENT_APPROVAL = oPDEXPENSE.MANAGEMENT_APPROVAL,
                        MANAGEMENT_COMMENT = oPDEXPENSE.MANAGEMENT_COMMENT,
                        MANAGEMENT_NAME = oPDEXPENSE.MANAGEMENT_NAME,
                        MANAGEMENT_APPROVAL_DATE = oPDEXPENSE.MANAGEMENT_APPROVAL_DATE,
                        MANAGEMENT_EMAILADDRESS = oPDEXPENSE.MANAGEMENT_EMAILADDRESS,


                        PERIOD_CONFINEMENT_DATE_FROM = oPDEXPENSE.PERIOD_CONFINEMENT_DATE_FROM,
                        PERIOD_CONFINEMENT_DATE_TO = oPDEXPENSE.PERIOD_CONFINEMENT_DATE_TO,
                        STATUS = oPDEXPENSE.STATUS,
                        TOTAL_AMOUNT_CLAIMED = oPDEXPENSE.TOTAL_AMOUNT_CLAIMED,
                        CLAIM_YEAR = oPDEXPENSE.CLAIM_YEAR,
                        TOTAL_AMOUNT_APPROVED = oPDEXPENSE.TOTAL_AMOUNT_APPROVED,
                        CREATED_DATE = oPDEXPENSE.CREATED_DATE,
                        MODIFIED_DATE = oPDEXPENSE.MODIFIED_DATE

                    };

                    return View(result2);
                }

                else
                {
                    return RedirectToAction("Index", "MANAPPROVAL");
                }
            }
            catch (Exception ex)
            {

                logger.Error("MANAPPROVAL : DetailsForHospitalExpense()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
        }


        // GET: OPDEXPENSEs/Edit/5
        public ActionResult MANOPDExpense(int? id)
        {
            try
            {

                if (Request.IsAuthenticated)
                {

                    AuthenticateUser();
                    if (id == null)
                    {
                        return RedirectToAction("Index", "MANAPPROVAL");
                    }
                    MedicalInfoEntities entities = new MedicalInfoEntities();
                    var result2 = new OPDEXPENSE_MASTERDETAIL()
                    {
                        listOPDEXPENSEPATIENT = entities.OPDEXPENSE_PATIENT.Where(e => e.OPDEXPENSE_ID == id).ToList(),
                        listOPDEXPENSEIMAGE = entities.OPDEXPENSE_IMAGE.Where(e => e.OPDEXPENSE_ID == id).ToList(),
                        opdEXPENSE = entities.OPDEXPENSEs.Where(e => e.OPDEXPENSE_ID == id).FirstOrDefault()

                    };

                    ViewData["OPDEXPENSE_ID"] = id;
                    return View(result2);
                }

                else
                {
                    return RedirectToAction("Index", "MANAPPROVAL");
                }

            }
            catch (Exception ex)
            {

                logger.Error("MANAPPROVAL : MANOPDExpense()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
        }

        // POST: OPDEXPENSEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MANOPDExpense([Bind(Include = "OPDEXPENSE_ID,EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,CLAIM_MONTH,CLAIM_YEAR,TOTAL_AMOUNT_CLAIMED,STATUS,OPDTYPE,HR_COMMENT,HR_APPROVAL_DATE,HR_APPROVAL,HR_NAME,FINANCE_COMMENT,FINANCE_APPROVAL,FINANCE_APPROVAL_DATE,FINANCE_NAME,MANAGEMENT_COMMENT,MANAGEMENT_APPROVAL,MANAGEMENT_APPROVAL_DATE,MANAGEMENT_NAME,TOTAL_AMOUNT_APPROVED,MODIFIED_DATE,EMPLOYEE_EMAILADDRESS,FINANCE_EMAILADDRESS,HR_EMAILADDRESS,MANAGEMENT_EMAILADDRESS,CREATED_DATE")] OPDEXPENSE oPDEXPENSE)
        {
            try
            {

                string buttonStatus = Request.Form["buttonName"];

                if (buttonStatus == "approved")
                {
                    oPDEXPENSE.STATUS = "MANApproved";

                    if (oPDEXPENSE.TOTAL_AMOUNT_APPROVED.ToString() == "")
                    {
                        ModelState.AddModelError("", "Please Add Total Approved Amount");
                    }


                }
                else if (buttonStatus == "rejected")
                {
                    oPDEXPENSE.STATUS = "MANRejected";

                    if (oPDEXPENSE.HR_COMMENT == null)
                    {
                        ModelState.AddModelError("", "Please Add MAN Comments");
                    }
                }
                else
                {
                    oPDEXPENSE.STATUS = "MANInProcess";
                }

                if (ModelState.IsValid)
                {
                    oPDEXPENSE.MODIFIED_DATE = DateTime.Now;
                    oPDEXPENSE.MANAGEMENT_APPROVAL_DATE = DateTime.Now;
                    oPDEXPENSE.MANAGEMENT_EMAILADDRESS = GetEmailAddress();
                    if (oPDEXPENSE.STATUS == "MANApproved")
                    {
                        oPDEXPENSE.HR_APPROVAL = true;
                        oPDEXPENSE.FINANCE_APPROVAL = true;
                        oPDEXPENSE.MANAGEMENT_APPROVAL = true;
                    }


                    db.Entry(oPDEXPENSE).State = EntityState.Modified;
                    db.SaveChanges();

                }
                return RedirectToAction("Index", "MANAPPROVAL");

            }
            catch (Exception ex)
            {

                logger.Error("MANAPPROVAL : MANOPDExpense([Bind])" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
        }





        // GET: OPDEXPENSEs/Edit/5
        public ActionResult MANHospitalExpense(int? id)
        {
            try
            {

                if (Request.IsAuthenticated)
                {
                    AuthenticateUser();
                    if (id == null)
                    {
                        return RedirectToAction("Index", "MANAPPROVAL");
                    }

                    MedicalInfoEntities entities = new MedicalInfoEntities();
                    OPDEXPENSE oPDEXPENSE = db.OPDEXPENSEs.Find(id);

                    var result2 = new HOSPITALEXPENSE_MASTERDETAIL()
                    {
                        listOPDEXPENSEPATIENT = entities.OPDEXPENSE_PATIENT.Where(e => e.OPDEXPENSE_ID == id).ToList(),
                        listOPDEXPENSEIMAGE = entities.OPDEXPENSE_IMAGE.Where(e => e.OPDEXPENSE_ID == id).ToList(),
                        OPDEXPENSE_ID = oPDEXPENSE.OPDEXPENSE_ID,
                        CLAIMANT_SUFFERED_ILLNESS = oPDEXPENSE.CLAIMANT_SUFFERED_ILLNESS,
                        CLAIMANT_SUFFERED_ILLNESS_DETAILS = oPDEXPENSE.CLAIMANT_SUFFERED_ILLNESS_DETAILS,
                        CLAIMANT_SUFFERED_ILLNESS_DATE = oPDEXPENSE.CLAIMANT_SUFFERED_ILLNESS_DATE,


                        DATE_ILLNESS_NOTICED = oPDEXPENSE.DATE_ILLNESS_NOTICED,
                        DATE_RECOVERY = oPDEXPENSE.DATE_RECOVERY,
                        DIAGNOSIS = oPDEXPENSE.DIAGNOSIS,
                        DOCTOR_NAME = oPDEXPENSE.DOCTOR_NAME,
                        DRUGS_PRESCRIBED_BOOL = oPDEXPENSE.DRUGS_PRESCRIBED_BOOL,
                        DRUGS_PRESCRIBED_DESCRIPTION = oPDEXPENSE.DRUGS_PRESCRIBED_DESCRIPTION,

                        EMPLOYEE_DEPARTMENT = oPDEXPENSE.EMPLOYEE_DEPARTMENT,
                        EMPLOYEE_NAME = oPDEXPENSE.EMPLOYEE_NAME,
                        EMPLOYEE_EMAILADDRESS = oPDEXPENSE.EMPLOYEE_EMAILADDRESS,

                        FINANCE_APPROVAL = oPDEXPENSE.FINANCE_APPROVAL,
                        FINANCE_COMMENT = oPDEXPENSE.FINANCE_COMMENT,
                        FINANCE_NAME = oPDEXPENSE.FINANCE_NAME,
                        FINANCE_APPROVAL_DATE = oPDEXPENSE.FINANCE_APPROVAL_DATE,
                        FINANCE_EMAILADDRESS = oPDEXPENSE.FINANCE_EMAILADDRESS,


                        HOSPITAL_NAME = oPDEXPENSE.HOSPITAL_NAME,

                        HR_APPROVAL = oPDEXPENSE.HR_APPROVAL,
                        HR_COMMENT = oPDEXPENSE.HR_COMMENT,
                        HR_NAME = oPDEXPENSE.HR_NAME,
                        HR_APPROVAL_DATE = oPDEXPENSE.HR_APPROVAL_DATE,
                        HR_EMAILADDRESS = oPDEXPENSE.HR_EMAILADDRESS,


                        MANAGEMENT_APPROVAL = oPDEXPENSE.MANAGEMENT_APPROVAL,
                        MANAGEMENT_COMMENT = oPDEXPENSE.MANAGEMENT_COMMENT,
                        MANAGEMENT_NAME = oPDEXPENSE.MANAGEMENT_NAME,
                        MANAGEMENT_APPROVAL_DATE = oPDEXPENSE.MANAGEMENT_APPROVAL_DATE,
                        MANAGEMENT_EMAILADDRESS = oPDEXPENSE.MANAGEMENT_EMAILADDRESS,

                        PERIOD_CONFINEMENT_DATE_FROM = oPDEXPENSE.PERIOD_CONFINEMENT_DATE_FROM,
                        PERIOD_CONFINEMENT_DATE_TO = oPDEXPENSE.PERIOD_CONFINEMENT_DATE_TO,
                        STATUS = oPDEXPENSE.STATUS,
                        OPDTYPE = oPDEXPENSE.OPDTYPE,
                        TOTAL_AMOUNT_CLAIMED = oPDEXPENSE.TOTAL_AMOUNT_CLAIMED,
                        CLAIM_YEAR = oPDEXPENSE.CLAIM_YEAR,
                        TOTAL_AMOUNT_APPROVED = oPDEXPENSE.TOTAL_AMOUNT_APPROVED,
                        CREATED_DATE = oPDEXPENSE.CREATED_DATE,
                        MODIFIED_DATE = oPDEXPENSE.MODIFIED_DATE




                    };

                    ViewData["OPDEXPENSE_ID"] = id;
                    return View(result2);
                }

                else
                {
                    return RedirectToAction("Index", "MANAPPROVAL");
                }

            }
            catch (Exception ex)
            {

                logger.Error("MANAPPROVAL : MANHospitalExpense()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }

        }

        // POST: OPDEXPENSEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult MANHospitalExpense([Bind(Include = "OPDEXPENSE_ID,EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,CLAIM_MONTH,TOTAL_AMOUNT_CLAIMED,DATE_ILLNESS_NOTICED,DATE_RECOVERY,DIAGNOSIS,CLAIMANT_SUFFERED_ILLNESS,CLAIMANT_SUFFERED_ILLNESS_DATE,CLAIMANT_SUFFERED_ILLNESS_DETAILS,HOSPITAL_NAME,DOCTOR_NAME,PERIOD_CONFINEMENT_DATE_FROM,PERIOD_CONFINEMENT_DATE_TO,DRUGS_PRESCRIBED_BOOL,DRUGS_PRESCRIBED_DESCRIPTION,OPDTYPE,STATUS,HR_COMMENT,HR_APPROVAL,HR_APPROVAL_DATE,HR_NAME,FINANCE_COMMENT,FINANCE_APPROVAL,FINANCE_APPROVAL_DATE,FINANCE_NAME,MANAGEMENT_COMMENT,MANAGEMENT_APPROVAL,MANAGEMENT_APPROVAL_DATE,MANAGEMENT_NAME,CLAIM_YEAR,TOTAL_AMOUNT_APPROVED,MODIFIED_DATE,EMPLOYEE_EMAILADDRESS,FINANCE_EMAILADDRESS,HR_EMAILADDRESS,MANAGEMENT_EMAILADDRESS,CREATED_DATE")] OPDEXPENSE oPDEXPENSE)
        {

            try
            {
                string buttonStatus = Request.Form["buttonName"];

                if (buttonStatus == "approved")
                {
                    oPDEXPENSE.STATUS = "MANApproved";

                    if (oPDEXPENSE.TOTAL_AMOUNT_APPROVED.ToString() == "")
                    {
                        ModelState.AddModelError("", "Please Add Total Approved Amount");
                    }


                }
                else if (buttonStatus == "rejected")
                {
                    oPDEXPENSE.STATUS = "MANRejected";

                    if (oPDEXPENSE.HR_COMMENT == null)
                    {
                        ModelState.AddModelError("", "Please Add MAN Comments");
                    }
                }
                else
                {
                    oPDEXPENSE.STATUS = "MANInProcess";
                }

                if (ModelState.IsValid)
                {
                    oPDEXPENSE.MODIFIED_DATE = DateTime.Now;
                    oPDEXPENSE.MANAGEMENT_APPROVAL_DATE = DateTime.Now;
                    oPDEXPENSE.MANAGEMENT_EMAILADDRESS = GetEmailAddress();
                    if (oPDEXPENSE.STATUS == "MANApproved")
                    {
                        oPDEXPENSE.HR_APPROVAL = true;
                        oPDEXPENSE.FINANCE_APPROVAL = true;
                        oPDEXPENSE.MANAGEMENT_APPROVAL = true;
                    }

                    db.Entry(oPDEXPENSE).State = EntityState.Modified;
                    db.SaveChanges();
                }
                return RedirectToAction("Index", "MANAPPROVAL");

            }
            catch (Exception ex)
            {

                logger.Error("MANAPPROVAL : MANHospitalExpense([Bind])" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
        }






        /// <summary>
        /// GET: /Img/DownloadFile
        /// </summary>
        /// <param name="fileId">File Id parameter</param>
        /// <returns>Return download file</returns>
        public ActionResult DownloadFile(int fileId)
        {
            // Model binding.
            ImgViewModel model = new ImgViewModel { FileAttach = null, ImgLst = new List<OPDEXPENSE_IMAGEOBJ>() };

            try
            {
                // Loading dile info.
                var fileInfo = this.db.GET_OPDEXPENSE_IMAGE_DETAILS(fileId).First();

                // Info.
                return this.GetFile(fileInfo.IMAGE_BASE64, fileInfo.IMAGE_EXT);
            }
            catch (Exception ex)
            {
                // Info
                Console.Write(ex);
            }

            // Info.
            return this.View(model);
        }

        #region Get file method.

        /// <summary>
        /// Get file method.
        /// </summary>
        /// <param name="fileContent">File content parameter.</param>
        /// <param name="fileContentType">File content type parameter</param>
        /// <returns>Returns - File.</returns>
        private FileResult GetFile(string fileContent, string fileContentType)
        {
            // Initialization.
            FileResult file = null;

            try
            {
                // Get file.
                byte[] byteContent = Convert.FromBase64String(fileContent);
                file = this.File(byteContent, fileContentType);
            }
            catch (Exception ex)
            {
                // Info.
                throw ex;
            }

            // info.
            return file;
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }



        #endregion


        private string GetEmailAddress()
        {
            OFFICEAPIMANAGERController managerController = new OFFICEAPIMANAGERController();
            string emailAddress = managerController.GetEmailAddress();

            return emailAddress;
        }


        private void AuthenticateUser()
        {
            OFFICEAPIMANAGERController managerController = new OFFICEAPIMANAGERController();
            ViewBag.RollType = managerController.AuthenticateUser();

            ViewBag.UserName = managerController.GetName();

        }
    }
}