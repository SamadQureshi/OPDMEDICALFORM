using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using OPDCLAIMFORM;
using OPDCLAIMFORM.Models;
using PagedList;
using NLog;
using System.Web.UI.WebControls;

namespace OPDCLAIMFORM.Controllers
{
    public class OPDEXPENSEsController : Controller
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
                    var opdExp = db.OPDEXPENSEs.Where(e => e.EMPLOYEE_EMAILADDRESS == emailAddress);

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

                    // return View(db.OPDEXPENSEs.ToList());
                }
                else
                {
                    return RedirectToAction("Index", "Home");

                }
            }
            catch(Exception ex)
            {

                logger.Error("OPD Expense : Index()" + ex.Message.ToString() );           
          
                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }
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


                    var result2 = new OPDEXPENSE_MASTERDETAIL()
                    {
                        listOPDEXPENSEPATIENT = entities.OPDEXPENSE_PATIENT.Where(e => e.OPDEXPENSE_ID == id).ToList(),
                        listOPDEXPENSEIMAGE = entities.OPDEXPENSE_IMAGE.Where(e => e.OPDEXPENSE_ID == id).ToList(),
                        opdEXPENSE = entities.OPDEXPENSEs.Where(e => e.OPDEXPENSE_ID == id).FirstOrDefault()

                    };

                    return View(result2);
                }
                else
                {
                    return RedirectToAction("Details()", "OPDExpenses");
                }
            }
            catch (Exception ex)
            {

                logger.Error("OPD Expense : Details" + ex.Message.ToString());

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

                logger.Error("OPD Expense : Create()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }

        }

        // POST: OPDEXPENSEs/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,CLAIM_MONTH,TOTAL_AMOUNT_CLAIMED,CLAIM_YEAR")] OPDEXPENSE oPDEXPENSE)
        {
            try
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
                    ViewData["OPDTYPE"] = oPDEXPENSE.OPDTYPE;

                    return RedirectToAction("Edit", "OPDExpenses", new { id = oPDEXPENSE.OPDEXPENSE_ID  });

                }
                return View(oPDEXPENSE);
            }
            catch (Exception ex)
            {
                logger.Error("OPD Expense : Create([Bind])" + ex.Message.ToString());

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

                    var opdInformation = GetOPDExpense(Convert.ToInt32(id));
                    ViewData["OPDEXPENSE_ID"] = id;
                    ViewData["OPDTYPE"] = opdInformation.opdEXPENSE.OPDTYPE;



                    if (!(AuthenticateEmailAddress(Convert.ToInt32(id))))
                    {
                        return RedirectToAction("Index", "Home");
                    }

                  
                        return View(opdInformation);
                   
                   
                   
                }
                else
                {
                    return RedirectToAction("Index", "OPDExpenses");
                }
            }
            catch (Exception ex)
            {
                logger.Error("OPD Expense : Edit()" + ex.Message.ToString());

                return View(new HttpStatusCodeResult(HttpStatusCode.BadRequest));
            }

        }

        // POST: OPDEXPENSEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "OPDEXPENSE_ID,EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,CLAIM_MONTH,CLAIM_YEAR,TOTAL_AMOUNT_CLAIMED,STATUS,OPDTYPE,CREATED_DATE")] OPDEXPENSE oPDEXPENSE)
        {
            try
            {
               string buttonStatus = Request.Form["buttonName"];

                AuthenticateUser();
                var opdInformation = GetOPDExpense(oPDEXPENSE.OPDEXPENSE_ID);
                ViewData["OPDEXPENSE_ID"] = oPDEXPENSE.OPDEXPENSE_ID;
                ViewData["OPDTYPE"] = oPDEXPENSE.OPDTYPE;
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
                   


                    if (opdInformation.listOPDEXPENSEPATIENT.Count > 0)
                    {
                        if (opdInformation.listOPDEXPENSEIMAGE.Count > 0)
                        {
                            
                            if(GetOPDExpenseAmount(oPDEXPENSE.OPDEXPENSE_ID, oPDEXPENSE.TOTAL_AMOUNT_CLAIMED))
                            {
                                if (ModelState.IsValid)
                                {
                                    oPDEXPENSE.MODIFIED_DATE = DateTime.Now;
                                    oPDEXPENSE.EMPLOYEE_EMAILADDRESS = GetEmailAddress();
                                    db.Entry(oPDEXPENSE).State = EntityState.Modified;
                                    db.SaveChanges();
                                    return RedirectToAction("Index");
                                }

                            }
                            else
                            {
                                ModelState.AddModelError("", "OPD Expense Amount between Images and Claimed is not same");
                                return View(opdInformation);
                            }
                           


                        }
                        else
                        {
                            ModelState.AddModelError("", "Please Add Patient Receipts");
                            return View(opdInformation);
                        }
                    }
                    else
                    {
                        ModelState.AddModelError("", "Please Add Patient Information");
                        return View(opdInformation);
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
                        return RedirectToAction("Index");
                    }

                }

                return View(opdInformation);
            }
            catch (Exception ex)
            {
                logger.Error("OPD Expense : Edit([Bind])" + ex.Message.ToString());

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
                logger.Error("OPD Expense : Delete()" + ex.Message.ToString());

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
                logger.Error("OPD Expense : DeleteConfirmed()" + ex.Message.ToString());

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

        private void AuthenticateUser()
        {
            OFFICEAPIMANAGERController managerController = new OFFICEAPIMANAGERController();           

            ViewBag.RollType = managerController.AuthenticateUser();

            ViewBag.UserName = managerController.GetName();
            
        }

        private bool AuthenticateEmailAddress(int Id)
        {

            var opdInformation = GetOPDExpense(Convert.ToInt32(Id));
            OFFICEAPIMANAGERController managerController = new OFFICEAPIMANAGERController();

            string currentEmailAddress = managerController.GetEmailAddress();

            if (currentEmailAddress.Equals(opdInformation.opdEXPENSE.EMPLOYEE_EMAILADDRESS))

                return true;
            else
                return false;

        }

        private OPDEXPENSE_MASTERDETAIL GetOPDExpense(int Id)
        {

            MedicalInfoEntities entities = new MedicalInfoEntities();
            var opdInformation = new OPDEXPENSE_MASTERDETAIL()
            {
                listOPDEXPENSEPATIENT = entities.OPDEXPENSE_PATIENT.Where(e => e.OPDEXPENSE_ID == Id).ToList(),
                listOPDEXPENSEIMAGE = entities.OPDEXPENSE_IMAGE.Where(e => e.OPDEXPENSE_ID == Id).ToList(),
                opdEXPENSE = entities.OPDEXPENSEs.Where(e => e.OPDEXPENSE_ID == Id).FirstOrDefault()

            };

            return opdInformation;
        }

        private bool GetOPDExpenseAmount(int Id, decimal? totalAmountClaimed )
        {
            bool result = false;
            MedicalInfoEntities entities = new MedicalInfoEntities();
            var opdInformation = new OPDEXPENSE_MASTERDETAIL()
            {
                listOPDEXPENSEPATIENT = entities.OPDEXPENSE_PATIENT.Where(e => e.OPDEXPENSE_ID == Id).ToList(),
                listOPDEXPENSEIMAGE = entities.OPDEXPENSE_IMAGE.Where(e => e.OPDEXPENSE_ID == Id).ToList(),
                opdEXPENSE = entities.OPDEXPENSEs.Where(e => e.OPDEXPENSE_ID == Id).FirstOrDefault()

            };


            decimal? totalAmount = 0;

            for (int count = 0; count <= opdInformation.listOPDEXPENSEIMAGE.Count - 1 ; count ++)
            {
                totalAmount = totalAmount + opdInformation.listOPDEXPENSEIMAGE[count].EXPENSE_AMOUNT;

            }
            
            if (totalAmount.Equals(totalAmountClaimed))
            {
                result = true;
            }

            return result;


        }
    }
}
