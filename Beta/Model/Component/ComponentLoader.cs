using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beta.Model.Component
{
    public abstract class ComponentLoader
    {
        private static List<ComponentMetadata> componentMetadatas = new List<ComponentMetadata>();

        public static List<ComponentMetadata> LoadComponents()
        {
            componentMetadatas.Clear();

            // TODO: 加载组件
            throw new NotImplementedException();

            return componentMetadatas;
        }
    }
}
