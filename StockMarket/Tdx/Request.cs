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
using System.Runtime.InteropServices;
using System.Net.Sockets;
using System.Runtime.Serialization;

namespace Stock.Market.Tdx
{
    public class Request
    {
        public virtual void Send(TcpClient client)
        {

        }

        public void Next()
        {
        }

        public static bool Ready()
        {
            return true;
        }

        public static void Res_seq_id(uint id)
        {
            if (id >= Seq_id || id == 0)		// id == 0 rewind
            {
                Seq_id = id;
                Received = true;
            }
        }

        public bool First {get; set;}
        public static uint Seq_id{get; set;}
        public static bool Received{get; set;}
    }

    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct ReqHead
    {
         public ReqHead(ushort cmd_id, ushort packet_len)
        {
            // TODO: Complete member initialization
            zip = (char)0x0c;
            seq_id = 0;
            packet_type = (char)1;
            len = len1 = (ushort)(packet_len - 20 + 2);
            cmd = cmd_id;
        }

        public char zip;		    	// always 0x0c: data-uncompressed
        public uint seq_id;		// 同一种命令的 seq_id。
        public char packet_type;	// 00: 回应。 1,2,3... request count
        public ushort len;		//	数据长度
        public ushort len1;		//  数据长度重复
        public ushort cmd;		// b4 bf: 分钟线。。b5 bf 单笔成交
    }

    public class StockHeartBeat : Request
    {
    }

    public class StockListReq : Request
    {
		public StockListReq(MarketInfo.MarketType market_code, ushort record_offset, ushort record_count, ushort record_total) {
        }
		public StockListReq(MarketInfo.MarketType market_code) : this(market_code, 0, 200, 0) {
        }

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi)]
        public struct StockListStruct
		{
            public StockListStruct(MarketInfo.MarketType market_code, ushort record_offset, ushort record_count)
            {
                header = new ReqHead((ushort)0x0524, (ushort)20); // 20  = size of StockListStruct
                block =MarketInfo.get_block_from_market_type(market_code);
                unknown1 = 0;
                unknown2 = 0;
                offset = record_offset;
                count = record_count;
            }
			public ReqHead	header;
			public ushort block;		//  00 00 上A, 	01 00 上B, 	02 00 深A, 	03 00 深B, 	0x0d: 权证
			public ushort unknown1;
			public ushort offset;
			public ushort count;
			public ushort unknown2;
		}
		public ushort total;   
    }

    public class StockHoldChgReq : Request
    {
    }

    public class InstantTransReq : Request
    {
        public InstantTransReq(int count)
        {
        }

        public InstantTransReq(string code, int count)
        {
        }

        public override void Send(TcpClient client)
        {
            NetworkStream st = client.GetStream();
            InstantTransReq.InstantTransStruct its = new InstantTransStruct();
            its.code = "000001".ToCharArray();
            its.count = 1;
            its.offset = 0;

            byte[] tosend = Struct_Transform.StructToBytes(its);
            if (st.CanWrite)
            {
                IAsyncResult ar = st.BeginWrite(tosend, 0, tosend.Length, null, null);
                if (!ar.AsyncWaitHandle.WaitOne())
                {
                    Console.WriteLine("Error");
                }
                st.EndWrite(ar); 
                //return 1;
            }
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

        [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
        public unsafe struct InstantTransStruct 
        {
            public ReqHead header;
            public ushort location;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
            public char[] code;
            public ushort offset;
            public ushort count;
        }
    }

}
