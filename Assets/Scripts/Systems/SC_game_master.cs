using TMPro;
using UnityEngine;

public class SC_game_master : MonoBehaviour
{
    public bool debug;
    public GameObject debugger;
    public TextMeshProUGUI fps_text;
    public TextMeshProUGUI save_slot;

    private float polling = 3f;
    private float time;
    private int framecount;
    public int targetfps =240;


   [HideInInspector] public string previous_scene;
    void Awake()
    {
        DontDestroyOnLoad(gameObject);

        if (debug)
        {
            Application.targetFrameRate = targetfps;
            debugger.SetActive(true);
        }
        else
        {
            Debug.unityLogger.logEnabled = false;
            Destroy(debugger);
        }
        save_slot.text = PlayerPrefs.GetInt("Save").ToString();
    }
    void Update()
    {
        time += Time.deltaTime;
        framecount++;
        if (time >= polling)
        {
            int frameRate = Mathf.RoundToInt(framecount / time);
            fps_text.text = frameRate.ToString();
            time -= polling;
            framecount = 0;

            if (frameRate > 100)
            {
                fps_text.color = Color.green;
            }

            if (frameRate <= 100 && frameRate >60 )
            {
                fps_text.color = Color.yellow;
            }

            if (frameRate <= 60)
            {
                fps_text.color = Color.red;
            }
        }

    }
}
