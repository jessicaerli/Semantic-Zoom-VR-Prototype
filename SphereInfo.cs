using System.Diagnostics;
using UnityEngine;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;
using TMPro; 



public class SphereInfo : MonoBehaviour
{
    public JSONData lebensmittelData;
    private TextMeshProUGUI itemNameText;

    void Start()
    {
        //uiText = GameObject.Find("LebensmittelText").GetComponent<UnityEngine.UI.Text>(); // Automatisch das Text-Objekt finden
        itemNameText = GetComponentInChildren<TextMeshProUGUI>();

        if (itemNameText == null)
        {
            UnityEngine.Debug.LogError("Kein Text in der Kugel gefunden!");
        }
        else
        {
            itemNameText.gameObject.SetActive(false);
        }
    }

    void OnMouseDown()
    {
        if (itemNameText != null && lebensmittelData != null)
        {
            itemNameText.text = $"Lebensmittel: {lebensmittelData.Lebensmittel}\n" +
                          $"Kategorie: {lebensmittelData.Kategorie}\n" +
                          $"Fett: {lebensmittelData.Fett}g";
            itemNameText.gameObject.SetActive(true);
        }
        else
        {
            UnityEngine.Debug.Log($"Geklickt: {lebensmittelData?.Lebensmittel} (Kategorie: {lebensmittelData?.Kategorie}, Fett: {lebensmittelData?.Fett}g)");
        }

    }
}
