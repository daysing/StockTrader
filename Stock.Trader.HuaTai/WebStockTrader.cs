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
        
        public void Init()
        {
            Login();
        }

        private void Login()
        {
            string address = "https://service.htsc.com.cn/service/loginAction.do?method=login";
            this.httpClient.DownloadString(address);
            
            string verifyCode = this.GetVerifyCode();
            if (verifyCode == "")
            {
                MessageBox.Show("获取验证码失败");
            }
//            t.vcode = verifyCode;
            LoginPostInfo t = new LoginPostInfo
            {
                macaddr = "00:26:c6:87:15:ce",
                hddInfo = "CVCV434102MF120BGN++",
                servicePwd = "750404",
                trdpwd = "8f572f18e6b09c54fc347f96fbd61564",
                trdpwdEns = "8f572f18e6b09c54fc347f96fbd61564",
                userName = "666600164015",
                vcode = verifyCode
            };


            string postString = URLHelper.GetData<LoginPostInfo>(t, this.encoding);
            byte[] postData = Encoding.UTF8.GetBytes(postString);
            this.httpClient.Encoding = this.encoding;

            // string str4 = this.httpClient.UploadString(address, data);
            byte[] responseData = this.httpClient.UploadData(address, "POST", postData);
            string str4 = Encoding.UTF8.GetString(responseData);//解码  

            if (str4.IndexOf("上次登录时间") != -1)
            {
                string input = this.httpClient.DownloadString("https://service.htsc.com.cn/service/flashbusiness_new3.jsp?etfCode=");
                String infoStr = StockUtil.Base64Decode(this.GetData(input), this.GB2312);
                Console.WriteLine(infoStr);
                resAccountInfo = JsonConvert.DeserializeObject<RespAccountInfo>(infoStr);                
            }
            else if (str4.IndexOf("系统升级中") != -1)
            {
                MessageBox.Show("系统升级中");
                return ;
            }
            return;

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
            FormInputValidateCode fivc = new FormInputValidateCode();
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

        public void SellStock(string code, float price, int num)
        {
            int exchange_type = StockUtil.GetExchangeType(code);
            StockHolder holder = GetStockHolder(exchange_type);
            SellQueryInfo t = new SellQueryInfo
            {
                branch_no = this.resAccountInfo.branch_no,
                custid = this.resAccountInfo.fund_account,
                fund_account = this.resAccountInfo.fund_account,
                op_branch_no = this.resAccountInfo.branch_no,
                op_station = this.resAccountInfo.op_station,
                password = this.resAccountInfo.trdpwd,
                uid = this.resAccountInfo.uid,
                exchange_type = exchange_type,
                stock_account = holder.stock_account,
                stock_code = code,
                entrust_amount = num,
                entrust_price = (float)price
            };
            string str2 = StockUtil.Base64Encode(URLHelper.GetDataWithOutEncode<SellQueryInfo>(t), this.GB2312);
            string address = "https://tradegw.htsc.com.cn/?" + str2;
            string str5 = StockUtil.Base64Decode(this.httpClient.DownloadString(address), this.GB2312);

            if (JsonConvert.DeserializeObject<RespSellStockResult>(str5).cssweb_code == SuccessCode)
           {
                // TODO
           }
           
        }

        public void BuyStock(string code, float price, int num)
        {
            int exchange_type = StockUtil.GetExchangeType(code);
            StockHolder holder = GetStockHolder(exchange_type);

            BuyQueryInfo t = new BuyQueryInfo
            {
                branch_no = this.resAccountInfo.branch_no,
                custid = this.resAccountInfo.fund_account,
                fund_account = this.resAccountInfo.fund_account,
                op_branch_no = this.resAccountInfo.branch_no,
                op_station = this.resAccountInfo.op_station,
                password = this.resAccountInfo.trdpwd,
                uid = this.resAccountInfo.uid,
                exchange_type = exchange_type,
                stock_account = holder.stock_account,
                stock_code = code,
                entrust_amount = num,
                entrust_price = price
            };

            string str2 = StockUtil.Base64Encode(URLHelper.GetDataWithOutEncode<BuyQueryInfo>(t), this.GB2312);
            string address = "https://tradegw.htsc.com.cn/?" + str2;
            string str5 = StockUtil.Base64Decode(this.httpClient.DownloadString(address),this.GB2312);

            if (JsonConvert.DeserializeObject<RespBuyStockResult>(str5).cssweb_code == SuccessCode)
            {
                // TODO:
            }

       }

        public void CancelStock(string code, float price, int num)
        {
            throw new NotImplementedException();
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
            StockQueryInfo t = new StockQueryInfo
            {
                branch_no = this.resAccountInfo.branch_no,
                custid = this.resAccountInfo.fund_account,
                fund_account = this.resAccountInfo.fund_account,
                op_branch_no = this.resAccountInfo.branch_no,
                op_station = this.resAccountInfo.op_station,
                password = this.resAccountInfo.trdpwd,
                uid = this.resAccountInfo.uid
            };
            string str2 = StockUtil.Base64Encode(URLHelper.GetDataWithOutEncode<StockQueryInfo>(t), this.encoding);

            string address = "https://tradegw.htsc.com.cn/?" + str2;
            RespStockResult result = JsonConvert.DeserializeObject<RespStockResult>(StockUtil.Base64Decode(this.httpClient.DownloadString(address), this.GB2312));
            List<TradingAccount.StockHolderInfo> list = new List<TradingAccount.StockHolderInfo>();
            foreach (HtStockInfo si in result.Item)
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
    }
}
