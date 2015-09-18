using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Stock.Strategy.Python
{
    public partial class AddStockForm : Form
    {
        private PythonStrategyControl ctrl = null;
        public AddStockForm(PythonStrategyControl ctrl)
        {
            InitializeComponent();
            this.ctrl = ctrl;
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            this.ctrl.AddStock(txtCode.Text, txtName.Text);
        }

        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Close();
        }
    }
}
