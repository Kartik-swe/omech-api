using Microsoft.AspNetCore.DataProtection.KeyManagement;
using System.Collections.Generic;
using System.Reflection.Metadata;
using System.Xml.Linq;

namespace omech.Models
{
    // paramter of class  IU_FLAG, USERNAME, F_NAME, L_NAME, USER_TYPE, PASSWORD, EMAIL, CONTACT_NO, IS_ACTIVE, CREATE_BY, USER_SRNO 
    public class IuUserModel : ComParaModel
    {
        public char IU_FLAG { get; set; }
        public string USERNAME { get; set; }
        public string F_NAME { get; set; }
        public string? L_NAME { get; set; }
        public int USER_TYPE {  get; set; }
        public string? PASSWORD {  get; set; }
        public string? EMAIL { get; set; }
        public string? CONTACT_NO { get; set; }
        public int IS_ACTIVE { get; set; }
        public int? USER_SRNO_PK { get; set; }


    }
}


