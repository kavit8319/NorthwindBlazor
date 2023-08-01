using Microsoft.AspNetCore.Components;
using Northwind.Interface.Server.Pages;
using Northwind.Interface.Server.Resources.Localization;
using Syncfusion.Blazor.DropDowns;
using System.Globalization;

namespace Northwind.Interface.Server.Components
{
    public class CultureSelectorModel : BaseComponentModel<GlobalResource>
    {
        [Inject]
        protected IConfiguration config { get; set; }
        [Inject]
        protected NavigationManager navigationManager { get; set; }

        protected CultureInfo[] suportedCultures = new[] { new CultureInfo("") };

        public SfDropDownList<string, CultureInfo> dd { get; set; }
        protected string PropValue { get; set; }

        protected override void OnInitialized()
        {
            suportedCultures = config.GetSection("Cultures").GetChildren().ToDictionary(el => el.Key, el => el.Value).Keys.ToArray().Select(el => new CultureInfo(el)).ToArray(); ;

        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender && dd != null)
                dd.Value = CurrentCulture.Name;
            base.OnAfterRender(firstRender);
        }



        public CultureInfo CurrentCulture { get; set; } = CultureInfo.CurrentCulture;

        protected void CultureChanged(SelectEventArgs<CultureInfo> culture)
        {

            if (culture.ItemData is CultureInfo newCulture)
            {

                var uri = new Uri(navigationManager.Uri)
                    .GetComponents(UriComponents.PathAndQuery, UriFormat.Unescaped);
                var cultureEscaped = Uri.EscapeDataString(newCulture.Name);
                var uriEscaped = Uri.EscapeDataString(uri);

                navigationManager.NavigateTo(
                $"Culture/SetCulture?culture={cultureEscaped}&redirectUri={uriEscaped}", forceLoad: true);

            }
        }
    }
}
