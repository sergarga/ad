using System;
using Gtk;

using PNotebook;

public partial class MainWindow: Gtk.Window
{	
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		ArticuloAction.Activated += delegate {
			addPage (new MyTreeView(), "Articulo");
		};

		CategoriaAction.Activated += delegate {
			addPage (new MyTreeView(), "Categoria");
		};

		notebook.SwitchPage += delegate {
			onPageChanged();
		};

		//treeView.AppendColumn ("id", new CellRendererText (), "text", 0);
		//treeView.AppendColumn ("nombre", new CellRendererText (), "text", 1);
		//treeView.Model = new ListStore (typeof(long), typeof(string));
	}

	private void onPageChanged(){
		Console.WriteLine("button.onPageChanged notebook.CurrentPage = {0}", notebook.CurrentPage);	
	}

	private void addPage(Widget widget, string label){
		HBox hBox = new HBox ();
		hBox.Add(new Label (label));
		Button button = new Button (new Image(Stock.Cancel, IconSize.Button));
		hBox.Add (button);
		hBox.ShowAll ();
		notebook.CurrentPage = notebook.AppendPage (widget, hBox);


		button.Clicked += delegate {
			widget.Destroy ();
			if(notebook.CurrentPage == -1){
				onPageChanged();
			}
		};
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
