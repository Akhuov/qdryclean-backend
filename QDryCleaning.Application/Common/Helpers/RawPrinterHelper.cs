using System.Runtime.InteropServices;

namespace QDryClean.Application.Common.Helpers
{
    public static class RawPrinterHelper
    {
        [DllImport("winspool.Drv", EntryPoint = "OpenPrinterA", SetLastError = true)]
        public static extern bool OpenPrinter(string szPrinter, out IntPtr hPrinter, IntPtr pd);

        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool ClosePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool StartDocPrinter(IntPtr hPrinter, int level, [In] DOCINFO di);

        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool EndDocPrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool StartPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool EndPagePrinter(IntPtr hPrinter);

        [DllImport("winspool.Drv", SetLastError = true)]
        public static extern bool WritePrinter(IntPtr hPrinter, byte[] bytes, int count, out int written);

        [StructLayout(LayoutKind.Sequential)]
        public class DOCINFO
        {
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDocName;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pOutputFile;
            [MarshalAs(UnmanagedType.LPStr)]
            public string pDataType;
        }

        public static bool SendBytesToPrinter(string printerName, byte[] bytes)
        {
            OpenPrinter(printerName, out var hPrinter, IntPtr.Zero);

            var di = new DOCINFO
            {
                pDocName = "Receipt",
                pDataType = "RAW"
            };

            StartDocPrinter(hPrinter, 1, di);
            StartPagePrinter(hPrinter);

            WritePrinter(hPrinter, bytes, bytes.Length, out _);

            EndPagePrinter(hPrinter);
            EndDocPrinter(hPrinter);
            ClosePrinter(hPrinter);

            return true;
        }
    }
}
