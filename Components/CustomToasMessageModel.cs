using Microsoft.AspNetCore.Components;
using Syncfusion.Blazor.Notifications;

namespace Northwind.Interface.Server.Components
{
    public class CustomToasMessageModel : ComponentBase, IDisposable
    {
        [Inject]
        public CustomToastService ToasService { get; set; }

        protected bool MessageVisible { get; set; }
        protected string MessageText { get; set; }
        protected MessageSeverity MessageSeverity { get; set; }

        protected override void OnInitialized()
        {
            ToasService.OnShow += ShowToast;
            ToasService.OnHide += HideToast;
        }
        private void ShowToast(string message, MessageSeverity level)
        {
            BuildToastSettings(level, message);
            MessageVisible = true;
            StateHasChanged();
        }

        private async void HideToast()
        {
            MessageVisible = false;
            await this.InvokeAsync(() => StateHasChanged());
        }

        private void BuildToastSettings(MessageSeverity level, string message)
        {
            switch (level)
            {
                case MessageSeverity.Info:
                    MessageSeverity = MessageSeverity.Info;
                    break;
                case MessageSeverity.Success:
                    MessageSeverity = MessageSeverity.Success;
                    break;
                case MessageSeverity.Warning:
                    MessageSeverity = MessageSeverity.Warning;
                    break;
                case MessageSeverity.Error:
                    MessageSeverity = MessageSeverity.Error;
                    break;
                default:
                    throw new ArgumentOutOfRangeException(nameof(level), level, null);
            }

            MessageText = message;
        }

        void IDisposable.Dispose()
        {
            ToasService.OnShow -= ShowToast;
            ToasService.OnHide -= HideToast;
        }

    }
}
