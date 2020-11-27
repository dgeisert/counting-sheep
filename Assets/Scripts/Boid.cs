using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Boid : MonoBehaviour
{
    public float speed, maxSpeed;
    public float dx, dz;
    public float doMove;
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
        Vector3 move = new Vector3(dx, 0, dz);
        if (move.magnitude > 5 || doMove > 0)
        {
            move = Mathf.Min(move.magnitude, maxSpeed) * move.normalized;
            dx = move.x;
            dz = move.z;
            transform.position += move * Time.deltaTime * speed;
            transform.LookAt(transform.position - move);
        }
        doMove += Time.deltaTime;
        if (doMove >= 1)
        {
            doMove = -2 * Random.value;
        }
    }
}