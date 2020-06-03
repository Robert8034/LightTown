using System;
using System.IO;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LightTown.Client.Web.Components.FileSelect
{
    public class FileSelect : ComponentBase
    {
        public int MaxMessageSize { get; set; } = 20 * 1024;
        public int MaxBufferSize { get; set; } = 1024 * 1024;

        [Inject]
        public IJSRuntime IJsRuntime { get; set; }

        [Parameter]
        public string Src { get; set; }

        [Parameter]
        public string Class { get; set; }
        
        public ElementReference _element;
        public IDisposable _reference;

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                _reference = DotNetObjectReference.Create(this);
                await IJsRuntime.InvokeAsync<object>("FileSelect.init", _element, _reference);
            }
        }



        internal Stream OpenFileStream(FileListEntry file)
        {
            return new RemoteFileListEntryStream(IJsRuntime, _element, file, MaxMessageSize, MaxBufferSize);
        }
    }
}