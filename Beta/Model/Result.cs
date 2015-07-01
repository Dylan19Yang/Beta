using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beta.Model
{
    public class Result
    {
        public string Title { get; set; }
        public string SubTitle { get; set; }
        public string IconPath { get; set; }
        public int Score { get; set; }

        public Func<bool> Action { get; set; }

        public Result() { }

        public Result(string Title = null, string IconPath = null, string SubTitle = null)
        {
            this.Title = Title;
            this.IconPath = IconPath;
            this.SubTitle = SubTitle;
        }

        public override string ToString()
        {
            return Title + SubTitle;
        }
    }
}
