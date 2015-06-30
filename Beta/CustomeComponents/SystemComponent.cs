using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Beta.Model;
using Beta.Model.ComponentSystem;
using Beta.Utils;

namespace Beta.CustomeComponents
{
    public class SystemComponent : Component
    {
        private static object lockObject = new object();
        private static List<Program> programs = new List<Program>();

        public override ComponentMetadata Metadata { get; set; }

        public override ComponentContext Context { get; set; }

        protected override void DoInit(ComponentContext context)
        {
            Context = context;

            // 加载程序数据
            LoadPrograms();
        }

        protected override List<Result> DoQuery(Query query)
        {
            if (query.QueryText.Trim().Length <= 1) return new List<Result>();

            FuzzyMatcher fuzzyMatcher = FuzzyMatcher.Create(query.QueryText);
            List<Program> results = programs.Where(o => MatchProgram(o, fuzzyMatcher)).ToList(); // 筛选出匹配的程序
            results = results.OrderByDescending(o => o.Score).ToList(); // 按分数排序
            return results.Select(o => new Result()
            {
                Title = o.Title,
                SubTitle = o.ExecutePath,
                IconPath = o.IcoPath,
                Score = o.Score,
                Action = () =>
                {
                    Context.GlobalAPI.HideApp();
                    Context.GlobalAPI.ShellRun(o.ExecutePath);
                    return true;
                }
            }).ToList();
        }

        private bool MatchProgram(Program program, FuzzyMatcher matcher)
        {
            if ((program.Score = matcher.Evaluate(program.Title).Score) > 0) return true;
            if ((program.Score = matcher.Evaluate(program.PinyinTitle).Score) > 0) return true;
            if (program.AbbrTitle != null && (program.Score = matcher.Evaluate(program.AbbrTitle).Score) > 0) return true;
            if (program.ExecuteName != null && (program.Score = matcher.Evaluate(program.ExecuteName).Score) > 0) return true;

            return false;
        }

        private void LoadPrograms()
        {
            lock (lockObject)
            {
                List<Program> tempProgramList = new List<Program>();

                // TODO: 下面三个方法实际上还未实现，现在返回的是假数据
                tempProgramList.AddRange(LoadProgramsFromAppPaths());
                tempProgramList.AddRange(LoadProgramsFromCommonStartMenu());
                tempProgramList.AddRange(LoadProgramsFromUserStartMenu());

                programs = tempProgramList.GroupBy(x => new { x.ExecutePath, x.ExecuteName }).Select(g => g.First()).ToList();
            }
        }

        private List<Program> LoadProgramsFromCommonStartMenu()
        {
            List<Program> test = new List<Program>();
            test.Add(new Program()
            {
                Title = "test1",
                IcoPath = "Test Icon Path",
                ExecutePath = "Test Execute Path"
            });
            return test;
        }

        private List<Program> LoadProgramsFromAppPaths()
        {
            List<Program> test = new List<Program>();
            test.Add(new Program()
            {
                Title = "test2 eeee T Python",
                IcoPath = "Test Icon Path",
                ExecutePath = "Test Execute Path"
            });
            return test;
        }

        private List<Program> LoadProgramsFromUserStartMenu()
        {
            List<Program> test = new List<Program>();
            test.Add(new Program()
            {
                Title = "test2 sdafdsa",
                IcoPath = "Test Icon Path",
                ExecutePath = "Test Execute Path"
            });
            return test;
        }
    }
}
