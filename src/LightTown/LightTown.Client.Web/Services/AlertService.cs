using System;
using System.Collections.Generic;
using LightTown.Client.Services.Abstraction;

namespace LightTown.Client.Web.Services
{
    public class AlertService : IAlertService<AlertService.Alert>
    {
        public Dictionary<Alert, AlertType> Alerts { get; set; }
        public Action<Alert> OnShowAlert { get; set; }
        public Action<Alert> OnCloseAlert { get; set; }

        public AlertService()
        {
            Alerts = new Dictionary<Alert, AlertType>();
        }

        public void ShowErrorAlert(bool closable, TimeSpan? closesAfter, string body, string title)
        {
            Alert alert = new Alert(AlertType.Error, closable, closesAfter, body, title);
            Alerts[alert] = AlertType.Error;

             OnShowAlert?.Invoke(alert);
        }

        public void ShowWarningAlert(bool closable, TimeSpan? closesAfter, string body, string title)
        {
            Alert alert = new Alert(AlertType.Warning, closable, closesAfter, body, title);
            Alerts[alert] = AlertType.Warning;

            OnShowAlert?.Invoke(alert);
        }

        public void ShowStandardAlert(bool closable, TimeSpan? closesAfter, string body, string title)
        {
            Alert alert = new Alert(AlertType.Standard, closable, closesAfter, body, title);
            Alerts[alert] = AlertType.Standard;

            OnShowAlert?.Invoke(alert);
        }

        public void ShowSuccessAlert(bool closable, TimeSpan? closesAfter, string body, string title)
        {
            Alert alert = new Alert(AlertType.Success, closable, closesAfter, body, title);
            Alerts[alert] = AlertType.Success;

            OnShowAlert?.Invoke(alert);
        }

        public class Alert
        {
            public AlertType AlertType { get; set; }
            public bool Closable { get; }
            public TimeSpan? ClosesAfter { get; }
            public string Body { get; }
            public string Title { get; }
            public bool Shown { get; set; }

            public Alert(AlertType alertType, bool closable, TimeSpan? closesAfter, string body, string title)
            {
                AlertType = alertType;
                Closable = closable;
                ClosesAfter = closesAfter;
                Body = body;
                Title = title;
                Shown = false;
            }
        }
    }
}
