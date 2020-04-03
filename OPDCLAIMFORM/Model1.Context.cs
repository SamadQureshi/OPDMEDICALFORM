﻿//------------------------------------------------------------------------------
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
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    using System.Data.Entity.Core.Objects;
    using System.Linq;
    
    public partial class MedicalInfoEntities : DbContext
    {
        public MedicalInfoEntities()
            : base("name=MedicalInfoEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<OPDEXPENSE> OPDEXPENSEs { get; set; }
        public virtual DbSet<OPDEXPENSE_IMAGE> OPDEXPENSE_IMAGE { get; set; }
        public virtual DbSet<OPDEXPENSE_PATIENT> OPDEXPENSE_PATIENT { get; set; }
        public virtual DbSet<RELATIONSHIP_EMPLOYEE> RELATIONSHIP_EMPLOYEE { get; set; }
        public virtual DbSet<Department> Departments { get; set; }
    
        public virtual int ADD_OPDEXPENSE_IMAGE(Nullable<int> oPDEXPENSE_ID, string iMAGE_NAME, string iMAGE_EXT, string iMAGE_BASE64, string nAME_EXPENSES, Nullable<decimal> eXPENSE_AMOUNT)
        {
            var oPDEXPENSE_IDParameter = oPDEXPENSE_ID.HasValue ?
                new ObjectParameter("OPDEXPENSE_ID", oPDEXPENSE_ID) :
                new ObjectParameter("OPDEXPENSE_ID", typeof(int));
    
            var iMAGE_NAMEParameter = iMAGE_NAME != null ?
                new ObjectParameter("IMAGE_NAME", iMAGE_NAME) :
                new ObjectParameter("IMAGE_NAME", typeof(string));
    
            var iMAGE_EXTParameter = iMAGE_EXT != null ?
                new ObjectParameter("IMAGE_EXT", iMAGE_EXT) :
                new ObjectParameter("IMAGE_EXT", typeof(string));
    
            var iMAGE_BASE64Parameter = iMAGE_BASE64 != null ?
                new ObjectParameter("IMAGE_BASE64", iMAGE_BASE64) :
                new ObjectParameter("IMAGE_BASE64", typeof(string));
    
            var nAME_EXPENSESParameter = nAME_EXPENSES != null ?
                new ObjectParameter("NAME_EXPENSES", nAME_EXPENSES) :
                new ObjectParameter("NAME_EXPENSES", typeof(string));
    
            var eXPENSE_AMOUNTParameter = eXPENSE_AMOUNT.HasValue ?
                new ObjectParameter("EXPENSE_AMOUNT", eXPENSE_AMOUNT) :
                new ObjectParameter("EXPENSE_AMOUNT", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("ADD_OPDEXPENSE_IMAGE", oPDEXPENSE_IDParameter, iMAGE_NAMEParameter, iMAGE_EXTParameter, iMAGE_BASE64Parameter, nAME_EXPENSESParameter, eXPENSE_AMOUNTParameter);
        }
    
        public virtual ObjectResult<GET_OPDEXPENSE_IMAGE_DETAILS_Result> GET_OPDEXPENSE_IMAGE_DETAILS(Nullable<int> iMAGE_ID)
        {
            var iMAGE_IDParameter = iMAGE_ID.HasValue ?
                new ObjectParameter("IMAGE_ID", iMAGE_ID) :
                new ObjectParameter("IMAGE_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_OPDEXPENSE_IMAGE_DETAILS_Result>("GET_OPDEXPENSE_IMAGE_DETAILS", iMAGE_IDParameter);
        }
    
        public virtual int DELETE_OPDEXPENSE_IMAGE(Nullable<int> iMAGE_ID)
        {
            var iMAGE_IDParameter = iMAGE_ID.HasValue ?
                new ObjectParameter("IMAGE_ID", iMAGE_ID) :
                new ObjectParameter("IMAGE_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DELETE_OPDEXPENSE_IMAGE", iMAGE_IDParameter);
        }
    
        public virtual int sp_insert_file(string iMAGE_NAME, string iMAGE_EXT, string iMAGE_BASE64, string nAME_EXPENSES, Nullable<decimal> eXPENSE_AMOUNT)
        {
            var iMAGE_NAMEParameter = iMAGE_NAME != null ?
                new ObjectParameter("IMAGE_NAME", iMAGE_NAME) :
                new ObjectParameter("IMAGE_NAME", typeof(string));
    
            var iMAGE_EXTParameter = iMAGE_EXT != null ?
                new ObjectParameter("IMAGE_EXT", iMAGE_EXT) :
                new ObjectParameter("IMAGE_EXT", typeof(string));
    
            var iMAGE_BASE64Parameter = iMAGE_BASE64 != null ?
                new ObjectParameter("IMAGE_BASE64", iMAGE_BASE64) :
                new ObjectParameter("IMAGE_BASE64", typeof(string));
    
            var nAME_EXPENSESParameter = nAME_EXPENSES != null ?
                new ObjectParameter("NAME_EXPENSES", nAME_EXPENSES) :
                new ObjectParameter("NAME_EXPENSES", typeof(string));
    
            var eXPENSE_AMOUNTParameter = eXPENSE_AMOUNT.HasValue ?
                new ObjectParameter("EXPENSE_AMOUNT", eXPENSE_AMOUNT) :
                new ObjectParameter("EXPENSE_AMOUNT", typeof(decimal));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("sp_insert_file", iMAGE_NAMEParameter, iMAGE_EXTParameter, iMAGE_BASE64Parameter, nAME_EXPENSESParameter, eXPENSE_AMOUNTParameter);
        }
    
        public virtual int DELETE_OPDEXPENSE(Nullable<int> oPDEXPENSE_ID)
        {
            var oPDEXPENSE_IDParameter = oPDEXPENSE_ID.HasValue ?
                new ObjectParameter("OPDEXPENSE_ID", oPDEXPENSE_ID) :
                new ObjectParameter("OPDEXPENSE_ID", typeof(int));
    
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction("DELETE_OPDEXPENSE", oPDEXPENSE_IDParameter);
        }
    
        public virtual ObjectResult<GET_OPDEXPENSE_IMAGE_Result> GET_OPDEXPENSE_IMAGE()
        {
            return ((IObjectContextAdapter)this).ObjectContext.ExecuteFunction<GET_OPDEXPENSE_IMAGE_Result>("GET_OPDEXPENSE_IMAGE");
        }
    }
}
