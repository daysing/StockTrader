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
using System.Security.Cryptography.X509Certificates;
using System.Net.Security;

namespace Stock.Common
{
    public class HttpClient : WebClient
    {
        //private int _timeOut;
        //private CookieContainer cookieContainer;
        private string Referer;

        public HttpClient()
        {
            this.Timeout = 0xea60;
            this.Referer = "";
            this.Cookies = new CookieContainer();

        }

        public HttpClient(CookieContainer cookies)
        {
            this.Timeout = 0xea60;
            this.Referer = "";
            this.Cookies = cookies;
        }


        public bool CheckValidationResult(object sender, X509Certificate certificate, X509Chain chain, SslPolicyErrors errors)
        {
            return true;
        }

        protected override WebRequest GetWebRequest(Uri address)
        {
            ServicePointManager.ServerCertificateValidationCallback = new RemoteCertificateValidationCallback(this.CheckValidationResult);
            WebRequest webRequest = base.GetWebRequest(address);
            webRequest.Timeout = this.Timeout;
            if (webRequest is HttpWebRequest)
            {
                HttpWebRequest request2 = webRequest as HttpWebRequest;
                request2.Headers.Clear();
                request2.CookieContainer = this.Cookies;
                request2.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                request2.Headers.Add("Accept-Language", "zh-cn");
                request2.Headers.Add("UA-CPU", "x86");
                request2.Headers.Add("Accept-Charset", "gb2312,utf-8;q=0.7,*;q=0.7");
                if (this.IsGzip)
                {
                    request2.Headers.Add("Accept-Encoding", "gzip, deflate");
                }
                if (this.Referer != "")
                {
                    request2.Referer = this.Referer;
                }
                request2.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; zh-CN; rv:1.9.0.5) Gecko/2008120122 Firefox/3.0.5)";
                if (this.Referer != "")
                {
                    request2.Referer = this.Referer;
                }
                if (webRequest.Method.ToLower() == "post")
                {
                    request2.ContentType = "application/x-www-form-urlencoded";
                }
                this.Referer = address.ToString();
            }
            return webRequest;
        }

        // Properties
        public CookieContainer Cookies { get; set; }
        public bool IsGzip { get; set; }
        public int Timeout { get; set; }

    }
}
