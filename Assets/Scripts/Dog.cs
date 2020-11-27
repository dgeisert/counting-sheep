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
    private AudioSource barkSound;
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
        barkSound = GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Controls.Up)
        {
            transform.position += (Vector3.forward + Vector3.right) * Time.deltaTime * speed;
        }
        if (Controls.Down)
        {
            transform.position -= (Vector3.forward + Vector3.right) * Time.deltaTime * speed;
        }
        if (Controls.Left)
        {
            transform.position -= (Vector3.right - Vector3.forward) * Time.deltaTime * speed;
        }
        if (Controls.Right)
        {
            transform.position += (Vector3.right - Vector3.forward) * Time.deltaTime * speed;
        }
        if (Controls.Bark)
        {
            barkSound.pitch = 0.75f + Random.value / 2;
            barkSound.Play();
        }
    }
}