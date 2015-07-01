using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace Beta.Utils
{
    public class ExpressionHelper
    {
        public bool flag = false;
        public string expre;
        public double result;
        public void startCalculate()
        {
            this.result = this.CalculateByExpression(expre);
        }
        public double CalculateByExpression(string expression)
        {
            Dictionary<string, Fraction> dic = new Dictionary<string, Fraction>();
            double result = 0D;
            int index = 0;
            string key = string.Empty;
            string value = string.Empty;
            double num = 0D;
            string pattern = string.Empty;
            string option = string.Empty;
            //step1 去除表达式里的空格
            expression = Regex.Replace(expression, @"\s+", string.Empty);
            //step2 替换第一个数字为分数表示
            pattern = @"^(\d+(\.\d+)?)";
            Match m = Regex.Match(expression, pattern);
            if (m.Success)
            {
                value = m.Groups[1].Value;
                num = double.Parse(value);
                key = string.Format("F{0}", index++);
                dic[key] = new Fraction
                {
                    Denominator = 1D,
                    Element = num
                };
                expression = Regex.Replace(expression, pattern, key);
            }
            //step3 替换紧贴括号的值为分数表示
            pattern = @"\((\d+(\.\d+)?)";
            m = Regex.Match(expression, pattern);
            while (m.Success)
            {
                value = m.Groups[1].Value;
                num = double.Parse(value);
                key = string.Format("F{0}", index++);
                dic[key] = new Fraction
                {
                    Denominator = 1D,
                    Element = num
                };
                expression = expression.Replace(m.Value, "(" + key);
                m = Regex.Match(expression, pattern);
            }
            //step4 替换+-*/后面跟的值为分数表示
            pattern = @"(\+|\-|\*|\/)(\d+(\.\d+)?)";
            m = Regex.Match(expression, pattern);
            while (m.Success)
            {
                option = m.Groups[1].Value;
                value = m.Groups[2].Value;
                num = double.Parse(value);
                key = string.Format("F{0}", index++);
                dic[key] = new Fraction
                {
                    Denominator = 1D,
                    Element = num
                };
                expression = expression.Replace(m.Value, option + key);
                m = Regex.Match(expression, pattern);
            }

            //step5 计算乘除和括号运算
            MatchCollection matchs = null;
            while (Regex.IsMatch(expression, @"\(|\)|\*\/"))
            {
                if (flag)
                {
                    return -99999999;
                }
                pattern = @"(F\d+)(\*|\/)(F\d+)";
                while (Regex.IsMatch(expression, pattern))
                {
                    m = Regex.Match(expression, pattern);
                    key = m.Groups[1].Value;
                    Fraction f1 = dic[key];
                    key = m.Groups[3].Value;
                    Fraction f2 = dic[key];
                    option = m.Groups[2].Value;
                    Fraction f = Calculate(f1, f2, option);
                    key = string.Format("F{0}", index++);
                    dic[key] = f;
                    expression = expression.Replace(m.Value, key);

                    while (Regex.IsMatch(expression, @"\(F\d+\)"))
                    {
                        m = Regex.Match(expression, @"\((F\d+)\)");
                        expression = expression.Replace(m.Value, m.Groups[1].Value);
                    }
                }

                //step3 计算括号里的值，并替换掉括号
                pattern = @"\((F\d+)(\+|\-)(F\d+)";
                while (Regex.IsMatch(expression, pattern))
                {
                    if (flag)
                    {
                        return -99999999;
                    }
                    m = Regex.Match(expression, pattern);
                    key = m.Groups[1].Value;
                    Fraction f1 = dic[key];
                    key = m.Groups[3].Value;
                    Fraction f2 = dic[key];
                    option = m.Groups[2].Value;
                    Fraction f = Calculate(f1, f2, option);
                    key = string.Format("F{0}", index++);
                    dic[key] = f;
                    expression = expression.Replace(m.Value, "(" + key);
                    while (Regex.IsMatch(expression, @"\(F\d+\)"))
                    {
                        m = Regex.Match(expression, @"\((F\d+)\)");
                        expression = expression.Replace(m.Value, m.Groups[1].Value);
                    }
                }
            }

            //step4 重复step2 计算乘除运算
            pattern = @"(F\d+)(\*|\/)(F\d+)";
            option = string.Empty;
            while (Regex.IsMatch(expression, pattern))
            {
                if (flag)
                {
                    return -99999999;
                }
                matchs = Regex.Matches(expression, pattern);
                foreach (Match match in matchs)
                {
                    key = match.Groups[1].Value;
                    Fraction f1 = dic[key];
                    key = match.Groups[3].Value;
                    Fraction f2 = dic[key];
                    option = match.Groups[2].Value;
                    Fraction f = Calculate(f1, f2, option);
                    key = string.Format("F{0}", index++);
                    dic[key] = f;
                    expression = expression.Replace(match.Value, key);
                }
            }

            //step5 重复step3 计算加减运算
            pattern = @"(F\d+)(\+|\-)(F\d+)";
            while (Regex.IsMatch(expression, pattern))
            {
                if (flag)
                {
                    return -99999999;
                }
                matchs = Regex.Matches(expression, pattern);
                foreach (Match match in matchs)
                {
                    key = match.Groups[1].Value;
                    Fraction f1 = dic[key];
                    key = match.Groups[3].Value;
                    Fraction f2 = dic[key];
                    option = match.Groups[2].Value;
                    Fraction f = Calculate(f1, f2, option);
                    key = string.Format("F{0}", index++);
                    dic[key] = f;
                    expression = expression.Replace(match.Value, key);
                }
            }

            //step6 最后一步 计算结果
            key = string.Format("F{0}", index - 1);
            Fraction fraction = dic[key];
            result = fraction.Element / fraction.Denominator;
            return result;
        }

        /// <summary>
        /// 计算两个分数的加减乘除，并返回一个分数表示
        /// </summary>
        /// <param name="expression"></param>
        /// <returns></returns>
        private Fraction Calculate(Fraction f1, Fraction f2, string option)
        {
            Fraction result = new Fraction();
            double dencominator = 0D;
            switch (option)
            {
                case "+":
                    dencominator = CalculateLeaseCommonMultiple(f1.Denominator, f2.Denominator);
                    f1.Element = (dencominator / f1.Denominator) * f1.Element;
                    f2.Element = (dencominator / f2.Denominator) * f2.Element;
                    result.Denominator = dencominator;
                    result.Element = f1.Element + f2.Element;
                    break;
                case "-":
                    dencominator = CalculateLeaseCommonMultiple(f1.Denominator, f2.Denominator);
                    f1.Element = (dencominator / f1.Denominator) * f1.Element;
                    f2.Element = (dencominator / f2.Denominator) * f2.Element;
                    result.Denominator = dencominator;
                    result.Element = f1.Element - f2.Element;
                    break;
                case "*":
                    dencominator = f1.Denominator * f2.Denominator;
                    result.Denominator = dencominator;
                    result.Element = f1.Element * f2.Element;
                    break;
                case "/":
                    dencominator = f1.Denominator * f2.Element;
                    result.Denominator = dencominator;
                    result.Element = f1.Element * f2.Denominator;
                    break;
            }
            return result;
        }

        /// <summary>
        /// 计算最小公倍数
        /// </summary>
        /// <param name="d1"></param>
        /// <param name="d2"></param>
        /// <returns></returns>
        private double CalculateLeaseCommonMultiple(double d1, double d2)
        {
            double result = Math.Max(d1, d2);
            double i = 1D;
            do
            {
                if (result % d1 == 0D
                    && result % d2 == 0D)
                {
                    break;
                }
                else
                {
                    result *= (++i);
                }
            } while (true);

            return result;
        }
    }
    /// <summary>
    /// 分数类
    /// </summary>
    public struct Fraction
    {
        private double m_Denominator;
        private double m_Element;
        /// <summary>
        /// 分母
        /// </summary>
        public double Denominator
        {
            get
            {
                return m_Denominator;
            }
            set
            {
                if (value == 0)
                {
                    throw new InvalidOperationException("分母不能为0");
                }
                m_Denominator = value;
            }
        }

        /// <summary>
        /// 分子
        /// </summary>
        public double Element
        {
            get
            {
                return m_Element;
            }
            set
            {
                m_Element = value;
            }
        }
    }

}
