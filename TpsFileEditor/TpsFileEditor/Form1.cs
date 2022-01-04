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

namespace TpsFileEditor
{
    public partial class Form1 : Form
    {
        private List<String> ServerRoots { get; set; } 
        public Form1()
        {
            InitializeComponent();
            getRootsFromTxt();
            ServerRoots = getRootsFromTxt();
            
        }
        private List<String> getRootsFromTxt()
        {
            List<String> returnList = new List<string>();

            var lines = File.ReadLines(@"C:\Users\user\source\repos\TpsFileEditor\TpsFileEditor\roots.txt");
            foreach (var line in lines)
                returnList.Add(line);
            
            return returnList;
        }
    }
}
