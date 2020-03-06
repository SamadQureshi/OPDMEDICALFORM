using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using System.Data.Entity;
namespace OPDCLAIMFORM.Controllers
{
    public class HOSPITALEXPENSEController : Controller
    {
        private MedicalInfoEntities db = new MedicalInfoEntities();

        // GET: OPDEXPENSEs
        public ActionResult Index()
        {
            return View(db.OPDEXPENSEs.Where(e => e.OPDTYPE == "Hospital Expense").ToList());
        }

        // GET: OPDEXPENSEs/Details/5
        public ActionResult Details(int? id)
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
        public ActionResult Create([Bind(Include = "OPDEXPENSE_ID,EMPLOYEE_NAME,DATE_ILLNESS_NOTICED,DATE_RECOVERY,DIAGNOSIS,CLAIMANT_SUFFERED_ILLNESS,CLAIMANT_SUFFERED_ILLNESS_DATE,CLAIMANT_SUFFERED_ILLNESS_DETAILS,HOSPITAL_NAME,DOCTOR_NAME,PERIOD_CONFINEMENT_DATE_FROM,PERIOD_CONFINEMENT_DATE_TO,DRUGS_PRESCRIBED_BOOL,DRUGS_PRESCRIBED_DESCRIPTION")] OPDEXPENSE oPDEXPENSE)
        {
            if (ModelState.IsValid)
            {
                oPDEXPENSE.OPDTYPE = "Hospital Expense";
                oPDEXPENSE.STATUS = "InProcess";
                db.OPDEXPENSEs.Add(oPDEXPENSE);
                db.SaveChanges();
                return RedirectToAction("Index");
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
        public ActionResult Edit([Bind(Include = "OPDEXPENSE_ID,EMPLOYEE_EMAILADDRESS,EMPLOYEE_NAME,EMPLOYEE_DEPARTMENT,CLAIM_MONTH,TOTAL_AMOUNT_CLAIMED,HR_COMMENT,HR_APPROVAL,HR_APPROVAL_DATE,HR_NAME,FINANCE_COMMENT,FINANCE_APPROVAL,FINANCE_APPROVAL_DATE,FINANCE_NAME,MANAGEMENT_COMMENT,MANAGEMENT_APPROVAL,MANAGEMENT_APPROVAL_DATE,MANAGEMENT_NAME,DATE_ILLNESS_NOTICED,DATE_RECOVERY,DIAGNOSIS,CLAIMANT_SUFFERED_ILLNESS,CLAIMANT_SUFFERED_ILLNESS_DATE,CLAIMANT_SUFFERED_ILLNESS_DETAILS,HOSPITAL_NAME,DOCTOR_NAME,PERIOD_CONFINEMENT_DATE_FROM,PERIOD_CONFINEMENT_DATE_TO,DRUGS_PRESCRIBED_BOOL,DRUGS_PRESCRIBED_DESCRIPTION,OPDTYPE")] OPDEXPENSE oPDEXPENSE)
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
