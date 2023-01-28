using System;
using Gtk;
using BotnetAPP.UI ;
using BotnetAPP.Data ; 

namespace BotnetAPP
{
    class Program
    {
        [STAThread]
        public static void Main(string[] args)
        {
            Application.Init();


            new Connection() ; 

            var app = new Application("org.server.server", GLib.ApplicationFlags.None);
            app.Register(GLib.Cancellable.Current);

            var win = new MainWindow();
            app.AddWindow(win);

            win.Show();
            Application.Run();
        }
    }
}
