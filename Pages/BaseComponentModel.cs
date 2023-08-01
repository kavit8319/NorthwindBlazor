using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Northwind.Interface.Server.BaseClasses;
using Northwind.Interface.Server.Components;
using Northwind.Interface.Server.Resources.Localization;
using Syncfusion.Blazor.Notifications;
using System.Reflection;

namespace Northwind.Interface.Server.Pages
{
    public class BaseComponentModel<TLocale> : ComponentBase
    {
        [Inject] public IStringLocalizer<GlobalResource> GlobalStringLocalizer { get; set; }
        [Inject] protected BaseHttpClient httpClient { get; set; }

        [Inject]
        protected IStringLocalizer<TLocale> localResource { get; set; }

        [CascadingParameter]
        protected BaseComponent component { get; set; }

        // public SimpleMessage SimpleMessage { get; set; } = new();

        [Parameter]
        public RenderFragment ChildContent { get; set; }

        public SfToastShowMessage ToastObj { get; set; }
     
        public string PropWith { get; set; }
        private string _headMessage;
        public string HeaderMessage
        {
            get
            {
                return _headMessage; }
            set
            {
                _headMessage = value;
                StateHasChanged();
            }
        }


        public async Task ShowMessage(string result, bool? isAdd = true)
        {
            //if (Parameter != null)
            //{
            PropWith = string.Empty;
            if (isAdd.HasValue)
            {
                if (isAdd.Value)
                    await ToastObj.ToastObj.ShowAsync(new ToastModel
                    {
                        Title = GlobalStringLocalizer["Toast_Message"],
                        Content = result,
                        CssClass = "e-toast-success",
                        Icon = "e-success toast-icons"
                    });
                else
                    await ToastObj.ToastObj.ShowAsync(new ToastModel
                    {
                        Title = GlobalStringLocalizer["Toast_Message"],
                        Content = result,
                        CssClass = "e-toast-success",
                        Icon = "e-success toast-icons"
                    });
            }
            else
                await ToastObj.ToastObj.ShowAsync(new ToastModel { Title = GlobalStringLocalizer["Toast_Message"], Content = result, CssClass = "e-toast-info", Icon = "e-info toast-icons" });

        }

        public async Task ShowErrorMessage(string message)
        {
            if (ToastObj != null)
            {
                PropWith = "100%";
                await ToastObj.ToastObj.ShowAsync(new ToastModel
                {
                    Title = GlobalStringLocalizer["Toast_Error"],
                    Content = message,
                    CssClass = "e-toast-danger",
                    Icon = "e-error toast-icons"
                });
            }
        }

        public async Task ShowErrorMessage(ICollection<string>messages)
        {
           await ShowErrorMessage(string.Join(",",messages));
        }

        //protected async Task<Client> Client()
        //{
        //    try
        //    {
        //        var servBuild = serviceCollectionProv.ServiceCollection.BuildServiceProvider();
        //        var localFactory = servBuild?.GetRequiredService<IHttpClientFactory>();
        //        var ser = servBuild?.GetRequiredService<Client>();
        //        if (ser != null && localFactory != null)
        //            ser.clientFactory = localFactory;
        //        if (ser != null && LocalStoreService != null && !string.IsNullOrEmpty((await LocalStoreService.GetItemAsync(Constans.AccessToken))))
        //        {
        //            var res = await LocalStoreService.GetItemAsync(Constans.AccessToken);
        //            ser.Token = res;
        //            return ser;
        //        }
        //        return ser;
        //    }
        //    catch (Exception)
        //    {
        //        throw;
        //    }
        //}

        /// <summary>
        /// Copies the data of one object to another. The target object 'pulls' properties of the first. 
        /// This any matching properties are written to the target.
        /// 
        /// The object copy is a shallow copy only. Any nested types will be copied as 
        /// whole values rather than individual property assignments (ie. via assignment)
        /// </summary>
        /// <param name="source">The source object to copy from</param>
        /// <param name="target">The object to copy to</param>
        /// <param name="excludedProperties">A comma delimited list of properties that should not be copied</param>
        /// <param name="memberAccess">Reflection binding access</param>
        public void CopyObjectData(object source, object target, string excludedProperties, BindingFlags memberAccess)
        {
            string[] excluded = null;
            if (!string.IsNullOrEmpty(excludedProperties))
                excluded = excludedProperties.Split(new char[1] { ',' }, StringSplitOptions.RemoveEmptyEntries);

            PropertyInfo [] miT = target.GetType().GetProperties(memberAccess);
            if (source != null)
                foreach (PropertyInfo Field in miT)
                {
                    string name = Field.Name;

                    // Skip over any property exceptions
                    if (!string.IsNullOrEmpty(excludedProperties) &&
                        excluded.Contains(name))
                        continue;

                    if (Field.MemberType == MemberTypes.Field)
                    {
                        FieldInfo SourceField = source.GetType().GetField(name);
                        if (SourceField == null)
                            continue;

                        object SourceValue = SourceField.GetValue(source);
                        Field.SetValue(target, SourceValue);
                    }
                    else if (Field.MemberType == MemberTypes.Property)
                    {
                        var piTarget = Field as PropertyInfo;
                        var SourceField = source.GetType().GetProperty(name, memberAccess);
                        if (SourceField == null)
                            continue;

                        if (piTarget != null && piTarget.CanWrite && SourceField.CanRead)
                        {
                            object SourceValue = SourceField.GetValue(source, null);
                            piTarget.SetValue(target, SourceValue, null);
                        }
                    }
                }
        }


    }
}
