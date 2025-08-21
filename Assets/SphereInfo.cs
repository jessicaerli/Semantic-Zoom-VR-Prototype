using UnityEngine;
using TMPro;

/* zusätzliche Info für Sphäre + setzen des Labels */
public class SphereInfo : MonoBehaviour
{
    public JSONData lebensmittelData;
    private TextMeshPro itemNameText;
    public Renderer sphereRenderer;
    public Color sphereColor;

    /* setzt Lebensmitteltext */
    void Start()
    {
        itemNameText = GetComponentInChildren<TextMeshPro>();

        if (itemNameText == null)
        {
            UnityEngine.Debug.LogError("Kein TextMeshPro gefunden!");
        }
        else
        {
            itemNameText.text = lebensmittelData?.Lebensmittel;
            itemNameText.gameObject.SetActive(true);
        }
    }
}