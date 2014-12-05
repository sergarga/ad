package serpis.ad;

import java.sql.Connection;
import java.sql.DriverManager;
import java.sql.PreparedStatement;
import java.sql.ResultSet;
import java.sql.SQLException;
import java.sql.Statement;
import java.util.Scanner;

public class JArticulo {

	private static int opcion = -1;
	private static Scanner scanner = new Scanner(System.in);
	
	public static void main(String[] args) throws ClassNotFoundException, SQLException {
		
		Connection connection = DriverManager.getConnection(
				"jdbc:mysql://localhost/dbprueba?user=root&password=sistemas");
		Statement statement = connection.createStatement();
		ResultSet resultSet;  

		while(opcion!=0){
			System.out.println("");
			System.out.println("*** Gestion bases de datos ***");
			System.out.println("0.- Salir");
			System.out.println("1.- Nuevo");
			System.out.println("2.- Editar");
			System.out.println("3.- Eliminar");
			System.out.println("4.- Visualizar");
			System.out.println("*******************************\n");
			
			opcion = scanner.nextInt();
			
			switch(opcion){
			
				case 0:
					System.out.println("Cerrando gestor...");
					break;
					
				case 1:
					System.out.println("Introduce nombre:");
					String nombre=scanner.nextLine();
					scanner.nextLine();
					
					System.out.println("Introduce precio:");
					Long precio=scanner.nextLong();
					
					System.out.println("Introduce categoria:");
					int categoria=scanner.nextInt();
					
					//int update = statement.executeUpdate("insert into articulo values(?,?,?);
					//TODO
					
					
					break;
					
				case 2:
					break;
					
				case 3:
					break;
					
				case 4:
					resultSet = statement.executeQuery("SELECT * FROM articulo");
					while(resultSet.next()){
						System.out.printf(""
								+ "id=%2s	categoria=%s	precio=%4s	nombre=%s\n"
								, resultSet.getObject("id")
								, resultSet.getObject("categoria")								
								, resultSet.getObject("precio")
								, resultSet.getObject("nombre"));
					}
					resultSet.close();
					break;
					
				default:	
					
			}
			
			statement.close();
			connection.close();
			
		}

	}

}
