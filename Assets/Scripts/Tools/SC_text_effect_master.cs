using System.Collections;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class SC_text_effect_master : MonoBehaviour
{
    public TMP_Text textMeshPro;
    [HideInInspector]public List<int> targetLetters = new List<int>();
    public float amplitude = 5f;
    public float frequency = 5f;

    private TMP_TextInfo textInfo;
    private Vector3[][] originalVertices;
    public string character_effect_start = "<";
    public string character_effect_end = "<";
    [HideInInspector] public bool coroutineStarted = false;
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
                if (!charInfo.isVisible)
                    continue;

                int c = charInfo.index;

                if (targetLetters.Contains(c))
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
                        Mathf.Sin(Time.time * frequency + i) * amplitude,
                        Mathf.Cos(Time.time * frequency + i) * amplitude,
                        0);

                    vertices[vertexIndex + 0] = originalVertices[materialIndex][vertexIndex + 0] + offset;
                    vertices[vertexIndex + 1] = originalVertices[materialIndex][vertexIndex + 1] + offset;
                    vertices[vertexIndex + 2] = originalVertices[materialIndex][vertexIndex + 2] + offset;
                    vertices[vertexIndex + 3] = originalVertices[materialIndex][vertexIndex + 3] + offset;
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
        targetLetters.Clear();
        RefreshText();
    }
}