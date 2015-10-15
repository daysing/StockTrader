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
using System.Net;
using Stock.Common;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;
using System.Reflection;
using System.Web;
using System.Text.RegularExpressions;
using Newtonsoft.Json;

namespace Stock.Trader.HuaTai
{
    class WebStockTrader : IStockTrader
    {

        #region nested class

        public class URLHelper
        {
            public static string GetData<T>(T t, Encoding encoding)
            {
                PropertyInfo[] properties = t.GetType().GetProperties();
                StringBuilder builder = new StringBuilder();
                foreach (PropertyInfo info in properties)
                {
                    string str = "";
                    object obj2 = info.GetValue(t, null);
                    if (obj2 != null)
                    {
                        str = obj2.ToString();
                        //str = HttpUtility.UrlEncode(obj2.ToString(), encoding);
                    }
                    builder.AppendFormat("{0}={1}&", info.Name, str);
                }
                return builder.ToString(0, builder.Length - 1);
            }

            public static string GetDataWithOutEncode<T>(T t)
            {
                PropertyInfo[] properties = t.GetType().GetProperties();
                StringBuilder builder = new StringBuilder();
                foreach (PropertyInfo info in properties)
                {
                    string str = "";
                    object obj2 = info.GetValue(t, null);
                    if (obj2 != null)
                    {
                        str = obj2.ToString();
                    }
                    builder.AppendFormat("{0}={1}&", info.Name, str);
                }
                return builder.ToString(0, builder.Length - 1);
            }
        }
        
        #endregion

        private CookieContainer Cookie;
        private Encoding encoding = Encoding.UTF8;
        private Encoding GB2312 = Encoding.GetEncoding("GB2312");
        private HttpClient httpClient = new HttpClient();
        private RespAccountInfo resAccountInfo;
        private static string SuccessCode = "success";
        FormInputValidateCode fivc = null;

        public void Init()
        {
            Login();
        }

        private void Login()
        {
            string loginUrl = "https://service.htsc.com.cn/service/loginAction.do?method=login";
            this.httpClient.DownloadString(loginUrl);
            
            string verifyCode = this.GetVerifyCode();
            if (verifyCode == "")
            {
                MessageBox.Show("获取验证码失败");
            }

            LoginPostInfo t = new LoginPostInfo
            {
                // TODO: 自动获取MAC, HDD INFO
                macaddr = "00:0C:29:1A:B4:32",
                hddInfo = "CVCV434102MF120BGN++",
                
                servicePwd = fivc.txtServicePwd.Text,
                trdpwd = fivc.txtTrdpwd.GetEncPswAes(),
                trdpwdEns = fivc.txtTrdpwd.GetEncPswAes(),
                userName = fivc.txtUsername.Text,
                vcode = verifyCode
            };

            string postString = URLHelper.GetData<LoginPostInfo>(t, this.encoding);
            byte[] postData = Encoding.UTF8.GetBytes(postString);
            this.httpClient.Encoding = this.encoding;

            // string str4 = this.httpClient.UploadString(address, data);
            byte[] responseData = this.httpClient.UploadData(loginUrl, "POST", postData);
            string resp = Encoding.UTF8.GetString(responseData);//解码  

            if (resp.IndexOf("上次登录时间") != -1)
            {
                string input = this.httpClient.DownloadString("https://service.htsc.com.cn/service/flashbusiness_new3.jsp?etfCode=");
                String infoStr = StockUtil.Base64Decode(this.GetData(input), this.GB2312);
                Console.WriteLine(infoStr);
                resAccountInfo = JsonConvert.DeserializeObject<RespAccountInfo>(infoStr);                
            }
            else if (resp.IndexOf("系统升级中") != -1)
            {
                MessageBox.Show("系统升级中");
                return;
            }
            else
            {
                Login();
            }
        }

        private string GetData(string input)
        {
            string pattern = "var\\sdata\\s=\\s\\\"(.*?)\\\";";
            Match match = new Regex(pattern, RegexOptions.IgnorePatternWhitespace | RegexOptions.Multiline | RegexOptions.IgnoreCase).Match(input);
            if (match.Success)
            {
                return match.Groups[1].Value;
            }
            return string.Empty;
        }

        private string GetVerifyCode()
        {
            MemoryStream stream = new MemoryStream(this.httpClient.DownloadData("https://service.htsc.com.cn/service/pic/verifyCodeImage.jsp?ran=" + new Random().Next()));
            Bitmap bitmap = new Bitmap(stream);
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
            Bitmap image = bitmap.Clone(rect, PixelFormat.Format8bppIndexed);
            fivc = new FormInputValidateCode();
            fivc.picValidateCode.Image = image;
            fivc.ShowDialog();
            return fivc.txtValidateCode.Text;
        }

        private StockHolder GetStockHolder(int exchangeType)
        {
            StockHolder holder = null;
            for (int i = 0; i < this.resAccountInfo.Item.Count; i++)
			{
			    if(this.resAccountInfo.Item[i].exchange_type == exchangeType.ToString())
                {
                    holder = this.resAccountInfo.Item[i];
                }
			}

            return holder;
        }

        public string SellStock(string code, float price, int num)
        {
            string ret = _SellStock(code, price, num);
            if (ret == "")
            {
                // LOG
                Login();
                return _SellStock(code, price, num);  
            }

            return ret;
        }

        private string _SellStock(string code, float price, int num)
        {
            int exchange_type = StockUtil.GetExchangeType(code);
            StockHolder holder = GetStockHolder(exchange_type);
            StockSaleRequest t = new StockSaleRequest
            {
                branch_no = this.resAccountInfo.branch_no,
                custid = this.resAccountInfo.fund_account,
                fund_account = this.resAccountInfo.fund_account,
                op_branch_no = this.resAccountInfo.branch_no,
                op_station = this.resAccountInfo.op_station,
                password = this.resAccountInfo.trdpwd,
                uid = this.resAccountInfo.uid,
                exchange_type = exchange_type.ToString(),
                stock_account = holder.stock_account,
                stock_code = code,
                entrust_amount = num,
                entrust_price = (float)price
            };
            string queryParams = StockUtil.Base64Encode(URLHelper.GetDataWithOutEncode<StockSaleRequest>(t), this.GB2312);
            string sellUrl = "https://tradegw.htsc.com.cn/?" + queryParams;
            string resp = StockUtil.Base64Decode(this.httpClient.DownloadString(sellUrl), this.GB2312);

            StockSaleResp ret = JsonConvert.DeserializeObject<StockSaleResp>(resp);
            if (ret.cssweb_code == SuccessCode)
            {
                return ret.Item[0].entrust_no.ToString();
            }

            return "";
        }

        public string BuyStock(string code, float price, int num)
        {
            string ret = _BuyStock(code, price, num);
            if (ret == "")
            {
                // LOG
                Login();
                _BuyStock(code, price, num);
            }
            return null;
       }

        private string _BuyStock(string code, float price, int num)
        {
            int exchange_type = StockUtil.GetExchangeType(code);
            StockHolder holder = GetStockHolder(exchange_type);

            StockBuyRequest t = new StockBuyRequest
            {
                branch_no = this.resAccountInfo.branch_no,
                custid = this.resAccountInfo.fund_account,
                fund_account = this.resAccountInfo.fund_account,
                op_branch_no = this.resAccountInfo.branch_no,
                op_station = this.resAccountInfo.op_station,
                password = this.resAccountInfo.trdpwd,
                uid = this.resAccountInfo.uid,
                exchange_type = exchange_type.ToString(),
                stock_account = holder.stock_account,
                stock_code = code,
                entrust_amount = num,
                entrust_price = price
            };

            string queryParams = StockUtil.Base64Encode(URLHelper.GetDataWithOutEncode<StockBuyRequest>(t), this.GB2312);
            string buyUrl = "https://tradegw.htsc.com.cn/?" + queryParams;
            string resp = StockUtil.Base64Decode(this.httpClient.DownloadString(buyUrl), this.GB2312);

            StockBuyResp ret = JsonConvert.DeserializeObject<StockBuyResp>(resp);
            if (JsonConvert.DeserializeObject<StockBuyResp>(resp).cssweb_code == SuccessCode)
            {
                return ret.Item[0].entrust_no.ToString();
            }
            return "";
        }

        public string CancelStock(string entrust_no)
        {
            //GetCancelListRequest t = new GetCancelListRequest
            //{
            //    branch_no = this.resAccountInfo.branch_no,
            //    custid = this.resAccountInfo.fund_account,
            //    fund_account = this.resAccountInfo.fund_account,
            //    op_branch_no = this.resAccountInfo.branch_no,
            //    op_station = this.resAccountInfo.op_station,
            //    password = this.resAccountInfo.trdpwd,
            //    uid = this.resAccountInfo.uid,
            //    exchange_type = "",
            //    stock_account = "",
            //    stock_code = "",
            //};
            //string queryParams = StockUtil.Base64Encode(URLHelper.GetDataWithOutEncode<GetCancelListRequest>(t), this.GB2312);
            //string buyUrl = "https://tradegw.htsc.com.cn/?" + queryParams;
            //string resp = StockUtil.Base64Decode(this.httpClient.DownloadString(buyUrl), this.GB2312);

            //if (JsonConvert.DeserializeObject<GetCancelListResp>(resp).cssweb_code == SuccessCode)
            //{
            //    // TODO:
            //}


            StockCancelRequest t1 = new StockCancelRequest
            {
                branch_no = this.resAccountInfo.branch_no,
                custid = this.resAccountInfo.fund_account,
                fund_account = this.resAccountInfo.fund_account,
                op_branch_no = this.resAccountInfo.branch_no,
                op_station = this.resAccountInfo.op_station,
                password = this.resAccountInfo.trdpwd,
                uid = this.resAccountInfo.uid,
                exchange_type = "",
                entrust_no = entrust_no
            };
            string cancelParams = StockUtil.Base64Encode(URLHelper.GetDataWithOutEncode<StockCancelRequest>(t1), this.GB2312);
            string cancelUrl = "https://tradegw.htsc.com.cn/?" + cancelParams;
            string resp = StockUtil.Base64Decode(this.httpClient.DownloadString(cancelUrl), this.GB2312);

            StockCancelResp ret = JsonConvert.DeserializeObject<StockCancelResp>(resp);
            if (JsonConvert.DeserializeObject<StockCancelResp>(resp).cssweb_code == SuccessCode)
            {
                return ret.Item[0].entrust_no.ToString();
            }

            return "";

        }

        public void GetTransactionInfo()
        {
            throw new NotImplementedException();
        }

        public void Keep()
        {
            GetTradingAccountInfo();
        }

        public TradingAccount GetTradingAccountInfo()
        {
            List<TradingAccount.StockHolderInfo> shis = GetStocks();
            TradingAccount account = new TradingAccount();
            account.AddStockHolder(shis);
            return account;
        }

        /// <summary>
        /// 获取信用账户的股票信息
        /// </summary>
        /// <returns></returns>
        private List<TradingAccount.StockHolderInfo> GetXYStocks()
        {
            return null;
        }

        private List<TradingAccount.StockHolderInfo> GetStocks()
        {
            GetStockPositionRequest t = new GetStockPositionRequest
            {
                branch_no = this.resAccountInfo.branch_no,
                custid = this.resAccountInfo.fund_account,
                fund_account = this.resAccountInfo.fund_account,
                op_branch_no = this.resAccountInfo.branch_no,
                op_station = this.resAccountInfo.op_station,
                password = this.resAccountInfo.trdpwd,
                uid = this.resAccountInfo.uid
            };
            string str2 = StockUtil.Base64Encode(URLHelper.GetDataWithOutEncode<GetStockPositionRequest>(t), this.encoding);

            string positionUrl = "https://tradegw.htsc.com.cn/?" + str2;
            GetStockPositionResp result = JsonConvert.DeserializeObject<GetStockPositionResp>(StockUtil.Base64Decode(this.httpClient.DownloadString(positionUrl), this.GB2312));
            List<TradingAccount.StockHolderInfo> list = new List<TradingAccount.StockHolderInfo>();
            foreach (GetStockPositionResp.GetStockPositionRespItem si in result.Item)
            {
                if (si.stock_code != null)
                {
                    TradingAccount.StockHolderInfo shi = new TradingAccount.StockHolderInfo
                    {
                        StockAccount = si.stock_account,
                        StockCode = si.stock_code,
                        StockName = si.stock_name,
                        ExchangeName = si.exchange_name,
                        MarketValue = (decimal) si.market_value,
                        CostPrice = (decimal)si.cost_price,
                        CurrentAmount = (int)si.current_amount,
                        EnableAmount = (int)si.enable_amount,
                        IncomeBalance = (decimal)si.income_balance,
                        KeepCostPrice = (decimal)si.keep_cost_price,
                        LastPrice = (decimal)si.last_price
                    };
                    list.Add(shi);
                }
            }
            return list;
        }

        public string PurchaseFundSZ(string code, float total)
        {
            throw new NotImplementedException();
        }

        public string RedempteFundSZ(string code, int num)
        {
            throw new NotImplementedException();
        }

        public string MergeFundSZ(string code, int num)
        {
            throw new NotImplementedException();
        }

        public string PartFundSZ(string code, int num)
        {
            throw new NotImplementedException();
        }

        public string PurchaseFundSH(string code, float total)
        {
            throw new NotImplementedException();
        }

        public string RedempteFundSH(string code, int num)
        {
            throw new NotImplementedException();
        }

        public string MergeFundSH(string code, int num)
        {
            throw new NotImplementedException();
        }

        public string PartFundSH(string code, int num)
        {
            throw new NotImplementedException();
        }
    }
}
