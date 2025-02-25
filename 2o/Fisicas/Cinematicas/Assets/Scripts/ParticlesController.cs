using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class ParticlesController : MonoBehaviour
{

    public int maxParticles;
    public float iniSpeed;
    public float iniAngulo;
    public float maxDuracion;
    public float gravedad;

    public GameObject particle;
    private List<GameObject> particles = new List<GameObject>();

    public bool randomAngle = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            CreateParticlesExplotion();
        }

        for (int i = 0; i < particles.Count; i++)
        {
            Vector3 pos = UpdateParticlePosition(particles[i].GetComponent<Particles>(), Time.deltaTime);

            if (particles[i].GetComponent<Particles>().time >= particles[i].GetComponent<Particles>().life)
            {
                GameObject toDestroy = particles[i];
                particles.Remove(toDestroy);
                Destroy(toDestroy);
            }
            else
            {
                particles[i].transform.position = pos;
            }
        }
    }

    private void CreateParticlesExplotion()
    {
        for (int i = 0; i < maxParticles; i++)
        {
            GameObject myParticle = Instantiate(particle);
            Particles stats = myParticle.GetComponent<Particles>();
            // position
            myParticle.transform.position = Vector3.zero;
            // vel ini
            stats.v0 = Random.Range(iniSpeed / 2, iniSpeed);
            // angle
            if (randomAngle == true)
            {
                stats.angle = Random.Range(0f, 360f);
            }
            else
            {
                stats.angle = 0;
            }

            stats.life = Random.Range(maxDuracion / 2, maxDuracion);

            stats.time = 0f;

            stats.gravity = gravedad;

            particles.Add(myParticle);

        }
    }

    private Vector3 UpdateParticlePosition(Particles p, float time)
    {
        Vector3 v = new Vector3();
        p.time += time;

        v.x = p.v0 * Mathf.Cos(p.angle) * p.time;
        v.y = p.v0 * Mathf.Sin(p.angle) * p.time - (p.gravity * Mathf.Pow(p.time, 2)) / 2;
        v.z = 0;
        
        return v;
    }
}