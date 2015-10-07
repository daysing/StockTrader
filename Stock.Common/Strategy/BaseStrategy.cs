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
using Stock.Strategy;
using Stock.Common;
using Stock.Market;
using System.Collections;
using Microsoft.Scripting.Hosting;
using Stock.Trader;
using System.Windows.Forms;
using Stock.Strategy.Python;

namespace Stock.Strategy
{
    public abstract class BaseStrategy : IStrategy, IStockTrader
    {
        private bool isValid;
        protected ICollection<string> stockPool = new List<string>();
        private IDictionary<String, BidCacheQueue> bids;

        protected StrategyControl control;

        public StrategyControl Control
        {
            get { return this.control; }
            set { this.control = value; }
        }


        #region 实现策略描述接口

        public event StockRemoveHandler OnStockRemove;
        public event StockAddHandler OnStockAdd;

        public abstract void Run();
        public virtual void OnStockDataChanged(object sender, Stock.Market.Bid data)
        {
            // NOTHING TO DO
        }

        public abstract void OnTicket(object sender);


        public void AddStock(string code)
        {
            if (OnStockAdd != null)
                OnStockAdd(this, code);

            if(!stockPool.Contains(code))
                stockPool.Add(code);
        }

        public void RemoveStock(string code)
        {
            if (OnStockRemove != null)
                OnStockRemove(this, code);

            stockPool.Remove(code);
        }

        public abstract string Name
        {
            get;
            set;
        }

        public abstract string Description
        {
            get;
            set;
        }
        public bool IsValid
        {
            get
            {
                return this.isValid;
            }
            set
            {
                this.isValid = value;
            }
        }

        public String[] StockPool
        {
            get
            {
                return this.stockPool.ToArray<String>();
            }
        }

        #endregion


        public BaseStrategy()
        {
            this.Init();
        }


        /// <summary>
        /// 股票池中的盘口数据
        /// </summary>
        public IDictionary<String, BidCacheQueue> Bids
        {
            get
            {
                if (bids == null)
                {
                    bids = new Dictionary<String, BidCacheQueue>();
                    
                    foreach (string code in StockMarketManager.bidCache.Keys)
	                {
		                if(stockPool.Contains(code)) {
                            bids.Add(code, StockMarketManager.bidCache[code]);
                        }
	                }
                }
                return bids;
            }
        }

        #region 交易接口的实现

        IStockTrader trader = StockTraderManager.Instance.GetStockTrader();

        public void Init()
        {
            // trader.Init();
        }

        public void SellStock(string code, float price, int num)
        {
            LogHelper.WriteLog(this.GetType(),"BaseStrategy.SellStock");
            trader.SellStock(code, price, num);
        }

        public void BuyStock(string code, float price, int num)
        {
            LogHelper.WriteLog(this.GetType(), "BaseStrategy.BuyStock");
        }

        public void CancelStock(string code, float price, int num)
        {
            LogHelper.WriteLog(this.GetType(), "BaseStrategy.CancelStock");
        }

        public void GetTransactionInfo()
        {
            LogHelper.WriteLog(this.GetType(), "BaseStrategy.GetTransactionInfo");
        }

        public void Keep()
        {
            LogHelper.WriteLog(this.GetType(), "BaseStrategy.Keep");
            trader.Keep();
        }

        public TradingAccount GetTradingAccountInfo()
        {
            return null;
            // throw new NotImplementedException();
        }

        public void PurchaseFundSZ(string code, float total)
        {
            throw new NotImplementedException();
        }

        public void RedempteFundSZ(string code, int num)
        {
            throw new NotImplementedException();
        }

        public void MergeFundSZ(string code, int num)
        {
            throw new NotImplementedException();
        }

        public void PartFundSZ(string code, int num)
        {
            throw new NotImplementedException();
        }

        public void PurchaseFundSH(string code, float total)
        {
            throw new NotImplementedException();
        }

        public void RedempteFundSH(string code, int num)
        {
            throw new NotImplementedException();
        }

        public void MergeFundSH(string code, int num)
        {
            throw new NotImplementedException();
        }

        public void PartFundSH(string code, int num)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
