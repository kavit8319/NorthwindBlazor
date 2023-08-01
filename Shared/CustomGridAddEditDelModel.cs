using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Localization;
using Newtonsoft.Json;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Pages;
using Northwind.Interface.Server.Resources.Localization;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Navigations;
using Action = Syncfusion.Blazor.Grids.Action;
using FailureEventArgs = Syncfusion.Blazor.Grids.FailureEventArgs;

namespace Northwind.Interface.Server.Shared
{
    public partial class CustomGridAddEditDel<TValue> : SfGrid<TValue>
    {
        public SfGrid<TValue> grid;
        public const int PAGE_COUNT = 5;
        public const int DEFAULT_PAGE_SIZE = 10;
        public string[] PageSizes = new string[] { "10", "20", "50" };
        [Parameter]
        public EventCallback<ActionEventArgs<TValue>> OnCustomActionBegin { get; set; }
        [Parameter]
        public EventCallback<RowSelectingEventArgs<TValue>> OnCustomRowSelecting { get; set; }

        [Parameter]
        public EventCallback<DetailsExpandingEventArgs<TValue>> OnCustomDetailExpanding { get; set; }
        [Parameter]
        public EventCallback<DetailsExpandedEventArgs<TValue>> OnCustomDetailExpanded { get; set; }
        [Parameter]
        public EventCallback<ActionEventArgs<TValue>> OnCustomActionComplete { get; set; }


        [Parameter]
        public EventCallback<Exception> RemoveReletionDate { get; set; }

        [Parameter] public Query QueryParam { get; set; }
        public bool InitialRender { get; set; }
        public RemoveDialog<TValue> removeDialog { get; set; }
        IReadOnlyDictionary<string, object> props { get; set; }

        [Inject] public IStringLocalizer<GlobalResource> GlobalStringLocalizer { get; set; }


        [CascadingParameter] protected BaseComponent BaseComponentCascading { get; set; }

        public List<ItemModel> Toolbaritems = new();

        public override Task SetParametersAsync(ParameterView parameters)
        {
            //Assign the additional parameters
            props = parameters.ToDictionary();

            return base.SetParametersAsync(parameters);

        }
        protected async override Task OnParametersSetAsync()
        {
            AllowPaging = true;
            AllowSorting = true;
            AllowFiltering = true;

            await base.OnParametersSetAsync();
        }

        protected override void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                InitialRender = true;
                Toolbaritems.AddRange(GetToolBarStrings());
            }
        }

        public async Task CustomCombobxFilter(string filterField, object value, string predicate)
        {
            if (grid != null)
                await grid.FilterByColumnAsync(filterField, "equal", value, predicate);
        }
        protected List<ItemModel> GetToolBarStrings()
        {
            if (GlobalStringLocalizer != null)
            {
                return new List<ItemModel>() {
                new (){Text = GlobalStringLocalizer["Add"], PrefixIcon = "e-add", Id = $"{grid.ID}_add", } ,
                new (){Text = GlobalStringLocalizer["Edit"], PrefixIcon = "e-edit", Id = $"{grid.ID}_edit", Visible = true},
                new (){Text = GlobalStringLocalizer["Delete"], PrefixIcon = "e-delete", Id = $"{grid.ID}_delete"}
            };
            }
            return new List<ItemModel>();
        }

        protected async void OnActionBegin(ActionEventArgs<TValue> obj)
        {

            if (OnCustomActionBegin.HasDelegate)
                await OnCustomActionBegin.InvokeAsync(obj);
            if (obj.RequestType == Action.Refresh)
            {
                // await grid.SelectRowAsync(obj.RowIndex);
                // grid.SelectedRowIndex = obj.RowIndex;
            }
        }

        protected async Task DetailExpandind(DetailsExpandingEventArgs<TValue> obj)
        {
            if (OnCustomDetailExpanding.HasDelegate)
                await OnCustomDetailExpanding.InvokeAsync(obj);
        }

        protected async Task DetailExpanded(DetailsExpandedEventArgs<TValue> obj)
        {
            if (OnCustomDetailExpanded.HasDelegate)
                await OnCustomDetailExpanded.InvokeAsync(obj);
        }


        protected async Task OnRowSelecting(RowSelectingEventArgs<TValue> obj)
        {
            if (OnCustomRowSelecting.HasDelegate)
                await OnCustomRowSelecting.InvokeAsync(obj);
        }

        public async Task CustomFilterByColumnAsync(ActionEventArgs<TValue> arg, object value, string fieldForFilter = null)
        {
            try
            {
                if (grid != null)
                {
                    if (grid.FilterSettings.Columns == null)
                    {
                        grid.FilterSettings.Columns = new List<GridFilterColumn>();
                    }
                    if (grid.FilterSettings.Columns.Count > 0)
                    {
                        grid.FilterSettings.Columns.RemoveAll(c => c.Field == arg.CurrentFilterObject.Field);
                    }
                    var columns = await grid.GetColumnsAsync(false);
                    // Fetch the Uid of OrderDate column.

                    var column = await grid.GetColumnByFieldAsync(arg.CurrentFilterObject.Field);
                    var filterGrid = new GridFilterColumn
                    {
                        Operator = arg.CurrentFilterObject.Operator,
                        Value = value,
                        Field = fieldForFilter ?? arg.CurrentFilterObject.Field,
                        Predicate = arg.CurrentFilterObject.Predicate,
                        Uid = columns[columns.IndexOf(column)].Uid
                    };
                    grid.FilterSettings.Columns.Add(filterGrid);

                    await grid.Refresh();
                }
            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }
        }

        public async Task CustomClearFilter(string field)
        {
            await grid.ClearFilteringAsync(field);
        }

        public async Task CustomActionCompleteCallback(ActionEventArgs<TValue> obj)
        {
            if (obj.RequestType == Syncfusion.Blazor.Grids.Action.Add || obj.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
            {
                grid.PreventRender(false);
            }
           
            if (obj.RequestType == Action.Save && obj != null && grid != null)
            {
                if (obj.Data != null)
                {
                    int index = 0;
                    var listPrimarykeys = await grid.GetPrimaryKeyFieldNamesAsync();
                    if (listPrimarykeys.Count > 1)
                    {
                        var list = await grid.GetCurrentViewRecordsAsync();
                        if (list != null && list.Count > 0)
                        {
                            var typeListObject = list.First().GetType();
                            var listProperties = typeListObject.GetProperties().Where(el => listPrimarykeys.Contains(el.Name));
                            var valuesProductFind = listProperties.Select(el => el.GetValue(obj.Data));
                            var resFindObje = list.FirstOrDefault(el =>
                            {
                                var valueObject = listProperties.Select(elProp => elProp.GetValue(el));
                                return valueObject.SequenceEqual(valuesProductFind);
                            });
                            index = list.IndexOf(resFindObje);
                        }
                    }
                    var res = await grid.GetRowIndexByPrimaryKeyAsync(value: obj.Data.GetType().GetProperty("Id").GetValue(obj.Data));
                    if (index == 0)
                        index = (int)res;
                    if (index >= 0)
                        await grid.SelectRowAsync(index: (int)index);
                    await BaseComponentCascading.ShowMessage(GlobalStringLocalizer[obj.Action == "Delete" ? "RowDelete" : (obj.Action == "Add" ? "RowAdd" : "RowEdit")], obj.Action == "Delete" ? (null) : (obj.Action == "Add"));
                    if (OnCustomActionComplete.HasDelegate)
                        await OnCustomActionComplete.InvokeAsync(obj);
                }
            }
        }

        public async Task CustomGoToPage(int index)
        {
            if (index > 0)
                await grid.GoToPageAsync(index)!;
        }

        public void CustomPreventRender(bool prevent)
        {
            grid.PreventRender(prevent);
        }
        public async Task SetCustomRowSelectedById(int Id)
        {
            var index = await grid.GetRowIndexByPrimaryKeyAsync(Id);
            await grid.SelectRowAsync(index);
        }
        public async Task SetCustomValueToCell(object primaryKey, string propery, object value)
        {
                    
            
            await grid.SetCellValueAsync(primaryKey, propery, value);
            //grid.SetRowDataAsync(primaryKey,)
            //await grid.UpdateCellAsync(index, propery, value);
            //await grid.ExpandCollapseDetailRowAsync(obj);
        }
        protected async Task CustomFailureCallback(FailureEventArgs obj)
        {
            var apiException = obj.Error as ApiException;
            
            if (obj != null && obj.Error != null && obj.Error.InnerException != null && !obj.Error.Message.Contains("Id:"))
                await BaseComponentCascading.ShowErrorMessage(apiException.Message);
            if (obj != null && obj.Error != null)
            {
                
                if (RemoveReletionDate.HasDelegate)
                    await RemoveReletionDate.InvokeAsync(apiException);
                else
                {
                   var res= JsonConvert.DeserializeObject<MessageError>(apiException.Response);
                    var error = GlobalStringLocalizer[res.Message];
                    await BaseComponentCascading.ShowErrorMessage(error != null ? error : obj.Error.Message);
                }
            }
        }
    }

    public class MessageError
    {
        public string Message { get; set; }
        public int StatusCode { get; set; }
        public bool IsError { get; set; }
    }
}
