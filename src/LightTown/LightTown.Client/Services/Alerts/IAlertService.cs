using System;
using System.Collections.Generic;

namespace LightTown.Client.Services.Alerts
{
    /// <summary>
    /// Alert service that should be implemented in every client.
    /// </summary>
    public interface IAlertService<T>
    {
        /// <summary>
        /// The list of alerts, add your current alerts here and remove them when they get closed.
        /// </summary>
        Dictionary<T, AlertType> Alerts { get; set; }

        /// <summary>
        /// This should be invoked whenever a "Show...Alert()" method gets called by the implementation of this <see cref="IAlertService{T}"/>. Set this action on your client so it can update UI when it gets called.
        /// </summary>
        Action<T> OnShowAlert { get; set; }

        /// <summary>
        /// This should be invoked whenever a Alert gets removed from the <see cref="Alerts"/> list. Set this action on your client so it can update UI when it gets called.
        /// </summary>
        Action<T> OnCloseAlert { get; set; }

        /// <summary>
        /// Show an error alert to the user.
        /// </summary>
        /// <param name="closable">Whether the alert should be closable or not.</param>
        /// <param name="closesAfter">How long it takes before the alert closes by itself, <see langword="null"></see> to not close it automatically.</param>
        /// <param name="body">The body text for the alert.</param>
        /// <param name="title">The title for the alert, optional.</param>
        void ShowErrorAlert(bool closable, TimeSpan? closesAfter, string body, string title = null);

        /// <summary>
        /// Show a warning alert to the user.
        /// </summary>
        /// <param name="closable">Whether the alert should be closable or not.</param>
        /// <param name="closesAfter">How long it takes before the alert closes by itself, <see langword="null"></see> to not close it automatically.</param>
        /// <param name="body">The body text for the alert.</param>
        /// <param name="title">The title for the alert, optional.</param>
        void ShowWarningAlert(bool closable, TimeSpan? closesAfter, string body, string title = null);

        /// <summary>
        /// Show a standard alert to the user.
        /// </summary>
        /// <param name="closable">Whether the alert should be closable or not.</param>
        /// <param name="closesAfter">How long it takes before the alert closes by itself, <see langword="null"></see> to not close it automatically.</param>
        /// <param name="body">The body text for the alert.</param>
        /// <param name="title">The title for the alert, optional.</param>
        void ShowStandardAlert(bool closable, TimeSpan? closesAfter, string body, string title = null);

        /// <summary>
        /// Show a success alert to the user.
        /// </summary>
        /// <param name="closable">Whether the alert should be closable or not.</param>
        /// <param name="closesAfter">How long it takes before the alert closes by itself, <see langword="null"></see> to not close it automatically.</param>
        /// <param name="body">The body text for the alert.</param>
        /// <param name="title">The title for the alert, optional.</param>
        void ShowSuccessAlert(bool closable, TimeSpan? closesAfter, string body, string title = null);
    }

    public enum AlertType
    {
        Error,
        Warning,
        Standard,
        Success
    }
}
