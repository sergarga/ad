using MySql.Data.MySqlClient;
using System;
using Gtk;

namespace PCategoria
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			App.Instance.DbConnection = new MySqlConnection (
				"DataSource=localhost;" +
				"Database=dbprueba;" +
				"User ID=root;" +
				"Password=sistemas"
				);
			App.Instance.DbConnection.Open ();

			Application.Init ();
			MainWindow win = new MainWindow ();
			win.Show ();
			Application.Run ();
		}
	}
}
