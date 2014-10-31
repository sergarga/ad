using System;
using System.Data;
using SerpisAd;

using PArticulo;

namespace PArticulo
{

	public partial class ArticuloView : Gtk.Window
	{
		private object id;
		private int opcion;

		public ArticuloView () : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}

		public ArticuloView(object id, int opcion) : this(){
			this.id=id;
			this.opcion = opcion;
			IDbCommand dbCommand =	App.Instance.DbConnection.CreateCommand ();

			if (opcion == 1) {
				dbCommand.CommandText = String.Format ("select * from categoria where id = {0}", id);
			} else {
				dbCommand.CommandText = String.Format ("select * from articulo where id = {0}", id);
			}

			IDataReader dataReader = dbCommand.ExecuteReader ();
			dataReader.Read ();

			entryNombre.Text = dataReader ["nombre"].ToString ();

			dataReader.Close ();
		}


		protected void OnSaveActionActivated (object sender, EventArgs e)
		{
			IDbCommand dbCommand =
				App.Instance.DbConnection.CreateCommand ();

			if (opcion == 1) {
				dbCommand.CommandText = String.Format ("update categoria set nombre=@nombre where id ={0}", id);
			} else {
				dbCommand.CommandText = String.Format ("update articulo set nombre=@nombre where id ={0}", id);
			}
			//DbCommandExtensions.AddParameter(dbCommand, "nombre", entryNombre.Text);
			dbCommand.AddParameter("nombre", entryNombre.Text);


			dbCommand.ExecuteNonQuery ();

			Destroy ();
		}

	}
}

