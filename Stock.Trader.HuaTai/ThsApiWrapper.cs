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
using System.Window;

namespace Stock.Trader.THS
{
    public class ThsApiWrapper
    {
        const string TITLE = @"网上股票交易系统5.0";
        public static IntPtr FindThsWindow()
        {
            return Win32API.FindWindow(null, TITLE);
        }

        public static String GetWindowText(IntPtr hWnd)
        {
            STRINGBUFFER sb;
            Win32API.GetWindowText(hWnd, out sb, 500);

            return sb.szText;
        }

        public static String GetClassName(IntPtr hWnd)
        {
            STRINGBUFFER sb;
            Win32API.GetClassName(hWnd, out sb, 40);

            return sb.szText;
        }

        /// <summary>
        /// 获取成交提示
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static IntPtr GetEntrustTips(IntPtr hWnd)
        {
            IntPtr confirmWindow = IntPtr.Zero;
            Win32API.EnumWindowsProc EnumWindowsProc = delegate(IntPtr p, int lParam)
            {
                if (p != IntPtr.Zero)
                {
                    String text = GetWindowText(p);
                    String clazz = GetClassName(p);

                    if (clazz == "#32770" && text == "")
                    {
                        IntPtr hParent = Win32API.GetParent(p);
                        String pText = GetWindowText(hParent);
                        if(pText == @"网上股票交易系统5.0") {
                            int staticId = 0x03EC;
                            IntPtr pStatic = Win32API.GetDlgItem(p, staticId);
                            if(pStatic != IntPtr.Zero) {
                                confirmWindow = p;

                                String sText = GetWindowText(pStatic);
     
                                // 检查static label, 获取合同编号
                                Console.WriteLine("IntPtr: {2}, text: {0}, clazz: {1}====Parent Ptr: {3}, Text;{4}", text, clazz, Convert.ToString(p.ToInt32(), 16), hParent, pText);
                                Console.WriteLine("text: {0}, Ptr{1}", sText, Convert.ToString(pStatic.ToInt32(), 16));
                            }
                        }

                    }
                    return true;
                }

                return false;
            };

           Win32API.EnumChildWindows(hWnd, EnumWindowsProc, new IntPtr(100));

           return confirmWindow;
        }
    }
}
