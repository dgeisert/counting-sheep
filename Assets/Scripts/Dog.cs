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
    public BoidAvoid dogAvoider, barkAvoider;
    private AudioSource barkSound, winAudio;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        barkSound = GetComponent<AudioSource>();
        winAudio = winParticle.gameObject.GetComponent<AudioSource>();
    }

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

        //if the dog is trying to go up a mountain, don't let it;
        if (IslandBuilder.Instance.GetHeight(transform.position + move) < (IslandBuilder.Instance.wallHeight * IslandBuilder.Instance.scale) - 0.2f)
        {
            //look in the direction of movement
            transform.position += move;
        }

        body.LookAt(transform.position - move);

        //Set height based on island
        transform.position += Vector3.up * (IslandBuilder.Instance.GetHeight(transform.position) - transform.position.y) * 0.5f;

        if (Controls.Bark)
        {
            StartCoroutine(Bark());
        }
    }

    IEnumerator Bark()
    {
        barkSound.pitch = 0.9f + Random.value / 5f;
        barkSound.Play();
        barkParticles.Play();
        barkAvoider.gameObject.SetActive(true);
        yield return null;
        barkAvoider.gameObject.SetActive(false);
    }

    public void Win()
    {
        winParticle.Play();
        winAudio.Play();
    }
}