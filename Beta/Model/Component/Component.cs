using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beta.Model.Component
{
    public abstract class Component : IComponent
    {
        public abstract ComponentMetadata Metadata { get; set; }
        public abstract ComponentContext Context { get; set; }

        protected abstract void DoInit(ComponentContext context);
        protected abstract List<Result> DoQuery(Query query);

        public List<Result> Query(Query query)
        {
            if (string.IsNullOrEmpty(query.QueryText)) return new List<Result>();

            return DoQuery(query);
        }

        public void Init(ComponentContext context)
        {
            DoInit(context);
        }
    }
}
