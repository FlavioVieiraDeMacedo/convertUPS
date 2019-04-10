using ConverterUPS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ConverterUPS.BO
{
    
    public static class Operations
    {
        

        public static List<Ef_Model> GetEFs(XmlNode StartNode)
        {

            XmlNodeList files = StartNode.ChildNodes;

            List<Ef_Model> Efs = new List<Ef_Model>();
            string father = "";


            foreach (XmlNode file in files)
            {
                if (file.Name == "File")
                {
                    XmlNodeList items1 = file.ChildNodes;

                    Ef_Model Ef = new Ef_Model();
                    Ef.FatherFile = father;
                    bool isEf = false;
                    bool EfLink = false;
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
                                    if (item2.FirstChild != null)
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
                                        //Ef.FilePath = DF_File_Path+Ef.FileID;
                                        if (item1.Attributes["useSFI"] != null)
                                        {
                                            if (item1.Attributes["useSFI"].Value == "true")
                                            {
                                                Ef.SFI = item1.Attributes["SFI"] != null ? item1.Attributes["SFI"].Value : "null";
                                            }
                                            else
                                            {
                                                Ef.SFI = "null";
                                            }
                                        }
                                        else
                                        {
                                            Ef.SFI = "null";
                                        }
                                        //Ef.SFI = item1.Attributes["SFI"] != null ? item1.Attributes["SFI"].Value : "null";
                                        Ef.FileType = item2.Attributes["EFStructure"] != null ? item2.Attributes["EFStructure"].Value : "null";
                                        //Ef.LCSI = item2.Attributes["LCSI"] != null ? item2.Attributes["LCSI"].Value : "null";
                                        Ef.LCSI = "Operational";
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
                                            Ef.DataGenerationType = item.Attributes["DataMustBeGenerated"] != null ? item.Attributes["DataMustBeGenerated"].Value : "null";
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

        public static List<Df_Model> GetDFs(XmlNode StartNode)
        {

            var DFs = new List<Df_Model>();

            foreach (XmlNode item in StartNode.ChildNodes)
            {
                XmlNodeList itens = item.ChildNodes;
                foreach (XmlNode item1 in itens)
                {
                    if (item1.Name == "FileHeader")
                    {
                        Df_Model DF = new Df_Model();
                        DF.FileID = item1.Attributes["ID"].InnerText;
                        DF.FileName = item1.Attributes["AliasName"].InnerText;
                        DF.FilePath = item1.Attributes["ID"].InnerText;
                        //DF.FilePath = DF_File_Path+DF.FileID;
                        //DF_File_Path = DF.FilePath;
                        DF.LCSI = "Operational";
                        DF.FileType = DF.FileID == "3F00" ? "MF" : "DF";
                        DF.Shareable = "true";
                        DF.TerminateDF = "NEV";
                        XmlNodeList itens1 = item1.ChildNodes;
                        foreach (XmlNode item2 in itens1)
                        {
                            if (item2.Name== "usim:AID")
                            {
                                DF.AID = item2.InnerText;
                            }
                            if (item2.Name == "usim:HeaderExt")
                            {
                                if (item2.Attributes["FileType"].Value == "DF")
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
                            if (item2.Name == "AccessCondition")
                            {
                                XmlNodeList list = item2.ChildNodes;
                                foreach (XmlNode item3 in list)
                                {
                                    if (item3.Name == "usim:AccessConditionReference")
                                    {
                                        XmlNodeList list2 = item3.ChildNodes;
                                        foreach (XmlNode lastItem in list2)
                                        {
                                            if (lastItem.Name == "FileID")
                                            {
                                                DF.EFArrID = lastItem.InnerText;
                                            }
                                            if (lastItem.Name == "RecNo")
                                            {
                                                DF.EFArrRecordNb = lastItem.InnerText;
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                }
            }

            return DFs;
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
                    xml += MontarArquivo(item.DFs);
                }
            }
            return xml;
        }

        public static string MontarDF(Df_Model DF)
        {
            String xml = "";
            if (DF.FileID != "" && DF.FileID != "null" && DF.FileID != null)
            {
                //AID = "A0000000871002FF54F00189000001FF";
                if (DF.AID!=null && DF.AID!="")
                {
                    xml = "\n<MF_DF FileName=\"" + DF.FileName + "\" FileID=\"" + DF.FileID + "\" FileType=\"" + DF.FileType + "\" FilePath=\"" + DF.FilePath + "\" LCSI =\"" + DF.LCSI + "\" AID=" + DF.AID + "\">";
                }
                else
                {
                    xml = "\n<MF_DF FileName=\"" + DF.FileName + "\" FileID=\"" + DF.FileID + "\" FileType=\"" + DF.FileType + "\" FilePath=\"" + DF.FilePath + "\" LCSI =\"" + DF.LCSI + "\">";
                }
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
            }
            else
            {

            }
            return xml;
        }

        public static string MontarEF(Ef_Model EF)
        {
            String xml = "";
            if (EF.FileID != "" && EF.FileID != "null" && EF.FileID != null)
            {
                
                var varFileName = " EF FileName=\"" + EF.FileName + "\"";
                var varFileID = " FileID=\"" + EF.FileID + "\"";
                var varFileType = " FileType=\"" + EF.FileType + "\"";
                var varFilePath = " FilePath=\"" + EF.FilePath + "\"";
                var varLCSI = " LCSI=\"" + EF.LCSI + "\"";


                /*var varFileParentAID = "";
                if (EF.FileParentAID != null && EF.FileParentAID != "null")
                {
                     varFileParentAID = " FileParentAID=\"" + EF.FileParentAID + "\"";
                }*/

                var varLinkFilePath = "";
                if (EF.FileType == "Link")
                {
                     varLinkFilePath = " LinkFilePath=\"" + EF.LinkFilePath + "\"";
                }

                var varSFI = "";
                if (EF.SFI != "null"&& EF.SFI != null)
                {
                     varSFI = " SFI=\"" + EF.SFI + "\"";
                }
            
                xml = "\n<" + varFileName + varFileID + varFileType + varFilePath + varLCSI + varLinkFilePath + varSFI + ">";
                
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

                if (EF.FileType != "Link")
                {
                    string Content = "";
                    if (EF.NbOfRecords == "null")
                    {
                        Content = "\r\n\t\t <EFContentType FileSize=\"" + EF.RecordSize + "\" DataGenerationType=\"" + EF.DataGenerationType + "\">";
                    }
                    else
                    {
                        Content = "\r\n\t\t <EFContentType NbOfRecords=\"" + EF.NbOfRecords + "\" RecordSize=\"" + EF.RecordSize + "\" DataGenerationType=\"" + EF.DataGenerationType + "\">";
                    }

                    xml += "\r\n\t <EFContent>";
                    xml += Content;

                    foreach (var item in EF.DataValues)
                    {
                        xml += "\r\n\t\t\t <DataValue>" + item + "</DataValue>";
                    }

                    xml += "\r\n\t\t </EFContentType>";
                    xml += "\r\n\t</EFContent>";
                }

                xml += "</EF>";

                
            }
            else
            {

            }
            return xml;
        }

    }
}
