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

namespace Stock.Trader.WeiTuo
{
    /// <summary>
    /// 账户信息,包括
    /// <ol>
    ///     <li>资金信息</li>
    ///     <li>每个股票的市值,成本,手续费,佣金等信息</li>
    /// </ol>
    /// </summary>
    public class TradingAccount
    {
        public class StockHolderInfo {
            public String Code { get; set; }
            public int Amount { get; set; }
            public decimal Cost { get; set; }
            /// <summary>
            /// 计算手续费
            /// </summary>
            private void ComputeXXX() {
                // 计算手续费

                // 计算税金

                // 计算成本

                // 调整持仓

                // 计算市值

                // 调整盈亏
            }
            
        }

        public class MoneyInfo {
            public decimal AvailableMoney { get; set; }
        }


        private IList<StockHolderInfo> stockHolders = new List<StockHolderInfo>();
        private MoneyInfo moneyInfo = new MoneyInfo();
        public StockHolderInfo[] StockHolders
        {
            get
            {
                return stockHolders.ToArray();
            }
        }

    }
}
