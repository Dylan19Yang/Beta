using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beta.Model.Component
{
    public class SystemComponent : Component
    {
        public override ComponentMetadata Metadata { get; set; }

        public override ComponentContext Context { get; set; }

        protected override void DoInit(ComponentContext context)
        {
            throw new NotImplementedException();
        }

        protected override List<Result> DoQuery(Query query)
        {
            throw new NotImplementedException();
        }
    }
}
