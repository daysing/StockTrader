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
using Stock.Trader.WeiTuo;
using Stock.Trader.WeiTuo.HuaTai;
using Stock.Common;
using Stock.Market;
using System.Collections;

namespace Stock.Strategy
{
    public abstract class BaseStrategy : IStrategy, IStockTrader
    {
        private bool isValid;
        protected ICollection<string> stockPool = new List<string>();
        private IDictionary<String, StockDataQueue> bids;

        #region 实现策略描述接口

        public event StockRemoveHandler OnStockRemove;
        public event StockAddHandler OnStockAdd;

        public abstract void Run();
        public abstract void OnStockDataChanged(object sender, Stock.Market.Bid data);


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
        public IDictionary<String, StockDataQueue> Bids
        {
            get
            {
                if (bids == null)
                {
                    bids = new Dictionary<String, StockDataQueue>();
                    
                    foreach (string code in StockMarketManager.stockDataCache.Keys)
	                {
		                if(stockPool.Contains(code)) {
                            bids.Add(code, StockMarketManager.stockDataCache[code]);
                        }
	                }
                }
                return bids;
            }
        }

        #region 交易接口的实现

        IStockTrader trader = new HuaTaiStockTrader();

        public void Init()
        {
            trader.Init();
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
            // NOTHING TODO
        }

        public void GetStockPositionList()
        {
            throw new NotImplementedException();
        }

        public void GetCashInfo()
        {
            throw new NotImplementedException();
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
