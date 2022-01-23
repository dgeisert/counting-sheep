using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class IslandBuilder : MonoBehaviour
{
    public static IslandBuilder Instance;
    public List<Color> mapColors;
    public List<GameObject> mapSpawns;

    public Material islandMat;
    public int size;
    public float scale = 1;
    public float height = 5;
    public float perlinScale = 0.1f;
    public Texture2D map, pushMap;
    public Color color, wallColor;
    public float wallHeight = 5f;
    Color[] push;

    List<Vector3> verts;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        size = map.height;
        push = pushMap.GetPixels();
        Color[] pixels = map.GetPixels();
        Mesh mesh = new Mesh();
        verts = new List<Vector3>();
        List<Vector3> verts2 = new List<Vector3>();
        List<Color> vertColors = new List<Color>();
        List<int> tris = new List<int>();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float y = 0;
                if (y > -1 && (j == 0 || i == 0 || j == size - 1 || i == size - 1))
                {
                    y = wallHeight + Random.value;
                }
                else
                {
                    y = GetHeight(i, j);
                }

                verts.Add(scale * new Vector3(i + Random.value / 4f * scale, pixels[i * size + j] != Color.black ? y : wallHeight + Random.value, j + Random.value / 4f * scale));
                if (i > 0 && j > 0
                    /* && (
                                           verts[(i - 1) * size + j - 1].y < wallHeight * scale ||
                                           verts[(i - 1) * size + j].y < wallHeight * scale ||
                                           verts[i * size + j - 1].y < wallHeight * scale ||
                                           verts[i * size + j].y < wallHeight * scale)*/
                )
                {
                    if (true)
                    /*verts[(i - 1) * size + j - 1].y < wallHeight * scale ||
                                            verts[(i - 1) * size + j].y < wallHeight * scale ||
                                            verts[i * size + j - 1].y < wallHeight * scale)*/
                    {
                        Color tri1Color =
                            (verts[(i - 1) * size + j - 1].y >= wallHeight * scale ||
                                verts[(i - 1) * size + j].y >= wallHeight * scale ||
                                verts[i * size + j - 1].y >= wallHeight * scale) ?
                            wallColor :
                            color;
                        vertColors.Add(tri1Color);
                        vertColors.Add(tri1Color);
                        vertColors.Add(tri1Color);

                        verts2.Add(verts[(i - 1) * size + j - 1]);
                        verts2.Add(verts[(i - 1) * size + j]);
                        verts2.Add(verts[i * size + j - 1]);

                        tris.Add(verts2.Count - 3);
                        tris.Add(verts2.Count - 2);
                        tris.Add(verts2.Count - 1);
                    }

                    if (true)
                    /*verts[i * size + j].y < wallHeight * scale ||
                                            verts[(i - 1) * size + j].y < wallHeight * scale ||
                                            verts[i * size + j - 1].y < wallHeight * scale)*/
                    {
                        Color tri2Color =
                            (verts[i * size + j].y >= wallHeight * scale ||
                                verts[(i - 1) * size + j].y >= wallHeight * scale ||
                                verts[i * size + j - 1].y >= wallHeight * scale) ?
                            wallColor :
                            color;

                        vertColors.Add(tri2Color);
                        vertColors.Add(tri2Color);
                        vertColors.Add(tri2Color);

                        verts2.Add(verts[i * size + j]);
                        verts2.Add(verts[(i - 1) * size + j]);
                        verts2.Add(verts[i * size + j - 1]);

                        tris.Add(verts2.Count - 1);
                        tris.Add(verts2.Count - 2);
                        tris.Add(verts2.Count - 3);
                    }

                    if (pixels[i * size + j] != Color.black && pixels[i * size + j] != Color.green)
                    {
                        if (mapColors.Contains(pixels[i * size + j]))
                        {
                            GameObject go = Instantiate(
                                mapSpawns[mapColors.IndexOf(pixels[i * size + j])],
                                verts[i * size + j] + transform.position,
                                Quaternion.Euler(0, Random.value * 360, 0)
                            );
                            switch (go.name)
                            {
                                case "BoidManager(Clone)":
                                    Dog.Instance.transform.position = verts[i * size + j] - (5 * Vector3.forward) + transform.position;
                                    break;
                                case "Wolf(Clone)":
                                    Wolf.Instance.AddWolf(go);
                                    break;
                                default:
                                    go.transform.SetParent(transform);
                                    break;
                            }
                        }
                    }
                }
            }
        }

        mesh.vertices = verts2.ToArray();
        mesh.triangles = tris.ToArray();
        mesh.colors = vertColors.ToArray();
        mesh.RecalculateNormals();

        gameObject.AddComponent<MeshFilter>().mesh = mesh;
        gameObject.AddComponent<MeshRenderer>().material = islandMat;
        gameObject.AddComponent<MeshCollider>();
    }

    private float GetHeight(int i, int j)
    {
        float y = (Mathf.PerlinNoise(transform.position.x + i * perlinScale, transform.position.z + j * perlinScale) +
                Mathf.PerlinNoise(transform.position.x + i * perlinScale / 2, transform.position.z + j * perlinScale / 2) +
                Mathf.PerlinNoise(transform.position.x + i * perlinScale / 8, transform.position.z + j * perlinScale / 8) - 1.5f) *
            height;
        return y;
    }

    public float GetHeight(Vector3 pos)
    {
        pos = transform.InverseTransformPoint(pos) / scale;
        float x2 = pos.x % 1;
        float x1 = (1 - x2);
        float z2 = pos.z % 1;
        float z1 = (1 - z2);
        int count = Mathf.FloorToInt(pos.x) * size + Mathf.FloorToInt(pos.z);
        return ((verts?.Count > count && count >= 0) ? verts[count].y * x1 * z1 : 0) +
            ((verts?.Count > (count + 1) && (count + 1) >= 0) ? verts[count + 1].y * x1 * z2 : 0) +
            ((verts?.Count > (count + size) && (count + size) >= 0) ? verts[count + size].y * x2 * z1 : 0) +
            ((verts?.Count > (count + size + 1) && (count + size + 1) >= 0) ? verts[count + size + 1].y * x2 * z2 : 0);
    }

    public Vector2 GetPush(Vector3 pos)
    {
        pos = transform.InverseTransformPoint(pos) / scale;
        int count = Mathf.RoundToInt(pos.x) * size + Mathf.RoundToInt(pos.z);
        if(push.Length <= count || count < 0)
        {
            return Vector2.zero;
        }
        return new Vector2(push[count].r - 0.5f, push[count].g - 0.5f);
    }
}