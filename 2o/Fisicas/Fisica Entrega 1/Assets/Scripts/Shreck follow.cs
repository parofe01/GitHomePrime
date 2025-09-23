using UnityEngine;
using UnityEngine.AI;

public class ShreckFollow : MonoBehaviour
{
    public Transform jugador;
    public float velocidad = 3.5f;

    private NavMeshAgent agente;

    void Start()
    {
        agente = GetComponent<NavMeshAgent>();
        agente.speed = velocidad;
    }

    void Update()
    {
        if (jugador != null)
        {
            agente.SetDestination(jugador.position);
        }
    }
}