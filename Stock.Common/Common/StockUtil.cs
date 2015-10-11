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

namespace Stock.Common
{
    public class StockUtil
    {
        private static Random random = new Random();
        public static string getCode(String code)
        {
            if (code.Length == 6)
                return code;
            else if (code.Length == 8)
                return code.Substring(2);

            return code;
        }

        public static string GetFullCode(String stockCode)
        {
            if (stockCode.Length == 6)
            {
                switch (stockCode.Substring(0, 2))
                {
                    case "00":
                    case "15":
                    case "16":
                    case "30":
                    case "39":
                        stockCode = "sz" + stockCode;
                        return stockCode;

                    case "51":
                    case "60":
                        stockCode = "sh" + stockCode;
                        return stockCode;
                }
            }
            return stockCode;
        }

        public static int GetExchangeType(String code)
        {
            if (GetFullCode(code) == "sz") return 2;
            else if(GetFullCode(code) == "sh") return 1;

            return 0;
        }

        public static string Base64Encode(String s, Encoding e)
        {
            byte[] bytes = e.GetBytes(s);

            return Convert.ToBase64String(bytes);
        }

        public static string Base64Decode(String s, Encoding e)
        {
            byte[] bytes = Convert.FromBase64String(s);
            return e.GetString(bytes);
        }

        public static string RandomString
        {
            get
            {
                return random.Next().ToString();
            }
        }
    }
}
