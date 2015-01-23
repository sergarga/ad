package serpis.ad;

import javax.persistence.Entity;
import javax.persistence.GeneratedValue;
import javax.persistence.Id;

import org.hibernate.annotations.GenericGenerator;

@Entity
public class Categoria {
	 
	private Long id;
	private String nombre;
	
	//CONSTRUCTORES
	
	public Categoria(){}
	
	public Categoria(Long id, String nombre){
		this.id = id;
		this.nombre=nombre;
	}
	
	//GETTERS Y SETTERS
	
		//ID
	
	@Id
	@GeneratedValue(generator="increment")
	@GenericGenerator(name="increment", strategy = "increment")
    public Long getId() {
		return id;
    }
	
	private void setId(Long id) {
		this.id = id;
    }
	
		//NOMBRE
	
	public String getNombre() {
		return nombre;
    }

    public void setNombre(String nombre) {
		this.nombre = nombre;
    }
    
	
}
