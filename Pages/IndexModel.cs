namespace Northwind.Interface.Server.Pages
{
    public class IndexModel<TLocal>: BaseComponentModel<TLocal>
    {
        protected override void OnInitialized()
        {
            component.HeaderMessage = "";
        }
    }
}
