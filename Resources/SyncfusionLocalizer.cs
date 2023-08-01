using System.Globalization;
using Syncfusion.Blazor;

namespace Northwind.Interface.Server.Resources
{
    public class SyncfusionLocalizer : ISyncfusionStringLocalizer
    {
        public string GetText(string key)
        {
            return this.ResourceManager.GetString(key,CultureInfo.CurrentCulture);
        }

        public System.Resources.ResourceManager ResourceManager
        {
            get
            {
                // Replace the ApplicationNamespace with your application name.
                return Localization.SfResources.ResourceManager;
            }
        }
    }
}
