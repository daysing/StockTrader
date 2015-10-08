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

namespace Stock.Trader.HuaTai
{

    public class Request
    {
        public string uid { get; set; }

        public string cssweb_type { get; set; }

        public string version
        {
            get
            {
                return "1";
            }
        }

        public string custid { get; set; }

        public string op_branch_no { get; set; }

        public string branch_no { get; set; }

        public string op_entrust_way { get { return "7"; } }

        public string exchange_type { get; set; }

        public string function_id { get; set; } 

        public string fund_account { get; set; }

        public string password { get; set; }

        public string op_station { get; set; }

        public string identity_type
        {
            get
            {
                return "";
            }
        }

        public string ram
        {
            get
            {
                return StockUtil.RandomString;
            }
        }
    }

    public class LoginPostInfo
    {
           // Methods
        public LoginPostInfo() { }

        // Properties
        public string userType
        {
            get
            {
                return "jy";
            }
        }
        public string loginEvent
        {
            get { return "1"; }
        }
        public string trdpwdEns
        {
            get;
            set;
        }

        public string macaddr { get; set; }
        public string hddInfo { get; set; }
        public string lipInfo { get; set; }
        public string topPath { get { return "null"; } }
        public string accountType
        {
            get { return "1"; }
        }
        public string userName
        {
            get;
            set;
        }
        public string servicePwd { get; set; }
        public string trdpwd
        {
            get;
            set;
        }
        public string vcode { get; set; }
    }

    public class StockQueryRequest : Request
    {

        public StockQueryRequest()
        {
            cssweb_type = "GET_STOCK_POSITION";
            function_id = "403";
        }

        public string position_str
        {
            get
            {
                return "";
            }
        }

        public string query_direction
        {
            get
            {
                return "";
            }
        }

        public int query_mode
        {
            get
            {
                return 0;
            }
        }

        public int request_num
        {
            get
            {
                return 100;
            }
        }

        public string stock_account
        {
            get
            {
                return "";
            }
        }

        public string stock_code
        {
            get
            {
                return "";
            }
        }

    }

    public class BuyRequest : Request
    {
        public BuyRequest() {
            this.cssweb_type = "STOCK_BUY";
            this.function_id = "302";
        }

        public int entrust_amount { get; set; }

        public int entrust_bs
        {
            get
            {
                return 1;
            }
        }

        public float entrust_price { get; set; }

        public int entrust_prop
        {
            get
            {
                return 0;
            }
        }

        public string stock_account { get; set; }

        public string stock_code { get; set; }

    }

    public class SellRequest : Request
    {
        // uid=153-1f64-7915165&cssweb_type=STOCK_SALE&version=1&custid=666600111111&op_branch_no=17&branch_no=17&op_entrust_way=7&op_station=IP$183.206.203.123;MAC$00-0C-29-1A-B4-32;HDD$                    &function_id=302&fund_account=666600111111&password=ojCz+oMyzH50kXNZv4/iTA$$&identity_type=&exchange_type=1&stock_account=A763438769&stock_code=600717&entrust_amount=100&entrust_price=10&entrust_prop=0&entrust_bs=2&ram=0.21561731165274978

        public SellRequest()
        {
            cssweb_type = "STOCK_SELL";
            function_id = "302";
        }
            public int entrust_amount { get; set; }

            public int entrust_bs
            {
                get
                {
                    return 2;
                }
            }

            public float entrust_price { get; set; }

            public int entrust_prop
            {
                get
                {
                    return 0;
                }
            }

            public string stock_account { get; set; }

            public string stock_code { get; set; }
     
    }

    public class CancelQueryRequest : Request
    {
        //uid=152-5841-6976124&version=1&custid=666600111111&op_branch_no=17&branch_no=17&op_entrust_way=7&op_station=IP$183.206.203.123;MAC$00-0C-29-1A-B4-32;HDD$                    &function_id=401&fund_account=66660111111&password=ojCz+oMyzH05kXNZv4/iTA$$&identity_type=&exchange_type=&stock_account=&stock_code=&locate_entrust_no=&query_direction=&
        //sort_direction=0&request_num=100&position_str=&ram=0.5226608067750931
        public CancelQueryRequest()
        {
            cssweb_type = "GET_CANCEL_LIST";
            function_id = "401";
        }

        public string stock_account { get; set; }
        public string stock_code { get; set; }
        public string locate_entrust_no {get{return "";} }
        public string query_direction { get{return "";}}
        public int sort_direction { get { return 0; } }
        public int request_num { get { return 100; } }
        public string position_str { get{return "";}}
    }

    public class CancelStockRequest : Request
    {
        public CancelStockRequest()
        {
            cssweb_type = "STOCK_CANCEL";
            function_id = "304";
        }

        public string batch_flag { get; set; }
        public string entrust_no { get; set; }

        // uid=153-759d-7924950&cssweb_type=STOCK_CANCEL&version=1&custid=666600111111&
        // op_branch_no=17&branch_no=17&op_entrust_way=7&op_station=IP$183.206.203.55;MAC$00-0D-27-1A-B4-32;HDD$                    &
        // function_id=304&fund_account=666600111111&password=ojCz+oMyzH00kXNZv4/iTA$$&
        // identity_type=&
        // batch_flag=0&exchange_type=&entrust_no=12140&ram=0.6111481911502779
    }
}
