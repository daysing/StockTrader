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

namespace Stock.Market
{
    public struct MarketInfo
    {
        public enum MarketType
        {
            MARKET_FIRST,
            MARKET_SHANGHAI_A = MARKET_FIRST,
            MARKET_SHANGHAI_B,
            MARKET_SHENZHEN_A,
            MARKET_SHENZHEN_B,
            MARKET_WARRANT,		// 权证
            MARKET_INDEX,			// 指数
            MARKET_MAX,
            MARKET_UNKNOWN = MARKET_MAX,
        };

        public static char get_block_from_market_type(MarketType t)
	    {
		    switch(t)
		    {
		    case MarketType.MARKET_SHANGHAI_A:
			    return (char)0;
		    case MarketType.MARKET_SHANGHAI_B:
			    return (char)1;
		    case MarketType.MARKET_SHENZHEN_A:
			    return (char)2;
		    case MarketType.MARKET_SHENZHEN_B:
			    return (char)3;
		    case MarketType.MARKET_INDEX:
			    return (char)11;			// 所有指数
		    case MarketType.MARKET_WARRANT:
			    return (char)13;			// 权证
		    default:
			    return (char)0;
		    }
	    }
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	public static uint[] stocks_count;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 6)]
	public static HashSet<string>[] stocks_set;

	public static int StocksCodeLen = 6;	// 100  股一手
	public static int StocksPerHand = 100;	// 100  股一手.
    public static float tax;				// 0.3 %

    }

}
