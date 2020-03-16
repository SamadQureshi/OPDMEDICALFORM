using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OPDCLAIMFORM;
using OPDCLAIMFORM.Models;

namespace OPDCLAIMFORM.Controllers
{
    public class OPDEXPENSEsController : Controller
    {
        private MedicalInfoEntities db = new MedicalInfoEntities();

        // GET: OPDEXPENSEs
        public ActionResult Index(string sortOrder, string currentFilter, string searchString)
        {
            if (Request.IsAuthenticated)
            {              


                ViewBag.CurrentSort = sortOrder;
                ViewBag.EmployeeNameSortParm = String.IsNullOrEmpty(sortOrder) ? "EmployeeName_desc" : "";              
                ViewBag.ClaimForMonthSortParm = String.IsNullOrEmpty(sortOrder) ? "ClaimForMonth_desc" : "";
                ViewBag.StatusSortParm = String.IsNullOrEmpty(sortOrder) ? "Status_desc" : "";
                ViewBag.OPDTypeSortParm = String.IsNullOrEmpty(sortOrder) ? "OPDType_desc" : "";                

                if (searchString == null)
                {
                  searchString = currentFilter;
                }

                ViewBag.CurrentFilter = searchString;

                //var students = from s in db.OPDEXPENSEs
                //               select s;a
                string emailAddress = GetEmailAddress();
                var students = db.OPDEXPENSEs.Where(e => e.EMPLOYEE_EMAILADDRESS == emailAddress);


                if (!String.IsNullOrEmpty(searchString))
                {
                    students = students.Where(s => s.STATUS.Contains(searchString));
                }
                switch (sortOrder)
                {
                    case "EmployeeName_desc":
                        students = students.OrderBy(s => s.EMPLOYEE_NAME);
                        ViewBag.EmployeeNameSortParm = "EmployeeName_asc";
                        break;
                    case "ClaimForMonth_desc":
                        students = students.OrderBy(s => s.CLAIM_MONTH);
                        ViewBag.ClaimForMonthSortParm = "ClaimForMonth_asc";
                        break;
                    case "Status_desc":
                        students = students.OrderBy(s => s.STATUS);
                        ViewBag.StatusSortParm = "Status_asc";
                        break;
                    case "OPDType_desc":
                        students = students.OrderBy(s => s.OPDTYPE);
                        ViewBag.OPDTypeSortParm = "OPDType_asc";
                        break;
                    case "EmployeeName_asc":
                        students = students.OrderByDescending(s => s.EMPLOYEE_NAME);
                        break;
                    case "ClaimForMonth_asc":
                        students = students.OrderByDescending(s => s.CLAIM_MONTH);                       
                        break;
                    case "Status_asc":
                        students = students.OrderByDescending(s => s.STATUS);                       
                        break;
                    case "OPDType_asc":
                        students = students.OrderByDescending(s => s.OPDTYPE);                    
                        break;
                }


                return View(students);

                // return View(db.OPDEXPENSEs.ToList());
            }
            else
            {
                return RedirectToAction("Index", "Home");

            }
        }

        // GET: OPDEXPENSEs/Details/5
        public ActionResult Details(int? id)
        {
           
            MedicalInfoEntities entities = new MedicalInfoEntities();

            if (id == null)
            {
                return RedirectToAction("Index", "OPDEXPENSEs");
            }          


            var result2 = new OPDEXPENSE_MASTERDETAIL()
            {
                listOPDEXPENSEPATIENT = entities.OPDEXPENSE_PATIENT.Where(e => e.OPDEXPENSE_ID == id).ToList(),
                listOPDEXPENSEIMAGE = entities.OPDEXPENSE_IMAGE.Where(e => e.OPDEXPENSE_ID == id).ToList(),
                opdEXPENSE = entities.OPDEXPENSEs.Where(e => e.OPDEXPENSE_ID == id).FirstOrDefault()

            };

            return View(result2);
        }

        // GET: OPDEXPENSEs/Create
        public ActionResult Create()
        {
            if (Request.IsAuthenticated)
            {
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
        public ActionResult Create([Bind(Include = "EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,CLAIM_MONTH,TOTAL_AMOUNT_CLAIMED,CLAIM_YEAR")] OPDEXPENSE oPDEXPENSE)
        {        
           
            if (ModelState.IsValid)
            {
               
                oPDEXPENSE.OPDTYPE = "OPD Expense";
                oPDEXPENSE.STATUS = "InProcess";
                oPDEXPENSE.CREATED_DATE = DateTime.Now;
                oPDEXPENSE.EMPLOYEE_EMAILADDRESS = GetEmailAddress();
                db.OPDEXPENSEs.Add(oPDEXPENSE);
                db.SaveChanges();
                ViewData["OPDEXPENSE_ID"] = oPDEXPENSE.OPDEXPENSE_ID;

                return RedirectToAction("Edit", "OPDEXPENSEs", new { id = oPDEXPENSE.OPDEXPENSE_ID });

            }

            return View(oPDEXPENSE);
        }

        // GET: OPDEXPENSEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (Request.IsAuthenticated)
            {
                if (id == null)
                {
                    return RedirectToAction("Index", "OPDEXPENSEs");
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
                return RedirectToAction("Index", "OPDEXPENSEs");
            }

        }

        // POST: OPDEXPENSEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OPDEXPENSE_ID,EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,CLAIM_MONTH,CLAIM_YEAR,TOTAL_AMOUNT_CLAIMED,STATUS,OPDTYPE")] OPDEXPENSE oPDEXPENSE)
        {
            if (ModelState.IsValid)
            {
                oPDEXPENSE.MODIFIED_DATE = DateTime.Now;               
                oPDEXPENSE.EMPLOYEE_EMAILADDRESS = GetEmailAddress();
                db.Entry(oPDEXPENSE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");             
            }
            return View(oPDEXPENSE);
        }

        // GET: OPDEXPENSEs/Delete/5
        public ActionResult Delete(int? id)
        {
            if (Request.IsAuthenticated)
            {

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

        #endregion


        private string GetEmailAddress()
        {
            OFFICEAPIMANAGERController managerController = new OFFICEAPIMANAGERController();
            string emailAddress = managerController.GetEmailAddress();

            return emailAddress;
        }
    }
}
