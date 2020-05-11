using Microsoft.AspNetCore.Components;

namespace LightTown.Client.Tests
{
    public class MockNavigationManager : NavigationManager
    {
        public MockNavigationManager() =>
            Initialize("http://localhost:5001/", "http://localhost:5001/test");

        protected override void NavigateToCore(string uri, bool forceLoad) =>
            WasNavigateInvoked = true;

        public bool WasNavigateInvoked { get; private set; }
    }
}