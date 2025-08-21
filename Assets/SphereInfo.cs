using UnityEngine;
using TMPro;

/* zus�tzliche Info f�r Sph�re + setzen des Labels */
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