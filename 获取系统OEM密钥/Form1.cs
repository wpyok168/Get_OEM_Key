using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Windows.Forms.VisualStyles;

namespace 获取系统OEM密钥
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            string sql = "Select OA3xOriginalProductKey From SoftwareLicensingService Where OA3xOriginalProductKey is not null";
            string oemkey = string.Empty;
            this.richTextBox1.SelectionAlignment = HorizontalAlignment.Center;

            using (ManagementObjectSearcher searcher = new ManagementObjectSearcher(sql))
            {
                using (ManagementObjectCollection mbo = searcher.Get())
                {
                    if (mbo.Count > 0)
                    {
                        foreach (ManagementBaseObject item in mbo)
                        {
                            ManagementObject mo = item as ManagementObject;
                           oemkey = mo.GetPropertyValue("OA3xOriginalProductKey").ToString();
                        }
                    }
                }
            }
            if (string.IsNullOrEmpty(oemkey))
            {
                this.richTextBox1.ForeColor=System.Drawing.Color.Red;
                this.richTextBox1.Text = "\r\n" + "系统不存在OEM密钥";
            }
            else
            {
                this.richTextBox1.ForeColor = System.Drawing.Color.Green;
                this.richTextBox1.Text = "系统OEM密钥：\r\n"+oemkey;
            }
        }
    }
}
