using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class SC_text_effect_master : MonoBehaviour
{
    [HideInInspector] public TMP_Text textMeshPro;
public List<int> targetLetters_movement = new List<int>();
 public List<int> targetLetters_ondulation = new List<int>();
 public List<int> targetLetters_scale = new List<int>();
 public List<int> targetLetters_color = new List<int>();
    [HideInInspector] public bool coroutineStarted = false;

    [Header("Noise_movement")]
    public bool movement;
    public float amplitude = 5f;
    public float frequency = 5f;

    [Header("Ondulation")]
    public bool ondulation;
    public float ondulation_amplitude = 5f;
    public float ondulation_frequency = 5f;

    [Header("Scale")]
    public bool Scale;
    public float size = 5f;
    public float frequency_ = 5f;

    [Header("Color")]
    public bool Color_;
    public Color color;
    public int glow_speed;
    public Color color_glow;

    private TMP_TextInfo textInfo;
    private Vector3[][] originalVertices;

    [Header("Char toggles")]
    public string movement_character_effect_start = "<";
    public string movement_character_effect_end = "<";

    public string ondulation_character_effect_start = "<";
    public string ondulation_character_effect_end = "<";

    public string scale_character_effect_start = "<";
    public string scale_character_effect_end = "<";

    public string color_character_effect_start = "<";
    public string color_character_effect_end = "<";

    float frameTimer = 0f;
    Dictionary<int, Vector3> lastOffsets = new Dictionary<int, Vector3>();

    void Start()
    {

    }

    private void Update()
    {
        if (!coroutineStarted)
        {
            StartCoroutine(AnimateLetters());
            coroutineStarted = true;
        }
    }

    IEnumerator AnimateLetters()
    {
        while (true)
        {
            frameTimer += Time.deltaTime;
            bool updateFrame = false;

            if (frameTimer >= frequency)
            {
                frameTimer = 0f;
                updateFrame = true;
            }

            originalVertices = new Vector3[textInfo.meshInfo.Length][];
            for (int i = 0; i < textInfo.meshInfo.Length; i++)
            {
                originalVertices[i] = new Vector3[textInfo.meshInfo[i].vertices.Length];
            }

            textMeshPro.ForceMeshUpdate();
            textInfo = textMeshPro.textInfo;

            if (originalVertices.Length != textInfo.meshInfo.Length)
            {
                originalVertices = new Vector3[textInfo.meshInfo.Length][];
                for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    originalVertices[i] = new Vector3[textInfo.meshInfo[i].vertices.Length];
                }
            }

            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible) continue;

                int c = charInfo.index;
                if (targetLetters_movement.Contains(c))
                {
                    int materialIndex = charInfo.materialReferenceIndex;
                    int vertexIndex = charInfo.vertexIndex;
                    Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                    if (originalVertices[materialIndex].Length < vertices.Length)
                    {
                        originalVertices[materialIndex] = new Vector3[vertices.Length];
                    }

                    originalVertices[materialIndex][vertexIndex + 0] = vertices[vertexIndex + 0];
                    originalVertices[materialIndex][vertexIndex + 1] = vertices[vertexIndex + 1];
                    originalVertices[materialIndex][vertexIndex + 2] = vertices[vertexIndex + 2];
                    originalVertices[materialIndex][vertexIndex + 3] = vertices[vertexIndex + 3];

                    if (updateFrame || !lastOffsets.ContainsKey(c))
                    {
                        lastOffsets[c] = new Vector3(
                            Random.Range(-amplitude, amplitude),
                            Random.Range(-amplitude, amplitude),
                            0
                        );
                    }

                    Vector3 offset = lastOffsets[c];

                    vertices[vertexIndex + 0] = originalVertices[materialIndex][vertexIndex + 0] + offset;
                    vertices[vertexIndex + 1] = originalVertices[materialIndex][vertexIndex + 1] + offset;
                    vertices[vertexIndex + 2] = originalVertices[materialIndex][vertexIndex + 2] + offset;
                    vertices[vertexIndex + 3] = originalVertices[materialIndex][vertexIndex + 3] + offset;
                }
            }
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible)
                    continue;

                int c = charInfo.index;

                if (targetLetters_scale.Contains(c))
                {
                    int materialIndex = charInfo.materialReferenceIndex;
                    int vertexIndex = charInfo.vertexIndex;
                    Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                    Vector3 charMid = (vertices[vertexIndex + 0] + vertices[vertexIndex + 2]) / 2f;

                    float scaleFactor = 1f + Mathf.Sin(Time.time * frequency_ + i) * size;

                    for (int j = 0; j < 4; j++)
                    {
                        vertices[vertexIndex + j] = charMid + (vertices[vertexIndex + j] - charMid) * scaleFactor;
                    }
                }
            }
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible)
                    continue;

                int c = charInfo.index;

                if (targetLetters_ondulation.Contains(c))
                {
                    int materialIndex = charInfo.materialReferenceIndex;
                    int vertexIndex = charInfo.vertexIndex;
                    Vector3[] vertices = textInfo.meshInfo[materialIndex].vertices;

                    // Sauvegarde dynamique des positions originales
                    if (originalVertices[materialIndex].Length < vertices.Length)
                    {
                        originalVertices[materialIndex] = new Vector3[vertices.Length];
                    }

                    originalVertices[materialIndex][vertexIndex + 0] = vertices[vertexIndex + 0];
                    originalVertices[materialIndex][vertexIndex + 1] = vertices[vertexIndex + 1];
                    originalVertices[materialIndex][vertexIndex + 2] = vertices[vertexIndex + 2];
                    originalVertices[materialIndex][vertexIndex + 3] = vertices[vertexIndex + 3];

                    Vector3 offset = new Vector3(
                        0,
                        Mathf.Sin(Time.time * ondulation_frequency + i) * ondulation_amplitude,
                        0);

                    vertices[vertexIndex + 0] = originalVertices[materialIndex][vertexIndex + 0] + offset;
                    vertices[vertexIndex + 1] = originalVertices[materialIndex][vertexIndex + 1] + offset;
                    vertices[vertexIndex + 2] = originalVertices[materialIndex][vertexIndex + 2] + offset;
                    vertices[vertexIndex + 3] = originalVertices[materialIndex][vertexIndex + 3] + offset;
                }
            }
            for (int i = 0; i < textInfo.characterCount; i++)
            {
                TMP_CharacterInfo charInfo = textInfo.characterInfo[i];
                if (!charInfo.isVisible)
                    continue;

                int c = charInfo.index;

                if (targetLetters_color.Contains(c))
                {
                    int materialIndex = charInfo.materialReferenceIndex;
                    int vertexIndex = charInfo.vertexIndex;

                    Color32[] colors = textInfo.meshInfo[materialIndex].colors32;
                    float value = (Mathf.Sin(Time.time * glow_speed) + 1f) / 2f;
                    colors[vertexIndex + 0] = Color.Lerp(color, color_glow, value);
                    colors[vertexIndex + 1] = Color.Lerp(color, color_glow, value);
                    colors[vertexIndex + 2] = Color.Lerp(color, color_glow, value);
                    colors[vertexIndex + 3] = Color.Lerp(color, color_glow, value);

                }
            }


            for (int i = 0; i < textInfo.meshInfo.Length; i++)
                {
                    textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
                    textMeshPro.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
                }

                yield return null;
  

        }
    }
    public void RefreshText()
    {
        coroutineStarted = false;
        textMeshPro.ForceMeshUpdate();
        textInfo = textMeshPro.textInfo;
    }
    public void reset_()
    {
        targetLetters_movement.Clear();
        targetLetters_scale.Clear();
        targetLetters_color.Clear();
        targetLetters_ondulation.Clear();
        RefreshText();
    }
}