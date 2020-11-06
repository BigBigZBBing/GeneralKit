﻿using GeneralKit.Attributes;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Text.RegularExpressions;

namespace GeneralKit
{
    public static class Kit
    {
        /// <summary>
        /// 判断对象为NULL或者默认值
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns></returns>
        public static Boolean IsNull(this object obj)
        {
            if (obj.GetType() == typeof(string))
            {
                return string.IsNullOrEmpty(obj.ToString());
            }
            else if (obj.GetType() == typeof(sbyte))
            {
                if (obj == null || ((sbyte)obj) == default(sbyte))
                    return true;
            }
            else if (obj.GetType() == typeof(byte))
            {
                if (obj == null || ((byte)obj) == default(byte))
                    return true;
            }
            else if (obj.GetType() == typeof(ushort))
            {
                if (obj == null || ((ushort)obj) == default(ushort))
                    return true;
            }
            else if (obj.GetType() == typeof(short))
            {
                if (obj == null || ((short)obj) == default(short))
                    return true;
            }
            else if (obj.GetType() == typeof(uint))
            {
                if (obj == null || ((uint)obj) == default(uint))
                    return true;
            }
            else if (obj.GetType() == typeof(int))
            {
                if (obj == null || ((int)obj) == default(int))
                    return true;
            }
            else if (obj.GetType() == typeof(ulong))
            {
                if (obj == null || ((ulong)obj) == default(ulong))
                    return true;
            }
            else if (obj.GetType() == typeof(long))
            {
                if (obj == null || ((long)obj) == default(long))
                    return true;
            }
            else if (obj.GetType() == typeof(float))
            {
                if (obj == null || ((float)obj) == default(float))
                    return true;
            }
            else if (obj.GetType() == typeof(double))
            {
                if (obj == null || ((double)obj) == default(double))
                    return true;
            }
            else if (obj.GetType() == typeof(decimal))
            {
                if (obj == null || ((decimal)obj) == default(decimal))
                    return true;
            }
            else if (obj.GetType() == typeof(DateTime))
            {
                if (obj == null || ((DateTime)obj) == default(DateTime))
                    return true;
            }
            else if (obj.GetType() == typeof(TimeSpan))
            {
                if (obj == null || ((TimeSpan)obj) == default(TimeSpan))
                    return true;
            }
            else if (obj == null)
            {
                return true;
            }
            return false;
        }

        /// <summary>
        /// 判断对象不为NULL或者默认值
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static Boolean NotNull(this object obj)
        {
            return !obj.IsNull();
        }

        /// <summary>
        /// 获取枚举的Remark值
        /// </summary>
        /// <typeparam name="TEnum"></typeparam>
        /// <param name="em"></param>
        /// <returns></returns>
        public static String Remark<TEnum>(this TEnum em) where TEnum : Enum
        {
            Type type = em.GetType();
            FieldInfo field = type.GetField(em.ToString());
            if (field != null)
            {
                var attrs = field.GetCustomAttributes(typeof(RemarkAttribute), true) as RemarkAttribute[];
                if (attrs != null && attrs.Length > 0)
                    return attrs[0].Remark;
            }
            return "";
        }

        /// <summary>
        /// <para>验证模型</para>
        /// <para>(未设置错误消息不验证)</para>
        /// <para>{0}属性名称</para>
        /// <para>{1}属性值</para>
        /// <para>{2}属性描述</para>
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Entity"></param>
        /// <returns></returns>
        public static Boolean ModelValidation<TEntity>(this TEntity Entity) where TEntity : ICheckVerify
        {
            StringBuilder strBuilder = new StringBuilder();
            try
            {
                PropertyInfo[] properties = Entity.GetType().GetProperties();
                if (properties != null)
                {
                    foreach (PropertyInfo propertie in properties)
                    {
                        Attribute Attributes = propertie.GetCustomAttribute(typeof(RuleAttribute));
                        if (Attributes.NotNull())
                        {
                            Type Type = propertie.PropertyType;
                            string Name = propertie.Name;
                            object Value = propertie.GetValue(Entity);
                            decimal @decimal;
                            RuleAttribute Attr_properties = Attributes as RuleAttribute;
                            if (Attr_properties.Error.NotNull())
                            {
                                if (Attr_properties.maxLength.NotNull() && Type == typeof(string))
                                {
                                    if (Value.ToString().Length > Attr_properties.MaxLength)
                                    {
                                        strBuilder.Append(string.Format("\r\n" + Attr_properties.Error, Name, Value, Attr_properties.Name));
                                        break;
                                    }
                                }
                                if (Attr_properties.minLength.NotNull() && Type == typeof(string))
                                {
                                    if (Value.ToString().Length < Attr_properties.MinLength)
                                    {
                                        strBuilder.Append(string.Format("\r\n" + Attr_properties.Error, Name, Value, Attr_properties.Name));
                                        break;
                                    }
                                }
                                if (Attr_properties.allowEmpty.NotNull())
                                {
                                    if (Value.IsNull())
                                    {
                                        strBuilder.Append(string.Format("\r\n" + Attr_properties.Error, Name, Value, Attr_properties.Name));
                                        break;
                                    }
                                }
                                if (Attr_properties.greater.NotNull() && decimal.TryParse(Value.ToString(), out @decimal))
                                {
                                    if (@decimal > Attr_properties.Greater)
                                    {
                                        strBuilder.Append(string.Format("\r\n" + Attr_properties.Error, Name, Value, Attr_properties.Name));
                                        break;
                                    }
                                }
                                if (Attr_properties.less.NotNull() && decimal.TryParse(Value.ToString(), out @decimal))
                                {
                                    if (@decimal < Attr_properties.Less)
                                    {
                                        strBuilder.Append(string.Format("\r\n" + Attr_properties.Error, Name, Value, Attr_properties.Name));
                                        break;
                                    }
                                }
                                if (Attr_properties.equal.NotNull() && decimal.TryParse(Value.ToString(), out @decimal))
                                {
                                    if (@decimal == Attr_properties.Equal)
                                    {
                                        strBuilder.Append(string.Format("\r\n" + Attr_properties.Error, Name, Value, Attr_properties.Name));
                                        break;
                                    }
                                }
                                if (Attr_properties.regExp.NotNull())
                                {
                                    if (!Regex.IsMatch(Value?.ToString(), Attr_properties.regExp))
                                    {
                                        strBuilder.Append(string.Format("\r\n" + Attr_properties.Error, Name, Value, Attr_properties.Name));
                                        break;
                                    }
                                }
                                if (Attr_properties.expType.NotNull())
                                {
                                    if (!Regex.IsMatch(Value?.ToString(), Attr_properties.expType?.Remark()))
                                    {
                                        strBuilder.Append(string.Format("\r\n" + Attr_properties.Error, Name, Value, Attr_properties.Name));
                                        break;
                                    }
                                }
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                throw new Exception(ex.Message);
            }

            if (strBuilder.Length > 0)
                throw new Exception(strBuilder.ToString());
            return true;
        }

        /// <summary>
        /// 金钱转大写
        /// </summary>
        /// <param name="Num">数字</param>
        /// <returns></returns>
        public static String MoneyUpper(this decimal num)
        {
            string str1 = "零壹贰叁肆伍陆柒捌玖";            //0-9所对应的汉字 
            string str2 = "万仟佰拾亿仟佰拾万仟佰拾元角分"; //数字位所对应的汉字 
            string str3 = "";    //从原num值中取出的值 
            string str4 = "";    //数字的字符串形式 
            string str5 = "";  //人民币大写金额形式 
            int i;    //循环变量 
            int j;    //num的值乘以100的字符串长度 
            string ch1 = "";    //数字的汉语读法 
            string ch2 = "";    //数字位的汉字读法 
            int nzero = 0;  //用来计算连续的零值是几个 
            int temp;            //从原num值中取出的值 

            num = Math.Round(Math.Abs(num), 2);    //将num取绝对值并四舍五入取2位小数 
            str4 = ((long)(num * 100)).ToString();        //将num乘100并转换成字符串形式 
            j = str4.Length;      //找出最高位 
            if (j > 15) { return "溢出"; }
            str2 = str2.Substring(15 - j);   //取出对应位数的str2的值。如：200.55,j为5所以str2=佰拾元角分 

            //循环取出每一位需要转换的值 
            for (i = 0; i < j; i++)
            {
                str3 = str4.Substring(i, 1);          //取出需转换的某一位的值 
                temp = Convert.ToInt32(str3);      //转换为数字 
                if (i != (j - 3) && i != (j - 7) && i != (j - 11) && i != (j - 15))
                {
                    //当所取位数不为元、万、亿、万亿上的数字时 
                    if (str3 == "0")
                    {
                        ch1 = "";
                        ch2 = "";
                        nzero = nzero + 1;
                    }
                    else
                    {
                        if (str3 != "0" && nzero != 0)
                        {
                            ch1 = "零" + str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                    }
                }
                else
                {
                    //该位是万亿，亿，万，元位等关键位 
                    if (str3 != "0" && nzero != 0)
                    {
                        ch1 = "零" + str1.Substring(temp * 1, 1);
                        ch2 = str2.Substring(i, 1);
                        nzero = 0;
                    }
                    else
                    {
                        if (str3 != "0" && nzero == 0)
                        {
                            ch1 = str1.Substring(temp * 1, 1);
                            ch2 = str2.Substring(i, 1);
                            nzero = 0;
                        }
                        else
                        {
                            if (str3 == "0" && nzero >= 3)
                            {
                                ch1 = "";
                                ch2 = "";
                                nzero = nzero + 1;
                            }
                            else
                            {
                                if (j >= 11)
                                {
                                    ch1 = "";
                                    nzero = nzero + 1;
                                }
                                else
                                {
                                    ch1 = "";
                                    ch2 = str2.Substring(i, 1);
                                    nzero = nzero + 1;
                                }
                            }
                        }
                    }
                }
                if (i == (j - 11) || i == (j - 3))
                    ch2 = str2.Substring(i, 1);
                str5 = str5 + ch1 + ch2;
                if (i == j - 1 && str3 == "0")
                    str5 = str5 + '整';
            }
            if (num == 0)
                str5 = "零元整";
            return str5;
        }

        /// <summary>
        /// 运行Shell命令
        /// </summary>
        /// <param name="command"></param>
        /// <returns></returns>
        public static String RunCmd(string command)
        {
            Process process = new Process();
            process.StartInfo.FileName = "cmd.exe";
            process.StartInfo.UseShellExecute = false;
            process.StartInfo.RedirectStandardInput = true;
            process.StartInfo.RedirectStandardOutput = true;
            process.StartInfo.RedirectStandardError = true;
            process.StartInfo.CreateNoWindow = true;
            process.Start();
            process.StandardInput.WriteLine(command);
            process.StandardInput.WriteLine("exit");
            process.StandardInput.AutoFlush = true;
            string value = process.StandardOutput.ReadToEnd();
            process.WaitForExit();
            process.Close();
            return value;
        }

        /// <summary>
        /// 获取模型的字段字符串
        /// </summary>
        public static String Key<TModel>(this TModel model, Expression<Func<TModel, object>> expression) where TModel : class
        {
            if (expression == null) throw new ArgumentNullException();
            return GetPropertyVlaue("Body.Operand.Member.Name", expression) as string;
        }

        private static object GetPropertyVlaue(string fullPath, object obj)
        {
            var o = obj;
            fullPath.Split(new[] { '.' }, StringSplitOptions.RemoveEmptyEntries).ToList().ForEach(point =>
            {
                var p = o.GetType().GetProperty(point, BindingFlags.Public | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Static);
                try { o = p.GetValue(o, null); } catch { }
            });
            return o;
        }
    }
}