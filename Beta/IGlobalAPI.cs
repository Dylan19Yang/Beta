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
        void PushResult(List<Result> results);
    }
}
