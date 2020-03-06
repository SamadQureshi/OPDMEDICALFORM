using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace OPDCLAIMFORM.Models
{
    public class OPDEXPENSE_MASTERDETAIL
    {
        public OPDEXPENSE  opdEXPENSE { get; set; }

        public List<OPDEXPENSE_PATIENT> listOPDEXPENSEPATIENT { get; set; }

        public List<OPDEXPENSE_IMAGE> listOPDEXPENSEIMAGE { get; set; }



    }
}