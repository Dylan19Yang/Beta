using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beta.Settings
{
    public interface ISetting
    {
        void Load();
        void Save();
    }
}
