using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SC_looparoundworld_sceneloader : MonoBehaviour
{
    public Vector3 sceneOffset = new Vector3(100, 0, 0);
    public bool isDuplicate = false; // Flag à cocher sur les instances dupliquées

    void Start()
    {
        // Ne pas charger si cette instance est dans une scène dupliquée
        if (!isDuplicate)
        {
            LoadDuplicateScene();
        }
    }

    public void LoadDuplicateScene()
    {
        StartCoroutine(LoadSceneAdditiveWithOffset(SceneManager.GetActiveScene().name, sceneOffset));
    }

    IEnumerator LoadSceneAdditiveWithOffset(string sceneName, Vector3 offset)
    {
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName, LoadSceneMode.Additive);

        while (!asyncLoad.isDone)
            yield return null;

        // Obtenir la scène nouvellement chargée
        Scene newScene = SceneManager.GetSceneAt(SceneManager.sceneCount - 1);

        // Activer la scène pour que ses objets soient accessibles
        SceneManager.SetActiveScene(newScene);

        GameObject[] rootObjects = newScene.GetRootGameObjects();

        foreach (GameObject obj in rootObjects)
        {
            obj.transform.position += offset;

            // Important : empêcher que le duplicateur dans la scène copiée duplique à nouveau
            SC_looparoundworld_sceneloader duplicator = obj.GetComponent<SC_looparoundworld_sceneloader>();
            if (duplicator != null)
            {
                duplicator.isDuplicate = true;
            }
        }

        Debug.Log("Scene duplicated with offset.");
    }
}