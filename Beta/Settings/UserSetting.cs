using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beta.Settings
{
    public class UserSetting : ISetting
    {
        private string ConfigFilename
        {
            get
            {
                return "config.json";
            }
        }
        public static string Suffixes = "lnk;exe;appref-ms;bat";

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

        public void Load()
        {
            throw new NotImplementedException();
        }

        public void Save()
        {
            throw new NotImplementedException();
        }
    }
}
