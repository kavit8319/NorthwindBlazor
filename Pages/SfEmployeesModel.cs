using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.JSInterop;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.ClientWebApi;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor.Buttons;
using Syncfusion.Blazor.Data;
using Syncfusion.Blazor.DropDowns;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;
using Syncfusion.Blazor.Navigations;
using Syncfusion.Blazor.Popups;
using Syncfusion.Blazor.TreeGrid;
using Syncfusion.Blazor.TreeGrid.Internal;
using System.Reflection;
using Action = Syncfusion.Blazor.Grids.Action;
using BeforeOpenEventArgs = Syncfusion.Blazor.Popups.BeforeOpenEventArgs;

namespace Northwind.Interface.Server.Pages
{
    public class SfEmployeesModel<TLocal> : BaseComponentModel<TLocal>
    {
        protected SfTreeGrid<EmployeeReturnView> employeeTree;
        [Inject]
        IJSRuntime Runtime { get; set; }

        protected string filterFirstname { get; set; }
        protected string filterLastName { get; set; }
        protected string filterTitle { get; set; }
        protected string filterTitleOfCourtesy { get; set; }
        protected string filterAddress { get; set; }
        protected string filterCity { get; set; }
        protected string filterCountry { get; set; }
        protected string filterPostCode { get; set; }
        protected DateTime? filterBirthDate { get; set; }
        protected DateTime? filterHireDate { get; set; }
        public string filterNote { get; set; }

        protected List<ItemModel> Toolbaritems = new();

        public int Count { get; set; } = 0;
        protected SfDropDownList<int, ReportToResult> ddlReportTo;
        protected CountryCityPostCode<EmployeeReturnView> CountryCityPostCode { get; set; }
        protected Query queryReportTo = new Query();
        public bool EnableCity { get; set; }
        protected bool EnablePostCode { get; set; }

        //protected SfSpeedDial sfSpeedDeail { get; set; }
        // protected int Count { get; set; } = 0;
        public Country selCountry { get; set; } = new();

        protected List<fileInfo> files = new();
        protected SfUploader uploader { get; set; }

        protected DialogSettings dialogSettings { get; set; } = new DialogSettings() { Width = "600", MinHeight = "620", Height = "620" };

        protected override void OnInitialized()
        {
            Toolbaritems.AddRange(GetToolBarStrings());
            editContextEmployees = new EditContext(employeeRetViwDialog);
            messageStore = new ValidationMessageStore(editContextEmployees);
            component.HeaderMessage = localResource["TitleEmployee"];
        }
        protected override void OnAfterRender(bool firstRender)
        {
            //  if(firstRender)
            //  sfSpeedDeail.Target = "#treeEmployee_gridcontrol_content_table";
        }
        protected ValidationMessageStore messageStore;

        public bool iferrorExist { get; set; }

        public async Task OnActionBegin(ActionEventArgs<EmployeeReturnView> emp)
        {
            try
            {
                if (emp.RequestType == Syncfusion.Blazor.Grids.Action.Filtering && emp.CurrentFilteringColumn != null)
                {
                    if (employeeTree.PageSettings.CurrentPage > 1)
                        await employeeTree.GoToPageAsync(1);
                }
                if (emp.RequestType == Syncfusion.Blazor.Grids.Action.Filtering && emp.CurrentFilteringColumn == null)
                {
                    var dotNetReference = DotNetObjectReference.Create(this);          // create dotnet ref
                    await Runtime.InvokeAsync<string>("filter", dotNetReference);
                }
                if (emp.RequestType == Syncfusion.Blazor.Grids.Action.Filtering && employeeTree != null)
                {
                    emp.Cancel = true;
                    switch (emp.CurrentFilteringColumn)
                    {
                        case nameof(EmployeeReturnView.FirstName):
                            await CustomFilterByColumnAsync(emp, string.IsNullOrEmpty(filterFirstname) ? null : filterFirstname);
                            break;
                        case nameof(EmployeeReturnView.LastName):
                            await CustomFilterByColumnAsync(emp, filterLastName);
                            break;
                        case nameof(EmployeeReturnView.TitleId):
                            await CustomFilterByColumnAsync(emp, filterTitle);
                            break;
                        case nameof(EmployeeReturnView.TitleOfCourtesy):
                            await CustomFilterByColumnAsync(emp, filterTitleOfCourtesy);
                            break;
                        case nameof(EmployeeReturnView.Address):

                            await CustomFilterByColumnAsync(emp, filterAddress);
                            break;
                        case nameof(EmployeeReturnView.Country):
                            await CustomFilterByColumnAsync(emp, filterCountry);
                            break;
                        case nameof(EmployeeReturnView.City):
                            await CustomFilterByColumnAsync(emp, filterCity);
                            break;
                        case nameof(EmployeeReturnView.BirthDate):
                            await CustomFilterByColumnAsync(emp, filterBirthDate);
                            break;
                        case nameof(EmployeeReturnView.HireDate):
                            await CustomFilterByColumnAsync(emp, filterHireDate);
                            break;
                        case nameof(EmployeeReturnView.Notes):
                            await CustomFilterByColumnAsync(emp, filterNote);
                            break;
                    }
                }


                //if (employeeTree != null && emp.RequestType == Action.Delete && employeeTree.removeDialog != null && employeeTree.removeDialog.Flag)
                //{
                //    emp.Cancel = true;  //Cancel default delete action.
                //    employeeTree.removeDialog.ShowRemoveMessage($"{Args.Data.CompanyName}");
                //    employeeTree.removeDialog.Flag = false;
                //}

                if (emp.RequestType == Action.Save && CountryCityPostCode.DdlCountry != null)
                {
                    emp.Data.Country = emp.Data.Country != null ? CountryCityPostCode.DdlCountry.GetDataByValue(emp.Data.Country).Name : null;
                }

                if (emp.RequestType == Action.BeginEdit)
                {
                    isAdd = false;
                    emp.Cancel = true;
                    queryReportTo.AddParams("employeeId", emp.Data.Id);
                    await sfDialog.ShowAsync(false);
                }

                if (emp.RequestType == Action.Add)
                {
                    emp.Cancel = true;
                    isAdd = true;
                    await sfDialog.ShowAsync(false);
                }

                if (emp.RequestType == Action.Delete && removeDialog.Flag)
                {
                    emp.Cancel = true;
                    await removeDialog.ShowRemoveMessage(emp.Data.FirstName + " " + emp.Data.LastName);
                }



                //if (productGrid != null && productGrid.removeDialog != null)
                //    if (emp.RequestType == Syncfusion.Blazor.Grids.Action.Delete && productGrid.removeDialog.Flag)
                //    {
                //        emp.Cancel = true;  //Cancel default delete action.
                //        productGrid.removeDialog.ShowRemoveMessage($" {emp.Data.FirstName} {emp.Data.LastName}");
                //        productGrid.removeDialog.Flag = false;
                //    }
            }
            catch (Exception ex)
            {
                await ShowErrorMessage(ex.Message);
            }
        }

        public async Task ActionComplete(ActionEventArgs<EmployeeReturnView> args)
        {
            if (args.Action == "Add")
            {
                var primaryKey = employeeTree.DataSource.Where(rec => rec.Id == args.Data.ReportsTo).FirstOrDefault().Id;
               await employeeTree.ExpandByKeyAsync(primaryKey);
            }
        }
        public async Task CustomFilterByColumnAsync(ActionEventArgs<EmployeeReturnView> arg, object value, string fieldForFilter = null)
        {
            try
            {


                if (employeeTree != null)
                {
                    if (employeeTree.FilterSettings.Columns == null)
                    {
                        employeeTree.FilterSettings.Columns = new List<TreeGridFilterColumn>();
                    }
                    if (employeeTree.FilterSettings.Columns.Count > 0)
                    {
                        employeeTree.FilterSettings.Columns.RemoveAll(c => c.Field == arg.CurrentFilterObject.Field);
                    }
                    var columns = await employeeTree.GetColumnsAsync();
                    // Fetch the Uid of OrderDate column.

                    var column = await employeeTree.GetColumnByFieldAsync(arg.CurrentFilterObject.Field);
                    var filterGrid = new TreeGridFilterColumn
                    {
                        Operator = arg.CurrentFilterObject.Operator,
                        Value = value,
                        Field = fieldForFilter ?? arg.CurrentFilterObject.Field,
                        Predicate = arg.CurrentFilterObject.Predicate
                    };
                    employeeTree.FilterSettings.Columns.Add(filterGrid);

                    await employeeTree.RefreshAsync();
                }
            }
            catch (Exception ex)
            {
                var str = ex.Message;
            }
        }
        public async Task onChange(UploadChangeEventArgs args, EmployeeReturnView val)
        {
            try
            {
                files = new List<fileInfo>();
                var file = args.Files[0];

                val.Photo = file.Stream.ToArray();
                string base64 = Convert.ToBase64String(val.Photo);
                files.Add(new fileInfo() { Path = @"data:image/" + file.FileInfo.Type + ";base64," + base64, Name = file.FileInfo.Name, Size = file.FileInfo.Size });

                employeeTree.PreventRender(false);
            }
            catch (Exception e)
            {
                await component.ShowErrorMessage(e.Message);
            }
        }

        protected async Task RemoveImage(MouseEventArgs e, object o)
        {
            var element = (EmployeeReturnView)o;
            element.Photo = null;
            files.Clear();
            await uploader.ClearAllAsync();
        }


        protected SfDataManager dm;
        protected SfDialog sfDialog;
        protected EmployeeReturnView employeeRetViwDialog = new();
        private bool isAdd;
        protected RemoveDialog<EmployeeReturnView> removeDialog { get; set; }
        private EmployeeReturnView selEmployeeReturnView { get; set; } = new();

        protected EditContext editContextEmployees;

        protected void OnOpendDialog(BeforeOpenEventArgs open)
        {
            try
            {
                CopyObjectData(isAdd ? new EmployeeReturnView() : selEmployeeReturnView, employeeRetViwDialog, null,
               BindingFlags.Public | BindingFlags.Instance);
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        protected async Task OnBtnOkClick()
        {
            try
            {
                messageStore.Clear();
                if (editContextEmployees.Validate())
                {
                    if (employeeTree != null)
                    {
                        employeeTree.EditSettings.AllowAdding = true;
                        if (employeeRetViwDialog.Id > 0)
                        {
                            var index = await employeeTree.GetSelectedRowIndexesAsync();
                            var selectIndex = index.First();
                            await employeeTree.UpdateRowAsync(selectIndex, employeeRetViwDialog);

                            await employeeTree.ExpandAtLevelAsync(employeeRetViwDialog.Lavel);
                            await employeeTree.SelectRowAsync(selectIndex);
                        }
                        else
                        {
                            await ValidateObjectOnDb();
                            if (editContextEmployees.Validate())
                            {
                                //await dm.InsertAsync<EmployeeReturnView>(employeeRetViwDialog);
                                //await employeeTree.RefreshAsync();
                                await employeeTree.AddRecordAsync(employeeRetViwDialog)!;
                            }
                        }
                    }
                    await sfDialog.HideAsync();
                }
            }
            catch (Exception e)
            {
                await component.ShowErrorMessage(e.Message);
            }
        }

        private async Task ValidateObjectOnDb()
        {
            var result = await (await httpClient.Client()).EmployeerExistAsync(employeeRetViwDialog.FirstName, employeeRetViwDialog.LastName);
            if (result)
            {
                messageStore.Add(() => employeeRetViwDialog, localResource["FirstNameAndLastName_Exist"]);
                editContextEmployees.NotifyValidationStateChanged();
                iferrorExist = true;
            }
            else
            {
                iferrorExist = false;
                messageStore.Clear();
                editContextEmployees.NotifyValidationStateChanged();
            }
        }

        protected async Task OnBtnCancelClick()
        {
            await sfDialog.HideAsync();
        }

        protected void OnRowSelected(RowSelectEventArgs<EmployeeReturnView> empl)
        {
            selEmployeeReturnView = empl.Data;
        }

        protected async Task SpeedDialItemClick(SpeedDialItemEventArgs obj)
        {
            try
            {
                switch (obj.Item.ID)
                {
                    case "Add":
                        {
                            isAdd = true;
                            await sfDialog.ShowAsync(false);
                            break;
                        }
                    case "Edit":
                        {
                            if (selEmployeeReturnView != null)
                            {
                                isAdd = false;
                                queryReportTo.AddParams("employeeId", selEmployeeReturnView.Id);
                                await sfDialog.ShowAsync(false);
                            }
                            break;
                        }
                    case "Delete":
                        {
                            if (selEmployeeReturnView != null)
                                await removeDialog.ShowRemoveMessage(selEmployeeReturnView.FirstName + " " + selEmployeeReturnView.LastName);
                            break;
                        }
                }
            }
            catch (Exception e)
            {
                await component.ShowErrorMessage(e.Message);
            }
        }

        protected List<ItemModel> GetToolBarStrings()
        {
            if (GlobalStringLocalizer != null)
            {
                return new List<ItemModel>() {
                new ItemModel(){Text = GlobalStringLocalizer["Add"], PrefixIcon = "e-add", Id = $"treeEmployee_gridcontrol_add" } ,
                new ItemModel(){Text = GlobalStringLocalizer["Edit"], PrefixIcon = "e-edit", Id = $"treeEmployee_gridcontrol_edit"},
                new ItemModel(){Text = GlobalStringLocalizer["Delete"], PrefixIcon = "e-delete", Id = $"treeEmployee_gridcontrol_delete"} };
            }
            else
                return new List<ItemModel>();
        }
    }

    public class fileInfo
    {
        public string Path { get; set; }
        public string Name { get; set; }
        public double Size { get; set; }
    }
}
