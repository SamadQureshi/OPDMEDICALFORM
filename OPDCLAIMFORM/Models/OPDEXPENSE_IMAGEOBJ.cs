using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPDCLAIMFORM.Models
{
    public class OPDEXPENSE_IMAGEOBJ
    {
        #region Properties

        /// <summary>
        /// Gets or sets Image ID.
        /// </summary>
        public int FileId { get; set; }

        /// <summary>
        /// Gets or sets Image name.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets Image extension.
        /// </summary>
        public string FileContentType { get; set; }

        public string ExpenseName { get; set; }

        public decimal? ExpenseAmount { get; set; }

        public int? OPDExpense_id { get; set; }

        public string OPDType { get; set; }

        #endregion


    }
}