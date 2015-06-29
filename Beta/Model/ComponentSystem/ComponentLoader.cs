using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Beta.Model.ComponentSystem
{
    public abstract class ComponentLoader
    {
        private static List<ComponentMetadata> componentMetadatas = new List<ComponentMetadata>();

        public static List<ComponentMetadata> LoadComponentMetadatas()
        {
            componentMetadatas.Clear();

            LoadSystemComponentMetadata();

            return componentMetadatas;
        }

        private static void LoadSystemComponentMetadata()
        {
            componentMetadatas.Add(new ComponentMetadata()
            {
                Name = "System components",
                Description = "Launch programs",
                Namespace = "Beta.CustomeComponents",
                ActionKeywork = "*"
            });
        }
    }
}
