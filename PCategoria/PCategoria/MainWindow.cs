using System;
using Gtk;
using System.Data;
using PCategoria;

public partial class MainWindow: Gtk.Window
{
	private IDbConnection dbConnection;
	private ListStore listStore;
	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();

		deleteAction.Sensitive = false;
		editAction.Sensitive = false;

		dbConnection = App.Instance.DbConnection;

		treeView.AppendColumn ("id", new CellRendererText (), "text", 0);
		treeView.AppendColumn ("nombre", new CellRendererText (), "text", 1);
		listStore = new ListStore (typeof(string), typeof(string));
		treeView.Model = listStore;

		fillListStore ();


		//treeView.Selection.Changed += selectionChanged;
		treeView.Selection.Changed += delegate {
			bool hasSelected = treeView.Selection.CountSelectedRows() > 0;
			deleteAction.Sensitive = hasSelected;
			editAction.Sensitive = hasSelected;
		};

	}




	private void fillListStore(){
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		dbCommand.CommandText = "select * from categoria";

		IDataReader dataReader = dbCommand.ExecuteReader ();
		while (dataReader.Read()) {
			object id = dataReader ["id"].ToString();
			object nombre = dataReader ["nombre"];
			listStore.AppendValues (id, nombre);
		}
		dataReader.Close ();
	}


	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		dbConnection.Close ();
		Application.Quit ();
		a.RetVal = true;
	}

	protected void OnAddActionActivated (object sender, EventArgs e)
	{
		IDbCommand dbCommand = dbConnection.CreateCommand ();
		string insertSql = "insert into categoria (nombre) values ('{0}')";
		insertSql= string.Format(insertSql, "Nuevo " + DateTime.Now);

		dbCommand.CommandText = insertSql;

		dbCommand.ExecuteNonQuery ();

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

	protected void OnEditActionActivated (object sender, EventArgs e)
	{
		TreeIter treeIter;
		treeView.Selection.GetSelected (out treeIter);
		object id=listStore.GetValue (treeIter, 0);
		new CategoriaView (id);

	}

}
