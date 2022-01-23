using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PushMaker : MonoBehaviour
{
    public Texture2D map, pushMap;
    public float strength = 0.2f;
    void Start()
    {
        if(map == null)
        {
            map = IslandBuilder.Instance.map;
            pushMap = IslandBuilder.Instance.pushMap;
        }
        int size = map.height;
        Color[] pixels = map.GetPixels();
        int count = pixels.Length;
        Color[] push = new Color[count];
        Color baseColor = new Color(0.5f, 0.5f, 0, 0);
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                Color c = baseColor;
                for (int k = -1; k < 2; k++)
                {
                    bool blackMe = pixels[i * size + j] == Color.black;
                    for (int l = -1; l < 2; l++)
                    {
                        if (k != 0 || l != 0)
                        {
                            int x = i + k;
                            int y = j + l;
                            bool black = x < 0 || y < 0 || x >= size || y >= size;
                            black = black || pixels[x * size + y] == Color.black;
                            if (black != blackMe)
                            {
                                c.r -= (blackMe ? -1 : 1) * k * strength;
                                c.g -= (blackMe ? -1 : 1) * l * strength;
                            }
                        }
                    }
                }
                push[i * size + j] = c;
            }
        }

        Color[] push2 = new Color[count];
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                if (push[i * size + j] == baseColor)
                {
                    Color c = baseColor;
                    for (int k = -1; k < 2; k++)
                    {
                        for (int l = -1; l < 2; l++)
                        {
                            if (k != 0 || l != 0)
                            {
                                int x = i + k;
                                int y = j + l;
                                bool black = x < 0 || y < 0 || x >= size || y >= size;
                                black = black || push[x * size + y] == baseColor;
                                if (!black)
                                {
                                    c.r += push[i * size + j].r / 8;
                                    c.g += push[i * size + j].g / 8;
                                }
                            }
                        }
                    }
                    push2[i * size + j] = c;
                }
                else
                {
                    push2[i * size + j] = push[i * size + j];
                }
            }
        }
        pushMap.SetPixels(push2);
        pushMap.Apply(false);
    }
}