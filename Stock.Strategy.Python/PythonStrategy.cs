using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.Strategy.Python
{
    public class PythonStrategy : BaseStrategy
    {
        private int basePoint = 0;

        /// <summary>
        /// 基点
        /// </summary>
        public int BasePoint
        {
            get { return basePoint; }
            set { basePoint = value; }
        }

        public override string Name
        {
            get
            {
                return "Python策略";
            }
            set
            {
            }
        }

        public override string Description
        {
            get
            {
                return "Python策略模型";
            }
            set
            {
            }
        }

        public override void Run()
        {
            // 定时检查是否成功交易(买入卖出)
        }

        private float ComputeYhsyl(String code, float price)
        {
            return 0f;
        }

        public override void OnTicket(object sender)
        {
            scope.SetVariable("WeiTuo", this);// 将this Set 到Ipy脚本的WeiTuo值中
            scope.SetVariable("Bids", this.Bids);    // 多个股票的盘口数据
            scope.SetVariable("StockPool", this.StockPool); // 股票池
            scope.SetVariable("TradeAccount", this.GetTradingAccountInfo());   // 账户信息

            ScriptSource code = engine.CreateScriptSourceFromFile("e:\\projects\\test.py");
            // code.Execute(scope);
        }

        public override void OnStockDataChanged(object sender, Stock.Market.Bid data)
        {

        }
    }
}
