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
            return View(db.OPDEXPENSEs.ToList());
        }

        // GET: OPDEXPENSEs/Details/5
        public ActionResult Details(int? id)
        {
           
            MedicalInfoEntities entities = new MedicalInfoEntities();

            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            
          
                //var result = (from b in entities.OPDEXPENSE_PATIENT 
                //                         where b.OPDEXPENSE_ID == id
                //                         select new OPDEXPENSE_PATIENT
                //                         {                                             
                //                             NAME = b.NAME,
                //                             AGE = b.AGE,
                //                             RELATIONSHIP_EMPLOYEE = b.RELATIONSHIP_EMPLOYEE
                //                         }).ToList();


            var result2 = new OPDEXPENSE_MASTERDETAIL()
            {
                listOPDEXPENSEPATIENT = entities.OPDEXPENSE_PATIENT.Where(e => e.OPDEXPENSE_ID == id).ToList(),
                opdEXPENSE = entities.OPDEXPENSEs.Where(e => e.OPDEXPENSE_ID == id).FirstOrDefault()
            };



           // using (MedicalInfoEntities entities = new MedicalInfoEntities())
           //{
           //OPDEXPENSE_PATIENT OPDExpense_Patient = (from c in entities.OPDEXPENSE_PATIENT
           //                                             where c.ID == id
           //                                             select c).FirstOrDefault();                
           // }

            //if (oPDEXPENSE == null)
            //{
            //    return HttpNotFound();
            //}

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
        public ActionResult Create([Bind(Include = "EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,CLAIM_MONTH,TOTAL_AMOUNT_CLAIMED")] OPDEXPENSE oPDEXPENSE)
        {
            if (ModelState.IsValid)
            {
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
            return View(oPDEXPENSE);
        }

        // POST: OPDEXPENSEs/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "EMPLOYEE_EMAILADDRESS,EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,CLAIM_MONTH,TOTAL_AMOUNT_CLAIMED")] OPDEXPENSE oPDEXPENSE)
        {
            if (ModelState.IsValid)
            {
                db.Entry(oPDEXPENSE).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(oPDEXPENSE);
        }

        // GET: OPDEXPENSEs/Delete/5
        public ActionResult Delete(int? id)
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
            return View(oPDEXPENSE);
        }

        // POST: OPDEXPENSEs/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(int id)
        {
            OPDEXPENSE oPDEXPENSE = db.OPDEXPENSEs.Find(id);
            db.OPDEXPENSEs.Remove(oPDEXPENSE);
            db.SaveChanges();
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
    }
}
