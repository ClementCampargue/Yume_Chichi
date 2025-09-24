using UnityEngine;

public class SC_respawn_point : MonoBehaviour
{


    void Start()
    {
        if (PlayerPrefs.GetInt("Respawn") == 1)
        {
            PlayerPrefs.SetInt("Respawn", 0);
            GameObject.FindWithTag("Player").transform.position = transform.position;
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
