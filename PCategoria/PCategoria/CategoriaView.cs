using System.Data;
using System;
using SerpisAd;

namespace PCategoria
{
	public partial class CategoriaView : Gtk.Window
	{
		private object id;
		public CategoriaView () : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}

		public CategoriaView(object id) : this(){
			this.id=id;
			IDbCommand dbCommand =
				App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = String.Format (
				"select * from categoria where id = {0}", id);

			IDataReader dataReader = dbCommand.ExecuteReader ();
			dataReader.Read ();

			entryNombre.Text = dataReader ["nombre"].ToString ();

			dataReader.Close ();
		}

		protected void OnSaveActionActivated (object sender, EventArgs e)
		{
			IDbCommand dbCommand =
				App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = String.Format (
				"update categoria set nombre=@nombre where id ={0}", id);

			DbCommandExtensions.AddParameter(dbCommand, "nombre", entryNombre.Text);

			dbCommand.ExecuteNonQuery ();

			Destroy ();
		}



	}
}

