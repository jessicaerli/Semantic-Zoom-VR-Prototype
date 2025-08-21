using UnityEngine;
using System.IO;
using System.Diagnostics;
using System.Collections.Generic;
using System.Net;
using System;

/* liest Datei und erstellt anhand dieser 3D-Sphären
 * diese werden in Dictionary gespeichert 
 * 
 */
public class JSONLoader : MonoBehaviour
{
    public GameObject spherePrefab;
    private Dictionary<string, GameObject> sphereDictionary = new Dictionary<string, GameObject>();
    //private Dictionary<string, GameObject> infoDictionary = new Dictionary<string, SphereInfo;
    private Dictionary<string, Vector3> clusterCenters = new Dictionary<string, Vector3>();


    void Start()
    {
        //dummy Kugel löschen
        GameObject dummySphere = GameObject.Find("Sphere"); 
        if (dummySphere != null)
        {
            Destroy(dummySphere);
        }

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

    // Cluster-Zentren erstellen je Kategorie
    void SetClusterCenters(JSONDataList dataList)
    {
        HashSet<string> categories = new HashSet<string>();

        foreach (JSONData item in dataList.items)
        {
            categories.Add(item.Kategorie);
        }

        foreach (string cat in categories)
        {
            Vector3 clusterPosition = new Vector3(
                UnityEngine.Random.Range(-10, 10),
                UnityEngine.Random.Range(1, 5),
                UnityEngine.Random.Range(-10, 10)
            );
            clusterCenters[cat] = clusterPosition;

            UnityEngine.Debug.Log($"Cluster-Zentrum für {cat} bei {clusterPosition}");
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

        // Position mit zufälliger Abweichung vom Zentrum
        Vector3 position = GetValidPosition(clusterCenter);
        GameObject sphere = Instantiate(spherePrefab, position, Quaternion.identity);
        SphereInfo sphereInfo = sphere.AddComponent<SphereInfo>();
        sphereInfo.lebensmittelData = item;

        // farbe je nach kategorie
        Renderer renderer = sphere.GetComponent<Renderer>();
        if (renderer != null)
        {
            Color color = GetColorForCategory(item.Kategorie);
            renderer.material.color = color;
        }

        // Kugel im Dictionary speichern
        sphereDictionary[item.Lebensmittel] = sphere;
    }

    Vector3 GetValidPosition(Vector3 clusterCenter)
    {
        Vector3 position;
        int maxAttempts = 20;  // falls die Position zu oft ungültig ist, aufgeben
        int attempts = 0;

        do
        {
            position = clusterCenter + new Vector3(
                UnityEngine.Random.Range(-4f, 4f),
                UnityEngine.Random.Range(-2f, 2f),
                UnityEngine.Random.Range(-4f, 4f)
            );
            attempts++;
        }
        // prüft u.A. ob es keine Kollision gibt
        while (Physics.CheckSphere(position, 0.6f) && attempts < maxAttempts);

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


  
}

