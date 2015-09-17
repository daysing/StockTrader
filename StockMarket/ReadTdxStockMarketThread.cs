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
using System.Net.Sockets;
using Stock.Market.Tdx;

namespace Stock.Market
{
    public class ReadTdxStockMarketThread : ReadStockMarketThread
    {
        public override void Run()
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Parse("218.18.103.38"), 7709);

            if (tcpClient.Connected)
            {
                Console.WriteLine("成功连接");
                NetworkStream ns = tcpClient.GetStream();
                Console.WriteLine(ns.ToString());

                tcpClient.SendBufferSize = 1024;
             
            }
        }

        public int testrun()
        {
            TcpClient tcpClient = new TcpClient();
            tcpClient.Connect(IPAddress.Parse("218.18.103.38"), 7709);

            if (tcpClient.Connected)
            {
                Console.WriteLine("成功连接");

                // InstantTransReq req = new InstantTransReq(1);
                StockListReq req = new StockListReq(MarketInfo.MarketType.MARKET_SHANGHAI_A);
                req.Send(tcpClient);

                Read(tcpClient);
            }

            return 0;
        }

        public void Read(TcpClient client)
        {
            NetworkStream st = client.GetStream();
            if ((client != null) && (client.Connected))
            {
                // asyncread(client);
                byte[] bytes = new Byte[1024];
                string data = string.Empty;
                int length = st.Read(bytes, 0, bytes.Length);
                Console.WriteLine("读取到数据");
            }
        }
    }
}
