using CustomInputControl;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json.Linq;
using Northwind.Interface.Server.Adaptors;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Pages;
using Northwind.Interface.Server.Resources.Localization;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using System.Linq.Expressions;

namespace Northwind.Interface.Server.Shared
{
    public class CountryCityPostCodeModel<T> : BaseComponentModel<GlobalResource>
    {
        public CustomDDList<string, Country, CountrysAdaptersddl> DdlCountry { get; set; }
        public CustomDDList<string, string, CityAdapterddl> DdlCity { get; set; }
        public CustomDDList<string, string, PostCodeAdapterddl> DdlPostCode { get; set; }

        [CascadingParameter]
        protected EditContext CascadedEditContext { get; set; }

        public Query QueryCity { get; set; } = new();
        public Query QueryCountry { get; set; } = new();
        public Query QueryPostCode { get; set; } = new();
        public Country SelCountry { get; set; } = new();

        [Parameter]
        public RenderFragment<T> RenderFragment { get; set; }
        [Parameter]
        public EventCallback<string> ValueCountryChanged { get; set; }
        [Parameter]
        public EventCallback<string> ValueCityChanged { get; set; }
        [Parameter]
        public EventCallback<string> ValuePostalCodeChanged { get; set; }

        [Parameter]
        public Expression<Func<string>> ForCountry { get; set; }
        [Parameter]
        public Expression<Func<string>> ForCity { get; set; }
        [Parameter]
        public Expression<Func<string>> ForPostCode { get; set; }
        [Parameter]
        public T ParamToRenderFragment { get; set; }
        private string valueCountry { get; set; }
        private string valueCity { get; set; }
     
        private string valuePostCode { get; set; }

        [Parameter]
        public string ValueCountry
        {
            get => valueCountry;
            set
            {
                if (!EqualityComparer<string>.Default.Equals(value, valueCountry))
                {
                    if (value == null && valueCountry != null)
                    {
                        Clear(true);
                        valueCountry = value;
                        ValueCountryChanged.InvokeAsync(value);
                    }
                    else if (value != null)
                    {
                        valueCountry = value;
                        ValueCountryChanged.InvokeAsync(value);
                    }
                    else
                    ValueCountryChanged.InvokeAsync(value);
                    if (value == null)
                        SelCountry = null;
                }
            }
        }
        [Parameter]
        public string ValueCity
        {
            get => valueCity;
            set
            {
                if (!EqualityComparer<string>.Default.Equals(value, valueCity) && !isCompleted)
                    SetValueCity(value);
                else if(!EqualityComparer<string>.Default.Equals(value, valueCity) && isCompleted)
                    SetValueCity(value);
            }
        }

        private void SetValueCity(string value)
        {
            if (value == null && isCompleted)
            {
                isCompleted = false;
                valueCity = value;
                ValueCityChanged.InvokeAsync(value);
            }
            if (value != null)
            {
                valueCity = value;
                ValueCityChanged.InvokeAsync(value);
            }
        }

         private void SetValuePostCode(string value)
        {
            if (value == null && isCompleted)
            {
                isCompleted = false;
                valuePostCode = value;
                ValuePostalCodeChanged.InvokeAsync(value);
            }
            if (value != null)
            {
                valuePostCode = value;
                ValuePostalCodeChanged.InvokeAsync(value);
            }
        }

        [Parameter]
        public string ValuePostalCode
        {
            get => valuePostCode;
            set
            {
                if (!EqualityComparer<string>.Default.Equals(value, valuePostCode) && !isCompleted)
                    SetValuePostCode(value);
                else if (!EqualityComparer<string>.Default.Equals(value, valuePostCode) && isCompleted)
                    SetValuePostCode(value);
            }
        }

        private bool IsEdit
        {
            get
            {
                if (CascadedEditContext == null)
                    return false;
                return Convert.ToInt32(CascadedEditContext.Model.GetType().GetProperty("Id").GetValue(CascadedEditContext.Model)) > 0;
            }
        }
        bool isCompleted;
        protected async Task OnActionCompleteCountry(ActionCompleteEventArgs<Country> obj)
        {
            try
            {
                if (!string.IsNullOrEmpty(ValueCountry))
                {
                    var result = obj.Result.Where(el => el.Name.Equals(ValueCountry)).FirstOrDefault();
                    if (result != null && !string.IsNullOrEmpty(result.Code) && IsEdit)
                    {
                        //await SetCityValue(ddlCountry.Value);
                        await LoadDateCityPostCode(result.Code);

                    }
                }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(ex.Message);
            }
        }

        private async Task LoadDateCityPostCode(string code)
        {
            QueryCity = new Query().AddParams(Constans.CountryCode, code);
            QueryPostCode = new Query().AddParams(Constans.CountryCode, code);
           
            if (!IsEdit)
            {
                ValueCity = valueCity = null;
                ValuePostalCode = valuePostCode = null;
            }
            else if(ValuePostalCode!=null)
                QueryPostCode.AddParams(Constans.CountryFilter, valuePostCode);
            await DdlPostCode.RefreshDataAsync();
            await DdlCity.RefreshDataAsync();
            
            isCompleted = true;
            //valueCity = valueCityForSel;
            //valuePostCode = valuePostCodeForSel;
           // Enable = true;
        }

        protected async Task OnCloseCountry(PopupEventArgs obj)
        {

            SelCountry = DdlCountry.GetDataByValue(DdlCountry.Value);
          //  obj.Cancel = true;
            if (SelCountry != null)
            {

                if (!string.IsNullOrEmpty(SelCountry.Code))
                {

                    await LoadDateCityPostCode(SelCountry.Code);

                    // QueryCity = new Query();
                    //QueryCity.AddParams(key: Constans.CountryCode, value: SelCountry.Code);
                    // var param = new Query();
                    // param.AddParams(key: Constans.CountryCode,SelCountry.Code);
                    // DdlCity.FilterAsync(null, param);
                    //await DdlCity.FilterAsync(null,QueryCity);
                }
            }
        }


        protected async Task FilterDDLCountry(FilteringEventArgs filter)
        {
            filter.PreventDefaultAction = true;
            await DdlCountry.ClearAsync();
            QueryCountry.AddParams(key: Constans.CountryFilter, value: filter.Text);
            if (DdlCountry != null)
                await DdlCountry.FilterAsync(null, QueryCountry);
           // Enable = true;
        }
        protected async Task FilterDDLCity(FilteringEventArgs filter)
        {
            if (SelCountry != null && DdlCity != null)
            {
                filter.PreventDefaultAction = true;
               await DdlCity.ClearAsync();
                QueryCity = new Query();
                QueryCity.AddParams(key: Constans.CountryFilter, value: filter.Text);
                await DdlCity.FilterAsync(null, QueryCity);
               // Enable = true;
            }
        }
        protected async Task FilterDDLPostCode(FilteringEventArgs filter)
        {
            if (SelCountry != null)
            {
                filter.PreventDefaultAction = true;
                await DdlPostCode.ClearAsync();
                QueryPostCode = new Query();
                QueryPostCode.AddParams(Constans.CountryCode, SelCountry.Code);
                QueryPostCode.AddParams(Constans.CountryFilter, filter.Text);
                //if (DdlPostCode != null)//for editing
                    await DdlPostCode.FilterAsync(null, QueryPostCode);
                //Enable = true;
            }
        }
        protected async Task OnCloseDDLCity(PopupEventArgs popup)
        {
            if (SelCountry != null)
            {
                //Enable = true;
                // ValueCity = valueCityForSel;
                QueryPostCode = new Query();
                QueryPostCode.AddParams(key: Constans.CountryCode, value: SelCountry.Code);
                //if (DdlCity != null && DdlPostCode != null)
                    await DdlPostCode.FilterAsync(null, QueryPostCode);

            }
        }

        protected void OnCloseDDLPostCode()
        {
            if (SelCountry != null)
            {
                //Enable = true;
               // valuePostCode = valuePostCodeForSel;
            }
        }

        protected async Task Clear(bool isClear)
        {
            if (isClear && DdlCity != null && DdlPostCode != null)
            {
                isCompleted= true;
                QueryCity = new Query().AddParams("clear", 0);
                QueryPostCode = new Query().AddParams("clear", 0);
                await DdlCity.ClearAsync();
                await DdlPostCode.ClearAsync();
              
                //await DdlCity.RefreshDataAsync();
                
                //await DdlPostCode.RefreshDataAsync();
                ValueCity =  null;
                isCompleted = true;
                QueryPostCode = null;
                QueryCity = null;
                //ValuePostalCode=valuePostCode = null;
              
                // ddlPostCode.Enabled = false;
                // ddlCity.Enabled = EnableCity = false;
                await DdlPostCode.FocusOutAsync();
                await DdlCity.FocusOutAsync();
                //await ddlPostCode.CallStateHasChangedAsync();
                //await ddlCity.CallStateHasChangedAsync();
            }
        }
    }


}
