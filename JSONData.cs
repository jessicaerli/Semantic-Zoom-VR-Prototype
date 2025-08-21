using UnityEngine;

[System.Serializable]
public class JSONData
{
    public string Lebensmittel;
    public string Kategorie;
    public string Eiweiﬂ;
    public string Fett;
    public string Kohlenhydrate;
    public string Kilokalorien;
    public string Verarbeitungsgrad;

    public float EiweiﬂAsFloat
    {
        get { return float.TryParse(Eiweiﬂ, out float value) ? value : 0f; }
    }

    public float FettAsFloat
    {
        get { return float.TryParse(Fett, out float value) ? value : 0f; }
    }


    public float KohlenhydrateAsFloat
    {
        get { return float.TryParse(Kohlenhydrate, out float value) ? value : 0f; }
    }

    public int KcalAsInt
    {
        get { return int.TryParse(Kilokalorien, out int value) ? value : 0; }
    }  
}

[System.Serializable]
public class JSONDataList
{
    public JSONData[] items;
}

