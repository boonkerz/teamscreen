using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Driver.Win.Utils
{
    public class StationHandle : IDisposable
    {
        public IntPtr Handle { get; private set; }
        public StationHandle(IntPtr handle)
        {
            Handle = handle;
        }
        public bool IsInvalid
        {
            get { return Handle == IntPtr.Zero; }
        }
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                PInvoke.CloseWindowStation(Handle);
            }
        }
    }
}
