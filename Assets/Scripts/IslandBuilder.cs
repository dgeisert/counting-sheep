using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public class IslandLevel
{
    [SerializeField] public Color color;
    [SerializeField] public GameObject[] objects;
    [SerializeField] public float objectRate;
}

public class IslandBuilder : MonoBehaviour
{
    public static IslandBuilder Instance;

    public Material islandMat;
    public int size = 40;
    public float scale = 1;
    public float height = 5;
    public float perlinScale = 0.1f;
    public bool random;
    List<Vector3> verts;
    List<GameObject> spawns;

    [SerializeField] public IslandLevel[] levels;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        Mesh mesh = new Mesh();
        spawns = new List<GameObject>();
        verts = new List<Vector3>();
        List<Vector3> verts2 = new List<Vector3>();
        List<Color> vertColors = new List<Color>();
        List<int> tris = new List<int>();
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                float y = GetHeight(i, j);
                if (y > -1 && (j == 0 || i == 0 || j == size - 1 || i == size - 1))
                {
                    y = -1;
                }

                verts.Add(scale * new Vector3(i + Random.value, y, j + Random.value));
                if (i > 0 && j > 0/* && (
                        verts[(i - 1) * size + j - 1].y > -scale ||
                        verts[(i - 1) * size + j].y > -scale ||
                        verts[i * size + j - 1].y > -scale ||
                        verts[i * size + j].y > -scale)*/)
                {

                    Color tri1Color = levels[0].color;
                    vertColors.Add(tri1Color);
                    vertColors.Add(tri1Color);
                    vertColors.Add(tri1Color);

                    verts2.Add(verts[(i - 1) * size + j - 1]);
                    verts2.Add(verts[(i - 1) * size + j]);
                    verts2.Add(verts[i * size + j - 1]);

                    tris.Add(verts2.Count - 3);
                    tris.Add(verts2.Count - 2);
                    tris.Add(verts2.Count - 1);

                    Color tri2Color = levels[0].color;

                    vertColors.Add(tri2Color);
                    vertColors.Add(tri2Color);
                    vertColors.Add(tri2Color);

                    verts2.Add(verts[i * size + j]);
                    verts2.Add(verts[(i - 1) * size + j]);
                    verts2.Add(verts[i * size + j - 1]);

                    tris.Add(verts2.Count - 1);
                    tris.Add(verts2.Count - 2);
                    tris.Add(verts2.Count - 3);

                    if (levels != null && levels[0].objects.Length > 0)
                    {
                        SpawnObject(i, j);
                    }
                }
                else
                {
                    spawns.Add(null);
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

    public void SpawnObject(int i, int j)
    {
        bool y = Mathf.PerlinNoise(transform.position.x + i * perlinScale * 2 + 12, transform.position.z + j * perlinScale * 2 + 12) >
            0.7f;
        if (y)
        {
            int count = i * size + j;
            GameObject spawn = (spawns?.Count > (count - 1) && (count - 1) >= 0) ? spawns[count - 1] : null;
            if (spawn == null)
            {
                spawn = (spawns?.Count > (count - size) && (count - size) >= 0) ? spawns[count - size] : null;
            }
            if (spawn == null)
            {
                spawn = (spawns?.Count > (count - 1 - size) && (count - 1 - size) >= 0) ? spawns[count - 1 - size] : null;
            }
            if (spawn == null)
            {
                spawn = RandomObject(levels[0]);
            }
            spawns.Add(spawn);
            GameObject.Instantiate(spawn,
                transform.position + verts[i * size + j],
                Quaternion.Euler(0, Random.value * 360, 0),
                transform);
        }
        else
        {
            spawns.Add(null);
        }
    }

    GameObject RandomObject(IslandLevel objectList)
    {
        return objectList.objects[Mathf.FloorToInt(Random.value * objectList.objects.Length)];
    }

    void GenerateMesh() { }
}