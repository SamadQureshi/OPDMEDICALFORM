//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace OPDCLAIMFORM
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel;
    using System.ComponentModel.DataAnnotations;

    public partial class OPDEXPENSE
    {
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2214:DoNotCallOverridableMethodsInConstructors")]
        public OPDEXPENSE()
        {
            this.OPDEXPENSE_IMAGE = new HashSet<OPDEXPENSE_IMAGE>();
            this.OPDEXPENSE_PATIENT = new HashSet<OPDEXPENSE_PATIENT>();
        }
    
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
        public Nullable<System.DateTime> DATE_ILLNESS_NOTICED { get; set; }
        public Nullable<System.DateTime> DATE_RECOVERY { get; set; }
        public string DIAGNOSIS { get; set; }
        public Nullable<bool> CLAIMANT_SUFFERED_ILLNESS { get; set; }
        public Nullable<System.DateTime> CLAIMANT_SUFFERED_ILLNESS_DATE { get; set; }
        public string CLAIMANT_SUFFERED_ILLNESS_DETAILS { get; set; }
        public string HOSPITAL_NAME { get; set; }
        public string DOCTOR_NAME { get; set; }
        public Nullable<System.DateTime> PERIOD_CONFINEMENT_DATE_FROM { get; set; }
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
        public string EXPENSE_NUMBER { get; set; }
    
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPDEXPENSE_IMAGE> OPDEXPENSE_IMAGE { get; set; }
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2227:CollectionPropertiesShouldBeReadOnly")]
        public virtual ICollection<OPDEXPENSE_PATIENT> OPDEXPENSE_PATIENT { get; set; }
    }
}
