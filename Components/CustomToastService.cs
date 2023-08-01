using Syncfusion.Blazor.Notifications;
using System.Timers;

namespace Northwind.Interface.Server.Components
{
    public class CustomToastService:IDisposable
    {
        public event Action<string, MessageSeverity>? OnShow;
        public event Action? OnHide;
        private System.Timers.Timer? Countdown;

        public void ShowToast(string message, MessageSeverity level)
        {
            OnShow?.Invoke(message, level);
            StartCountdown();
        }

        private void StartCountdown()
        {
            SetCountdown();

            if (Countdown!.Enabled)
            {
                Countdown.Stop();
                Countdown.Start();
            }
            else
            {
                Countdown!.Start();
            }
        }

        private void SetCountdown()
        {
            if (Countdown != null) return;

            Countdown = new System.Timers.Timer(5000);
            Countdown.Elapsed += HideToast;
            Countdown.AutoReset = false;
        }

        private void HideToast(object? source, ElapsedEventArgs args)
            => OnHide?.Invoke();

        public void Dispose()
            => Countdown?.Dispose();
    }
}
