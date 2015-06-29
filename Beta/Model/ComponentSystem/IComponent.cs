using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beta.Model.ComponentSystem
{
    public interface IComponent
    {
        List<Result> Query(Query query);
        void Init(ComponentContext context);
    }
}
