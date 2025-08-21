using UnityEngine;
using LazySquirrelLabs.SphereGenerator.Generators;

/* ändert Mesh bei Sphere */
public class MeshScript : MonoBehaviour
{
    public MeshFilter meshFilter;
    public string verarbeitungsgrad = "ii";
    public float radius = 1.0f;
    public float targetScale = 0.4f;
    public Mesh origin;

    void Start()
    {
        origin = meshFilter.sharedMesh;
        ApplyMesh();  
    }

    /* Anzahl an Vertices je Verarbeitungsgrad */
    void ApplyMesh()
    {
        string prefix = verarbeitungsgrad.Split(' ')[0].ToLower();
        ushort depth = prefix switch
        {
            "i" => 6,
            "ii" => 4,
            "iii" => 3,
            "iv" => 0,
            _ => 2
        };

        var generator = new IcosphereGenerator(radius, depth);
        Mesh mesh = generator.Generate();
        meshFilter.mesh = mesh;
        transform.localScale = Vector3.one * targetScale;
    }

    public void UndoMesh()
    {
        if (origin != null)
        {
            meshFilter.mesh = origin;
        }
    }
}