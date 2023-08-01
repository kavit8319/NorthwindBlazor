using Microsoft.AspNetCore.Components;
using Northwind.Interface.Server.Pages;
using Syncfusion.Blazor.Grids;
using Syncfusion.Blazor.Popups;
using Syncfusion.Blazor.TreeGrid;

namespace Northwind.Interface.Server.Shared
{
    public class RemoveDialogModel<TypeGrid> : BaseComponentModel<TypeGrid>
    {
        protected SfDialog Dialog { get; set; }
        public bool Flag = true;
        [Parameter]
        public bool ShowTemplateContent { get; set; } = true;
        [Parameter]
        public SfGrid<TypeGrid> Grid { get; set; }
        [Parameter]
        public SfTreeGrid<TypeGrid> TreeGrid { get; set; }


        protected string stringMessage { get; set; }

        public void Closed()
        {
            Flag = true;
        }

        public async Task ShowRemoveMessage(string strMessage)
        {
            stringMessage = strMessage;
            await Dialog?.ShowAsync();
        }

        protected async Task OkClick()
        {
            Flag = false;
            if (Grid != null)
            {
                await Grid?.DeleteRecordAsync();   //Delete the record programmatically while clicking OK button.
            }
            if (TreeGrid != null)
                await TreeGrid.DeleteRecordAsync();
            await Dialog.HideAsync();
        }
        protected async Task CancelClick()
        {
            await Dialog.HideAsync();
            Flag = true;
        }
    }
}
