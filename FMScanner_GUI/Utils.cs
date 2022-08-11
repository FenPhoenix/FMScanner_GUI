using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace FMScanner_GUI
{
    internal static class Utils
    {
        internal static void CancelIfNotDisposed(this CancellationTokenSource value)
        {
            try { value.Cancel(); } catch (ObjectDisposedException) { }
        }

        internal static CancellationTokenSource Recreate(this CancellationTokenSource cts)
        {
            cts.Dispose();
            return new CancellationTokenSource();
        }
    }
}
