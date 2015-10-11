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

namespace Stock.Formula
{
    public class AbstractFormula
    {
        #region 行情函数

        public ValueList OPEN
        {
            get { return null; }
        }

        public ValueList O
        {
            get { return OPEN; }
        }

        public ValueList CLOSE
        {
            get { return null; }
        }

        public ValueList C
        {
            get { return CLOSE; }
        }

        public static ValueList HIGH
        {
            get { return null; }
        }

        public static ValueList H
        {
            get { return HIGH; }
        }

        public ValueList LOW
        {
            get { return null; }
        }

        public ValueList L
        {
            get { return LOW; }
        }

        public ValueList VOL
        {
            get { return null; }
        }

        public ValueList V
        {
            get { return VOL; }
        }

        public ValueList ADVANCE
        {
            get { return null; }
        }

        public ValueList DECLINE
        {
            get { return null; }
        }

        public ValueList AMOUNT
        {
            get { return null; }
        }

        #endregion

        #region 引用函数

        public ValueList REF(ValueList X, int A) {
            return null;
        }

        public ValueList SUM(ValueList X, int N)
        {
            return null;
        }

        public ValueList MA(ValueList X, int M)
        {
            return null;
        }

        public ValueList EXPMA(ValueList X, int M)
        {
            return null;
        }

        public ValueList EMA(ValueList X, int M)
        {
            return EXPMA(X, M);
        }

        public ValueList MEMA(ValueList X, int M)
        {
            return null;
        }

        public ValueList EXPMEMA(ValueList X, int M)
        {
            return null;
        }

        public ValueList DMA
        {
            get { return null; }
        }

        public ValueList HHV
        {
            get { return null; }
        }

        public ValueList LLV
        {
            get { return null; }
        }

        #endregion

        #region 逻辑函数

        public bool CROSS(ValueList A, ValueList B)
        {
            return false;
        }

        public bool LONGCROSS(ValueList A, ValueList B, int N)
        {
            return false;
        }

        public bool UPNDAY(ValueList CLOSE, int M)
        {
            return false;
        }

        public bool DOWNNDAY(ValueList CLOSE, int M)
        {
            return false;
        }

        #endregion

        #region 统计函数

        /// <summary>
        /// 平均绝对方差
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public ValueList AVEDEV(ValueList X, int N)
        {
            return null;
        }

        /// <summary>
        /// 数据偏差平方和
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public ValueList DEVSQ(ValueList X, int N)
        {
            return null;
        }

        /// <summary>
        /// 估算标准差
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public ValueList STD(ValueList X, int N)
        {
            return null;
        }

        /// <summary>
        /// 总体标准差
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public ValueList STDP(ValueList X, int N)
        {
            return null;
        }

        /// <summary>
        /// 线性回归预测值
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public ValueList FORCAST(ValueList X, int N)
        {
            return null;
        }

        /// <summary>
        /// 估算样本方差
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public ValueList VAR(ValueList X, int N)
        {
            return null;
        }

        /// <summary>
        /// 总体样本方差
        /// </summary>
        /// <param name="X"></param>
        /// <param name="N"></param>
        /// <returns></returns>
        public ValueList VARP(ValueList X, int N)
        {
            return null;
        }

        #endregion

        #region 形态函数
        #endregion
    }
}
