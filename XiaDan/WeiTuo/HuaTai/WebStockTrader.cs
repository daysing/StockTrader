using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using Stock.Common;
using XiaDan.WeiTuo.HuaTai;
using System.Windows.Forms;
using System.IO;
using System.Drawing;
using System.Drawing.Imaging;

namespace Stock.Trader.WeiTuo.HuaTai
{
    class WebStockTrader : IStockTrader
    {
        private CookieContainer Cookie;
        private Encoding encoding;
        private Encoding UTF8;
        private HttpClient httpClient;
        
        public void Init()
        {
            throw new NotImplementedException();
        }

        public void Login()
        {
            string address = "https://service.htsc.com.cn/service/loginAction.do?method=login";
            this.httpClient.DownloadString(address);

            LoginPostInfo t = new LoginPostInfo
            {
                // hddInfo = this.HddInfo,
                // macaddr = this.Mac,
                // servicePwd = this.TransactionPassword,
                trdpwd = "781113",
                trdpwdEns = "750404",
                userName = "666600164015"
            };

            string verifyCode = this.GetVerifyCode();
            if (verifyCode == "")
            {
                MessageBox.Show("获取验证码失败");
            }

            string str4 = this.httpClient.UploadString(address, data);

        }

        private string GetVerifyCode()
        {
            for (int i = 0; i < 15; i++)
            {
                MemoryStream stream = new MemoryStream(this.httpClient.DownloadData("https://service.htsc.com.cn/service/pic/verifyCodeImage.jsp?ran=" + StringHelper.GetRndNumber()));
                Bitmap bitmap = new Bitmap(stream);
                Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);
                Bitmap image = bitmap.Clone(rect, PixelFormat.Format8bppIndexed);
                string str = new OCR().Recognise(image);
                if (str.Length == 4)
                {
                    return str;
                }
            }
            return "";
        }

 


        public void SellStock(string code, float price, int num)
        {
            throw new NotImplementedException();
        }

        public void BuyStock(string code, float price, int num)
        {
            throw new NotImplementedException();
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
            throw new NotImplementedException();
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
    }
}
