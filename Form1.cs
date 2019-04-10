using ConverterUPS.BO;
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
            string upsPath = txtPathUPS.Text;
            try
            {
                doc.Load( upsPath);
            }
            catch (Exception)
            {
                MessageBox.Show("Error on UPS path!");
            }

            //try
            //{
                XmlNodeList initialNodes = doc.SelectNodes("/ProfileRoot/Profile/ProfileBody/ChipVariant/CardContent/FileSystem");
                List<Df_Model> DFs = Operations.GetMFs(initialNodes);
                string a = Operations.MontarArquivo(DFs);

                a = a.Replace("Always", "ALW");
                a = a.Replace("Never", "NEV");
                a = a.Replace("Second PIN Appl 1", "LPIN1");
                a = a.Replace("PIN Appl 1", "GPIN1");
                a = a.Replace("Transparent", "TR");
                a = a.Replace("Linear Fixed", "LF");
                var nameDocWithExt = upsPath.Split('\\');
                var nameDoc = nameDocWithExt[nameDocWithExt.Count() - 1].Split('.')[0];

                string savePath = txtSavePath.Text +"\\"+ nameDoc +".ePML";
                File.WriteAllText(savePath, a);
                //File.WriteAllText(@"C:\Users\macedofl\Desktop\FilesConvertUPSto\teste.xml", a);
                MessageBox.Show("Success !");
            //}
            //catch (Exception)
            //{
            //    MessageBox.Show("Error on save the document!");
            //}
        }


       


        
       

       
        
        
    }
    
}
