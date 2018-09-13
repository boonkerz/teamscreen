using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using System.Threading.Tasks;

namespace TeamScreenService
{
    namespace Toolkit
    {
        public static class ApplicationLoader
        {

            [StructLayout(LayoutKind.Sequential)]
            private struct SECURITY_ATTRIBUTES
            {
                public int Length;
                public IntPtr lpSecurityDescriptor;
                public bool bInheritHandle;
            }

            [StructLayout(LayoutKind.Sequential)]
            private struct STARTUPINFO
            {
                public int cb;
                public String lpReserved;
                public String lpDesktop;
                public String lpTitle;
                public uint dwX;
                public uint dwY;
                public uint dwXSize;
                public uint dwYSize;
                public uint dwXCountChars;
                public uint dwYCountChars;
                public uint dwFillAttribute;
                public uint dwFlags;
                public short wShowWindow;
                public short cbReserved2;
                public IntPtr lpReserved2;
                public IntPtr hStdInput;
                public IntPtr hStdOutput;
                public IntPtr hStdError;
            }

            [StructLayout(LayoutKind.Sequential)]
            public struct PROCESS_INFORMATION
            {
                public IntPtr hProcess;
                public IntPtr hThread;
                public uint dwProcessId;
                public uint dwThreadId;
            }
            private enum TOKEN_TYPE : int
            {
                TokenPrimary = 1,
                TokenImpersonation = 2
            }

            private enum SECURITY_IMPERSONATION_LEVEL : int
            {
                SecurityAnonymous = 0,
                SecurityIdentification = 1,
                SecurityImpersonation = 2,
                SecurityDelegation = 3,
            }


            private const int TOKEN_DUPLICATE = 0x0002;
            private const uint MAXIMUM_ALLOWED = 0x2000000;
            private const int CREATE_NEW_CONSOLE = 0x00000010;
            private const int CREATE_NO_WINDOW = 0x08000000;
            private const int DETACHED_PROCESS = 0x00000008;


            private const int IDLE_PRIORITY_CLASS = 0x40;
            private const int NORMAL_PRIORITY_CLASS = 0x20;
            private const int HIGH_PRIORITY_CLASS = 0x80;
            private const int REALTIME_PRIORITY_CLASS = 0x100;



            [DllImport("kernel32.dll", SetLastError = true)]
            private static extern bool CloseHandle(IntPtr hSnapshot);

            [DllImport("kernel32.dll")]
            static extern uint WTSGetActiveConsoleSessionId();

            [DllImport("advapi32.dll", EntryPoint = "CreateProcessAsUser", SetLastError = true, CharSet = CharSet.Ansi, CallingConvention = CallingConvention.StdCall)]
            private extern static bool CreateProcessAsUser(IntPtr hToken, String lpApplicationName, String lpCommandLine, ref SECURITY_ATTRIBUTES lpProcessAttributes,
                ref SECURITY_ATTRIBUTES lpThreadAttributes, bool bInheritHandle, int dwCreationFlags, IntPtr lpEnvironment,
                String lpCurrentDirectory, ref STARTUPINFO lpStartupInfo, out PROCESS_INFORMATION lpProcessInformation);

            [DllImport("kernel32.dll")]
            private static extern bool ProcessIdToSessionId(uint dwProcessId, ref uint pSessionId);

            [DllImport("advapi32.dll", EntryPoint = "DuplicateTokenEx")]
            private extern static bool DuplicateTokenEx(IntPtr ExistingTokenHandle, uint dwDesiredAccess,
                ref SECURITY_ATTRIBUTES lpThreadAttributes, int TokenType,
                int ImpersonationLevel, ref IntPtr DuplicateTokenHandle);

            [DllImport("kernel32.dll")]
            private static extern IntPtr OpenProcess(uint dwDesiredAccess, bool bInheritHandle, uint dwProcessId);

            [DllImport("advapi32", SetLastError = true), SuppressUnmanagedCodeSecurityAttribute]
            private static extern bool OpenProcessToken(IntPtr ProcessHandle, int DesiredAccess, ref IntPtr TokenHandle);

            public static bool StartProcessAndBypassUAC(String applicationName, out PROCESS_INFORMATION procInfo)
            {
                using (var f = System.IO.File.CreateText(System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location) + "\\logStartService.txt"))
                {
                    try
                    {
                        uint winlogonPid = 0;
                        IntPtr hUserTokenDup = IntPtr.Zero, hPToken = IntPtr.Zero, hProcess = IntPtr.Zero;
                        procInfo = new PROCESS_INFORMATION();
                        f.WriteLine("StartProcessAndBypassUAC " + applicationName);
                        // obtain the currently active session id; every logged on user in the system has a unique session id
                        uint dwSessionId = WTSGetActiveConsoleSessionId();
                        f.WriteLine("WTSGetActiveConsoleSessionId ");
                        // obtain the process id of the winlogon process that is running within the currently active session
                        Process[] processes = Process.GetProcessesByName("winlogon");
                        f.WriteLine("GetProcessesByName ");
                        foreach (Process p in processes)
                        {
                            if ((uint)p.SessionId == dwSessionId)
                            {
                                winlogonPid = (uint)p.Id;
                            }
                        }
                        f.WriteLine("winlogonPid " + winlogonPid);
                        // obtain a handle to the winlogon process
                        hProcess = OpenProcess(MAXIMUM_ALLOWED, false, winlogonPid);
                        f.WriteLine("OpenProcess ");
                        // obtain a handle to the access token of the winlogon process
                        if (!OpenProcessToken(hProcess, TOKEN_DUPLICATE, ref hPToken))
                        {
                            f.WriteLine("!OpenProcessToken ");
                            CloseHandle(hProcess);
                            return false;
                        }
                        f.WriteLine("OpenProcessToken ");
                        // Security attibute structure used in DuplicateTokenEx and CreateProcessAsUser
                        // I would prefer to not have to use a security attribute variable and to just 
                        // simply pass null and inherit (by default) the security attributes
                        // of the existing token. However, in C# structures are value types and therefore
                        // cannot be assigned the null value.
                        SECURITY_ATTRIBUTES sa = new SECURITY_ATTRIBUTES();
                        sa.Length = Marshal.SizeOf(sa);

                        // copy the access token of the winlogon process; the newly created token will be a primary token
                        if (!DuplicateTokenEx(hPToken, MAXIMUM_ALLOWED, ref sa, (int)SECURITY_IMPERSONATION_LEVEL.SecurityIdentification, (int)TOKEN_TYPE.TokenPrimary, ref hUserTokenDup))
                        {
                            f.WriteLine("!DuplicateTokenEx ");
                            CloseHandle(hProcess);
                            CloseHandle(hPToken);
                            return false;
                        }
                        f.WriteLine("DuplicateTokenEx ");
                        // By default CreateProcessAsUser creates a process on a non-interactive window station, meaning
                        // the window station has a desktop that is invisible and the process is incapable of receiving
                        // user input. To remedy this we set the lpDesktop parameter to indicate we want to enable user 
                        // interaction with the new process.
                        STARTUPINFO si = new STARTUPINFO();
                        si.cb = (int)Marshal.SizeOf(si);
                        si.lpDesktop = "Winsta0\\Winlogon"; // interactive window station parameter; basically this indicates that the process created can display a GUI on the desktop
                        f.WriteLine("Winsta0\\Winlogon");
                        // flags that specify the priority and creation method of the process
                        int dwCreationFlags = NORMAL_PRIORITY_CLASS | CREATE_NO_WINDOW | DETACHED_PROCESS;

                        // create a new process in the current user's logon session
                        bool result = CreateProcessAsUser(hUserTokenDup,        // client's access token
                                                        null,                   // file to execute
                                                        applicationName,        // command line
                                                        ref sa,                 // pointer to process SECURITY_ATTRIBUTES
                                                        ref sa,                 // pointer to thread SECURITY_ATTRIBUTES
                                                        false,                  // handles are not inheritable
                                                        dwCreationFlags,        // creation flags
                                                        IntPtr.Zero,            // pointer to new environment block 
                                                        null,                   // name of current directory 
                                                        ref si,                 // pointer to STARTUPINFO structure
                                                        out procInfo            // receives information about new process
                                                        );
                        if (!result)
                        {
                            f.WriteLine(new Win32Exception(Marshal.GetLastWin32Error()).Message);
                        }
                        f.WriteLine("CreateProcessAsUser ");
                        // invalidate the handles
                        CloseHandle(hProcess);
                        CloseHandle(hPToken);
                        CloseHandle(hUserTokenDup);

                        return result; // return the result
                    }
                    catch (Exception e)
                    {
                        procInfo = new PROCESS_INFORMATION();
                        f.WriteLine(e.Message);
                        return false;
                    }

                }
            }

        }
    }
}
