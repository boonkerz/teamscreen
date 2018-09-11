using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace Driver.Win.Utils
{
    public class DesktopInfo : IDisposable
    {
        private StationHandle m_hCurWinsta;
        private StationHandle m_hWinsta;
        private DesktopHandle m_hDesk;
        public string Username;
        private string _Users_DesktopPath;
        public Desktops Current_Desktop = Desktops.ScreenSaver;

        private StreamWriter f = new StreamWriter(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\desktopinfo.txt", true);

        public DesktopInfo()
        {

            /*using (var searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem"))
            {
                using (var collection = searcher.Get())
                {
                    var s = ((string)collection.Cast<ManagementBaseObject>().First()["UserName"]).Split('\\');
                    if (s.Length > 1)
                        Username = s.LastOrDefault();
                    else
                        Username = s.FirstOrDefault();
                }
            }
            _Users_DesktopPath = @"c:\users\" + Username + @"\desktop\";
        */
            f.AutoFlush = true;


            /*m_hCurWinsta = new StationHandle(PInvoke.GetProcessWindowStation());
            if (m_hCurWinsta.IsInvalid)
            {

                var er = new Win32Exception(Marshal.GetLastWin32Error());

                throw er;
            }
            
            m_hWinsta = new StationHandle(PInvoke.OpenWindowStation("Winsta0", false,
                ACCESS_MASK.MAXIMUM_ALLOWED));
            if (m_hWinsta.IsInvalid)
            {

                var er = new Win32Exception(Marshal.GetLastWin32Error());

                throw er;
            }
          
            if (!PInvoke.SetProcessWindowStation(m_hWinsta.Handle))
            {

                var er = new Win32Exception(Marshal.GetLastWin32Error());

                throw er;
            }
            m_hDesk = new DesktopHandle(PInvoke.OpenDesktop("default", 0, false,ACCESS_MASK.MAXIMUM_ALLOWED));
            if (m_hDesk.IsInvalid)
            {

                var er = new Win32Exception(Marshal.GetLastWin32Error());

                throw er;
            }
           
            if (!PInvoke.SetThreadDesktop(m_hDesk.Handle))
            {

                var er = new Win32Exception(Marshal.GetLastWin32Error());

                throw er;
            }
            Current_Desktop = GetDesktop(m_hDesk);*/

        }
        public Desktops GetActiveDesktop()
        {
            using (var s = new DesktopHandle(PInvoke.OpenInputDesktop(0, false, PInvoke.ACCESS_MASK.MAXIMUM_ALLOWED)))
            {
                return GetDesktop(s);
            }
        }

        public void SwitchToInputDesktop()
        {
            f.WriteLine("SwitchtoDesktop 1 ");
            var s = PInvoke.OpenInputDesktop(0, false, PInvoke.ACCESS_MASK.MAXIMUM_ALLOWED);
            f.WriteLine("SwitchtoDesktop 2 ");
            bool success = PInvoke.SetThreadDesktop(s);
            f.WriteLine("SwitchtoDesktop 3 " + success);
            PInvoke.CloseDesktop(s);
            f.WriteLine("SwitchtoDesktop 4 ");
        }


        private Desktops GetDesktop(DesktopHandle s)
        {
            f.WriteLine("Get Desktop Invalid" + s.IsInvalid);
            if (s.IsInvalid)
                return Desktops.Default;
            int needed = 0;
            string name = string.Empty;
            PInvoke.GetUserObjectInformation(s.Handle, PInvoke.UOI_NAME, IntPtr.Zero, 0, ref needed);

            // get the name.
            IntPtr ptr = Marshal.AllocHGlobal(needed);
            bool result = PInvoke.GetUserObjectInformation(s.Handle, PInvoke.UOI_NAME, ptr, needed, ref needed);
            f.WriteLine("Get Desktop Result" + result);
            name = Marshal.PtrToStringAnsi(ptr);
            f.WriteLine("Get Desktop Name" + name);
            Marshal.FreeHGlobal(ptr);
            if (!result)
                return Desktops.Default;
            if (name == "Default")
                return Desktops.Default;
            else if (name == "Screensaver")
                return Desktops.ScreenSaver;
            else
                return Desktops.Winlogon;
        }
        public void FileEvent(string filename, byte[] file)
        {
            try
            {
                System.IO.File.WriteAllBytes(_Users_DesktopPath + filename, file);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }

        public void FolderEvent(string relativefolderpath)
        {
            try
            {
                System.IO.Directory.CreateDirectory(_Users_DesktopPath + relativefolderpath);
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
            }

        }
        public bool SwitchDesktop(Desktops dname)
        {
            f.WriteLine("Switch TO" + Enum.GetName(dname.GetType(), dname));
            var desktop = new DesktopHandle(PInvoke.OpenDesktop(Enum.GetName(dname.GetType(), dname), 0, false,
                PInvoke.ACCESS_MASK.DESKTOP_CREATEMENU | PInvoke.ACCESS_MASK.DESKTOP_CREATEWINDOW |
                PInvoke.ACCESS_MASK.DESKTOP_ENUMERATE | PInvoke.ACCESS_MASK.DESKTOP_HOOKCONTROL |
                PInvoke.ACCESS_MASK.DESKTOP_WRITEOBJECTS | PInvoke.ACCESS_MASK.DESKTOP_READOBJECTS |
                PInvoke.ACCESS_MASK.DESKTOP_SWITCHDESKTOP | PInvoke.ACCESS_MASK.GENERIC_WRITE));
            f.WriteLine("Switch Invalid" + desktop.IsInvalid);
            if (desktop.IsInvalid)
            {
                return false;
            }
            if (!PInvoke.SetThreadDesktop(desktop.Handle))
            {
                f.WriteLine("Switch failed" + Marshal.GetLastWin32Error());
                desktop.Dispose();
                return false;
            }
            m_hDesk.Dispose();
            m_hDesk = desktop;
            Current_Desktop = dname;
            f.WriteLine("Switch Success" + dname);
            return true;

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
                m_hCurWinsta.Dispose();
                m_hWinsta.Dispose();
                m_hDesk.Dispose();
            }
        }
    }
}
