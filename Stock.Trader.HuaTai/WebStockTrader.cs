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
using Stock.OCR;

namespace Stock.Trader.HuaTai
{
    public class WebStockTrader : BaseStockTrader
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

        private Encoding encoding = Encoding.UTF8;
        private Encoding GB2312 = Encoding.GetEncoding("GB2312");
        private HttpClient httpClient = new HttpClient();
        private RespAccountInfo resAccountInfo;
        private static string SuccessCode = "success";
        FormInputValidateCode fivc = null;

        #region 重写方法

        public override void Init()
        {
            Login();
        }

        protected override void Login()
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
            httpClient.Timeout = 0xea90;
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

        protected override TraderResult internalSellStock(string code, float price, int num)
        {
            int exchange_type = StockUtil.GetExchangeType(code);
            StockHolderInfo holder = GetStockHolder(exchange_type);
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
                stock_code = StockUtil.GetShortCode(code),
                entrust_amount = num,
                entrust_price = (float)price
            };
            StockSaleResp resp = getResp<StockSaleResp, StockSaleRequest>(t);

            TraderResult ret = new TraderResult();
            if (resp.cssweb_code == SuccessCode)
            {
                ret.Code = TraderResultEnum.SUCCESS;
                ret.EntrustNo = resp.Item[0].entrust_no.ToString();
            }
            else
            {
                ret.Code = TraderResultEnum.ERROR;
            }

            return ret;
        }

        protected override TraderResult internalBuyStock(string code, float price, int num)
        {
            int exchange_type = StockUtil.GetExchangeType(code);
            StockHolderInfo holder = GetStockHolder(exchange_type);

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

            StockBuyResp resp = getResp<StockBuyResp, StockBuyRequest>(t);
            TraderResult ret = new TraderResult();
            if (resp.cssweb_code == SuccessCode)
            {
                ret.Code = TraderResultEnum.SUCCESS;
                ret.EntrustNo = resp.Item[0].entrust_no.ToString();
            }
            else
            {
                ret.Code = TraderResultEnum.ERROR;
            }
            return ret;
        }

        protected override TraderResult internalGetTodayTradeList()
        {
            GetTodayTradeRequest t = new GetTodayTradeRequest
            {
                branch_no = this.resAccountInfo.branch_no,
                custid = this.resAccountInfo.fund_account,
                fund_account = this.resAccountInfo.fund_account,
                op_branch_no = this.resAccountInfo.branch_no,
                op_station = this.resAccountInfo.op_station,
                password = this.resAccountInfo.trdpwd,
                uid = this.resAccountInfo.uid,
                exchange_type = ""
            };

            GetTodayTradeResponse ret = getResp<GetTodayTradeResponse, GetTodayTradeRequest>(t);
            TraderResult result = new TraderResult();
            result.Result = ret;
            if (ret.cssweb_code == SuccessCode)
            {
                result.Code = TraderResultEnum.SUCCESS;
            }
            else
            {
                result.Code = TraderResultEnum.ERROR;
            }

            return result;
        }

        protected override void internalKeep()
        {
            GetTradingAccountInfo();
        }

        protected override TraderResult internalGetTradingAccountInfo()
        {
            List<TradingAccount.StockHolderInfo> shis = GetStocks();
            TradingAccount account = new TradingAccount();
            account.AddStockHolder(shis);

            TraderResult result = new TraderResult();
            result.Code = TraderResultEnum.SUCCESS;
            result.Result = account;

            return result;
        }

        protected override TraderResult internalCancelStock(string entrust_no)
        {

            StockCancelRequest t = new StockCancelRequest
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

            StockCancelResp resp = getResp<StockCancelResp, StockCancelRequest>(t);
            TraderResult ret = new TraderResult();
            if (resp.cssweb_code == SuccessCode)
            {
                ret.Code = TraderResultEnum.SUCCESS;
                ret.EntrustNo = resp.Item[0].entrust_no.ToString();
            }
            else
            {
                ret.Code = TraderResultEnum.ERROR;
            }

            return ret;
        }

        #endregion 重写方法

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
            //OcrUtil ocrUtil = new OcrUtil();
            //string code = ocrUtil.Recognise(image);
            //fivc.txtValidateCode.Text = code;
            fivc.ShowDialog();
            return fivc.txtValidateCode.Text;
        }

        /// <summary>
        /// 获取股东信息
        /// </summary>
        /// <param name="exchangeType"></param>
        /// <returns></returns>
        private StockHolderInfo GetStockHolder(int exchangeType)
        {
            StockHolderInfo holder = null;
            for (int i = 0; i < this.resAccountInfo.Item.Count; i++)
            {
                if (this.resAccountInfo.Item[i].exchange_type == exchangeType.ToString())
                {
                    holder = this.resAccountInfo.Item[i];
                }
            }

            return holder;
        }

        /// <summary>
        /// 获取股票持仓
        /// </summary>
        /// <returns></returns>
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

            GetStockPositionResp result = getResp<GetStockPositionResp, GetStockPositionRequest>(t);
            //string str2 = StockUtil.Base64Encode(URLHelper.GetDataWithOutEncode<GetStockPositionRequest>(t), this.encoding);
            //string positionUrl = "https://tradegw.htsc.com.cn/?" + str2;            
            //string respStr = this.httpClient.DownloadString(positionUrl);
            //GetStockPositionResp result = JsonConvert.DeserializeObject<GetStockPositionResp>(StockUtil.Base64Decode(respStr, this.GB2312));
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

        private T getResp<T, R>(R request)
        {
            string queryParams = StockUtil.Base64Encode(URLHelper.GetDataWithOutEncode<R>(request), this.GB2312);
            string sellUrl = "https://tradegw.htsc.com.cn/?" + queryParams;
            string resp = StockUtil.Base64Decode(this.httpClient.DownloadString(sellUrl), this.GB2312);

            Console.WriteLine("Web操作返回结果", resp);
            T ret = JsonConvert.DeserializeObject<T>(resp);
            return ret;
        }

        #region 不支持的操作

        public void GetCancelList()
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

        }

        public void GetTodayEntrusterList()
        {
            GetTodayEntrusterRequest t = new GetTodayEntrusterRequest
            {
                branch_no = this.resAccountInfo.branch_no,
                custid = this.resAccountInfo.fund_account,
                fund_account = this.resAccountInfo.fund_account,
                op_branch_no = this.resAccountInfo.branch_no,
                op_station = this.resAccountInfo.op_station,
                password = this.resAccountInfo.trdpwd,
                uid = this.resAccountInfo.uid,
                exchange_type = ""
            };
            //string entrusterParams = StockUtil.Base64Encode(URLHelper.GetDataWithOutEncode<GetTodayEntrusterRequest>(t1), this.GB2312);
            //string entrusterUrl = "https://tradegw.htsc.com.cn/?" + entrusterParams;
            //string resp = StockUtil.Base64Decode(this.httpClient.DownloadString(entrusterUrl), this.GB2312);

            //GetTodayEntrustResp ret = JsonConvert.DeserializeObject<GetTodayEntrustResp>(resp);
            GetTodayEntrustResp ret = getResp<GetTodayEntrustResp, GetTodayEntrusterRequest>(t);
            TraderResult result = new TraderResult();
            result.Result = ret;
            if (ret.cssweb_code == SuccessCode)
            {
                result.Code = TraderResultEnum.SUCCESS;
            }

            // return result;
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

        #endregion
    }
}
