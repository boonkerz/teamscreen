using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Management;
using System.Runtime.InteropServices;
using System.Text;

namespace Driver.Windows.Desktop
{
    public class DesktopInfo : IDisposable
    {
        private StationHandle m_hCurWinsta;
        private StationHandle m_hWinsta;
        private DesktopHandle m_hDesk;
        public string Username;
        private string _Users_DesktopPath;
        public Desktops Current_Desktop;

        public DesktopInfo()
        {
            using (var searcher = new ManagementObjectSearcher("SELECT UserName FROM Win32_ComputerSystem"))
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
        
            m_hCurWinsta = new StationHandle(PInvoke.GetProcessWindowStation());
            if (m_hCurWinsta.IsInvalid)
            {

                var er = new Win32Exception(Marshal.GetLastWin32Error());

                throw er;
            }

            m_hWinsta = new StationHandle(PInvoke.OpenWindowStation("winsta0", false,
                ACCESS_MASK.WINSTA_ENUMDESKTOPS |
                ACCESS_MASK.WINSTA_READATTRIBUTES |
                ACCESS_MASK.WINSTA_ACCESSCLIPBOARD |
                ACCESS_MASK.WINSTA_CREATEDESKTOP |
                ACCESS_MASK.WINSTA_WRITEATTRIBUTES |
                ACCESS_MASK.WINSTA_ACCESSGLOBALATOMS |
                ACCESS_MASK.WINSTA_EXITWINDOWS |
                ACCESS_MASK.WINSTA_ENUMERATE |
                ACCESS_MASK.WINSTA_READSCREEN));
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
            m_hDesk = new DesktopHandle(PInvoke.OpenDesktop("default", 0, false,
                    ACCESS_MASK.DESKTOP_CREATEMENU |
                    ACCESS_MASK.DESKTOP_CREATEWINDOW |
                    ACCESS_MASK.DESKTOP_ENUMERATE |
                    ACCESS_MASK.DESKTOP_HOOKCONTROL |
                    ACCESS_MASK.DESKTOP_JOURNALPLAYBACK |
                    ACCESS_MASK.DESKTOP_JOURNALRECORD |
                    ACCESS_MASK.DESKTOP_READOBJECTS |
                    ACCESS_MASK.DESKTOP_SWITCHDESKTOP |
                    ACCESS_MASK.DESKTOP_WRITEOBJECTS));
            if (m_hDesk.IsInvalid)
            {

                var er = new Win32Exception(Marshal.GetLastWin32Error());

                throw er;
            }
            if (!PInvoke.SetThreadDesktop(m_hDesk.Handle))
            {

                //var er = new Win32Exception(Marshal.GetLastWin32Error());

                //throw er;
            }
            Current_Desktop = GetDesktop(m_hDesk);

        }
        public Desktops GetActiveDesktop()
        {
            using (var s = new DesktopHandle(PInvoke.OpenInputDesktop(0, false, ACCESS_MASK.DESKTOP_SWITCHDESKTOP)))
            {
                return GetDesktop(s);
            }
        }
        private Desktops GetDesktop(DesktopHandle s)
        {
            if (s.IsInvalid)
                return Desktops.Default;
            int needed = 0;
            string name = string.Empty;
            PInvoke.GetUserObjectInformation(s.Handle, PInvoke.UOI_NAME, IntPtr.Zero, 0, ref needed);

            // get the name.
            IntPtr ptr = Marshal.AllocHGlobal(needed);
            bool result = PInvoke.GetUserObjectInformation(s.Handle, PInvoke.UOI_NAME, ptr, needed, ref needed);
            name = Marshal.PtrToStringAnsi(ptr).ToLower();
            Marshal.FreeHGlobal(ptr);

            if (!result)
                return Desktops.Default;
            if (name == "default")
                return Desktops.Default;
            else if (name == "screensaver")
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
            var desktop = new DesktopHandle(PInvoke.OpenDesktop(Enum.GetName(dname.GetType(), dname), 0, false,
                 ACCESS_MASK.DESKTOP_CREATEMENU | ACCESS_MASK.DESKTOP_CREATEWINDOW |
                 ACCESS_MASK.DESKTOP_ENUMERATE | ACCESS_MASK.DESKTOP_HOOKCONTROL |
                 ACCESS_MASK.DESKTOP_WRITEOBJECTS | ACCESS_MASK.DESKTOP_READOBJECTS |
                 ACCESS_MASK.DESKTOP_SWITCHDESKTOP | ACCESS_MASK.GENERIC_WRITE));

            if (desktop.IsInvalid)
                return false;
            if (!PInvoke.SetThreadDesktop(desktop.Handle))
            {
                desktop.Dispose();
                return false;
            }
            m_hDesk.Dispose();
            m_hDesk = desktop;
            Current_Desktop = dname;
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
