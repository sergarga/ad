using System;
using Gtk;
using MySql.Data.MySqlClient;

public partial class MainWindow: Gtk.Window
{
	private MySqlConnection mySqlConnection;
	private ListStore listStore;
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		deleteAction.Sensitive = false;

		mySqlConnection = new MySqlConnection (
			"DataSource=localhost;" +
			"Database=dbprueba;" +
			"User ID=root;" +
			"Password=sistemas"
		);

		mySqlConnection.Open ();

		treeView.AppendColumn ("id", new CellRendererText (), "text", 0);
		treeView.AppendColumn ("nombre", new CellRendererText (), "text", 1);
		listStore = new ListStore (typeof(string), typeof(string));
		treeView.Model = listStore;

		fillListStore ();



		//treeView.Selection.Changed += selectionChanged;
		treeView.Selection.Changed += delegate {
			deleteAction.Sensitive = treeView.Selection.CountSelectedRows() > 0;
		};

	}


	private void fillListStore(){
		MySqlCommand mySqlCommand = mySqlConnection.CreateCommand ();
		mySqlCommand.CommandText = "select * from categoria";

		MySqlDataReader mySqlDataReader = mySqlCommand.ExecuteReader ();
		while (mySqlDataReader.Read()) {
			object id = mySqlDataReader ["id"].ToString();
			object nombre = mySqlDataReader ["nombre"];
			listStore.AppendValues (id, nombre);
		}
		mySqlDataReader.Close ();
	}


	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		mySqlConnection.Close ();
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnAddActionActivated (object sender, EventArgs e)
	{
		MySqlCommand mySqlCommand = mySqlConnection.CreateCommand ();
		string insertSql = "insert into categoria (nombre) values ('{0}')";
		insertSql= string.Format(insertSql, "Nuevo " + DateTime.Now);

		mySqlCommand.CommandText = insertSql;

		mySqlCommand.ExecuteNonQuery ();

		listStore.Clear();
		fillListStore ();
	}

	protected void OnRefreshActionActivated (object sender, EventArgs e)
	{
		listStore.Clear();
		fillListStore ();
	}
	
	protected void OnDeleteActionActivated (object sender, EventArgs e)
	{
		TreeIter treeIter;
		treeView.Selection.GetSelected (out treeIter);
		object id=listStore.GetValue (treeIter, 0);

		string pregunta = "¿Quieres eliminar el registro '{0}'?";
		pregunta = string.Format(pregunta, id); 


		if (Confirm(pregunta)) {

			MySqlCommand mySqlCommand = mySqlConnection.CreateCommand ();
			string deleteSql = "delete from categoria where id = '{0}'";
			deleteSql= string.Format(deleteSql, id);
			mySqlCommand.CommandText = deleteSql;
			mySqlCommand.ExecuteNonQuery ();

			listStore.Clear();
			fillListStore ();
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

}
