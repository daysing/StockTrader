/*
 * This library is part of Stock Trader System
 *
 * Copyright (c) qiujoe (http://www.github.com/qiujoe)
 *
 * This library is free software; you can redistribute it and/or
 * modify it under the terms of the GNU Lesser General Public
 * License as published by the Free Software Foundation; either
 * version 2.1 of the License, or (at your option) any later version.
 *
 * This library is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the GNU
 * Lesser General Public License for more details.
 *
 * For further information about StockTrader, please see the
 * project website: http://www.github.com/qiujoe/StockTrader
 *
 * You should have received a copy of the GNU Lesser General Public
 * License along with this library; if not, write to the Free Software
 * Foundation, Inc., 59 Temple Place, Suite 330, Boston, MA  02111-1307  USA
 */


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

using Stock.Trader;
using Stock.Strategy;
using Stock.Strategy.Settings;
using Stock.Strategy.Python;
using Stock.Market;
using System.Reflection;
using System.Window;

namespace StockTrader
{
    public partial class Form1 : Form
    {
        Stock.Trader.XiaDan xiadan = null;
        IntPtr nextClipboardViewer;
        public Form1()
        {
            InitializeComponent();
            InitStrategyMenu();
            InitListView();
            xiadan = Stock.Trader.XiaDan.Instance;
            xiadan.Init();

            nextClipboardViewer = (IntPtr)Win32API.SetClipboardViewer(this.Handle);
        }

        protected override void WndProc(ref Message m)
        {
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    DisplayClipboardData();
                    Win32API.SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                case WM_CHANGECBCHAIN:
                    if (m.WParam == nextClipboardViewer)
                        nextClipboardViewer = m.LParam;
                    else
                        Win32API.SendMessage(nextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;

                default:
                    base.WndProc(ref m);
                    break;
            }
        }

        private void DisplayClipboardData()
        {
            try
            {
                IDataObject iData = new DataObject();
                iData = Clipboard.GetDataObject();

                MessageBox.Show(iData.GetData(DataFormats.UnicodeText).ToString());

                //if (iData.GetDataPresent(DataFormats.Rtf))
                //    richTextBox1.Rtf = (string)iData.GetData(DataFormats.Rtf);
                //else if (iData.GetDataPresent(DataFormats.Text))
                //    richTextBox1.Text = (string)iData.GetData(DataFormats.Text);
                //else
                //    richTextBox1.Text = "[Clipboard data is not RTF or ASCII Text]";
            }
            catch (Exception e)
            {
                MessageBox.Show(e.ToString());
            }
        }


        /// <summary>
        /// 初始化右键菜单策略
        /// </summary>
        private void InitStrategyMenu()
        {
            // 从服务器获取策略数据，
            StrategyDesc[] sds = LoadStrategyList();

            foreach (var sd in sds)
            {
                ToolStripMenuItem tsmi = new ToolStripMenuItem();
                tsmi.Text = sd.name;
                tsmi.Tag = sd;
                tsmi.Click += new EventHandler(AddStrategyToListView);
                if(sd.group ==0)
                    this.miFfjjStrategy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmi});
                else if(sd.group == 1)
                    this.miGpStrategy.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] { tsmi});
            }
        }

        /// <summary>
        /// 策略的描述
        /// </summary>
        class StrategyDesc
        {
            public String name;
            public String desc;
            public String clazz;
            public int group;
            public int id;
        }

        private StrategyDesc[] LoadStrategyList()
        {
            StrategyDesc[] sd = new StrategyDesc[] { new StrategyDesc(), new StrategyDesc() };
            sd[0].clazz = "Stock.Strategy.Settings.RotationStrategyForm";
            sd[0].desc = "说明：分级A轮动策略";
            sd[0].name = "分级A轮动策略";
            sd[0].group = 0;
            sd[1].clazz = "Stock.Strategy.Settings.RotationStrategyForm";
            sd[1].desc = "说明：箱体高抛低吸策略";
            sd[1].name = "箱体策略";
            sd[1].group = 1;

            return sd;
        }

        private StrategyDesc[] LoadMyStrategyList()
        {
            StrategyDesc[] sd = new StrategyDesc[] { new StrategyDesc(), new StrategyDesc() };
            sd[0].clazz = "Stock.Strategy.Settings.RotationStrategyForm";
            sd[0].desc = "说明：分级A轮动策略";
            sd[0].name = "分级A轮动策略";
            sd[0].group = 0;

            return sd;
        }

        private void InitListView()
        {
            StrategyDesc[] sds = LoadMyStrategyList();
            foreach (StrategyDesc sd in sds)
            {
                this.AddStrategyToListView(sd);
            }

            // this.listView1.Items[0].Selected = true;
            this.panel1.Controls.Add((Control)this.listView1.Items[0].Tag);

        }

       

        /// <summary>
        /// 加入策略到列表视图，同时生成一个策略实例
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        void AddStrategyToListView(object sender, EventArgs e)
        {
            StrategyDesc sd = (StrategyDesc)((ToolStripMenuItem)sender).Tag;

            AddStrategyToListView(sd);
        }

        private void AddStrategyToListView(StrategyDesc sd)
        {
            PythonStrategyControl control = new Stock.Strategy.Rotation.RatationStrategyControl();
            // Assembly asm = Assembly.LoadFile("E:\\projects\\StockTrader\\StockTrader\\bin\\Debug\\StockStrategy.dll");
            // PythonStrategyControl control = (PythonStrategyControl)asm.GetType(sd.clazz).Assembly.CreateInstance(sd.clazz);
            

            StrategyManager.Instance.AddMyStrategy(control.Strategy);

            System.Windows.Forms.ListViewItem lvi = new System.Windows.Forms.ListViewItem(new string[] {
            sd.name,
            sd.desc}, -1);
            lvi.Group = this.listView1.Groups[sd.group];
            lvi.Tag = control;
            this.listView1.Items.Add(lvi);
        }

        #region 测试下单
        private void button16_Click(object sender, EventArgs e)
        {
            xiadan = Stock.Trader.XiaDan.Instance;
            xiadan.Init();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            xiadan.SellStock(textBox1.Text, float.Parse(textBox2.Text), int.Parse(textBox3.Text));
        }

        private void button2_Click(object sender, EventArgs e)
        {
            xiadan.BuyStock(textBox1.Text, float.Parse(textBox2.Text), int.Parse(textBox3.Text));
        }

        private void button3_Click(object sender, EventArgs e)
        {
            xiadan.CancelStock(textBox1.Text, float.Parse(textBox2.Text), int.Parse(textBox3.Text));

        }

        private void button4_Click(object sender, EventArgs e)
        {
            xiadan.GetCashInfo();
        }

        private void button8_Click(object sender, EventArgs e)
        {
            xiadan.PurchaseFundSZ(textBox1.Text, float.Parse(textBox2.Text));
        }

        private void button9_Click(object sender, EventArgs e)
        {
            xiadan.RedempteFundSZ(textBox1.Text, int.Parse(textBox3.Text));
        }

        private void button11_Click(object sender, EventArgs e)
        {
            xiadan.MergeFundSZ(textBox1.Text, int.Parse(textBox3.Text));
        }

        private void button10_Click(object sender, EventArgs e)
        {
            xiadan.PartFundSZ(textBox1.Text, int.Parse(textBox3.Text));
        }

        private void button15_Click(object sender, EventArgs e)
        {
            xiadan.PurchaseFundSH(textBox1.Text, float.Parse(textBox2.Text));

        }

        private void button14_Click(object sender, EventArgs e)
        {
            xiadan.RedempteFundSH(textBox1.Text, int.Parse(textBox3.Text));
        }

        private void button13_Click(object sender, EventArgs e)
        {
            xiadan.MergeFundSH(textBox1.Text, int.Parse(textBox3.Text));
        }

        private void button12_Click(object sender, EventArgs e)
        {
            xiadan.PartFundSH(textBox1.Text, int.Parse(textBox3.Text));
        }
        #endregion

        /// <summary>
        /// 选中item
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void listView1_ItemSelectionChanged(object sender, ListViewItemSelectionChangedEventArgs e)
        {
            if (!e.IsSelected) return;

            if (this.panel1.HasChildren)
                this.panel1.Controls.Clear();

            this.panel1.Controls.Add((Control)e.Item.Tag);

        }

        private void button5_Click(object sender, EventArgs e)
        {

            StockMarketManager smm = new StockMarketManager();
            IStrategy[] strategies = StrategyManager.Instance.ReadMyStrategies();

            foreach (IStrategy strategy in strategies)
            {
                smm.RegisterStrategy(strategy);
            }

            smm.Start();

        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (this.tabControl1.SelectedTab.Text == "持仓")
            {
                GetStockPosition();
            }
        }

        private void GetStockPosition()
        {
            lvStockPosition.Items.Clear();
            TradingAccount account = this.xiadan.GetCashInfo();
            foreach (TradingAccount.StockHolderInfo shi in account.StockHolders)
            {
                ListViewItem lvi = new ListViewItem(new string[] {shi.StockCode,
                    shi.StockName,
                    shi.CurrentAmount.ToString(),
                    "0",
                    shi.EnableAmount.ToString(),
                    shi.CostPrice.ToString(),
                    shi.KeepCostPrice.ToString(),
                     shi.LastPrice.ToString(),
                     "0",
                     shi.IncomeBalance.ToString(),
                     shi.MarketValue.ToString(),
                     shi.ExchangeName,
                     shi.StockAccount

                });
                lvStockPosition.Items.Add(lvi);
            }
        }

 

    }
}
