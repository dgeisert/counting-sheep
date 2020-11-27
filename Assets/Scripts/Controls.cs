using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Controls : MonoBehaviour
{
    public static Controls Instance;
    public static bool Up
    {
        get
        {
            return Input.GetKey(KeyCode.W) ||
                Input.GetKey(KeyCode.UpArrow);
        }
    }
    public static bool Down
    {
        get
        {
            return Input.GetKey(KeyCode.S) ||
                Input.GetKey(KeyCode.DownArrow);
        }
    }
    public static bool Left
    {
        get
        {
            return Input.GetKey(KeyCode.A) ||
                Input.GetKey(KeyCode.LeftArrow);
        }
    }
    public static bool Right
    {
        get
        {
            return Input.GetKey(KeyCode.D) ||
                Input.GetKey(KeyCode.RightArrow);
        }
    }
    public static bool Bark
    {
        get
        {
            return Input.GetKeyDown(KeyCode.E) ||
                Input.GetKeyDown(KeyCode.Space);
        }
    }
    public static bool Pause
    {
        get
        {
            return Input.GetKeyDown(KeyCode.P);
        }
    }
}