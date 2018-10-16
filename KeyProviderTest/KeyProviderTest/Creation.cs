using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using KeePassLib.Keys;

namespace KeyProviderTest.Forms
{
    public partial class Creation : Form
    {
        
        private Info m_Info = null;
        private KeyProviderQueryContext m_kpContext = null;
        public void InitEx(Info Info,KeyProviderQueryContext ctx)
        {

            m_Info = Info;
            m_kpContext = ctx;

        }
        public Creation()
        {
            InitializeComponent();
        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            
            m_Info.Secret = Encoding.UTF8.GetBytes(textBox1.Text);
            
            Close();
        }
    }
}
