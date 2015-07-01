using System;
using System.IO;
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
                    #region try
                    /*
                    Context.GlobalAPI.HideApp();
                    Context.GlobalAPI.ShellRun(o.ExecutePath);
                    */
                    Context.InvokeMethodInMainWindow("HideApp", null);
                    Context.InvokeMethodInMainWindow("ShellRun", new object[] { o.ExecutePath, false });
                    #endregion
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

        public static void LoadPrograms()
        {
            lock (lockObject)
            {
                List<Program> tempProgramList = new List<Program>();

                tempProgramList.AddRange(LoadProgramsFromAppPaths());
                tempProgramList.AddRange(LoadProgramsFromCommonStartMenu());
                tempProgramList.AddRange(LoadProgramsFromUserStartMenu());

                programs = tempProgramList.GroupBy(x => new { x.ExecutePath, x.ExecuteName }).Select(g => g.First()).ToList();
            }
        }

        private static List<Program> LoadProgramsFromCommonStartMenu()
        {
            List<Program> list = new List<Program>();
            FileSystemHelper.GetAppFromDirectory(FileSystemHelper.GetCommonStartMenuPath(), list);
            return list;
        }

        private static List<Program> LoadProgramsFromAppPaths()
        {
            List<Program> list = new List<Program>();
            FileSystemHelper.ReadAppPaths(@"SOFTWARE\Microsoft\Windows\CurrentVersion\App Paths", list);
            FileSystemHelper.ReadAppPaths(@"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\App Paths", list);
            return list;
        }

        private static List<Program> LoadProgramsFromUserStartMenu()
        {
            List<Program> list = new List<Program>();
            string baseDirectory = Environment.GetFolderPath(Environment.SpecialFolder.Programs);
            if (Directory.Exists(baseDirectory))
            {
                FileSystemHelper.GetAppFromDirectory(baseDirectory, list);
                FileChangeWatcher.AddWatch(baseDirectory);
            }
            return list;
        }

        
    }
}
