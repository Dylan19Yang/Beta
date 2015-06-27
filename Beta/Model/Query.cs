using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beta.Model
{
    public class Query
    {
        public string QueryText { get; set; }
        public string ActionName { get; private set; }
        public List<string> ActionParameters { get; private set; }

        public Query(string queryText)
        {
            QueryText = queryText;

            ParseQuery();
        }

        private void ParseQuery()
        {
            if (string.IsNullOrEmpty(QueryText)) return;

            string[] queries = QueryText.Split(' ');

            if (queries.Length == 1) return;

            ActionParameters = new List<string>();

            ActionName = queries[0];
            for (int i = 1; i < queries.Length; i++)
            {
                if (!string.IsNullOrEmpty(queries[i]))
                {
                    ActionParameters.Add(queries[i]);
                }
            }
        }

        public string GetExtraQueryText()
        {
            string[] queries = QueryText.Split(new char[] {' '}, 2, System.StringSplitOptions.None);

            if (queries.Length > 1) return queries[1];

            return string.Empty;
        }
    }
}
