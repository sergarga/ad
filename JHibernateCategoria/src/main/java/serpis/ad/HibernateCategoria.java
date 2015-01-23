package serpis.ad;

import java.text.SimpleDateFormat;
import java.util.Date;
import java.util.List;
import java.util.Scanner;

import javax.persistence.EntityManager;
import javax.persistence.EntityManagerFactory;
import javax.persistence.Persistence;

public class HibernateCategoria {
	
	private static EntityManagerFactory entityManagerFactory;
	
	public static void main (String [] args){
		
		int opcion=-1;
		
		
		Scanner sc = new Scanner(System.in);
		entityManagerFactory = Persistence.createEntityManagerFactory("serpis.ad.jpa.mysql");
		
		while(opcion!=0){
			System.out.println("");
			System.out.println("*** Gestion bases de datos con Hibernate ***");
			System.out.println("0.- Salir");
			System.out.println("1.- Nuevo");
			System.out.println("2.- Editar");
			System.out.println("3.- Eliminar");
			System.out.println("4.- Visualizar");
			System.out.println("*******************************\n");
			
			opcion = sc.nextInt();
			sc.nextLine();
			
			switch(opcion){
			
				case 0://SALIR
					System.out.println("Cerrando gestor...");
					break;
					
				case 1://INSERTAR
					System.out.println("Introduce nombre:");
					String nombre=sc.nextLine();
					
					persistNuevasCategorias(nombre);			
			
					break;
					
				case 2://EDITAR
					System.out.println("Introduce nombre:");
					String edit=sc.nextLine();
					
					System.out.println("Introduce id de la categoria a editar:");
					Long id=sc.nextLong();
					sc.nextLine();
					
					editarCategoria(id, edit);
					
					break;
					
				case 3://ELIMINAR
					System.out.println("Introduce id de la categoria a eliminar:");
					Long ide=sc.nextLong();
					sc.nextLine();
					
					removeCategoria(ide);
					
					break;
					
				case 4:
					
					showCategorias();
					
					break;
					
				default:	
					
			}
			
			 //Espero 3 segundos para visualizar operacion
			
			try {
				Thread.sleep(3000);
			} catch (InterruptedException e) {
				// TODO Auto-generated catch block
				e.printStackTrace();
			}
		}
		entityManagerFactory.close();
	}
	
	//INSERT
	
	public static void persistNuevasCategorias(String nombre){
		
		EntityManager entityManager = entityManagerFactory.createEntityManager();
		entityManager.getTransaction().begin();
		
		Categoria categoria = new Categoria();
		categoria.setNombre(nombre);
		
		entityManager.persist(categoria);
		
		entityManager.getTransaction().commit();
		entityManager.close();
		
	}
	
	//LISTAR
	
	public static void showCategorias(){
		
		EntityManager entityManager = entityManagerFactory.createEntityManager();
		entityManager.getTransaction().begin();
		
		List<Categoria> categorias = 
				entityManager.createQuery("from Categoria", Categoria.class).getResultList();
		
		for (Categoria categoria: categorias){
			System.out.printf("id=%d nombre=%s\n", categoria.getId(), categoria.getNombre());
		}
		
		entityManager.getTransaction().commit();
		entityManager.close();
		
	}
	
	//ENCONTRAR Y BORRAR CATEGORIA
	
	/*public static Categoria findCategoria(Long id){
		EntityManager entityManager = entityManagerFactory.createEntityManager();
		Categoria categoria = entityManager.find(Categoria.class, id);
		return categoria;
	}*/
	
	public static void removeCategoria(Long id){
		EntityManager entityManager = entityManagerFactory.createEntityManager();
		entityManager.getTransaction().begin();
		
		entityManager.remove(entityManager.find(Categoria.class, id));
		
		entityManager.getTransaction().commit();
		entityManager.close();
	}
	
	//EDITAR
	
	public static void editarCategoria(Long id, String nombre){
		EntityManager entityManager = entityManagerFactory.createEntityManager();
		entityManager.getTransaction().begin();
		
		Categoria categoria = entityManager.find(Categoria.class, id);
		categoria.setNombre(nombre);
		entityManager.merge(categoria);
		
		entityManager.getTransaction().commit();
		entityManager.close();
	}
	

	
}
