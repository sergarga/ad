using System;
using System.Data;
using SerpisAd;

using PArticulo;

namespace PArticulo
{

	public partial class ArticuloView : Gtk.Window
	{
		private object id;


		public ArticuloView () : base(Gtk.WindowType.Toplevel)
		{
			this.Build ();
		}

		public ArticuloView(object id) : this(){
			this.id=id;

			IDbCommand dbCommand =	App.Instance.DbConnection.CreateCommand ();


			dbCommand.CommandText = String.Format ("select * from articulo where id = {0}", id);


			IDataReader dataReader = dbCommand.ExecuteReader ();
			dataReader.Read ();


			entryNombre.Text = dataReader ["nombre"].ToString ();
			entryCat.Text = dataReader ["categoria"].ToString ();
			entryP.Text = dataReader ["precio"].ToString ();

			dataReader.Close ();
		}


		protected void OnSaveActionActivated (object sender, EventArgs e)
		{
			IDbCommand dbCommand = App.Instance.DbConnection.CreateCommand ();

			dbCommand.CommandText = String.Format ("update articulo set nombre=@nombre, categoria=@categoria, precio=@precio where id ={0}", id);
			dbCommand.AddParameter("nombre", entryNombre.Text);
			dbCommand.AddParameter("categoria", entryCat.Text);
			dbCommand.AddParameter("precio", entryP.Text);

			dbCommand.ExecuteNonQuery ();

			Destroy ();
		}

	}
}

