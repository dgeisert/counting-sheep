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
    // Start is called before the first frame update
    void Start()
    {
        Instance = this;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow))
        {
            transform.position += (Vector3.forward + Vector3.right) * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow))
        {
            transform.position -= (Vector3.forward + Vector3.right) * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position -= (Vector3.right - Vector3.forward) * Time.deltaTime * speed;
        }
        if(Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow))
        {
            transform.position += (Vector3.right - Vector3.forward) * Time.deltaTime * speed;
        }
    }
}
