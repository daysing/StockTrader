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
using System.Threading;

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

        public static void SetWindowText(IntPtr hWnd)
        {
            Win32API.SetWindowText(hWnd, "hello");
        }

        public static String GetClassName(IntPtr hWnd)
        {
            STRINGBUFFER sb;
            Win32API.GetClassName(hWnd, out sb, 40);

            return sb.szText;
        }

        public static IntPtr GetPriceTipInfoAndClickYes(IntPtr hWnd)
        {
            IntPtr tipInfoWindow = IntPtr.Zero;
            Win32API.EnumWindowsProc EnumWindowsProc = delegate(IntPtr p, int lParam)
            {
                if (p != IntPtr.Zero)
                {
                    String text = GetWindowText(p);
                    String clazz = GetClassName(p);

                    if (clazz == "#32770" && text == "")
                    {
                        Console.WriteLine("寻找价格提示窗口：{0}", Convert.ToString(p.ToInt64(), 16));
                        IntPtr hParent = Win32API.GetParent(p);
                        String pText = GetWindowText(hParent);
                        if (pText == @"网上股票交易系统5.0")
                        {
                            int staticId = 0x0410;
                            IntPtr pStatic = Win32API.GetDlgItem(p, staticId);
                            if (pStatic != IntPtr.Zero)
                            {
                                tipInfoWindow = p;

                                String sText = GetWindowText(pStatic);
                                if (sText == "委托价格的小数部分应为 2 位，是否继续？")
                                {
                                    int btnYesId = 0x0006;
                                    IntPtr btnYes = Win32API.GetDlgItem(p, btnYesId);
                                    Win32API.SendMessage(btnYes, Win32Code.WM_SETFOCUS, 0, 0);
                                    Win32API.SendMessage(btnYes, Win32Code.WM_LBUTTONDOWN, 0, 0);
                                    Win32API.SendMessage(btnYes, Win32Code.WM_LBUTTONUP, 0, 0);
                                    Win32API.SendMessage(btnYes, Win32Code.WM_LBUTTONDOWN, 0, 0);
                                    Win32API.SendMessage(btnYes, Win32Code.WM_LBUTTONUP, 0, 0);
                                }
                            }
                        }
                    }
                    return true;
                }

                return false;
            };

            Win32API.EnumChildWindows(IntPtr.Zero, EnumWindowsProc, new IntPtr(100));

            return tipInfoWindow;
        }

        /// <summary>
        /// 获取成交提示
        /// </summary>
        /// <param name="hWnd"></param>
        /// <returns></returns>
        public static String GetEntrustTipsAndClickYes(IntPtr hWnd)
        {
            string no = "0";
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
                                byte[] lParamStr = new byte[100];
                                Win32API.SendMessage(pStatic, Win32Code.WM_GETTEXT, 100, lParamStr);
                                String sText = Encoding.Unicode.GetString(lParamStr);
                                Console.WriteLine("找到委托提示窗口：{0}, 合同：{1}", Convert.ToString(p.ToInt64(), 16), sText);
                                if (sText.Contains("委托已成功提交"))
                                {
                                    int i = sText.IndexOf("。");
                                    no = sText.Substring(17, i - 17);
                                    int btnYesId = 0x0002;
                                    IntPtr btnYes = Win32API.GetDlgItem(p, btnYesId);

                                    //int pbId;
                                    //Win32API.GetWindowThreadProcessId(btnYes, out pbId);
                                    POINT pt = new POINT();
                                    pt.x = 1;
                                    pt.y = 1;
                                    Win32API.SendMessage(btnYes, Win32Code.WM_SETFOCUS, 0, 0);
                                    Win32API.PostMessage(btnYes, Win32Code.WM_MOUSEMOVE, 0, ref pt);
                                    Win32API.PostMessage(btnYes, Win32Code.WM_LBUTTONDOWN, 0, ref pt);
                                    Win32API.PostMessage(btnYes, Win32Code.WM_LBUTTONUP, 0, ref pt);
                                    Win32API.PostMessage(btnYes, Win32Code.WM_LBUTTONDOWN, 0, ref pt);
                                    Win32API.PostMessage(btnYes, Win32Code.WM_LBUTTONUP, 0, ref pt);
                                    //Win32API.keybd_event(13, 0, 0, pbId);
                                    //Win32API.keybd_event(13, 0, 2, pbId);

                                    Win32API.SendMessage(btnYes, Win32Code.WM_SETFOCUS, 0, 0);
                                    Win32API.SendMessage(btnYes, Win32Code.WM_LBUTTONDOWN, 0, 0);
                                    Win32API.SendMessage(btnYes, Win32Code.WM_LBUTTONUP, 0, 0);
                                    Win32API.SendMessage(btnYes, Win32Code.WM_LBUTTONDOWN, 0, 0);
                                    Win32API.SendMessage(btnYes, Win32Code.WM_LBUTTONUP, 0, 0);
                                }
                            }
                        }

                    }
                    return true;
                }

                return false;
            };

            Win32API.EnumChildWindows(IntPtr.Zero, EnumWindowsProc, new IntPtr(100));

           return no;
        }
    }
}
