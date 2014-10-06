using System;
using MySql.Data.MySqlClient;

namespace PHolaMySql
{
	class MainClass
	{
		public static void Main (string[] args)
		{
			string connectionString =
				"Datasource=localhost;" +
				"Database=dbprueba;" +
				"User ID=root;" +
				"Password=sistemas";

			MySqlConnection mySqlConnection = new MySqlConnection (connectionString); 
			mySqlConnection.Open();

			Console.WriteLine ("Hola MySql");

			MySqlCommand mySqlCommand = mySqlConnection.CreateCommand();
			/*mySqlCommand.CommandText =
				string.Format("insert into categoria (nombre) values ('{0}')", DateTime.Now);
				//string.Format("insert into categoria (nombre) values ('"+DateTime.Now+"')");
			mySqlCommand.ExecuteNonQuery();*/

			mySqlCommand.CommandText = "select * from categoria";
			MySqlDataReader query = mySqlCommand.ExecuteReader ();
		
			Console.WriteLine ("FieldCount = {0}", query.FieldCount);
			for (int i=0; i<query.FieldCount; i++) {
				Console.WriteLine ("Column {0} = {1}",i, query.GetName(i));
			}

			while (query.Read()) {
				/*for (int i=0; i<query.FieldCount; i++) {
					Console.Write(query.GetValue (i)+"  ");
				}*/
				object id = query ["id"];
				object nombre = query ["nombre"];
				Console.WriteLine ("id = {0} nombre = {1}", id, nombre);
			}

			query.Close ();

			mySqlConnection.Close();
		}
	}
}
