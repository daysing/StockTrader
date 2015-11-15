using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Stock.Trader.HuaTai
{
    public class MockStockTrader : BaseStockTrader
    {
        public override void Init()
        {
        }
        protected override TraderResult internalBuyStock(string code, float price, int num)
        {
            Console.WriteLine(String.Format("买入股票：{0}, 买入价格:{1}: 买入数量: {2}, 时间： {3}", code, price, num, DateTime.Now));
            TraderResult result = new TraderResult();
            result.Code = TraderResultEnum.SUCCESS;

            result.EntrustNo = new Random().Next();
            return result;
        }

        protected override TraderResult internalSellStock(string code, float price, int num)
        {
            Console.WriteLine(String.Format("卖出股票：{0}, 卖出价格:{1}: 卖出数量: {2}， 时间: {3}", code, price, num, DateTime.Now));
            TraderResult result = new TraderResult();
            result.Code = TraderResultEnum.SUCCESS;

            result.EntrustNo = new Random().Next();
            return result;
        }
    }
}
