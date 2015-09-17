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
using System.Linq;
using System.Text;
using Stock.Common;
using System.Net;

namespace Stock.Market
{
    public class ReadSinaStockMarketThread : ReadStockMarketThread
    {
        private CookieContainer Cookie = new CookieContainer();

        private const string dataurl = "http://hq.sinajs.cn/list={0}";

        public override void Run()
        {
            HttpClient client = new HttpClient(this.Cookie);
            IList<String> t_codes = new List<String>();
            foreach (string c in codes)
            {
                t_codes.Add(StockUtil.GetFullCode(c));
            }
            
            string address = String.Format(dataurl, String.Join(",", t_codes.ToArray()));

            string[] data = client.DownloadString(address).Split(new char[] { '\n' });
            Dictionary<string, Bid> dictionary2 = new Dictionary<string, Bid>();
            for (int i = 0; i < data.Count(); i++)
            {
                if (data[i].Length != 0)
                {
                    Bid bid = Parse(data[i]);
                    StockMarketManager.AddBid(bid);
                    // dictionary2.Add(codes[i], info);
                }
            }
        }

        const int BUY_1_A = 10;
        const int BUY_1_P = BUY_1_A + 1;
        const int BUY_2_A = BUY_1_P+1;
        const int BUY_2_P = BUY_2_A + 1;
        const int BUY_3_A = BUY_2_P + 1;
        const int BUY_3_P = BUY_3_A + 1;
        const int BUY_4_A = BUY_3_P + 1;
        const int BUY_4_P = BUY_4_A + 1;
        const int BUY_5_A = BUY_4_P + 1;
        const int BUY_5_P = BUY_5_A + 1;

        const int SELL_1_A = BUY_5_P + 1;
        const int SELL_1_P = SELL_1_A + 1;
        const int SELL_2_A = SELL_1_P + 1;
        const int SELL_2_P = SELL_2_A + 1;
        const int SELL_3_A = SELL_2_P + 1;
        const int SELL_3_P = SELL_3_A + 1;
        const int SELL_4_A = SELL_3_P + 1;
        const int SELL_4_P = SELL_4_A + 1;
        const int SELL_5_A = SELL_4_P + 1;
        const int SELL_5_P = SELL_5_A + 1;
        private Bid Parse(string data)
        {
            Bid bid = new Bid();
            String[] items  = data.Split(new char[]{','});

            bid.Code = data.Substring(13, 6);
            bid.Name = items[0];
            // bid.TodayOpen = decimal.Parse(items[1]);
            // bid.YesterdayClose = decimal.Parse(items[2]);
            bid.CurrentPrice = decimal.Parse(items[3]);
            // bid.High = decimal.Parse(items[4]);
            // bid.Low = decimal.Parse(items[5]);
            //bid.Buy = decimal.Parse(items[6]);
            //bid.Sell = decimal.Parse(items[7]);
            bid.Volumn = int.Parse(items[8]);
            bid.Turnover = decimal.Parse(items[9]);

            bid.AddBuyGoodsData(new Bid.GoodsData(decimal.Parse(items[BUY_1_P]), int.Parse(items[BUY_1_A])));
            bid.AddBuyGoodsData(new Bid.GoodsData(decimal.Parse(items[BUY_2_P]), int.Parse(items[BUY_2_A])));
            bid.AddBuyGoodsData(new Bid.GoodsData(decimal.Parse(items[BUY_3_P]), int.Parse(items[BUY_3_A])));
            bid.AddBuyGoodsData(new Bid.GoodsData(decimal.Parse(items[BUY_4_P]), int.Parse(items[BUY_4_A])));
            bid.AddBuyGoodsData(new Bid.GoodsData(decimal.Parse(items[BUY_5_P]), int.Parse(items[BUY_5_A])));


            bid.AddSellGoodsData(new Bid.GoodsData(decimal.Parse(items[SELL_1_P]), int.Parse(items[SELL_1_A])));
            bid.AddSellGoodsData(new Bid.GoodsData(decimal.Parse(items[SELL_2_P]), int.Parse(items[SELL_2_A])));
            bid.AddSellGoodsData(new Bid.GoodsData(decimal.Parse(items[SELL_3_P]), int.Parse(items[SELL_3_A])));
            bid.AddSellGoodsData(new Bid.GoodsData(decimal.Parse(items[SELL_4_P]), int.Parse(items[SELL_4_A])));
            bid.AddSellGoodsData(new Bid.GoodsData(decimal.Parse(items[SELL_5_P]), int.Parse(items[SELL_5_A])));

            return bid;
        }
    }
}
