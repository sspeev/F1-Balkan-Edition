using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// A self-contained script which controls the equalizer visualizer in the lobby scene.
/// It retrieves spectrum data from Unity's AudioListener and scales visual pillars.
/// </summary>
public class MusicVisualziation : MonoBehaviour
{
    [Header("Visualizer Pillars")]
    [SerializeField] private GameObject pillarPrefab;
    [SerializeField] private int pillarAmount = 32;
    [SerializeField] private float radius = 10f;
    [SerializeField] private float sensitivity = 100f;
    [SerializeField] private float speed = 10f;
    [SerializeField] private bool useCircleLayout = true;

    [Header("Spectrum Settings")]
    [SerializeField] private FFTWindow fftWindow = FFTWindow.BlackmanHarris;

    private float[] spectrum = new float[512];
    private List<GameObject> pillars = new List<GameObject>();
    private GameObject folder;

    void Start()
    {
        CreatePillars();
    }

    private void CreatePillars()
    {
        pillars.Clear();
        folder = new GameObject("Pillars-" + pillarAmount);
        folder.transform.SetParent(transform);
        folder.transform.localPosition = Vector3.zero;

        for (int i = 0; i < pillarAmount; i++)
        {
            GameObject p;
            if (pillarPrefab != null)
            {
                p = Instantiate(pillarPrefab, folder.transform);
            }
            else
            {
                p = GameObject.CreatePrimitive(PrimitiveType.Cube);
                p.transform.SetParent(folder.transform);
            }

            if (useCircleLayout)
            {
                float angle = i * Mathf.PI * 2f / pillarAmount;
                Vector3 pos = new Vector3(Mathf.Cos(angle) * radius, 0, Mathf.Sin(angle) * radius);
                p.transform.localPosition = pos;
                p.transform.localRotation = Quaternion.LookRotation(pos);
            }
            else
            {
                // Line layout
                float spacing = 2f;
                float startX = -((pillarAmount - 1) * spacing) / 2f;
                p.transform.localPosition = new Vector3(startX + i * spacing, 0, 0);
            }

            pillars.Add(p);
        }
    }

    void Update()
    {
        if (pillars.Count == 0) return;

        AudioListener.GetSpectrumData(spectrum, 0, fftWindow);

        for (int i = 0; i < pillars.Count; i++)
        {
            // Map pillar to spectrum index
            int specIndex = Mathf.Clamp(i * (spectrum.Length / pillars.Count), 0, spectrum.Length - 1);
            float level = spectrum[specIndex] * sensitivity;

            Vector3 previousScale = pillars[i].transform.localScale;
            previousScale.y = Mathf.Lerp(previousScale.y, Mathf.Max(0.1f, level), speed * Time.deltaTime);
            pillars[i].transform.localScale = previousScale;

            // Adjust position so it scales from the base (assuming pivot is in center, move it up by half scale)
            Vector3 pos = pillars[i].transform.localPosition;
            pos.y = previousScale.y * 0.5f;
            pillars[i].transform.localPosition = pos;
        }
    }

    public void Rebuild()
    {
        if (folder != null)
        {
            Destroy(folder);
        }
        CreatePillars();
    }
}
