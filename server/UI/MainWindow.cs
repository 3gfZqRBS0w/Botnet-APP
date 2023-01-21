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
        private List<Zombie> Zombies = new List<Zombie>(){
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING), 
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING), 
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING), 
            new Zombie(Connection.GetRandomIpAddress(), Shared.Action.ISWAITING),
        } ;
         
         enum Column
        {
            IP,
            ACTION,
            GIVINGORDER, 
        }





         private Statusbar _statusbar;
         private TreeView treeView;
         private VBox ZombiePage ; 



        
        public MainWindow() : base("BotnetApp - Main") {

            SetDefaultSize(500, 500);
            SetPosition(WindowPosition.Center);
            BorderWidth = 8;
            DeleteEvent += delegate { Application.Quit(); };
            Resizable = false;



            /**
            * PAGE PERMETTANT LE LANCEMENT D'UNE ATTAQUE DDOS GRÂCE AUX PC INFECTÉ 
            * LA CIBLE DOIT ÊTRE CONFIGURER SUR LA PAGE 
            **/

            ZombiePage = new VBox() ; 

            ScrolledWindow sw = new ScrolledWindow();
            sw.ShadowType = ShadowType.EtchedIn;
            sw.SetPolicy(PolicyType.Automatic, PolicyType.Automatic);

            Button AttackButton = new Button("Launch Attack") ; 

            ZombiePage.PackStart(sw,true , true, 0) ; 
            ZombiePage.PackEnd(AttackButton, false, true, 0) ; 



            treeView = new TreeView();
            CellRenderer rendererText = new CellRendererText ();

            treeView.AppendColumn("ORDER", new CellRendererToggle() {Active = true}) ; 
            treeView.AppendColumn("IP", new CellRendererText (), "text", 1);
            treeView.AppendColumn("ACTION", new CellRendererText (), "text", 2);

            ListStore ListStore = new ListStore (typeof(CheckButton),typeof (string), typeof (string));


            treeView.Model = ListStore ;

            foreach ( Zombie zombie in Zombies) {
                ListStore.AppendValues(new CheckButton(),zombie.IP, zombie.Action ) ; 
            }


            sw.Add(treeView);



            _mainBox = new VBox() ;

            notebook = new Notebook() ;
            notebook.ShowTabs = true ;


            notebook.AppendPage(new Frame(), new Label("Dashboard")) ; 
            notebook.AppendPage(ZombiePage, new Label("Zombie"));
            notebook.AppendPage(new Frame() , new Label("History"));
            notebook.AppendPage(new Frame() , new Label("Setting"));
            notebook.AppendPage(new Frame() , new Label("About"));
            
            
            _mainBox.PackEnd(notebook, true, true, 0) ;


            Add(_mainBox) ;


            ShowAll() ; 
        }

    }
}