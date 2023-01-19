using System ;
using Gtk ;
using System.Threading;
using System.Collections.Generic ;

using BotnetAPP.Shared ; 

namespace BotnetAPP.UI {
    public class MainWindow : Window {


/*
        private Toolbar _toolbar ;
        private ToolButton _attackButton ;
        private ToolButton _historyButton ;
        private ToolButton _aboutButton ;
        private ToolButton _settingButton ; 
        */

        private Notebook notebook ;
        private VBox _mainBox ;
        private List<Zombie> _connectedZombie = new List<Zombie>(){} ;



          private ListStore _store;
        private Statusbar _statusbar;
         private TreeView treeView;





        
        public MainWindow() : base("BotnetApp - Main") {

            SetDefaultSize(500, 500);
            SetPosition(WindowPosition.Center);
            BorderWidth = 8;
            DeleteEvent += delegate { Application.Quit(); };
            Resizable = false;

            _mainBox = new VBox() ;

            notebook = new Notebook() ;
            notebook.ShowTabs = true ;

            notebook.AppendPage(new Frame() {Label = "test\ntest\ntest"}, new Gtk.Label("Zombie"));
            notebook.AppendPage(new Frame() , new Gtk.Label("History"));
            notebook.AppendPage(new Frame() , new Gtk.Label("Setting"));
            notebook.AppendPage(new Frame() , new Gtk.Label("About"));



_mainBox.PackEnd(notebook, true, true, 0) ;


            Add(_mainBox) ; 


            ShowAll() ; 
        }

    }
}