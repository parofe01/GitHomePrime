using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class instanciarSuelo : MonoBehaviour {

	// Variables
	public GameObject[] tipoSuelos;
	public int numeroTipoSuelos;
	public float multiplicadorDesplazamientoY, velocidadSuelo;

	// Use this for initialization
	void Start () 
	{
		for (int i = 0; i < numeroTipoSuelos; i++)
		{
			// En Random.Range, cuando se trabaja en int, el maximo se excluye, en float se incluye
			int tipoSuelo = Random.Range(0, numeroTipoSuelos);

			// Definimos las coordenas de la generación del terreno
			float desplazamientoY = multiplicadorDesplazamientoY * i;
			Vector3 posicion = new Vector3(transform.position.x, transform.position.y + desplazamientoY, transform.position.z);

			// Quaternion.Identity es para que no altere la rotación
			GameObject parteSuelo = Instantiate(tipoSuelos[tipoSuelo], posicion, Quaternion.identity);

			// En la jerarquia de objetos, cada suelo que se genere, saldrá como hijo de controlador de suelos, dentro de ese objeto
			// en lugar de en la jerarquia general, por lo que será más facil controlar los objetos que genere el padre puesto que
			// se quedan dentro de el
			parteSuelo.transform.SetParent(transform);
		}
    }
	
	// Update is called once per frame
	void Update () 
	{
		transform.Translate(Vector3.down * Time.deltaTime * velocidadSuelo);

		Transform posicionControlador = gameObject.transform.GetChild(0);

		if (posicionControlador.position.y <= -10)
		{
			Destroy(gameObject.transform.GetChild(1).gameObject);
			int tipoSuelo = Random.Range(0, numeroTipoSuelos);
			//Vector3 posicion = new Vector3(transform.position.x, transform.position.y + multiplicadorDesplazamientoY * 2, transform.position.z);
			Vector3 posicion = new Vector3(0, 20, 0);
			GameObject parteSuelo = Instantiate(tipoSuelos[tipoSuelo], posicion, Quaternion.identity) as GameObject;
			parteSuelo .transform.SetParent(transform);

			posicionControlador.position = new Vector3(0f, 0f, 0f);
        }
    }
}
