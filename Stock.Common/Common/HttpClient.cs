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
using System.Threading;

namespace Stock.Common
{
    public delegate void TimeoutCaller(object userdata);

    public class Calculagraph
    {
        /// <summary>
        /// 时间到事件
        /// </summary>
        public event TimeoutCaller TimeOver;

        /// <summary>
        /// 开始时间
        /// </summary>
        private DateTime _startTime;
        private TimeSpan _timeout = new TimeSpan(0, 0, 10);
        private bool _hasStarted = false;
        object _userdata;

        /// <summary>
        /// 计时器构造方法
        /// </summary>
        /// <param name="userdata">计时结束时回调的用户数据</param>
        public Calculagraph(object userdata)
        {
            TimeOver += new TimeoutCaller(OnTimeOver);
            _userdata = userdata;
        }

        /// <summary>
        /// 超时退出
        /// </summary>
        /// <param name="userdata"></param>
        public virtual void OnTimeOver(object userdata)
        {
            Stop();
        }

        /// <summary>
        /// 过期时间(秒)
        /// </summary>
        public int Timeout
        {
            get
            {
                return _timeout.Seconds;
            }
            set
            {
                if (value <= 0)
                    return;
                _timeout = new TimeSpan(0, 0, value);
            }
        }

        /// <summary>
        /// 是否已经开始计时
        /// </summary>
        public bool HasStarted
        {
            get
            {
                return _hasStarted;
            }
        }

        /// <summary>
        /// 开始计时
        /// </summary>
        public void Start()
        {
            Reset();
            _hasStarted = true;
            Thread th = new Thread(WaitCall);
            th.IsBackground = true;
            th.Start();
        }

        /// <summary>
        /// 重置
        /// </summary>
        public void Reset()
        {
            _startTime = DateTime.Now;
        }

        /// <summary>
        /// 停止计时
        /// </summary>
        public void Stop()
        {
            _hasStarted = false;
        }

        /// <summary>
        /// 检查是否过期
        /// </summary>
        /// <returns></returns>
        private bool checkTimeout()
        {
            return (DateTime.Now - _startTime).Seconds >= Timeout;
        }

        private void WaitCall()
        {
            try
            {
                //循环检测是否过期
                while (_hasStarted && !checkTimeout())
                {
                    Thread.Sleep(1000);
                }
                if (TimeOver != null)
                    TimeOver(_userdata);
            }
            catch (Exception)
            {
                Stop();
            }
        }
    }
    public class HttpClient : WebClient
    {
        //private int _timeOut;
        //private CookieContainer cookieContainer;
        private string Referer;

        public HttpClient()
        {
            this.Timeout = 0xea60;
            this.Proxy = null;
            this.Referer = "";
            this.Cookies = new CookieContainer();
            ServicePointManager.DefaultConnectionLimit = 10;
        }

        public HttpClient(CookieContainer cookies)
        {
            this.Timeout = 0x500;
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
            if (webRequest is HttpWebRequest)
            {
                HttpWebRequest req = webRequest as HttpWebRequest;
                req.Headers.Clear();
                req.CookieContainer = this.Cookies;
                req.Accept = "text/html,application/xhtml+xml,application/xml;q=0.9,*/*;q=0.8";
                req.Headers.Add("Accept-Language", "zh-cn");
                req.Headers.Add("UA-CPU", "x86");
                req.Headers.Add("Accept-Charset", "gb2312,utf-8;q=0.7,*;q=0.7");
                req.Timeout = this.Timeout;
                req.ReadWriteTimeout = this.Timeout;
                if (this.IsGzip)
                {
                    req.Headers.Add("Accept-Encoding", "gzip, deflate");
                }
                if (this.Referer != "")
                {
                    req.Referer = this.Referer;
                }
                req.UserAgent = "Mozilla/5.0 (Windows; U; Windows NT 6.0; zh-CN; rv:1.9.0.5) Gecko/2008120122 Firefox/3.0.5)";
                if (this.Referer != "")
                {
                    req.Referer = this.Referer;
                }
                if (webRequest.Method.ToLower() == "post")
                {
                    req.ContentType = "application/x-www-form-urlencoded";
                }
                this.Referer = address.ToString();
            }
            return webRequest;
        }

        public void DownloadStringAsyncWithTimeout(Uri address)
        {
            if (_timer == null)
            {
                _timer = new Calculagraph(this);
                _timer.Timeout = Timeout;
                _timer.TimeOver += new TimeoutCaller(_timer_TimeOver);
                this.DownloadProgressChanged += new DownloadProgressChangedEventHandler(HttpClient_DownloadProgressChanged);
            }
            DownloadStringAsync(address);
            _timer.Start();
        }
        

        void HttpClient_DownloadProgressChanged(object sender, DownloadProgressChangedEventArgs e)
        {
            if (e.ProgressPercentage == 100)
            {
                Console.WriteLine("复位计数器");
                _timer.Reset();//重置计时器
            }
        }

        /// <summary>
        /// 超时的操作
        /// </summary>
        /// <param name="userdata"></param>
        private void _timer_TimeOver(object userdata)
        {
            Console.WriteLine("计数器发现超时");
            CancelAsync();
            if (DownloadStringTimeout != null)
                DownloadStringTimeout(this);
        }

        public delegate void DownloadStringTimeoutEventHandler(object sender);
        public event DownloadStringTimeoutEventHandler DownloadStringTimeout;

        // Properties
        public CookieContainer Cookies { get; set; }
        public bool IsGzip { get; set; }
        public int Timeout { get; set; }
        private Calculagraph _timer;
    }
}
