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
    public partial class Login : Form
    {
        private Info m_Info = null;
        private KeyProviderQueryContext m_kpContext = null;
        private byte[] m_bmode = null;
        public void InitEx(Info Info,KeyProviderQueryContext ctx)
        {

            m_Info = Info;
            m_kpContext = ctx;
            

        }
        public byte[] Access()
        {
            return m_bmode;
        }
        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            string str = textBox1.Text.ToString();
            byte[] by = Encoding.UTF8.GetBytes(str);

            m_bmode = by;
            Close();

        }

        private void Form1_Load(object sender, EventArgs e)
        {

        }

        private void textBox1_TextChanged(object sender, EventArgs e)
        {
            
            
        }

        private void label1_Click(object sender, EventArgs e)
        {

        }
    }
}
