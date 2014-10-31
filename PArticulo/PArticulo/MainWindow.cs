using System;
using Gtk;
using System.Data;

using SerpisAd;
using PArticulo;

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

		comboboxA1.AppendText ("Empieza por");
		comboboxA.AppendText ("nombre");
		comboboxA.AppendText ("id");


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

		comboboxC1.AppendText ("Empieza por");
		comboboxC.AppendText ("nombre");
		comboboxC.AppendText ("id");

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

	//ACCION AÑADIR 

	protected void OnAddActionCActivated (object sender, EventArgs e)
	{
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		string insertSql = "insert into categoria (nombre) values ('{0}')";
		insertSql= string.Format(insertSql, "Nuevo " + DateTime.Now);

		dbCommand.CommandText = insertSql;

		dbCommand.ExecuteNonQuery ();
	}

	protected void OnAddActionAActivated (object sender, EventArgs e)
	{
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		string insertSql = "insert into articulo (nombre) values ('{0}')";
		insertSql= string.Format(insertSql, "Nuevo " + DateTime.Now);

		dbCommand.CommandText = insertSql;

		dbCommand.ExecuteNonQuery ();
	}

	//ACCION BORRAR

	protected void OnDeleteActionAActivated (object sender, EventArgs e)
	{
		TreeIter treeIter;
		treeViewA.Selection.GetSelected (out treeIter);
		object id=listStoreA.GetValue (treeIter, 0);

		string pregunta = "¿Quieres eliminar el registro '{0}'?";
		pregunta = string.Format(pregunta, id); 


		if (Confirm(pregunta)) {

			IDbCommand mySqlCommand = dbConnection.CreateCommand ();
			string deleteSql = "delete from articulo where id = '{0}'";
			deleteSql= string.Format(deleteSql, id);
			mySqlCommand.CommandText = deleteSql;
			mySqlCommand.ExecuteNonQuery ();

		} 
	}

	protected void OnDeleteActionCActivated (object sender, EventArgs e)
	{
		TreeIter treeIter;
		treeViewC.Selection.GetSelected (out treeIter);
		object id=listStoreC.GetValue (treeIter, 0);

		string pregunta = "¿Quieres eliminar el registro '{0}'?";
		pregunta = string.Format(pregunta, id); 


		if (Confirm(pregunta)) {

			IDbCommand mySqlCommand = dbConnection.CreateCommand ();
			string deleteSql = "delete from categoria where id = '{0}'";
			deleteSql= string.Format(deleteSql, id);
			mySqlCommand.CommandText = deleteSql;
			mySqlCommand.ExecuteNonQuery ();

		} 
	}

	public bool Confirm(string text){
		MessageDialog messageDialog = new MessageDialog (
			this,
			DialogFlags.Modal,
			MessageType.Question,
			ButtonsType.YesNo,
			text
			);
		messageDialog.Title = Title;
		ResponseType result = (ResponseType)messageDialog.Run ();
		messageDialog.Destroy ();
		return result == ResponseType.Yes;
	}

	//ACCION EDITAR

	protected void OnEditActionCActivated (object sender, EventArgs e)
	{
		TreeIter treeIter;
		treeViewC.Selection.GetSelected (out treeIter);
		object id=listStoreC.GetValue (treeIter, 0);
		new ArticuloView (id, 1);
	}


	protected void OnEditActionAActivated (object sender, EventArgs e)
	{
		TreeIter treeIter;
		treeViewA.Selection.GetSelected (out treeIter);
		object id=listStoreA.GetValue (treeIter, 0);
		new ArticuloView (id, 0);
	}

	//ACCION BUSCAR

	protected void OnButtonAClicked (object sender, EventArgs e)
	{
		listStoreA.Clear ();

		String selected = comboboxA.ActiveText;
		String search = entryA.Text;

		IDbCommand dbCommand = dbConnection.CreateCommand ();
		String sql = "select * from articulo where {0} like '{1}%'";
		sql = string.Format (sql, selected, search);
		dbCommand.CommandText = sql;

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

	protected void OnButtonCClicked (object sender, EventArgs e)
	{
		listStoreC.Clear ();

		String selected = comboboxC.ActiveText;
		String search = entryC.Text;

		IDbCommand dbCommand = dbConnection.CreateCommand ();
		String sql = "select * from categoria where {0} like '{1}%'";
		sql = string.Format (sql, selected, search);
		dbCommand.CommandText = sql;

		IDataReader dataReader = dbCommand.ExecuteReader ();
		while (dataReader.Read()) {
			object id = dataReader ["id"].ToString();
			object nombre = dataReader ["nombre"];
			listStoreC.AppendValues (id, nombre);
		}
		dataReader.Close ();
	}
}
