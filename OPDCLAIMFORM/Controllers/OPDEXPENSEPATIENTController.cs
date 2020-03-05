using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace OPDCLAIMFORM.Controllers
{
    public class OPDEXPENSEPATIENTController : Controller
    {
        public ActionResult Index(int? id)
        {
           
                ViewData["OPDEXPENSE_ID"] = id;

                MedicalInfoEntities entities = new MedicalInfoEntities();

                List<OPDEXPENSE_PATIENT> opdExpense_Patient = entities.OPDEXPENSE_PATIENT.Where(e => e.OPDEXPENSE_ID == id).ToList();
                                   
                //Add a Dummy Row.
                opdExpense_Patient.Insert(0, new OPDEXPENSE_PATIENT());

            
            return View(opdExpense_Patient);

        }

        [HttpPost]
        public JsonResult InsertOPDExpensePatient(OPDEXPENSE_PATIENT opdExpense_Patient)
        {
                using (MedicalInfoEntities entities = new MedicalInfoEntities())
                {
                    entities.OPDEXPENSE_PATIENT.Add(opdExpense_Patient);
                    entities.SaveChanges();
                }
            
            return Json(opdExpense_Patient);
        }

        [HttpPost]
        public ActionResult UpdateOPDExpensePatient(OPDEXPENSE_PATIENT opdExpense_Patient)
        {
            using (MedicalInfoEntities entities = new MedicalInfoEntities())
            {
                OPDEXPENSE_PATIENT updatedOPDExpense_Patient = (from c in entities.OPDEXPENSE_PATIENT
                                                                where c.ID == opdExpense_Patient.ID
                                                                select c).FirstOrDefault();
                updatedOPDExpense_Patient.NAME = opdExpense_Patient.NAME;
                updatedOPDExpense_Patient.AGE = opdExpense_Patient.AGE;
                updatedOPDExpense_Patient.RELATIONSHIP_EMPLOYEE = opdExpense_Patient.RELATIONSHIP_EMPLOYEE;
                entities.SaveChanges();
            }

            return new EmptyResult();
        }

        [HttpPost]
        public ActionResult DeleteOPDExpensePatient(int ID)
        {
            using (MedicalInfoEntities entities = new MedicalInfoEntities())
            {
                OPDEXPENSE_PATIENT OPDExpense_Patient = (from c in entities.OPDEXPENSE_PATIENT
                                                         where c.ID == ID
                                                         select c).FirstOrDefault();
                entities.OPDEXPENSE_PATIENT.Remove(OPDExpense_Patient);
                entities.SaveChanges();
            }

            return new EmptyResult();
        }
    }
}