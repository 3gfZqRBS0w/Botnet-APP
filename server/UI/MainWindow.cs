using System ;
using Gtk ;
using System.Threading;
using System.Collections.Generic ;
using BotnetAPP.Network ; 

using BotnetAPP.Shared ; 

namespace BotnetAPP.UI {
    public class MainWindow : Window {

        private Notebook notebook ;
        private VBox _mainBox ;

        private Zombies zombies = new Zombies() ;


         enum Column
        {
            IP,
            ACTION,
            GIVINGORDER, 
        }





         private Statusbar _statusbar;
         private TreeView treeView;
         private VBox ZombiePage ;
         private VBox DashboardPage ; 



        
        public MainWindow() : base("BotnetApp - Main") {

            SetDefaultSize(500, 500);
            SetPosition(WindowPosition.Center);
            //BorderWidth = 20;
            DeleteEvent += delegate { Application.Quit(); };
            Resizable = false;


            /**
            * Temporaire 
            * permet de remplir le tableau avec de faux zombies
            **/

            for (int i = 1; i < 10; i++) {
                zombies.AddZombie(new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING)) ;
            } 

            /**
            * PAGE PERMETTANT D'AFFICHER LES INFORMATIONS GÉNÉRALE DE L'ETAT ACTUEL
            **/
            DashboardPage = new VBox() ;

            DashboardPage.PackStart(new Label("Nombre de machine infecté"), true, false, 2 ) ; 




            /**
            * PAGE PERMETTANT LE LANCEMENT D'UNE ATTAQUE DDOS GRÂCE AUX PC INFECTÉ 
            * LA CIBLE DOIT ÊTRE CONFIGURER SUR LA PAGE 
            **/

            ZombiePage = new VBox() ; 

            ScrolledWindow sw = new ScrolledWindow();
            sw.ShadowType = ShadowType.EtchedIn;
            sw.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            Button AttackButton = new Button("Launch Attack") ; 

            treeView = new TreeView();
            CellRenderer rendererText = new CellRendererText ();

            treeView.AppendColumn("ORDER", new CellRendererToggle() {Active = true}) ; 
            treeView.AppendColumn("IP", new CellRendererText (), "text", 1);
            treeView.AppendColumn("ACTION", new CellRendererText (), "text", 2);

            ListStore ListStore = new ListStore (typeof(CheckButton),typeof (string), typeof (string));


            treeView.Model = ListStore ;

            foreach ( Zombie zombie in zombies.ListOfZombie) {
                ListStore.AppendValues(new CheckButton(),zombie.IP, zombie.Action ) ; 
            }

            sw.Add(treeView);

            ZombiePage.PackStart(sw,true , true, 0) ; 
            ZombiePage.PackEnd(AttackButton, false, true, 0) ;



            /**
                PAGE DE PARAMÈTRE 
                PERMET DE CONFIGURER L'ATTAQUE
            **/

            VBox SettingPage = new VBox() ;

            SettingPage.BorderWidth = 20;

            Label TargetIPLibelle = new Label("IP Visé") ;
            Entry TargetIPEntry = new Entry() {PlaceholderText = "IP Visé"} ; 

            Label DurationAttackTitle = new Label("Durée de l'attaque") ; 
            SpinButton DurationAttackSetting  = new SpinButton(0,1440,5) ;

            Label LabelPortLibelle = new Label("Port Visé") ;
            SpinButton PortAttackSetting  = new SpinButton(1024,49151,1) ;


            Button BoutonValider = new Button("Valider") ; 


            SettingPage.PackStart(DurationAttackTitle, false, false, 10) ; 
            SettingPage.PackStart(DurationAttackSetting, false, true , 10) ; 
            

            SettingPage.PackStart(TargetIPLibelle, false, false, 10) ;
            SettingPage.PackStart(TargetIPEntry, false, false,10) ;

            SettingPage.PackStart(LabelPortLibelle, false, false, 10) ;
            SettingPage.PackStart(PortAttackSetting, false, false, 10) ;  
            

            SettingPage.PackEnd(BoutonValider, false, false, 0) ; 



            /**
            PAGE PRINCIPALE
            **/



            _mainBox = new VBox() ;

            notebook = new Notebook() ;
            notebook.ShowTabs = true ;

            notebook.AppendPage(DashboardPage, new Label("Dashboard")) ; 
            notebook.AppendPage(ZombiePage, new Label("Zombie"));
            notebook.AppendPage(new Frame() , new Label("History"));
            notebook.AppendPage(SettingPage , new Label("Setting"));
            notebook.AppendPage(new Frame() , new Label("About"));
            
            
            _mainBox.PackEnd(notebook, true, true, 0) ;


            Add(_mainBox) ;


            ShowAll() ; 
        }

    }
}