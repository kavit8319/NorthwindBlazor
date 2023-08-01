using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Notifications;


namespace Northwind.Interface.Server.Components
{
    public partial class SfToastShowMessage : SfToast
    {
        
        public SfToast ToastObj { get; set; }
        public string PropWith { get; set; }

      

     
        protected override void OnAfterRender(bool firstRender)
        {
           
                base.OnAfterRender(firstRender);
        }
    }
}
