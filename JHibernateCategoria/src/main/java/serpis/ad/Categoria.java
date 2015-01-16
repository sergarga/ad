package serpis.ad;

import javax.persistence.Entity;
import javax.persistence.Id;

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

    private void setNombre(String nombre) {
		this.nombre = nombre;
    }
	
}
