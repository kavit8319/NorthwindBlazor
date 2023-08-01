using Microsoft.AspNetCore.Components.Web;
using Northwind.Interface.Server.AddModelRequiredAttribution;
using Northwind.Interface.Server.Shared;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Inputs;

namespace Northwind.Interface.Server.Pages
{
    public class SfCategorysModel<TLocal> : BaseComponentModel<TLocal>
    {
        protected CustomGridAddEditDel<CategoryReturnView> categoryGrid;

        protected List<fileInfo> files = new();
        protected bool IsFileRemoveEnable { get; set; } = false;
        protected SfUploader uploader
        {
            get; set;
        }
        protected Dictionary<string, string> HeaderRemove;
        public RemoveDialog<CategoryReturnView> removeDialog { get; set; }
        protected override void OnInitialized()
        {
            component.HeaderMessage = localResource["Title"];
        }
        public async Task OnActionBeginCustomer(ActionEventArgs<CategoryReturnView> actionEvent)
        {
            try
            {


                if (actionEvent.RequestType == Syncfusion.Blazor.Grids.Action.Save && actionEvent.Action == "Add")
                {
                    if (files.Any())
                    {
                        var file = files.First<fileInfo>();
                        actionEvent.Data.Picture = Convert.FromBase64String(file.Path.Split(",").Last());
                        IsFileRemoveEnable = false;
                    }
                    //await categoryGrid.AddRecordAsync();
                }
                if (actionEvent.RequestType == Syncfusion.Blazor.Grids.Action.BeginEdit)
                {
                    if ((actionEvent.Data as CategoryReturnView).Picture != null)
                        IsFileRemoveEnable = true;
                }
                if (actionEvent.RequestType == Syncfusion.Blazor.Grids.Action.Save && actionEvent.Action == "Edit")
                {

                    if (files.Any())
                    {

                        var file = files.First<fileInfo>();
                        actionEvent.Data.Picture = Convert.FromBase64String(file.Path.Split(",").Last());
                    }
                }
                if (categoryGrid != null && categoryGrid.removeDialog != null)
                    if (actionEvent.RequestType == Syncfusion.Blazor.Grids.Action.Delete && categoryGrid.removeDialog.Flag)
                    {
                        if (!removeReletionDate)
                        {
                            HeaderRemove = new Dictionary<string, string>() { { Constans.Comfirm, "false" } };
                            actionEvent.Cancel = true; //Cancel default delete action.
                            await categoryGrid.removeDialog.ShowRemoveMessage($" {actionEvent.Data.CategoryName}");
                            categoryGrid.removeDialog.Flag = false;
                        }
                        removeReletionDate = false;
                    }
            }
            catch (Exception ex)
            {
                await component.ShowErrorMessage(ex.ToString());
            }
        }

        private bool removeReletionDate;
        public async Task RemoveReletionProduct(Exception ex)
        {
            HeaderRemove = new Dictionary<string, string>() { { Constans.Comfirm, "true" } };
            removeDialog.Flag = false;
            removeReletionDate = true;

            await categoryGrid.SetCustomRowSelectedById(Convert.ToInt32(ex.Message.Split(":").Last()));
            await removeDialog.ShowRemoveMessage(categoryGrid.GlobalStringLocalizer[ex.InnerException.Message]);
        }
        public void onChange(UploadChangeEventArgs args, CategoryReturnView val)
        {
            IsFileRemoveEnable = false;
            categoryGrid?.PreventRender(false);
            files = new List<fileInfo>();
            var file = args.Files[0];
            val.Picture = file.Stream.ToArray();
            string base64 = Convert.ToBase64String(val.Picture);
            files.Add(new fileInfo() { Path = @"data:image/" + file.FileInfo.Type + ";base64," + base64, Name = file.FileInfo.Name, Size = file.FileInfo.Size });
        }

        protected async Task RemoveImage(MouseEventArgs e, object o)
        {
            var element = (CategoryReturnView)o;
            element.Picture = null;
            files.Clear();
            await uploader.ClearAllAsync();
        }
    }
}
