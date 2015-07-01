using System;
using System.IO;
using System.Windows.Forms;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json;

using Beta.Model;

namespace Beta.Settings
{
    [Serializable]
    public class UserSetting : ISetting
    {
        private readonly string settingFolder = Path.Combine(Path.GetDirectoryName(Application.ExecutablePath), "Setting");
        private readonly string settingFilename = "setting.json";
        private string SettingPath
        {
            get
            {
                return Path.Combine(settingFolder, settingFilename);
            }
        }

        [JsonProperty]
        public string ProgramSuffixes { get; set; }

        [JsonProperty]
        public List<WebSearchEngine> WebSearchEngines { get; set; }

        [JsonProperty]
        public string HotKey { get; set; }

        #region Singleton

        private static object locker = new object();
        protected static UserSetting serializedObject;

        private UserSetting() { }

        public static UserSetting Instance
        {
            get
            {
                if (serializedObject == null)
                {
                    lock (locker)
                    {
                        if (serializedObject == null)
                        {
                            serializedObject = new UserSetting();
                            serializedObject.Load();
                        }
                    }
                }
                return serializedObject;
            }
        }

        #endregion

        private UserSetting LoadDefault()
        {
            ProgramSuffixes = "lnk;exe;appref-ms;bat";
            HotKey = "Alt + Space";
            WebSearchEngines = LoadDefaultWebSearchEngines();

            return this;
        }

        public List<WebSearchEngine> LoadDefaultWebSearchEngines()
        {
            return new List<WebSearchEngine>()
            {
                // Google
                new WebSearchEngine()
                {
                    Title = "Google",
                    IconPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Images\Google-icon.png",
                    ActionWord = "google",
                    URL = "https://www.google.com/search?q={q}"
                },
                // Baidu
                new WebSearchEngine()
                {
                    Title = "Baidu",
                    IconPath = Path.GetDirectoryName(Application.ExecutablePath) + @"\Images\Baidu-icon.png",
                    ActionWord = "baidu",
                    URL = "https://www.baidu.com/s?wd={q}"
                }
            };
        }

        #region ISetting

        public void Load()
        {
            if (!File.Exists(SettingPath))
            {
                if (!Directory.Exists(settingFolder))
                {
                    Directory.CreateDirectory(settingFolder);
                }
                File.Create(SettingPath).Close();
            }

            string json = File.ReadAllText(SettingPath);
            if (!string.IsNullOrEmpty(json))
            {
                try
                {
                    serializedObject = JsonConvert.DeserializeObject<UserSetting>(json);
                }
                catch (Exception e)
                {
                    Console.WriteLine("Cannot load setting information from {0} : {1}", SettingPath, e.Message);
                    serializedObject = LoadDefault();
                }
            }
            else
            {
                serializedObject = LoadDefault();
            }
        }

        public void Save()
        {
            lock (locker)
            {
                string json = JsonConvert.SerializeObject(serializedObject, Formatting.Indented);
                File.WriteAllText(SettingPath, json);
            }
        }

        #endregion
    }
}
