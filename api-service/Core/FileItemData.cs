using Core.DTO;

namespace Core
{
    public class FileItemData : IDisposable
    {
        private bool disposedValue;

        // TODO: Rename, to Item, maybe?
        public FileSystemItemDto Info
        {
            get;
            set;
        }

        public Stream Data
        {
            get;
            set;
        }

        public bool IsFolder => false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (Data != null)
                    {
                        Data.Dispose();
                    }
                }

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }
    }
}