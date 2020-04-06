using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OPDCLAIMFORM.Models;
using NLog;
namespace OPDCLAIMFORM.Controllers
{
    public class HOSPITALEXPENSEController : Controller
    {
        private readonly MedicalInfoEntities db = new MedicalInfoEntities();
        private static readonly Logger logger = LogManager.GetCurrentClassLogger();
        // GET: OPDEXPENSEs
        public ActionResult Index()
        {

            //return View(db.OPDEXPENSEs.Where(e => e.OPDTYPE == "Hospital Expense").ToList());
            return RedirectToAction("Index", "OPDExpenses");


        }

        // GET: OPDEXPENSEs/Details/5
        public ActionResult Details(int? id)
        {
            MedicalInfoEntities entities = new MedicalInfoEntities();
            try
            {
                if (Request.IsAuthenticated)
                {
                    AuthenticateUser();

                    if (id == null)
                    {
                        return RedirectToAction("Index", "OPDExpenses");
                    }

                    OPDEXPENSE oPDEXPENSE = db.OPDEXPENSEs.Find(id);

                    var hospitalInformation = GetHospitalExpense(oPDEXPENSE);                  
                  
                    return View(hospitalInformation);
                }
                else
                {
                    return RedirectToAction("Index", "OPDExpenses");
                }

            }
            catch (Exception ex)
            {

                logger.Error("Hospital Expense : Details()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
        }

        // GET: OPDEXPENSEs/Create
        public ActionResult Create()
        {
            try
            {
                if (Request.IsAuthenticated)
                {
                    AuthenticateUser();

                    return View();
                }
                else
                {
                    return RedirectToAction("Index", "OPDExpenses");
                }
            }
            catch (Exception ex)
            {

                logger.Error("Hospital Expense : Create()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }

        }

        // POST: OPDEXPENSEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OPDEXPENSE_ID,EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,DATE_ILLNESS_NOTICED,DATE_RECOVERY,DIAGNOSIS,CLAIMANT_SUFFERED_ILLNESS,CLAIMANT_SUFFERED_ILLNESS_DATE,CLAIMANT_SUFFERED_ILLNESS_DETAILS,HOSPITAL_NAME,DOCTOR_NAME,PERIOD_CONFINEMENT_DATE_FROM,PERIOD_CONFINEMENT_DATE_TO,DRUGS_PRESCRIBED_BOOL,DRUGS_PRESCRIBED_DESCRIPTION,TOTAL_AMOUNT_CLAIMED,CLAIM_YEAR")] OPDEXPENSE oPDEXPENSE)
        {
            try
            {

                string buttonStatus = Request.Form["buttonName"];


                if (ModelState.IsValid)
                {
                    oPDEXPENSE.OPDTYPE = "Hospital Expense";
                    oPDEXPENSE.STATUS = "InProcess";
                    oPDEXPENSE.CREATED_DATE = DateTime.Now;
                    oPDEXPENSE.EMPLOYEE_EMAILADDRESS = GetEmailAddress();
                    db.OPDEXPENSEs.Add(oPDEXPENSE);
                    db.SaveChanges();                  
                    return RedirectToAction("Edit", "HOSPITALEXPENSE", new { id = oPDEXPENSE.OPDEXPENSE_ID , opdType = oPDEXPENSE.OPDTYPE });
                }

                return RedirectToAction("Index", "OPDExpenses"); 
            }
            catch (Exception ex)
            {

                logger.Error("Hospital Expense : Create()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
        }

        // GET: OPDEXPENSEs/Edit/5
        public ActionResult Edit(int? id)
        {

            try
            {

                if (Request.IsAuthenticated)
                {
                    AuthenticateUser();

                    if (id == null)
                    {
                        return RedirectToAction("Index", "OPDExpenses");
                    }                    

                    OPDEXPENSE oPDEXPENSE = db.OPDEXPENSEs.Find(id);
                    var hospitalInformation = GetHospitalExpense(oPDEXPENSE);                  

                    ViewData["OPDEXPENSE_ID"] = id;
                    ViewData["OPDTYPE"] = oPDEXPENSE.OPDTYPE;


                    if (!AuthenticateEmailAddress(Convert.ToInt32(id)))
                    {
                        return RedirectToAction("Index", "Home");
                    }


                   
                        return View(hospitalInformation);
                       
                }
                else
                {
                    return RedirectToAction("Index", "OPDExpenses");
                }
            }
            catch (Exception ex)
            {

                logger.Error("Hospital Expense : Create()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }

        }

        // POST: OPDEXPENSEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OPDEXPENSE_ID,EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,CLAIM_MONTH,TOTAL_AMOUNT_CLAIMED,HR_COMMENT,HR_APPROVAL,HR_APPROVAL_DATE,HR_NAME,FINANCE_COMMENT,FINANCE_APPROVAL,FINANCE_APPROVAL_DATE,FINANCE_NAME,MANAGEMENT_COMMENT,MANAGEMENT_APPROVAL,MANAGEMENT_APPROVAL_DATE,MANAGEMENT_NAME,DATE_ILLNESS_NOTICED,DATE_RECOVERY,DIAGNOSIS,CLAIMANT_SUFFERED_ILLNESS,CLAIMANT_SUFFERED_ILLNESS_DATE,CLAIMANT_SUFFERED_ILLNESS_DETAILS,HOSPITAL_NAME,DOCTOR_NAME,PERIOD_CONFINEMENT_DATE_FROM,PERIOD_CONFINEMENT_DATE_TO,DRUGS_PRESCRIBED_BOOL,DRUGS_PRESCRIBED_DESCRIPTION,OPDTYPE,STATUS,CLAIM_YEAR,CREATED_DATE,CLAIMANT_SUFFERED_ILLNESS_DATE,CLAIMANT_SUFFERED_ILLNESS_DETAILS,DRUGS_PRESCRIBED_DESCRIPTION")] OPDEXPENSE oPDEXPENSE)
        {

            try
            {
                AuthenticateUser();
                var hospitalInformation = GetHospitalExpense(oPDEXPENSE);
                ViewData["OPDEXPENSE_ID"] = oPDEXPENSE.OPDEXPENSE_ID;
                ViewData["OPDTYPE"] = oPDEXPENSE.OPDTYPE;

                string buttonStatus = Request.Form["buttonName"];

                if (buttonStatus == "submit")
                {
                    oPDEXPENSE.STATUS = "Submitted";
                }
                else
                {
                    oPDEXPENSE.STATUS = "InProcess";
                }


                if (oPDEXPENSE.STATUS == "Submitted")
                {
                    if (hospitalInformation.listOPDEXPENSEPATIENT.Count > 0)
                    {
                        if (hospitalInformation.listOPDEXPENSEIMAGE.Count > 0)
                        {
                            if (GetHOSExpenseAmount(oPDEXPENSE))
                            {
                                if (ModelState.IsValid)
                                {
                                    oPDEXPENSE.MODIFIED_DATE = DateTime.Now;
                                    oPDEXPENSE.EMPLOYEE_EMAILADDRESS = GetEmailAddress();
                                    db.Entry(oPDEXPENSE).State = EntityState.Modified;
                                    db.SaveChanges();
                                    return RedirectToAction("Index", "OPDExpenses");
                                }

                            }
                            else
                            {
                                ModelState.AddModelError("", "OPD Expense Amount between Images and Claimed is not same");
                                return View(hospitalInformation);
                            }


                    }
                    else
                        {
                            ModelState.AddModelError("", "Please Add Patient Receipts");
                            return View(hospitalInformation);
                        }

                    }
                    else
                    {
                        ModelState.AddModelError("", "Please Add Patient Information");
                        return View(hospitalInformation);
                    }
                }
                else
                {
                    if (ModelState.IsValid)
                    {
                        oPDEXPENSE.MODIFIED_DATE = DateTime.Now;
                        oPDEXPENSE.EMPLOYEE_EMAILADDRESS = GetEmailAddress();
                        db.Entry(oPDEXPENSE).State = EntityState.Modified;
                        db.SaveChanges();
                        return RedirectToAction("Index", "OPDExpenses");
                    }
                }

                           
                return View(hospitalInformation); 
            }
            catch (Exception ex)
            {

                logger.Error("Hospital Expense : Create()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
        }

        // GET: OPDEXPENSEs/Delete/5
        public ActionResult Delete(int? id)
        {
            try
            {
                if (Request.IsAuthenticated)
                {
                    AuthenticateUser();

                    if (!(AuthenticateEmailAddress(Convert.ToInt32(id))))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                    if (id == null)
                    {
                        return RedirectToAction("Index", "OPDExpenses");
                    }

                    var fileInfo = this.db.DELETE_OPDEXPENSE(id);

                    return RedirectToAction("Index", "OPDExpenses");
                }
                else
                {
                    return RedirectToAction("Index", "OPDExpenses");
                }

            }
            catch (Exception ex)
            {

                logger.Error("Hospital Expense : Create()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
        }

        // POST: OPDEXPENSEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            try
            {

                if (Request.IsAuthenticated)
                {
                    AuthenticateUser();
                    if (id == 0)
                    {
                        return RedirectToAction("Index", "OPDExpenses");
                    }

                    else
                    {
                        var fileInfo = this.db.DELETE_OPDEXPENSE(id);
                    }

                    return RedirectToAction("Index", "OPDExpenses");
                }
                else
                {
                    return RedirectToAction("Index", "OPDExpenses");
                }


            }
            catch (Exception ex)
            {

                logger.Error("Hospital Expense : Create()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
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
            FileResult file;
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

        #endregion

        private string GetEmailAddress()
        {
            OFFICEAPIMANAGERController managerController = new OFFICEAPIMANAGERController();
            string emailAddress = managerController.GetEmailAddress();

            return emailAddress;
        }

        private string GetName()
        {
            OFFICEAPIMANAGERController managerController = new OFFICEAPIMANAGERController();
            string name = managerController.GetName();

            return name;
        }

        private void AuthenticateUser()
        {
            OFFICEAPIMANAGERController managerController = new OFFICEAPIMANAGERController();

            ViewBag.RollType = managerController.AuthenticateUser();

            ViewBag.UserName = managerController.GetName();

        }

        private HOSPITALEXPENSE_MASTERDETAIL GetHospitalExpense(OPDEXPENSE oPDEXPENSE)
        {

            MedicalInfoEntities entities = new MedicalInfoEntities();


            var hospitalInformation = new HOSPITALEXPENSE_MASTERDETAIL()
            {
                listOPDEXPENSEPATIENT = entities.OPDEXPENSE_PATIENT.Where(e => e.OPDEXPENSE_ID == oPDEXPENSE.OPDEXPENSE_ID).ToList(),
                listOPDEXPENSEIMAGE = entities.OPDEXPENSE_IMAGE.Where(e => e.OPDEXPENSE_ID == oPDEXPENSE.OPDEXPENSE_ID).ToList(),
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
                FINANCE_APPROVAL = oPDEXPENSE.FINANCE_APPROVAL,
                FINANCE_COMMENT = oPDEXPENSE.FINANCE_COMMENT,
                FINANCE_NAME = oPDEXPENSE.FINANCE_NAME,
                HOSPITAL_NAME = oPDEXPENSE.HOSPITAL_NAME,
                HR_APPROVAL = oPDEXPENSE.HR_APPROVAL,
                HR_COMMENT = oPDEXPENSE.HR_COMMENT,
                HR_NAME = oPDEXPENSE.HR_NAME,
                MANAGEMENT_APPROVAL = oPDEXPENSE.MANAGEMENT_APPROVAL,
                MANAGEMENT_COMMENT = oPDEXPENSE.MANAGEMENT_COMMENT,
                MANAGEMENT_NAME = oPDEXPENSE.MANAGEMENT_NAME,
                PERIOD_CONFINEMENT_DATE_FROM = oPDEXPENSE.PERIOD_CONFINEMENT_DATE_FROM,
                PERIOD_CONFINEMENT_DATE_TO = oPDEXPENSE.PERIOD_CONFINEMENT_DATE_TO,
                STATUS = oPDEXPENSE.STATUS,
                OPDTYPE = oPDEXPENSE.OPDTYPE,
                TOTAL_AMOUNT_CLAIMED = oPDEXPENSE.TOTAL_AMOUNT_CLAIMED,
                CLAIM_YEAR = oPDEXPENSE.CLAIM_YEAR,
                CREATED_DATE = oPDEXPENSE.CREATED_DATE,
                MODIFIED_DATE = oPDEXPENSE.MODIFIED_DATE
            };

            return hospitalInformation;
        }

        private bool GetHOSExpenseAmount(OPDEXPENSE oPDEXPENSE)
        {
            bool result = false;
            MedicalInfoEntities entities = new MedicalInfoEntities();
            var hospitalInformation = new HOSPITALEXPENSE_MASTERDETAIL()
            {
                listOPDEXPENSEPATIENT = entities.OPDEXPENSE_PATIENT.Where(e => e.OPDEXPENSE_ID == oPDEXPENSE.OPDEXPENSE_ID).ToList(),
                listOPDEXPENSEIMAGE = entities.OPDEXPENSE_IMAGE.Where(e => e.OPDEXPENSE_ID == oPDEXPENSE.OPDEXPENSE_ID).ToList(),
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
                FINANCE_APPROVAL = oPDEXPENSE.FINANCE_APPROVAL,
                FINANCE_COMMENT = oPDEXPENSE.FINANCE_COMMENT,
                FINANCE_NAME = oPDEXPENSE.FINANCE_NAME,
                HOSPITAL_NAME = oPDEXPENSE.HOSPITAL_NAME,
                HR_APPROVAL = oPDEXPENSE.HR_APPROVAL,
                HR_COMMENT = oPDEXPENSE.HR_COMMENT,
                HR_NAME = oPDEXPENSE.HR_NAME,
                MANAGEMENT_APPROVAL = oPDEXPENSE.MANAGEMENT_APPROVAL,
                MANAGEMENT_COMMENT = oPDEXPENSE.MANAGEMENT_COMMENT,
                MANAGEMENT_NAME = oPDEXPENSE.MANAGEMENT_NAME,
                PERIOD_CONFINEMENT_DATE_FROM = oPDEXPENSE.PERIOD_CONFINEMENT_DATE_FROM,
                PERIOD_CONFINEMENT_DATE_TO = oPDEXPENSE.PERIOD_CONFINEMENT_DATE_TO,
                STATUS = oPDEXPENSE.STATUS,
                OPDTYPE = oPDEXPENSE.OPDTYPE,
                TOTAL_AMOUNT_CLAIMED = oPDEXPENSE.TOTAL_AMOUNT_CLAIMED,
                CLAIM_YEAR = oPDEXPENSE.CLAIM_YEAR,
                CREATED_DATE = oPDEXPENSE.CREATED_DATE,
                MODIFIED_DATE = oPDEXPENSE.MODIFIED_DATE
            };


            decimal? totalAmount = 0;

            for (int count = 0; count <= hospitalInformation.listOPDEXPENSEIMAGE.Count - 1; count++)
            {
                totalAmount = totalAmount + hospitalInformation.listOPDEXPENSEIMAGE[count].EXPENSE_AMOUNT;

            }

            if (totalAmount.Equals(hospitalInformation.TOTAL_AMOUNT_CLAIMED))
            {
                result = true;
            }

            return result;


        }


        private bool AuthenticateEmailAddress(int id)
        {

            OPDEXPENSE oPDEXPENSE = db.OPDEXPENSEs.Find(id);

            OFFICEAPIMANAGERController managerController = new OFFICEAPIMANAGERController();

            string currentEmailAddress = managerController.GetEmailAddress();

            if (currentEmailAddress.Equals(oPDEXPENSE.EMPLOYEE_EMAILADDRESS))

                return true;
            else
                return false;

        }

      

    }
}
