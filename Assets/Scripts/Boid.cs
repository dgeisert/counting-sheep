using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float dx, dz;
    public float x
    {
        get
        {
            return transform.position.x;
        }
    }
    public float z
    {
        get
        {
            return transform.position.z;
        }
    }
    public Vector3 xyz
    {
        get
        {
            return transform.position;
        }
        set
        {
            transform.position = value;
        }
    }
    public Vector3 visualNearbyCenter;

    public void Move()
    {
        transform.position += new Vector3(dx, 0, dz) * Time.deltaTime;
        transform.LookAt(transform.position - new Vector3(dx, 0, dz));
    }
}