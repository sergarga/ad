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
			entryP.Text = dataReader ["precio"].ToString ();

			dataReader.Close ();

			RellenaComboCat ();
			comboboxC.Active = 0;
		}


		protected void OnSaveActionActivated (object sender, EventArgs e)
		{
			IDbCommand dbCommand = App.Instance.DbConnection.CreateCommand ();

			dbCommand.CommandText = String.Format ("update articulo set nombre=@nombre, categoria=@categoria, precio=@precio where id ={0}", id);
			dbCommand.AddParameter("nombre", entryNombre.Text);
			dbCommand.AddParameter("categoria", comboboxC.ActiveText);
			dbCommand.AddParameter("precio", decimal.Parse(entryP.Text));

			dbCommand.ExecuteNonQuery ();

			Destroy ();
		}

		protected void RellenaComboCat (){
			IDbCommand dbCommand =	App.Instance.DbConnection.CreateCommand ();
			dbCommand.CommandText = "select id from categoria";
			IDataReader dataReader = dbCommand.ExecuteReader ();
			while (dataReader.Read ()) {
				comboboxC.AppendText (dataReader ["id"].ToString ());
			}
			dataReader.Close ();

		}
	}
}

