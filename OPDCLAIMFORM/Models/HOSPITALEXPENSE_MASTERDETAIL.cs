using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace OPDCLAIMFORM.Models
{
    public class HOSPITALEXPENSE_MASTERDETAIL
    {
     
        public List<OPDEXPENSE_PATIENT> listOPDEXPENSEPATIENT { get; set; }

        public List<OPDEXPENSE_IMAGE> listOPDEXPENSEIMAGE { get; set; }


        public int OPDEXPENSE_ID { get; set; }
        public string EMPLOYEE_EMAILADDRESS { get; set; }

        [DisplayName("Employee Name")]
        [Required(ErrorMessage = "The Employee Name is required.")]
        [RegularExpression("^[a-zA-Z ]*$", ErrorMessage = "Incorrect Employee Name")]
        public string EMPLOYEE_NAME { get; set; }
        public string EMPLOYEE_DEPARTMENT { get; set; }
        public string CLAIM_MONTH { get; set; }

        [DisplayName("Total Amount")]
        [Required(ErrorMessage = "The Total Amount is required.")]
        [Range(0, Int32.MaxValue, ErrorMessage = "Invalid Number")]
        public Nullable<decimal> TOTAL_AMOUNT_CLAIMED { get; set; }
        public string HR_COMMENT { get; set; }
        public Nullable<bool> HR_APPROVAL { get; set; }
        public Nullable<System.DateTime> HR_APPROVAL_DATE { get; set; }
        public string HR_NAME { get; set; }
        public string FINANCE_COMMENT { get; set; }
        public Nullable<bool> FINANCE_APPROVAL { get; set; }
        public Nullable<System.DateTime> FINANCE_APPROVAL_DATE { get; set; }
        public string FINANCE_NAME { get; set; }
        public string MANAGEMENT_COMMENT { get; set; }
        public Nullable<bool> MANAGEMENT_APPROVAL { get; set; }
        public Nullable<System.DateTime> MANAGEMENT_APPROVAL_DATE { get; set; }
        public string MANAGEMENT_NAME { get; set; }

        [Required(ErrorMessage = "The Date of Illness is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true,DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> DATE_ILLNESS_NOTICED { get; set; }



        [Required(ErrorMessage = "The Date of Recovery is required.")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> DATE_RECOVERY { get; set; }



        [Required(ErrorMessage = "The Diagnosis is required.")]
        public string DIAGNOSIS { get; set; }
        public Nullable<bool> CLAIMANT_SUFFERED_ILLNESS { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        public Nullable<System.DateTime> CLAIMANT_SUFFERED_ILLNESS_DATE { get; set; }

        public string CLAIMANT_SUFFERED_ILLNESS_DETAILS { get; set; }

        [Required(ErrorMessage = "The Hospital Name is required.")]
        public string HOSPITAL_NAME { get; set; }

        [Required(ErrorMessage = "The Doctor Name is required.")]
        public string DOCTOR_NAME { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "The Period of Confinement is required.")]
        public Nullable<System.DateTime> PERIOD_CONFINEMENT_DATE_FROM { get; set; }


        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:MM/dd/yyyy}")]
        [Required(ErrorMessage = "The Period of Confinement is required.")]
        public Nullable<System.DateTime> PERIOD_CONFINEMENT_DATE_TO { get; set; }
        public Nullable<bool> DRUGS_PRESCRIBED_BOOL { get; set; }
        public string DRUGS_PRESCRIBED_DESCRIPTION { get; set; }
        public string OPDTYPE { get; set; }
        public Nullable<System.DateTime> CREATED_DATE { get; set; }
        public Nullable<System.DateTime> MODIFIED_DATE { get; set; }
        public string CREATED_BY { get; set; }
        public string MODIFIED_BY { get; set; }
        public string HR_EMAILADDRESS { get; set; }
        public string FINANCE_EMAILADDRESS { get; set; }
        public string MANAGEMENT_EMAILADDRESS { get; set; }
        public string STATUS { get; set; }
        public string CLAIM_YEAR { get; set; }

        public Nullable<decimal> TOTAL_AMOUNT_APPROVED { get; set; }


    }
}