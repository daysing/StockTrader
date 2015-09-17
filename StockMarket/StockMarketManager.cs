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
using System.Collections;

using Stock.Strategy;
using System.Threading;

namespace Stock.Market
{
    public sealed class StockMarketManager
    {
        public static IDictionary<String, StockDataQueue> stockDataCache = new Dictionary<String, StockDataQueue>();

        public void StartListenStockMarket(int i)
        {

        }

        public static void AddBid(Bid bid) {
            if(!stockDataCache.ContainsKey(bid.Code))
                   stockDataCache.Add(bid.Code, new StockDataQueue());

            stockDataCache[bid.Code].Enqueue(bid);
        }

        public void Start()
        {
            ReadStockMarketThread rsmt = new ReadSinaStockMarketThread();
            foreach (string code in StockMarketManager.stockDataCache.Keys)
	        {
                rsmt.AddStock(code);
	        } 

            while (true)
            {
                Thread.Sleep(2000);
                rsmt.Run();
            }
        }

        /// <summary>
        /// 增加一个策略,和关注的股票价格
        /// </summary>
        /// <param name="code">股票代码</param>
        /// <param name="strategy">策略实例</param>
        public void AddStrategy(IStrategy strategy)
        {
            foreach (string code in strategy.StockPool)
            {
                if (!StockMarketManager.stockDataCache.ContainsKey(code))
                {
                    StockMarketManager.stockDataCache.Add(code, new StockDataQueue());
                }
                StockMarketManager.stockDataCache[code].OnStockDataChange += strategy.OnStockDataChanged;
            }
        }
        

    }
}
