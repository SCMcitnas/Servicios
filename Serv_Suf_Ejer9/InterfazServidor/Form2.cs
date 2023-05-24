using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace InterfazServidor
{
    public partial class Form2 : Form
    {
        

        public Form2()
        {
            InitializeComponent();

            textBox1.Text = Form1.IP_SERVER;
            textBox2.Text = Form1.port.ToString();
        }

        private void btnAceptar_Click(object sender, EventArgs e)
        {
            bool bienIp = false;
            bool bienPort = false;

            if (textBox1.Text != "".Trim())
            {
                Form1.IP_SERVER = textBox1.Text;
                lblErrorIP.Text = "";
                bienIp = true;
            }
            else
            {
                lblErrorIP.Text = "IP incorrecta";
            }
            

            try
            {
                Form1.port = Convert.ToInt32(textBox2.Text);
                lblErrorPuerto.Text = "";
                bienPort = true;
            }
            catch (FormatException ex)
            {
                lblErrorPuerto.Text = "Puerto incorrecto";
            }

            if (bienIp && bienPort)
            {
                this.Hide();
            }
        }
    }
}
