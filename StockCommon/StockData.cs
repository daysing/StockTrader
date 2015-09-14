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

namespace Stock.Market
{
    /// <summary>
    /// 股票实时交易数据
    /// </summary>
    public class StockData
    {
        /// <summary>
        /// 每价格挂单的数据
        /// </summary>
        public class GoodsData
        {
            private float price;
            private int orderAmount;
        }

        public String Code {get; set;}    // 证券代码
        private float price;    // 成交价
        private int volume;     // 成交量
        private DateTime pushTime;  // 推送时间

        private ICollection<GoodsData> buyList = new List<GoodsData>();
        private ICollection<GoodsData> sellList = new List<GoodsData>();

        public GoodsData[] BuyList
        {
            get
            {
                return this.buyList.ToArray<GoodsData>();   
            }
        }

        public GoodsData[] SellList
        {
            get
            {
                return this.sellList.ToArray<GoodsData>();   
            }
        }

        public void AddBuyGoodsData(GoodsData data)
        {
            this.buyList.Add(data);
        }

        public void AddSellGoodsData(GoodsData data)
        {
            this.sellList.Add(data);
        }
    }
}
