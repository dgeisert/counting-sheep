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

    public void Start()
    {
        transform.position += Vector3.up * IslandBuilder.Instance.GetHeight(transform.position);
    }

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
            transform.position += Vector3.up * (IslandBuilder.Instance.GetHeight(transform.position) - transform.position.y) * 0.5f;
        }
        doMove += Time.deltaTime;
        if (doMove >= 1)
        {
            doMove = -2 * Random.value;
        }
    }

    public void Avoid(Vector3 pos, float range, float turn, float modifier)
    {
        //if avoider is near move away
        float dist = Vector3.Distance(pos, xyz);
        if (dist < range)
        {
            Vector3 dir = (new Vector3(transform.position.x - pos.x, transform.position.y - pos.y, transform.position.z - pos.z)).normalized;
            dx += (range - dist) / range * dir.x * turn * modifier;
            dz += (range - dist) / range * dir.z * turn * modifier;
        }
    }

    public void Die()
    {
        BoidManager.sheepFlock.boids.Remove(this);
        Destroy(gameObject);
    }
}