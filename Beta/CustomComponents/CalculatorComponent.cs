using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.RegularExpressions;
using System.Threading;

using Beta.Model;
using Beta.Model.ComponentSystem;
using Beta.Utils;

namespace Beta.CustomComponents
{
    public class CalculatorComponent : Component
    {
        public override ComponentMetadata Metadata { get; set; }

        public override ComponentContext Context { get; set; }

        private ExpressionHelper ex;

        protected override void DoInit(ComponentContext context)
        {
            Context = context;
        }

        protected override List<Result> DoQuery(Query query)
        {
            if (query.QueryText.Trim().Length <= 1) return new List<Result>();

            if (!CheckExpressionValid(query.QueryText)) return new List<Result>();

            ex = new ExpressionHelper()
            {
                expre = query.QueryText,
            };
            ex.startCalculate();

            double answer = ex.result;
            if (answer == -99999999) return new List<Result>();

            Result result = new Result()
            {
                Title = answer.ToString(),
                SubTitle = query.QueryText,
                IconPath = "%windir%/system32/calc.exe",
                Score = 1000,
                Action = () =>
                {
                    Context.InvokeMethodInMainWindow("HideApp", null);
                    Context.InvokeMethodInMainWindow("ShellRun", new object[] { "%windir%/system32/calc.exe", false });
                    return true;
                }
            };
            return new List<Result>() { result };
        }

        private bool CheckExpressionValid(string input)
        {
            string pattern = @"^(((?<o>\()[-+]?([0-9]+[-+*/])*)+[0-9]+((?<-o>\))([-+*/][0-9]+)*)+($|[-+*/]))*(?(o)(?!))$";
            return Regex.IsMatch("(" + input.Replace(" ", "") + ")", pattern); 
        }

    }
}
