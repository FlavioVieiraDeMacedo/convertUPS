using ConverterUPS.Model;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;

namespace ConverterUPS
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            XmlDocument doc = new XmlDocument();
            doc.Load("C:\\Users\\Flavio\\Desktop\\work-GD\\ARAMCL110657V2.01.ups");

            XmlNode MF = doc.SelectSingleNode("/ProfileRoot/Profile/ProfileBody/ChipVariant/CardContent/FileSystem/File");
            XmlNamespaceManager ns = new XmlNamespaceManager(doc.NameTable);
            ns.AddNamespace("usim", "http://www.gieseckedevrient.com/USIM");


            XmlNodeList initialNodes= doc.SelectNodes("/ProfileRoot/Profile/ProfileBody/ChipVariant/CardContent/FileSystem");


            List<Df_Model> DFs = GetMFs(initialNodes);
            string a = MontarArquivo(DFs);
            
            a = a.Replace("Always", "ALW");
            a = a.Replace("Never", "NEV");
            a = a.Replace("Second PIN Appl 1", "LPIN1");
            a = a.Replace("PIN Appl 1", "GPIN1");
            File.WriteAllText(@"C:\Users\Flavio\Desktop\work-GD\teste.xml", a);


        }


        public static string MontarArquivo(List<Df_Model> files)
        {
            string xml = "";
            foreach (var item in files)
            {
                xml += MontarDF(item);
                foreach (var item1 in item.EFs)
                {
                    xml += MontarEF(item1);

                }
                if (item.DFs.Count > 0)
                {
                    xml+= MontarArquivo(item.DFs);
                }
            }
            return xml;
        }
        public static string MontarDF(Df_Model DF)
        {
            String xml = "";
            xml = "\n<MF_DF FileName=\"" + DF.FileName + "\" FileID = \"" + DF.FileID + "\" FileType = \"" + DF.FileType + "\" FilePath = \"" + DF.FilePath + "\" LCSI = \"" + DF.LCSI + "\" >";
            xml += "\n\t <AccessConditions3G>";
            xml += "\n\t\t <DFAccessConditions3GType>";
            xml += "\n\t\t\t<DeleteSelf>" + DF.DeleteSelf + "</DeleteSelf>";
            xml += "\n\t\t\t<TerminateDF>" + DF.TerminateDF + "</TerminateDF>";
            xml += "\n\t\t\t<Activate>" + DF.Activate + "</Activate>";
            xml += "\n\t\t\t<Deactivate>" + DF.Deactivate + "</Deactivate>";
            xml += "\n\t\t\t<CreateChildDF>" + DF.CreateChildDF + "</CreateChildDF>";
            xml += "\n\t\t\t<CreateChildEF>" + DF.CreateChildEF + "</CreateChildEF>";
            xml += "\n\t\t\t<DeleteChild>" + DF.DeleteChild + "</DeleteChild>";
            xml += "\n\t\t\t<EFArrID>" + DF.EFArrID + "</EFArrID>";
            xml += "\n\t\t\t<EFArrRecordNb>" + DF.EFArrRecordNb + "</EFArrRecordNb>";
            xml += "\n\t\t\t<Shareable>" + DF.Shareable + "</Shareable>";
            xml += "\n\t\t</DFAccessConditions3GType>";
            xml += "\n\t</AccessConditions3G>";
            xml += "\n </MF_DF>";

            return xml;
        }

        public static string MontarEF(Ef_Model EF)
        {
            String xml = "";
            if (EF.FileType=="Link")
            {
                xml += "\n<EF FileName=\"" + EF.FileName + "\" FileID=\"" + EF.FileID + "\" FileType=\"" + EF.FileType + "\" FilePath=\"" + EF.FilePath + "\" LCSI=\"" + EF.LCSI + "\" LinkFilePath=\"" + EF.LinkFilePath+ "\">";
                xml += "\n\t <AccessConditions3G>";
                xml += "\n\t\t <EFAccessConditions3GType>";
                xml += "\n\t\t\t <Read>" + EF.Read + "</Read>";
                xml += "\n\t\t\t <Update>" + EF.Update + "</Update>";
                xml += "\n\t\t\t <Resize>" + EF.Resize + "</Resize>";
                xml += "\n\t\t\t <Activate>" + EF.Activate + "</Activate>";
                xml += "\n\t\t\t <Deactivate>" + EF.Deactivate + "</Deactivate>";
                xml += "\n\t\t\t <DeleteItself>" + EF.DeleteItself + "</DeleteItself>";
                xml += "\n\t\t\t <EFArrID>" + EF.EFArrID + "</EFArrID>";
                xml += "\n\t\t\t <EFArrRecordNb>" + EF.EFArrRecordNb + "</EFArrRecordNb>";
                xml += "\n\t\t\t <Readable>" + EF.Readable + "</Readable>";
                xml += "\n\t\t\t <Shareable>" + EF.Shareable + "</Shareable>";
                xml += "\n\t\t </EFAccessConditions3GType>";
                xml += "\n\t\t </AccessConditions3G>";
                xml += "\n </EF>";
            }
            else
            {
                xml += "\r\n<EF FileName=\"" + EF.FileName+ "\" FileID=\"" + EF.FileID+ "\" FileType=\"" + EF.FileType+ "\" FilePath=\"" + EF.FilePath+ "\" LCSI=\"" + EF.LCSI+ "\">";
                xml += "\r\n\t <AccessConditions3G>";
                xml += "\r\n\t\t <EFAccessConditions3GType>";
                xml += "\r\n\t\t\t <Read>" + EF.Read + "</Read>";
                xml += "\r\n\t\t\t <Update>" + EF.Update + "</Update>";
                xml += "\r\n\t\t\t <Resize>" + EF.Resize + "</Resize>";
                xml += "\r\n\t\t\t <Activate>" + EF.Activate + "</Activate>";
                xml += "\r\n\t\t\t <Deactivate>" + EF.Deactivate + "</Deactivate>";
                xml += "\r\n\t\t\t <DeleteItself>" + EF.DeleteItself + "</DeleteItself>";
                xml += "\r\n\t\t\t <EFArrID>" + EF.EFArrID + "</EFArrID>";
                xml += "\r\n\t\t\t <EFArrRecordNb>" + EF.EFArrRecordNb + "</EFArrRecordNb>";
                xml += "\r\n\t\t\t <Readable>" + EF.Readable + "</Readable>";
                xml += "\r\n\t\t\t <Shareable>" + EF.Shareable + "</Shareable>";
            
                xml += "\r\n\t\t </EFAccessConditions3GType>";
                xml += "\r\n\t </AccessConditions3G>";
                xml += "\r\n\t <EFContent>";
                xml += "\r\n\t\t <EFContentType NbOfRecords=\"" + EF.NbOfRecords+ "\" RecordSize=\"" + EF.RecordSize+ "\" DataGenerationType=\"" + EF.DataGenerationType+ "\">";
                foreach (var item in EF.DataValues)
                {
                    xml += "\r\n\t\t\t <DataValue>" + item+"</DataValue>";
                }
                xml += "\r\n\t\t </EFContent>";
                xml += "\r\n\t </EF>";
            }
            return xml;
        }



        public static List<Df_Model> GetMFs(XmlNodeList StartNode)
        {
            List<Df_Model> superDFs = new List<Df_Model>();

            foreach (XmlNode item in StartNode)
            {
                Df_Model MF = new Df_Model();
                MF.EFs = GetEFs(item);
                MF.DFs = GetDFs(item);
                superDFs.Add(MF);
            }
            return superDFs;
        }

       

        public static List<Df_Model> GetDFs(XmlNode StartNode)
        {
            
            var DFs = new List<Df_Model>();

            foreach (XmlNode item in StartNode.ChildNodes)
            {
                XmlNodeList itens = item.ChildNodes;
                foreach (XmlNode item1 in itens)
                {
                    if (item1.Name== "FileHeader")
                    {
                        Df_Model DF = new Df_Model();
                        DF.FileID = item1.Attributes["ID"].InnerText;
                        DF.FileName = item1.Attributes["AliasName"].InnerText;
                        DF.FilePath = item1.Attributes["ID"].InnerText;
                        DF.LCSI = "Operational";
                        DF.FileType = DF.FileID == "3F00" ? "MF" : "DF";
                        DF.Shareable = "true";
                        DF.TerminateDF = "NEV";
                        XmlNodeList itens1 = item1.ChildNodes;
                        foreach (XmlNode item2 in itens1)
                        {
                            if (item2.Name== "usim:HeaderExt")
                            {
                                if (item2.Attributes["FileType"].Value =="DF")
                                {
                                    DF.EFs = GetEFs(item);
                                    DF.DFs = GetDFs(item);

                                    DFs.Add(DF);
                                }
                            }
                            if (item2.Name == "usim:AttributeLists")
                            {
                                XmlNodeList atributes = item2.FirstChild.ChildNodes;
                                foreach (XmlNode atribute in atributes)
                                {
                                    XmlNodeList at = atribute.ChildNodes;
                                    XmlNodeList a = at[0].ChildNodes;
                                    switch (a[1].InnerText)
                                    {
                                        case "AC_Create":
                                            DF.CreateChildEF = a[0].InnerText;
                                            break;
                                        case "AC_Create_DF":
                                            DF.CreateChildDF = a[0].InnerText;
                                            break;
                                        case "AC_Delete":
                                            DF.DeleteSelf = a[0].InnerText;
                                            break;
                                        case "AC_Delete_Child":
                                            DF.DeleteChild = a[0].InnerText;
                                            break;
                                        case "AC_Activate":
                                            DF.Activate = a[0].InnerText;
                                            break;
                                        case "AC_Deactivate":
                                            DF.Deactivate = a[0].InnerText;
                                            break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            return DFs;
        }


        
        public static List<Ef_Model> GetEFs(XmlNode StartNode)
        {
            
            XmlNodeList files = StartNode.ChildNodes;

            List<Ef_Model> Efs = new List<Ef_Model>();
            string father = "";// StartNode.FirstChild.Attributes["ID"].Value;
            

            foreach (XmlNode file in files)
            {
                if (file.Name == "File")
                {
                    XmlNodeList items1 = file.ChildNodes;

                    Ef_Model Ef = new Ef_Model();
                    Ef.FatherFile = father;
                    bool isEf = false;
                    bool  EfLink= false;
                    List<string> content = new List<string>();
                    foreach (XmlNode item1 in items1)
                    {//fileheader(ef)
                        
                        
                        if (item1.Name == "FileHeader")
                        {
                            XmlNodeList items2 = item1.ChildNodes;
                            
                            foreach (XmlNode item2 in items2)
                            {
                                if (item2.Name == "usim:LinkInformation")
                                {
                                    if (item2.FirstChild!=null)
                                    {
                                        if (item2.FirstChild.Name == "Outgoing")
                                        {
                                            XmlNodeList list = item2.FirstChild.ChildNodes;
                                            EfLink = true;
                                            foreach (XmlNode item in list)
                                            {
                                                if (item.Name == "FID")
                                                {
                                                    Ef.LinkFilePath += item.InnerText;
                                                }
                                                if (item.Name == "AID")
                                                {
                                                    Ef.FileParentAID = item.InnerText;
                                                }
                                            }
                                        }
                                    }
                                }
                                
                                if (item2.Name == "usim:HeaderExt")
                                {
                                    if (item2.Attributes["FileType"].Value == "EF")
                                    {
                                        isEf = true;

                                        Ef.FileName = item1.Attributes["AliasName"] != null ? item1.Attributes["AliasName"].Value : "null";
                                        
                                        Ef.FileID = item1.Attributes["ID"] != null ? item1.Attributes["ID"].Value : "null";
                                        Ef.FilePath = father + Ef.FileID;
                                        //Ef.FilePath = 
                                        Ef.SFI = item1.Attributes["SFI"] != null ? item1.Attributes["SFI"].Value : "null";
                                        Ef.FileType = item2.Attributes["EFStructure"] != null ? item2.Attributes["EFStructure"].Value : "null";
                                        Ef.LCSI = item2.Attributes["LCSI"] != null ? item2.Attributes["LCSI"].Value : "null";
                                        Ef.RecordSize = item1.Attributes["Record_Size"] != null ? item1.Attributes["Record_Size"].Value : "null";
                                        Ef.NbOfRecords = item1.Attributes["Record_Count"] != null ? item1.Attributes["Record_Count"].Value : "null";


                                    }
                                }

                                if (item2.Name == "usim:AttributeLists")
                                {
                                    XmlNodeList list = item2.FirstChild.ChildNodes;
                                    foreach (XmlNode item in list)
                                    {
                                        XmlNodeList a = item.FirstChild.ChildNodes;

                                        switch (a[1].InnerText)
                                        {
                                            //public string EFArrID { get; set; }
                                            //public string EFArrRecordNb { get; set; }

                                            case "AC_Read":
                                                Ef.Read = a[0].InnerText;
                                                break;
                                            case "AC_Update":
                                                Ef.Update = a[0].InnerText;
                                                break;
                                            case "AC_Delete":
                                                Ef.DeleteItself = a[0].InnerText;
                                                break;
                                            case "AC_Activate":
                                                Ef.Activate = a[0].InnerText;
                                                break;
                                            case "AC_Deactivate":
                                                Ef.Deactivate = a[0].InnerText;
                                                break;
                                            case "AC_Resize":
                                                Ef.Resize = a[0].InnerText;
                                                break;
                                        }
                                        Ef.Readable = "false";
                                        Ef.Shareable = "true";
                                    }
                                }

                                if (item2.Name == "Status")
                                {
                                    XmlNodeList list = item2.ChildNodes;
                                    foreach (XmlNode item in list)
                                    {
                                        if (item.Name == "usim:Production")
                                        {
                                            Ef.DataGenerationType = item.Attributes["DataMustBeGenerated"] !=null? item.Attributes["DataMustBeGenerated"].Value:"null";
                                            Ef.DataGenerationType = Ef.DataGenerationType == "false" ? "Static" : "Dynamic";
                                        }
                                    }
                                }

                                if (item2.Name == "AccessCondition")
                                {
                                    XmlNodeList list = item2.ChildNodes;
                                    foreach (XmlNode item in list)
                                    {
                                        if (item.Name == "usim:AccessConditionReference")
                                        {
                                            XmlNodeList list2 = item.ChildNodes;
                                            foreach (XmlNode lastItem in list2)
                                            {
                                                if (lastItem.Name == "FileID")
                                                {
                                                    Ef.EFArrID = lastItem.InnerText;
                                                }
                                                if (lastItem.Name == "RecNo")
                                                {
                                                    Ef.EFArrRecordNb = lastItem.InnerText;
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }

                        if (item1.Name == "Item")
                        {
                            XmlNodeList items2 = item1.ChildNodes;
                            foreach (XmlNode item2 in items2)
                            {
                                if (item2.Name == "ItemData")
                                {
                                    content.Add(item2.InnerText);
                                }
                            }
                            Ef.DataValues = content;
                        }
                        

                    }
                    if (EfLink)
                    {
                        Ef.FileType = "Link";
                    }
                    if (isEf)
                    {
                        Efs.Add(Ef);
                    }
                }

            }
            return Efs;
        }

    }
    
}
