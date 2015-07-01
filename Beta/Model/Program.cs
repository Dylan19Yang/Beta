using System;
using System.IO;
using System.Diagnostics;

using Beta.Utils;

namespace Beta.Model
{
    public class Program
    {
        private static readonly global::System.Text.RegularExpressions.Regex AbbrRegexp = new global::System.Text.RegularExpressions.Regex("[^A-Z0-9]", global::System.Text.RegularExpressions.RegexOptions.Compiled);
        private string m_Title;
        public string Title
        {
            get
            {
                return m_Title;
            }
            set
            {
                m_Title = value;
                string pinyin = m_Title.Unidecode();
                PinyinTitle = pinyin;
                AbbrTitle = AbbrRegexp.Replace(global::System.Threading.Thread.CurrentThread.CurrentCulture.TextInfo.ToTitleCase(pinyin), "");
                if (AbbrTitle.Length < 2) AbbrTitle = null;
            }
        }
        public string PinyinTitle { get; private set; }
        public string AbbrTitle { get; private set; }
        public string IcoPath { get; set; }
        public string ExecutePath { get; set; }
        public string ExecuteName { get; set; }
        public int Score { get; set; }

        public Program() { }
        public Program(string filepath)
        {
            Title = Path.GetFileNameWithoutExtension(filepath);
            IcoPath = filepath;
            ExecutePath = filepath;

            switch (Path.GetExtension(filepath).ToLower())
            {
                case ".exe":
                    ExecuteName = Path.GetFileName(filepath);
                    try
                    {
                        FileVersionInfo versionInfo = FileVersionInfo.GetVersionInfo(filepath);
                        if (!string.IsNullOrEmpty(versionInfo.FileDescription))
                        {
                            Title = versionInfo.FileDescription;
                        }
                    }
                    catch (Exception) { }
                    break;
            }
        }
    }
}
