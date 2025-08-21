using System;
using System.Collections.Generic;
using UnityEngine;

/* liest Datei und erstellt pro Datenobjekt Sphäre
 * diese werden in Dictionary gespeichert */
public class JSONLoader : MonoBehaviour
{
    public GameObject spherePrefab;
    private Dictionary<string, GameObject> sphereDictionary = new Dictionary<string, GameObject>();
    private Dictionary<string, Vector3> clusterCenters = new Dictionary<string, Vector3>();

    void Start()
    {
        // json Datei laden und in String konvertieren
        TextAsset jsonFile = Resources.Load<TextAsset>("lebensmittel");
        if (jsonFile == null) { UnityEngine.Debug.Log("Datei nicht gefunden."); }
        string jsonData = jsonFile.text;

        // string in liste von objekten konvertieren
        JSONDataList dataList = JsonUtility.FromJson<JSONDataList>(jsonData);

        if (dataList?.items == null) { UnityEngine.Debug.LogError("Fehler beim Laden der JSON-Daten."); return; }
        UnityEngine.Debug.Log("Anzahl der Objekte: " + dataList.items.Length);

        SetClusterCenters(dataList);
        foreach (JSONData item in dataList.items)
        {
            CreateSphereForItem(item);
        }
    }

    /* Cluster-Zentrum je Kategorie */
    void SetClusterCenters(JSONDataList dataList)
    {
        Vector3 userPos = Camera.main.transform.position;
        Transform cam = Camera.main.transform;

        Vector3 forward = cam.forward;
        forward.y = 0;
        forward.Normalize();

        HashSet<string> categories = new HashSet<string>();
        foreach (JSONData item in dataList.items)
        {
            categories.Add(item.Kategorie);
        }

        int count = categories.Count;
        float radius = 3.0f;   // Abstand zu User
        float heightOffset = 1.4f;

        int i = 0;
        foreach (string cat in categories)
        {
            // gleichmäßig um User rundherum platzieren
            float angle = (i / (float)count) * Mathf.PI * 2f;
            Vector3 offset = new Vector3(Mathf.Cos(angle), 0, Mathf.Sin(angle)) * radius;

            // Positionierung anhand der User Position + Offset – Spaze
            Vector3 clusterPosition = userPos + offset - new Vector3(0, heightOffset, 0);

            clusterCenters[cat] = clusterPosition;
            i++;
        }
    }


    void CreateSphereForItem(JSONData item)
    {
        if (sphereDictionary.ContainsKey(item.Lebensmittel))
        {
            UnityEngine.Debug.LogWarning("!!!!! Doppelte Kugel für: " + item.Lebensmittel);
            return;
        }

        // prüfen, ob clusterzentrum für kategorie besteht
        Vector3 clusterCenter = clusterCenters.ContainsKey(item.Kategorie) 
            ? clusterCenters[item.Kategorie] : Vector3.zero;

        // position mit random abstand vom zentrum
        Vector3 position = GetValidPosition(clusterCenter);

        // Sphäre zum User rotieren
        Transform cam = Camera.main.transform;
        Quaternion rotation = Quaternion.LookRotation(cam.position - position);

        GameObject sphere = Instantiate(spherePrefab, position, rotation);
        SphereInfo sphereInfo = sphere.AddComponent<SphereInfo>();
        sphereInfo.lebensmittelData = item;

        // farbe je nach kategorie
        Renderer renderer = sphere.GetComponent<Renderer>();
        if (renderer != null)
        {
            renderer.material = new Material(renderer.material);
            Color color = GetColorForCategory(item.Kategorie);
            renderer.material.color = color;

            /* per default minimum glow */
            float protein = item.EiweißAsFloat;
            float minIntensity = 1.0f;
            Color emissionColor = color * Mathf.LinearToGammaSpace(minIntensity);
            renderer.material.SetColor("_EmissionColor", emissionColor);
            renderer.material.EnableKeyword("_EMISSION");

            sphereInfo.sphereRenderer = renderer;
            sphereInfo.sphereColor = color;
        }

        /* sphäre in dictionary abspeichern */
        sphereDictionary[item.Lebensmittel] = sphere;
    }

    Vector3 GetValidPosition(Vector3 clusterCenter)
    {
        Vector3 position;
        int maxAttempts = 30;  // falls zu oft ungültig, aufgeben
        int attempts = 0;

        float minDistance = 1.0f;  // mindestabstand zwischen den Sphären
        float spreadXZ = 2.5f;      
        float spreadY = 3.4f;

        do
        {
            Vector3 randomOffset = new Vector3(
            UnityEngine.Random.Range(-spreadXZ, spreadXZ),
            UnityEngine.Random.Range(0f, spreadY),   // nur nach oben
            UnityEngine.Random.Range(-spreadXZ, spreadXZ)
        );

            position = clusterCenter + randomOffset;

            attempts++;
        }
        // kollisions- und maxAttempts-prüfung
        while (Physics.CheckSphere(position, minDistance) && attempts < maxAttempts);

        return position;
    }

    Color GetColorForCategory(string category)
    {
        switch (category)
        {
            case "Obst": return Color.green;
            case "Backwaren": return Color.yellow;
            case "Milch- oder Eiprodukt": return Color.blue;
            case "Getränke": return Color.cyan;
            case "Frühstück": return Color.red;
            default: return Color.white;
        }
    }

    public Dictionary<string, GameObject> GetSpheres()
    {
        return sphereDictionary;
    }
}