using Gtk;
using MySql.Data.MySqlClient;
using System;
using System.Data;
using System.Xml;


public partial class MainWindow: Gtk.Window
{	


	public MainWindow (): base (Gtk.WindowType.Toplevel)
	{
		Build ();
		MySqlConnection mySqlConnection = new MySqlConnection (
			"DataSource=localhost;Database=dbprueba;User ID=root;Password=sistemas"
		);
		string query = "SELECT * FROM articulo";

		mySqlConnection.Open ();

		MySqlDataAdapter mySqlDataAdapter = new MySqlDataAdapter (query, mySqlConnection);

		
		DataSet dataSet = new DataSet();
		mySqlDataAdapter.Fill(dataSet);

		show (dataSet);

		DataTable dataTable = dataSet.Tables [0];
		DataRow dataRow = dataTable.Rows [0];
		dataRow ["nombre"] = DateTime.Now.ToString ();

		new MySqlCommandBuilder (mySqlDataAdapter);

		mySqlDataAdapter.RowUpdating += async delegate(object sender, MySqlRowUpdatingEventArgs e) {
			Console.WriteLine("e.Command.CommandText={0}", e.Command.CommandText);
		};

		mySqlDataAdapter.Update (dataSet);
	}

	private void show(DataSet dataSet){

		DataTable dataTable = dataSet.Tables [0];

		foreach (DataColumn dataColumn in dataTable.Columns)
			Console.WriteLine (dataColumn.ColumnName);

		foreach (DataRow dataRow in dataTable.Rows)
			foreach(DataColumn dataColumn in dataTable.Columns)
				Console.WriteLine("{0}={1}", dataColumn.ColumnName, dataRow[dataColumn]);
	}

	protected void OnDeleteEvent (object sender, DeleteEventArgs a)
	{
		Application.Quit ();
		a.RetVal = true;
	}
}
