using System;
using System.Collections.Generic;

namespace LightTown.Client.Services.Popups
{
    /// <summary>
    /// Popup service that should be implemented in every client.
    /// </summary>
    public interface IPopupService<T>
    {
        /// <summary>
        /// The list of popups, add your current popups here and remove them when they get closed.
        /// </summary>
        Dictionary<T, PopupType> Popups { get; set; }

        /// <summary>
        /// This should be invoked whenever a "Show...Popup()" method gets called by the implementation of this <see cref="IPopupService{T}"/>. Set this action on your client so it can update UI when it gets called.
        /// </summary>
        Action<T> OnShowPopup { get; set; }

        /// <summary>
        /// This should be invoked whenever a popup gets removed from the <see cref="Popups"/> list. Set this action on your client so it can update UI when it gets called.
        /// </summary>
        Action<T> OnClosePopup { get; set; }

        /// <summary>
        /// Show an error popup to the user.
        /// </summary>
        /// <param name="closable">Whether the popup should be closable or not.</param>
        /// <param name="closesAfter">How long it takes before the popup closes by itself, <see langword="null"></see> to not close it automatically.</param>
        /// <param name="body">The body text for the popup.</param>
        /// <param name="title">The title for the popup, optional.</param>
        void ShowErrorPopup(bool closable, TimeSpan? closesAfter, string body, string title = null);

        /// <summary>
        /// Show a warning popup to the user.
        /// </summary>
        /// <param name="closable">Whether the popup should be closable or not.</param>
        /// <param name="closesAfter">How long it takes before the popup closes by itself, <see langword="null"></see> to not close it automatically.</param>
        /// <param name="body">The body text for the popup.</param>
        /// <param name="title">The title for the popup, optional.</param>
        void ShowWarningPopup(bool closable, TimeSpan? closesAfter, string body, string title = null);

        /// <summary>
        /// Show a standard popup to the user.
        /// </summary>
        /// <param name="closable">Whether the popup should be closable or not.</param>
        /// <param name="closesAfter">How long it takes before the popup closes by itself, <see langword="null"></see> to not close it automatically.</param>
        /// <param name="body">The body text for the popup.</param>
        /// <param name="title">The title for the popup, optional.</param>
        void ShowStandardPopup(bool closable, TimeSpan? closesAfter, string body, string title = null);

        /// <summary>
        /// Show a success popup to the user.
        /// </summary>
        /// <param name="closable">Whether the popup should be closable or not.</param>
        /// <param name="closesAfter">How long it takes before the popup closes by itself, <see langword="null"></see> to not close it automatically.</param>
        /// <param name="body">The body text for the popup.</param>
        /// <param name="title">The title for the popup, optional.</param>
        void ShowSuccessPopup(bool closable, TimeSpan? closesAfter, string body, string title = null);
    }

    public enum PopupType
    {
        Error,
        Warning,
        Standard,
        Success
    }
}