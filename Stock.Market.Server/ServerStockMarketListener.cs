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
using System.Runtime.InteropServices;
using WebSocketSharp;
using System.Diagnostics;
using Stock.Trader.Settings;
using System.Threading;
using System.Windows.Forms;
using Stock.Common;

namespace Stock.Market.Server
{

    /// <summary>
    /// 从Server端获取行情数据
    /// 1、 登录
    /// 2、 POST json request
    /// 3、 response json
    /// </summary>
    public class ServerStockMarketListener : StockMarketListener
    {
        public ServerStockMarketListener()
        {
        }

        private Bid Parse(string data)
        {
            Bid bid = null;
            bid = new Bid();
            string[] item = data.Split(new Char[] { ':', ',' });
            if (item[9].Length != 8) return null;
            string exchange = item[11].Substring(1, 4);
            string code = item[9].Substring(1, 6);
            if (exchange == "XSHG")
                bid.Code = "sh" + item[9].Substring(1, 6);
            else // if (exchange == "XSHE")
                bid.Code = "sz" + item[9].Substring(1, 6);

            //if (!bid.Code.StartsWith("sz150") && !bid.Code.StartsWith("sh502") &&
            //    !bid.Code.StartsWith("sh000") && !bid.Code.StartsWith("sz399"))
            //    return null;

            bid.CurrentPrice = float.Parse(item[53]);
            bid.High = float.Parse(item[57]);
            bid.Open = float.Parse(item[55]);
            bid.Low = float.Parse(item[59]);
            bid.LastClose = float.Parse(item[61]);
            bid.PushTime = new DateTime(long.Parse(item[63])).ToString();
            bid.Name = item[7].Substring(1,item[7].Length-2);
            bid.Volumn = long.Parse(item[5]);
            bid.Turnover = float.Parse(item[3]);

            bid.AddBuyGoodsData(new Bid.GoodsData(float.Parse(item[13]), int.Parse(item[23])));
            bid.AddBuyGoodsData(new Bid.GoodsData(float.Parse(item[15]), int.Parse(item[25])));
            bid.AddBuyGoodsData(new Bid.GoodsData(float.Parse(item[17]), int.Parse(item[27])));
            bid.AddBuyGoodsData(new Bid.GoodsData(float.Parse(item[19]), int.Parse(item[29])));
            bid.AddBuyGoodsData(new Bid.GoodsData(float.Parse(item[21]), int.Parse(item[31])));

            bid.AddSellGoodsData(new Bid.GoodsData(float.Parse(item[33]), int.Parse(item[43])));
            bid.AddSellGoodsData(new Bid.GoodsData(float.Parse(item[35]), int.Parse(item[45])));
            bid.AddSellGoodsData(new Bid.GoodsData(float.Parse(item[37]), int.Parse(item[47])));
            bid.AddSellGoodsData(new Bid.GoodsData(float.Parse(item[39]), int.Parse(item[49])));
            bid.AddSellGoodsData(new Bid.GoodsData(float.Parse(item[41]), int.Parse(item[51])));
 
            return bid;
        }

        const string SERVER = "server";
        private WebSocket ws = null;
        public override void Run()
        {
            WSClient.Instance.ReceivedRealTime += new WSClient.Received(WSClient_ReceivedRealTime);
            WSClient.Instance.SendMessage("30:" + String.Join(",", codes));

            //ws = new WebSocket(String.Format("ws://{0}/websock/stockmarket", Configure.GetStockTraderItem(SERVER)));
            //internalRun();
        }

        void WSClient_ReceivedRealTime(string message)
        {
            Bid bid = Parse(message);
            StockMarketManager.AddBid(bid);
        }

       // private void internalRun()
       // {
       //     connectWebSocketServer();
       // }

       // void connectWebSocketServer()
       // {
       //     ws.OnMessage += new EventHandler<MessageEventArgs>(ws_OnMessage);
       //     ws.OnOpen += new EventHandler(ws_OnOpen);
       //     ws.OnError += new EventHandler<ErrorEventArgs>(ws_OnError);
       //     ws.Connect();
       //     ws.Send(String.Join(",", codes));
       // }

       // public override void Close()
       // {
       //     ws.Close();
       // }

       // void ws_OnError(object sender, ErrorEventArgs e)
       // {
       //     ws.OnMessage -= new EventHandler<MessageEventArgs>(ws_OnMessage);
       //     ws.OnOpen -= new EventHandler(ws_OnOpen);
       //     ws.OnError -= new EventHandler<ErrorEventArgs>(ws_OnError);
       //     ws.Close();
       //     ws = null;
       //     MessageBox.Show("链接行情服务器出错，1秒后重连");
       //     Thread.Sleep(500);
       //     connectWebSocketServer();
       //     //ws.Connect();
       //     //ws.Send(String.Join(",", codes));
       //     //Console.WriteLine(e.Exception);
       //}

       // void ws_OnOpen(object sender, EventArgs e)
       // {
       //     Console.WriteLine("打开链接");
       // }

       // void ws_OnMessage(object sender, MessageEventArgs e)
       // {
       //     Bid bid = Parse(e.Data);
       //     StockMarketManager.AddBid(bid);
       //     if (bid != null && bid.Code == "sh502007") Console.WriteLine(e.Data);
       // }

       // private void Login()
       // {
       // }
    }
}
