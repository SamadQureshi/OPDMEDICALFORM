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
        public ActionResult Index()
        {
            return View(db.OPDEXPENSEs.Where(e => e.OPDTYPE == "OPD Expense").ToList());
        }

        // GET: OPDEXPENSEs/Details/5
        public ActionResult Details(int? id)
        {
           
            MedicalInfoEntities entities = new MedicalInfoEntities();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
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
            return View();
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
                db.OPDEXPENSEs.Add(oPDEXPENSE);
                db.SaveChanges();              
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "OPDEXPENSEPATIENT", new { id = oPDEXPENSE.OPDEXPENSE_ID });
            }

            return View(oPDEXPENSE);
        }

        // GET: OPDEXPENSEs/Edit/5
        public ActionResult Edit(int? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            OPDEXPENSE oPDEXPENSE = db.OPDEXPENSEs.Find(id);
            if (oPDEXPENSE == null)
            {
                return HttpNotFound();
            }

            ViewData["OPDEXPENSE_ID"] = id;
            return View(oPDEXPENSE);
        }

        // POST: OPDEXPENSEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EMPLOYEE_EMAILADDRESS,EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,CLAIM_MONTH,TOTAL_AMOUNT_CLAIMED,CLAIM_YEAR")] OPDEXPENSE oPDEXPENSE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oPDEXPENSE).State = EntityState.Modified;
                db.SaveChanges();
                //return RedirectToAction("Index");
                return RedirectToAction("Index", "OPDEXPENSEPATIENT", new { id = oPDEXPENSE.OPDEXPENSE_ID });
            }
            return View(oPDEXPENSE);
        }

        // GET: OPDEXPENSEs/Delete/5
        public ActionResult Delete(int? id)
        {

            var fileInfo = this.db.DELETE_OPDEXPENSE(id);

            return RedirectToAction("Index");

            //if (id == null)
            //{
            //    return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            //}
            //OPDEXPENSE oPDEXPENSE = db.OPDEXPENSEs.Find(id);
            //if (oPDEXPENSE == null)
            //{
            //    return HttpNotFound();
            //}
            //return View(oPDEXPENSE);
        }

        // POST: OPDEXPENSEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
         
          
           var fileInfo = this.db.DELETE_OPDEXPENSE(id);


           // OPDEXPENSE oPDEXPENSE = db.OPDEXPENSEs.Find(id);
           // db.OPDEXPENSEs.Remove(oPDEXPENSE);
           // db.SaveChanges();
            return RedirectToAction("Index");
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

    }
}
