using TMPro;
using UnityEngine;

public class SC_text_effects : MonoBehaviour
{
    public float amplitude = 5f;
    public float frequency = 2f;
    public float speed = 2f;

    TMP_Text textMesh;
    TMP_TextInfo textInfo;
    Vector3[][] originalVertices;     // cache des vertices "originaux" par material
    int previousCharacterCount = -1;

    void Awake()
    {
        textMesh = GetComponent<TMP_Text>();
    }

    void Update()
    {
        // S'assure que TMP a bien rafraîchi sa géométrie avant d'y accéder
        textMesh.ForceMeshUpdate();
        textInfo = textMesh.textInfo;

        // Si le nombre de caractères ou la taille des buffers change => recache la géométrie d'origine
        if (originalVertices == null ||
            originalVertices.Length != textInfo.meshInfo.Length ||
            textInfo.characterCount != previousCharacterCount)
        {
            CacheOriginalVertices();
            previousCharacterCount = textInfo.characterCount;
        }
        else
        {
            // petit contrôle supplémentaire : si la taille d'un buffer a changé (rare), recache aussi
            for (int m = 0; m < textInfo.meshInfo.Length; m++)
            {
                if (originalVertices[m] == null || originalVertices[m].Length != textInfo.meshInfo[m].vertices.Length)
                {
                    CacheOriginalVertices();
                    break;
                }
            }
        }

        AnimateVertices();
    }

    void CacheOriginalVertices()
    {
        // copie sûre des vertices actuels (après ForceMeshUpdate)
        textInfo = textMesh.textInfo;
        originalVertices = new Vector3[textInfo.meshInfo.Length][];
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            var verts = textInfo.meshInfo[i].vertices;
            originalVertices[i] = new Vector3[verts.Length];
            System.Array.Copy(verts, originalVertices[i], verts.Length);
        }
    }

    void AnimateVertices()
    {
        float time = Time.time * speed;

        for (int i = 0; i < textInfo.characterCount; i++)
        {
            if (!textInfo.characterInfo[i].isVisible)
                continue;

            int matIndex = textInfo.characterInfo[i].materialReferenceIndex;
            int vertexIndex = textInfo.characterInfo[i].vertexIndex;

            Vector3[] destinationVertices = textInfo.meshInfo[matIndex].vertices;
            Vector3[] sourceVertices = originalVertices[matIndex];

            // protection supplémentaire au cas où l'index serait hors limite
            if (sourceVertices == null || vertexIndex + 3 >= sourceVertices.Length || vertexIndex + 3 >= destinationVertices.Length)
                continue;

            float wave = Mathf.Sin(time + i * frequency) * amplitude;

            for (int j = 0; j < 4; j++)
            {
                destinationVertices[vertexIndex + j] = sourceVertices[vertexIndex + j] + new Vector3(0, wave, 0);
            }
        }

        // applique les changements sur chaque mesh (material)
        for (int i = 0; i < textInfo.meshInfo.Length; i++)
        {
            textInfo.meshInfo[i].mesh.vertices = textInfo.meshInfo[i].vertices;
            textMesh.UpdateGeometry(textInfo.meshInfo[i].mesh, i);
        }
    }
}