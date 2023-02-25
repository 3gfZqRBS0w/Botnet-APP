using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Runtime.InteropServices;


namespace LegitimeAPP.OS {

    public static class Output {


        [DllImport("user32.dll")]

        static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);
        
       
        [DllImport("Kernel32")]        
        private static extern IntPtr GetConsoleWindow();
        
        const int SW_HIDE=0;
        const int SW_SHOW=5;

        public static void Hide() {
            IntPtr hwnd;
            hwnd=GetConsoleWindow();
            ShowWindow(hwnd,SW_HIDE);
        }

        public static void Show() {
            IntPtr hwnd ;
            hwnd=GetConsoleWindow() ;
            ShowWindow(hwnd,SW_SHOW);
        }
    }
}