using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dog : MonoBehaviour
{
    public static Dog Instance;
    public static Vector3 xyz
    {
        get
        {
            return Instance.transform.position;
        }
    }

    [SerializeField] private float speed = 1;
    [SerializeField] private Transform body;
    [SerializeField] private ParticleSystem barkParticles;
    [SerializeField] private ParticleSystem winParticle;
    private AudioSource barkSound, winAudio;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        barkSound = GetComponent<AudioSource>();
        winAudio = winParticle.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 move = Vector3.zero;
        if (Controls.Up)
        {
            move += (Vector3.forward + Vector3.right) * Time.deltaTime * speed;
        }
        if (Controls.Down)
        {
            move -= (Vector3.forward + Vector3.right) * Time.deltaTime * speed;
        }
        if (Controls.Left)
        {
            move -= (Vector3.right - Vector3.forward) * Time.deltaTime * speed;
        }
        if (Controls.Right)
        {
            move += (Vector3.right - Vector3.forward) * Time.deltaTime * speed;
        }
        //look in the direction of movement
        transform.position += move;
        body.LookAt(transform.position - move);

        if (Controls.Bark)
        {
            barkSound.pitch = 0.9f + Random.value / 5f;
            barkSound.Play();
            barkParticles.Play();
        }
    }

    public void Win()
    {
        winParticle.Play();
        winAudio.Play();
    }
}