using System;
using Gtk;
using System.Data;

using SerpisAd;

public partial class MainWindow: Gtk.Window
{	

	private IDbConnection dbConnection;
	private ListStore listStoreA;
	private ListStore listStoreC;


	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		dbConnection = App.Instance.DbConnection;

		//PESTAÑA ARTICULO
		deleteActionA.Sensitive = false;
		editActionA.Sensitive = false;

		treeViewA.Selection.Changed += delegate {
			bool hasSelected = treeViewA.Selection.CountSelectedRows() > 0;
			deleteActionA.Sensitive = hasSelected;
			editActionA.Sensitive = hasSelected;
		};

		treeViewA.AppendColumn ("id", new CellRendererText (), "text", 0);
		treeViewA.AppendColumn ("nombre", new CellRendererText (), "text", 1);
		treeViewA.AppendColumn ("categoria", new CellRendererText (), "text", 2);
		treeViewA.AppendColumn ("precio", new CellRendererText (), "text", 3);
		listStoreA = new ListStore (typeof(string), typeof(string), typeof(string), typeof(string));
		treeViewA.Model = listStoreA;

		fillListStoreA ();



		//PESTAÑA CATEGORIA
		deleteActionC.Sensitive = false;
		editActionC.Sensitive = false;

		treeViewC.Selection.Changed += delegate {
			bool hasSelected = treeViewC.Selection.CountSelectedRows() > 0;
			deleteActionC.Sensitive = hasSelected;
			editActionC.Sensitive = hasSelected;
		};

		treeViewC.AppendColumn ("id", new CellRendererText (), "text", 0);
		treeViewC.AppendColumn ("nombre", new CellRendererText (), "text", 1);
		listStoreC = new ListStore (typeof(string), typeof(string));
		treeViewC.Model = listStoreC;

		fillListStoreC ();

	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		dbConnection.Close ();
		Application.Quit ();
		a.RetVal = true;
	}

	//LLENAR TREEVIEWS

	private void fillListStoreA(){
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = "select * from articulo";

		IDataReader dataReader = dbCommand.ExecuteReader ();
		while (dataReader.Read()) {
			object id = dataReader ["id"].ToString();
			object nombre = dataReader ["nombre"];
			object categoria = dataReader ["categoria"].ToString();
			object precio = dataReader ["precio"].ToString ();
			listStoreA.AppendValues (id, nombre, categoria, precio);
		}
		dataReader.Close ();
	}

	private void fillListStoreC(){
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = "select * from categoria";

		IDataReader dataReader = dbCommand.ExecuteReader ();
		while (dataReader.Read()) {
			object id = dataReader ["id"].ToString();
			object nombre = dataReader ["nombre"];
			listStoreC.AppendValues (id, nombre);
		}
		dataReader.Close ();
	}
	
	//ACCION ACTUALIZAR TREEVIEWS

	protected void OnRefreshActionAActivated (object sender, EventArgs e)
	{
		listStoreA.Clear();
		fillListStoreA ();
	}

	protected void OnRefreshActionCActivated (object sender, EventArgs e)
	{
		listStoreC.Clear();
		fillListStoreC ();
	}

}
