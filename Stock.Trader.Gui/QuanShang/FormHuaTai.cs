using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stock.Trader
{
    public partial class FormHuaTai : Form
    {
        public FormHuaTai()
        {
            InitializeComponent();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            this.txtTransPwd.GetEncPswAes();

        }
    }
}
