using UnityEngine;
using UnityEngine.InputSystem;


public class ActivatorScript : MonoBehaviour
{
    public JSONLoader jsonLoader;
    private bool glowActive = false;
    private float minGlowIntensity = 0.7f;
    public InputActionReference customZoomInteraction;
    public InputActionReference undoZoomInteraction;

    public GameObject orbPrefab;
    private float orbPerKcal = 50f;     // 1 Orb pro 50 Kalorien

    private void Start()
    {
        customZoomInteraction.action.Enable();
        undoZoomInteraction.action.Enable();

        customZoomInteraction.action.performed += context => glowActive = true;
        customZoomInteraction.action.canceled += context => glowActive = false;

        customZoomInteraction.action.started += ActivateTransformation;
        customZoomInteraction.action.performed += ActivateTransformation;
        undoZoomInteraction.action.canceled += UndoTransformation;
    }

    void ActivateTransformation(InputAction.CallbackContext context)
    {
        foreach (var kvp in jsonLoader.GetSpheres())
        {
            GameObject sphere = kvp.Value;
            SphereInfo info = sphere.GetComponent<SphereInfo>();
            if (info == null || info.sphereRenderer == null) continue;

            /*  PROTEIN GLOW */
            float protein = info.lebensmittelData.EiweißAsFloat;
            float maxExtra = 4.0f;
            float scaled = Mathf.InverseLerp(0f, 25f, protein);
            float totalIntensity = minGlowIntensity + scaled * maxExtra;
            Color emissionColor = info.sphereColor * Mathf.LinearToGammaSpace(totalIntensity);
            info.sphereRenderer.material.SetColor("_EmissionColor", emissionColor);
            info.sphereRenderer.material.EnableKeyword("_EMISSION");

            /* KALORIEN KÜGELCHEN */
            /* Anzahl Orbs und Radius Range berechnen */
            int orbCount = Mathf.FloorToInt(info.lebensmittelData.KcalAsInt / orbPerKcal);    
            float minRadius = 0.35f;
            float maxRadius = 0.56f;
            for (int i = 0; i < orbCount; i++)
            {
                GameObject orb = Instantiate(orbPrefab);
                orb.tag = "Orb";
                OrbScript orbScript = orb.AddComponent<OrbScript>();
                orbScript.target = sphere.transform;

                /* bei einem Kügelchen minRadius, sonst Radius auf Intervall verteilt */
                float t = (orbCount == 1) ? minRadius : (float)i / (orbCount - 1);
                orbScript.orbitRadius = Mathf.Lerp(minRadius, maxRadius, t);
                orbScript.orbitSpeed = UnityEngine.Random.Range(20f, 60f);
            }

            /* VERARBEITUNGSGRAD VERTICES */
            MeshScript meshSc = sphere.AddComponent<MeshScript>();
            meshSc.meshFilter = sphere.GetComponent<MeshFilter>();
            meshSc.verarbeitungsgrad = info.lebensmittelData.Verarbeitungsgrad;
            meshSc.radius = 0.4f;
        }
    }


    /* Macht Änderungen rückgängig */
    void UndoTransformation(InputAction.CallbackContext context)
    {
        foreach (var kvp in jsonLoader.GetSpheres())
        {
            GameObject sphere = kvp.Value;
            SphereInfo info = sphere.GetComponent<SphereInfo>();
            if (info == null || info.sphereRenderer == null) continue;

            /* setzt Protein-Glow auf min-Intensity */
            Color emissionColor = info.sphereColor * Mathf.LinearToGammaSpace(minGlowIntensity);
            info.sphereRenderer.material.SetColor("_EmissionColor", emissionColor);
            info.sphereRenderer.material.EnableKeyword("_EMISSION");

            /* entfernt Verarbeitungsgrad-Mesh */
            MeshScript meshScript = sphere.GetComponent<MeshScript>();
            if (meshScript != null)
            {
                meshScript.UndoMesh();
            }
        }

        /* entfernt alle Kalorien-Orbs */
        GameObject[] orbs = GameObject.FindGameObjectsWithTag("Orb");
        foreach (GameObject orb in orbs)
        {
            Destroy(orb);
        }
    }
}
