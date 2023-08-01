using Northwind.Interface.Server.Resources.Localization;
using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using System.Resources;

namespace Northwind.Interface.Server.Data
{
    public interface IShowMessage<out T>
    {
        string HeaderMessage { get; set; }
        public void SetTitleHeader();
    }

    public class ShowHeaderMessage<TLocale>:IShowMessage<TLocale>
    {
        ResourceManager resmgr;
        public event Action OnChange;
        private void NotifyStateChanged() => OnChange?.Invoke();
        public string HeaderMessage { get; set; }
        public void SetTitleHeader()
        {
            var t = typeof(TLocale);
            if (t != typeof(GlobalResource))
            {
                resmgr = new ResourceManager($"Northwind.Interface.Server.Resources.Localization.{t.Name}", Assembly.GetExecutingAssembly());
                var prop = t.GetProperty("ResourceManager");
                if (prop != null)
                {
                    HeaderMessage = resmgr.GetString("Title");
                    NotifyStateChanged();
                    //StateHasChanged();
                }
            }
        }
    }
}
