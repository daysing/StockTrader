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
