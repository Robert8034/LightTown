using System;
using System.Collections.Generic;

namespace LightTown.Client.Services.Popups
{
    public class BlazorPopupService : IPopupService<BlazorPopupService.Popup>
    {
        public Dictionary<Popup, PopupType> Popups { get; set; }
        public Action<Popup> OnShowPopup { get; set; }
        public Action<Popup> OnClosePopup { get; set; }

        public BlazorPopupService()
        {
            Popups = new Dictionary<Popup, PopupType>();
        }

        public void ShowErrorPopup(bool closable, TimeSpan? closesAfter, string body, string title)
        {
            Popup popup = new Popup(PopupType.Error, closable, closesAfter, body, title);
            Popups[popup] = PopupType.Error;

             OnShowPopup?.Invoke(popup);
        }

        public void ShowWarningPopup(bool closable, TimeSpan? closesAfter, string body, string title)
        {
            Popup popup = new Popup(PopupType.Warning, closable, closesAfter, body, title);
            Popups[popup] = PopupType.Warning;

            OnShowPopup?.Invoke(popup);
        }

        public void ShowStandardPopup(bool closable, TimeSpan? closesAfter, string body, string title)
        {
            Popup popup = new Popup(PopupType.Standard, closable, closesAfter, body, title);
            Popups[popup] = PopupType.Standard;

            OnShowPopup?.Invoke(popup);
        }

        public void ShowSuccessPopup(bool closable, TimeSpan? closesAfter, string body, string title)
        {
            Popup popup = new Popup(PopupType.Success, closable, closesAfter, body, title);
            Popups[popup] = PopupType.Success;

            OnShowPopup?.Invoke(popup);
        }

        public class Popup
        {
            public PopupType PopupType { get; set; }
            public bool Closable { get; }
            public TimeSpan? ClosesAfter { get; }
            public string Body { get; }
            public string Title { get; }
            public bool Shown { get; set; }

            public Popup(PopupType popupType, bool closable, TimeSpan? closesAfter, string body, string title)
            {
                PopupType = popupType;
                Closable = closable;
                ClosesAfter = closesAfter;
                Body = body;
                Title = title;
                Shown = false;
            }
        }
    }
}
