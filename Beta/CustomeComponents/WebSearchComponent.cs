using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Windows.Forms;

using Beta.Model;
using Beta.Model.ComponentSystem;

namespace Beta.CustomeComponents
{
    class WebSearchComponent : Component
    {
        private List<WebSearchEngine> webSearchEngines = new List<WebSearchEngine>();

        public override ComponentMetadata Metadata { get; set; }

        public override ComponentContext Context { get; set; }

        protected override void DoInit(ComponentContext context)
        {
            Context = context;

            LoadDefaultSearchEngines();
        }

        protected override List<Model.Result> DoQuery(Model.Query query)
        {
            WebSearchEngine webSearchEngine = webSearchEngines.FirstOrDefault(o => o.ActionWord == query.ActionName);

            List<Result> results = new List<Result>();

            if (webSearchEngine != null)
            {
                string searchQuery = query.ActionParameters.Count > 0 ? query.GetExtraQueryText() : "";
                string title = searchQuery;
                string subTitle = webSearchEngine.Title + " Search";

                if (string.IsNullOrEmpty(searchQuery))
                {
                    title = subTitle;
                    subTitle = null;
                }

                results.Add(new Result()
                {
                    Title = title,
                    SubTitle = subTitle,
                    IconPath = webSearchEngine.IconPath,
                    Score = 1000,
                    Action = () =>
                    {
                        Process.Start(webSearchEngine.URL.Replace("{q}", Uri.EscapeDataString(searchQuery)));
                        return true;
                    }
                });
            }

            return results;
        }

        private void LoadDefaultSearchEngines()
        {
            // Google
            webSearchEngines.Add(new WebSearchEngine()
            {
                Title = "Google",
                IconPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Images\Google-icon.png",
                ActionWord = "google",
                URL = "https://www.google.com/search?q={q}"
            });

            // Baidu
            webSearchEngines.Add(new WebSearchEngine()
            {
                Title = "Baidu",
                IconPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Images\Baidu-icon.png",
                ActionWord = "baidu",
                URL = "https://www.baidu.com/s?wd={q}"
            });
        }
    }
}
