using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConverterUPS.Model
{
    public class Df_Model
    {
        public string FileName { get; set; }
        public string FileID { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string LCSI { get; set; }

        public string AID { get; set; }
        //security
        public string DeleteSelf { get; set; }
        public string TerminateDF { get; set; }
        public string Activate { get; set; }
        public string Deactivate { get; set; }
        public string CreateChildDF { get; set; }
        public string CreateChildEF { get; set; }
        public string DeleteChild { get; set; }
        public string EFArrID { get; set; }
        public string EFArrRecordNb { get; set; }
        public string Shareable { get; set; }

        public List<Df_Model> DFs { get; set; }
        public List<Ef_Model> EFs { get; set; }

    }
}
