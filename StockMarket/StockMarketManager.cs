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
        public static IDictionary<String, BidCacheQueue> bidCache = new Dictionary<String, BidCacheQueue>();
        private bool started = false;

        public static void AddBid(Bid bid) {
            if(!bidCache.ContainsKey(bid.Code))
                   bidCache.Add(bid.Code, new BidCacheQueue());

            bidCache[bid.Code].Enqueue(bid);
        }

        /// <summary>
        /// 启动行情监听器
        /// </summary>
        public void Start()
        {
            if (started)
                throw new Exception();

            StockMarketListener rsmt = new SinaStockMarketListener();
            foreach (string code in StockMarketManager.bidCache.Keys)
            {
                rsmt.AddStock(code);
            }

            Thread t = new Thread(new ThreadStart(rsmt.Run));
            t.Start();
        }

        /// <summary>
        /// 在行情市场中登记一个策略，当关注股票价格发生变动的时候，即时提醒策略。
        /// </summary>
        /// <param name="strategy">策略实例</param>
        public void RegisterStrategy(IStrategy strategy)
        {
            foreach (string code in strategy.StockPool)
            {
                if (!StockMarketManager.bidCache.ContainsKey(code))
                {
                    StockMarketManager.bidCache.Add(code, new BidCacheQueue());
                }
                StockMarketManager.bidCache[code].OnBidChange += strategy.OnStockDataChanged;
            }
        }
        

    }
}
