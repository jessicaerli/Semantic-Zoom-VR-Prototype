using UnityEngine;

/* Animationsscript für Kalorienkügelchen */
public class OrbScript : MonoBehaviour
{
    public Transform target; // Zentrum der Rotation => zugehörige Sphäre
    public float orbitSpeed = 50f;
    public float orbitRadius = 0.5f;
    private float angle;

    void Update()
    {
        if (target == null) return;

        angle += orbitSpeed * Time.deltaTime;
        float rad = angle * Mathf.Deg2Rad;
        Vector3 offset = new Vector3(Mathf.Cos(rad), 0, Mathf.Sin(rad)) * orbitRadius;
        transform.position = target.position + offset;
    }
}