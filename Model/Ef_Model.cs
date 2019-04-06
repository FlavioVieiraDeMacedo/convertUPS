using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConverterUPS.Model
{
    public class Ef_Model
    {
        public string FatherFile{ get; set; }

        public string FileName { get; set; }
        public string FileID { get; set; }
        public string FileType { get; set; }
        public string FilePath { get; set; }
        public string SFI { get; set; }
        public string LCSI { get; set; }
        
        //acess
        public string Read { get; set; }
        public string Update { get; set; }
        public string Resize { get; set; }
        public string Activate { get; set; }
        public string Deactivate { get; set; }
        public string DeleteItself { get; set; }
        public string EFArrID { get; set; }
        public string EFArrRecordNb { get; set; }
        public string Readable { get; set; }
        public string Shareable { get; set; }

        //content
        public string DataGenerationType { get; set; }
        

        public string NbOfRecords { get; set; }
        public string RecordSize { get; set; }
        public List<string> DataValues { get; set; }

        public string LinkFilePath { get; set; }
        public string FileParentAID { get; set; }
    }
}
