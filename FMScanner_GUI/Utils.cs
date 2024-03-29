﻿using System;
using System.Threading;

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

        internal static bool EndsWithI(this string str, string str2) => str.EndsWith(str2, StringComparison.OrdinalIgnoreCase);
    }
}
