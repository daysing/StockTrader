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
using System.Runtime.CompilerServices;

namespace Stock.Trader.HuaTai
{

    public class Response
    {
        public string cssweb_code { get; set; }
        public string cssweb_type { get; set; }
    }

    public class RespAccountInfo : Response
    {
        public string branch_no { get; set; }
        public string fund_account { get; set; }
        public List<StockHolder> Item { get; set; }
        public string op_station { get; set; }
        public string trdpwd { get; set; }
        public string uid { get; set; }
    }

    public class StockHolder
    {
        public string exchange_name { get; set; }
        public string exchange_type { get; set; }
        public string fund_account { get; set; }
        public int main_flag { get; set; }
        public string stock_account { get; set; }
    }

    public class GetStockPositionResp : Response
    {
        public class GetStockPositionRespItem
        {
            public float av_buy_price { get; set; }

            public float av_income_balance { get; set; }

            public float cost_price { get; set; }

            public int cssweb_test { get; set; }

            public float current_amount { get; set; }

            public float enable_amount { get; set; }

            public string exchange_name { get; set; }

            public int exchange_type { get; set; }

            public int hand_flag { get; set; }

            public float income_balance { get; set; }

            public float keep_cost_price { get; set; }

            public float last_price { get; set; }

            public float market_value { get; set; }

            public string stock_account { get; set; }

            public string stock_code { get; set; }

            public string stock_name { get; set; }
        }

        public List<GetStockPositionRespItem> Item { get; set; }
    }


    public class StockBuyResp : Response
    {
        public List<StockBuyRespItem> Item { get; set; }
    }

    public class StockBuyRespItem
    {
        public int cssweb_test { get; set; }
        public int entrust_no { get; set; }
    }

    public class StockSaleResp : Response
    {
        public List<StockSaleRespItem> Item { get; set; }
    }

    public class StockSaleRespItem
    {
        public int cssweb_test { get; set; }
        public int entrust_no { get; set; }
    }

   public class GetCancelListResp : Response {
       public List<GetCancelListRespItem> Item { get; set; }
       // "{\"cssweb_code\":\"success\",\"cssweb_type\":\"GET_CANCEL_LIST\",\"item\":[{\"stock_name\":\"天津港\",\"entrust_time\":\"101145\",\"entrust_bs\":\"2\",\"bs_name\":\"卖出\",\"entrust_amount\":\"1000.00\",\"entrust_price\":\"10.520\",\"business_amount\":\"0\",\"business_price\":\"0\",\"stock_code\":\"600717\",\"entrust_no\":\"8305\",\"stock_account\":\"A763438769\",\"exchange_type\":\"1\",\"exchange_name\":\"上海Ａ\",\"entrust_status\":\"2\",\"status_name\":\"已报\",\"entrust_prop\":\"0\",\"prop_name\":\"买卖\"},{\"cssweb_test\":\"0\"}]}"
   }

   public class GetCancelListRespItem
   {
       public string stock_name;
       public string entrust_time;
       public string entrust_bs;
       public string bs_name;
       public string entrust_amount;
       public string entrust_price;
       public string business_amount;
       public string business_price;
       public string stock_code;
       public string entrust_no;
       public string stock_account;
       public string exchange_type;
       public string exchange_name;
       public string entrust_status;
       public string status_name;
       public string entrust_prop;
       public string prop_name;
       public string cssweb_test;

       // "item\":[{\"stock_name\":\"天津港\",\"entrust_time\":\"101145\",\"entrust_bs\":\"2\",\"bs_name\":\"卖出\",\"entrust_amount\":\"1000.00\",\"entrust_price\":\"10.520\",
       // \"business_amount\":\"0\",\"business_price\":\"0\",\"stock_code\":\"600717\",\"entrust_no\":\"8305\",\"stock_account\":\"A763438769\",
       // \"exchange_type\":\"1\",\"exchange_name\":\"上海Ａ\",\"entrust_status\":\"2\",\"status_name\":\"已报\",
       // \"entrust_prop\":\"0\",\"prop_name\":\"买卖\"},{\"cssweb_test\":\"0\"}]}"
   }

   public class StockCancelResp : Response
   {
       public List<CancelResultItem> Item
       {
           get;
           set;
       }
   }

   public class CancelResultItem
   {
       public int cssweb_test { get; set; }
       public int entrust_no { get; set; }
   }
}
