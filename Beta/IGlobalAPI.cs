using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Beta.Model;

namespace Beta
{
    public interface IGlobalAPI
    {
        void CloseApp();
        void HideApp();
        void ShowApp();
        void ShowSettingWindow();
        void PushResult(List<Result> results);

        bool ShellRun(string cmd, bool runAsAdministrator = false);

        //void ShowMsg(string title, string subTitle, string iconPath);
    }
}
