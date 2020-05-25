using System;
using System.IO;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace LightTown.Client.Web.Components.FileSelect
{
    public abstract class FileListEntryStream : Stream
    {
        protected readonly IJSRuntime _jsRuntime;
        protected readonly ElementReference _inputFileElement;
        protected readonly FileListEntry _file;
        private long _position;

        public FileListEntryStream(IJSRuntime jsRuntime, ElementReference inputFileElement, FileListEntry file)
        {
            _jsRuntime = jsRuntime;
            _inputFileElement = inputFileElement;
            _file = file;
        }

        public override bool CanRead => true;

        public override bool CanSeek => false;

        public override bool CanWrite => false;

        public override long Length => _file.Size;

        public override long Position
        {
            get => _position;
            set => throw new NotSupportedException();
        }

        public override void Flush()
            => throw new NotSupportedException();

        public override int Read(byte[] buffer, int offset, int count)
            => throw new NotSupportedException("Synchronous reads are not supported");

        public override long Seek(long offset, SeekOrigin origin)
            => throw new NotSupportedException();

        public override void SetLength(long value)
            => throw new NotSupportedException();

        public override void Write(byte[] buffer, int offset, int count)
            => throw new NotSupportedException();

        public override async Task<int> ReadAsync(byte[] buffer, int offset, int count, CancellationToken cancellationToken)
        {
            var maxBytesToRead = (int)Math.Min(count, Length - Position);
            if (maxBytesToRead == 0)
            {
                return 0;
            }

            var actualBytesRead = await CopyFileDataIntoBuffer(_position, buffer, offset, maxBytesToRead, cancellationToken);
            _position += actualBytesRead;
            _file.RaiseOnDataRead();
            return actualBytesRead;
        }

        protected abstract Task<int> CopyFileDataIntoBuffer(long sourceOffset, byte[] destination, int destinationOffset, int maxBytes, CancellationToken cancellationToken);
    }
}