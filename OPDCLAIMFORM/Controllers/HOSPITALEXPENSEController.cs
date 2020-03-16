using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
using OPDCLAIMFORM.Models;

namespace OPDCLAIMFORM.Controllers
{
    public class HOSPITALEXPENSEController : Controller
    {
        private readonly MedicalInfoEntities db = new MedicalInfoEntities();

        // GET: OPDEXPENSEs
        public ActionResult Index()
        {
            
            return View(db.OPDEXPENSEs.Where(e => e.OPDTYPE == "Hospital Expense").ToList());
        }

        // GET: OPDEXPENSEs/Details/5
        public ActionResult Details(int? id)
        {
            MedicalInfoEntities entities = new MedicalInfoEntities();

            if (Request.IsAuthenticated)
            {
                AuthenticateUser();

                if (id == null)
                {
                    return RedirectToAction("Index", "OPDEXPENSEs");
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
                    TOTAL_AMOUNT_CLAIMED = oPDEXPENSE.TOTAL_AMOUNT_CLAIMED,
                    CLAIM_YEAR = oPDEXPENSE.CLAIM_YEAR,
                    CREATED_DATE = oPDEXPENSE.CREATED_DATE,
                    MODIFIED_DATE = oPDEXPENSE.MODIFIED_DATE


                };

                return View(result2);
            }
            else
            {
                return RedirectToAction("Index", "OPDEXPENSEs");
            }
        }

        // GET: OPDEXPENSEs/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
                AuthenticateUser();

                return View();
            }
            else
            {
                return RedirectToAction("Index", "OPDEXPENSEs");
            }

        }

        // POST: OPDEXPENSEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "OPDEXPENSE_ID,EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,DATE_ILLNESS_NOTICED,DATE_RECOVERY,DIAGNOSIS,CLAIMANT_SUFFERED_ILLNESS,CLAIMANT_SUFFERED_ILLNESS_DATE,CLAIMANT_SUFFERED_ILLNESS_DETAILS,HOSPITAL_NAME,DOCTOR_NAME,PERIOD_CONFINEMENT_DATE_FROM,PERIOD_CONFINEMENT_DATE_TO,DRUGS_PRESCRIBED_BOOL,DRUGS_PRESCRIBED_DESCRIPTION,TOTAL_AMOUNT_CLAIMED,CLAIM_YEAR")] OPDEXPENSE oPDEXPENSE)
        {
            if (ModelState.IsValid)
            {
                oPDEXPENSE.OPDTYPE = "Hospital Expense";
                oPDEXPENSE.STATUS = "InProcess";
                oPDEXPENSE.CREATED_DATE = DateTime.Now;
                oPDEXPENSE.EMPLOYEE_EMAILADDRESS = GetEmailAddress();
                db.OPDEXPENSEs.Add(oPDEXPENSE);
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Edit", "HOSPITALEXPENSE", new { id = oPDEXPENSE.OPDEXPENSE_ID });
            }

            return View(oPDEXPENSE);
        }

        // GET: OPDEXPENSEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Request.IsAuthenticated)
            {
                AuthenticateUser();

                if (id == null)
                {
                    return RedirectToAction("Index", "OPDEXPENSEs");
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

                ViewData["OPDEXPENSE_ID"] = id;
                return View(result2);
            }
            else
            {
                return RedirectToAction("Index", "OPDEXPENSEs");
            }

        }

        // POST: OPDEXPENSEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OPDEXPENSE_ID,EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,CLAIM_MONTH,TOTAL_AMOUNT_CLAIMED,HR_COMMENT,HR_APPROVAL,HR_APPROVAL_DATE,HR_NAME,FINANCE_COMMENT,FINANCE_APPROVAL,FINANCE_APPROVAL_DATE,FINANCE_NAME,MANAGEMENT_COMMENT,MANAGEMENT_APPROVAL,MANAGEMENT_APPROVAL_DATE,MANAGEMENT_NAME,DATE_ILLNESS_NOTICED,DATE_RECOVERY,DIAGNOSIS,CLAIMANT_SUFFERED_ILLNESS,CLAIMANT_SUFFERED_ILLNESS_DATE,CLAIMANT_SUFFERED_ILLNESS_DETAILS,HOSPITAL_NAME,DOCTOR_NAME,PERIOD_CONFINEMENT_DATE_FROM,PERIOD_CONFINEMENT_DATE_TO,DRUGS_PRESCRIBED_BOOL,DRUGS_PRESCRIBED_DESCRIPTION,OPDTYPE,STATUS,CLAIM_YEAR,CREATED_DATE")] OPDEXPENSE oPDEXPENSE)
        {
            if (ModelState.IsValid)
            {
                oPDEXPENSE.MODIFIED_DATE = DateTime.Now;
                oPDEXPENSE.EMPLOYEE_EMAILADDRESS = GetEmailAddress();
                db.Entry(oPDEXPENSE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index", "OPDEXPENSEs");
            }
            return View(oPDEXPENSE);
        }

        // GET: OPDEXPENSEs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Request.IsAuthenticated)
            {
                AuthenticateUser();
                if (id == null)
                {
                    return RedirectToAction("Index", "OPDEXPENSEs");
                }

                var fileInfo = this.db.DELETE_OPDEXPENSE(id);

                return RedirectToAction("Index", "OPDEXPENSEs");
            }
            else
            {
                return RedirectToAction("Index", "OPDEXPENSEs");
            }
        }

        // POST: OPDEXPENSEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {

            if (Request.IsAuthenticated)
            {
                AuthenticateUser();
                if (id == 0)
                {
                    return RedirectToAction("Index", "OPDEXPENSEs");
                }

                else
                {
                    var fileInfo = this.db.DELETE_OPDEXPENSE(id);
                }

                return RedirectToAction("Index", "OPDEXPENSEs");
            }
            else
            {
                return RedirectToAction("Index", "OPDEXPENSEs");
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
    }
}
