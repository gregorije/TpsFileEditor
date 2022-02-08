using System;
using System.Collections.Generic;
using System.Configuration;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Linq;


namespace TpsFileEditor
{
    public partial class Form1 : Form
    {
        private List<String> ServerRoots { get; set; } 
        public Form1()
        {
            this.StartPosition = FormStartPosition.CenterScreen;
            InitializeComponent();
            getRootsFromTxt();
            ServerRoots = getRootsFromTxt();
            FillListElements(ServerRoots);
            
        }
        private void FillListElements(List<String> serverRoots)
        {
            foreach (var item in serverRoots)
            {
                this.ddlRoot.AddItem(item);
            }
        }
        private List<String> getRootsFromTxt()
        {
            List<String> returnList = new List<string>();

            var lines = File.ReadLines(@"C:\Program Files (x86)\HP Inc\TpsFileEditorSetup\roots.txt");
            foreach (var line in lines)
                returnList.Add(line);
            
            return returnList;
        }

        private void bunifuThinButton21_Click(object sender, EventArgs e)
        {
            if(proveriPolja() && proveriInstalaciju())
            {
                popuniLastLockerStatusStateFile();
                overridePaketomatId();
                overrideNumberOfLockersOnPacketMachine();
                overrideServerRoot();
                MessageBox.Show("Izmena je uspesno izvrsena!");
            }
        }

        private bool proveriPolja()
        {
            if(tbPakId.Text == "" || tbBrVrata.Text == "" || ddlRoot.selectedIndex==-1)
            {
                MessageBox.Show("Popuni sva polja!");
                return false;
            }
            return true;
        }
        
        private bool proveriInstalaciju()
        {
            string putanja = @"C:\Program Files (x86)\HP Inc\TpsApp\TpServisProject.exe";
            if (!File.Exists(putanja))
            {
                MessageBox.Show("Aplikacija nije instalirana, instaliraj aplikaciju!");
                return false;
            }
            return true;
        }

        private void popuniLastLockerStatusStateFile()
        {
            string xmlFile = @"C:\Program Files (x86)\HP Inc\TpsApp\PackageMachineControl\LastLockersStatusState.xml";
            System.Xml.XmlDocument xmlDoc = new System.Xml.XmlDocument();
            xmlDoc.Load(xmlFile);

            xmlDoc.SelectSingleNode("root/BreakdownStatuses").InnerText = ispisiSlovo("Z,");
            xmlDoc.SelectSingleNode("root/BreakdownAdditionalStatuses").InnerText = ispisiSlovo("N,");

            xmlDoc.Save(xmlFile);

        }
        private string ispisiSlovo(string slovo)
        {
            var brojVrata = int.Parse(tbBrVrata.Text);
            string text = "";
            for (int i = 0; i < brojVrata; i++)
            {
                text += slovo;
            }
            return text;
        }

        private void overridePaketomatId()
        {
            XDocument doc = XDocument.Load(@"C:\Program Files (x86)\HP Inc\TpsApp\TpServisProject.exe.config");
            var list = from appNode in doc.Descendants("appSettings").Elements()
                       where appNode.Attribute("key").Value == "PaketomatID"
                       select appNode;
            var e = list.FirstOrDefault();

            e.Attribute("value").SetValue(tbPakId.Text);

            doc.Save(@"C:\Program Files (x86)\HP Inc\TpsApp\TpServisProject.exe.config");
        }
        private void overrideNumberOfLockersOnPacketMachine()
        {
            XDocument doc = XDocument.Load(@"C:\Program Files (x86)\HP Inc\TpsApp\TpServisProject.exe.config");
            var list = from appNode in doc.Descendants("appSettings").Elements()
                       where appNode.Attribute("key").Value == "NumberOfLockersOnPacketMachine"
                       select appNode;
            var e = list.FirstOrDefault();

            e.Attribute("value").SetValue(tbBrVrata.Text);

            doc.Save(@"C:\Program Files (x86)\HP Inc\TpsApp\TpServisProject.exe.config");
        }
        private void overrideServerRoot()
        {
            XDocument doc = XDocument.Load(@"C:\Program Files (x86)\HP Inc\TpsApp\TpServisProject.exe.config");
            var list = from appNode in doc.Descendants("appSettings").Elements()
                       where appNode.Attribute("key").Value == "baseUrl"
                       select appNode;
            var e = list.FirstOrDefault();

            e.Attribute("value").SetValue(ddlRoot.selectedValue);

            doc.Save(@"C:\Program Files (x86)\HP Inc\TpsApp\TpServisProject.exe.config");
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
