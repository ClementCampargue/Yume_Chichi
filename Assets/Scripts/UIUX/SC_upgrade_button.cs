using JetBrains.Annotations;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SC_upgrade_button : MonoBehaviour,
    IPointerEnterHandler, IPointerExitHandler,
    ISelectHandler, IDeselectHandler
{

    private Image image;
    public Sprite sprite;
    public string name;
    private TextMeshProUGUI name_tx;
    private TextMeshProUGUI description_tx;
    [TextArea(5, 20)]
    public string description;
    private SC_Pause_menu pause;
    private SC_Player_transformations player;
    public int index;
   // private GameObject bubble;
    void Start()
    {
        image = GameObject.Find("Upgrade_image").GetComponent<Image>();
        // bubble = GameObject.Find("Description_bubble");
        name_tx = GameObject.Find("Upgrade_name").GetComponent<TextMeshProUGUI>();
        pause = GameObject.Find("Pause_menu").GetComponent<SC_Pause_menu>();
        player = GameObject.Find("Player").GetComponent<SC_Player_transformations>();
        description_tx = GameObject.Find("Upgrade_description").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {

    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        Activate();
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        Deactivate();
    }

    public void OnSelect(BaseEventData eventData)
    {
        Activate();
    }

    public void OnDeselect(BaseEventData eventData)
    {
        Deactivate();
    }
    void Activate()
    {
        image.sprite = sprite;
        name_tx.text = name;
        description_tx.text = description;
        //  bubble.SetActive(true);
    }

    void Deactivate()
    {
        //    bubble.SetActive(false);

    }


    public void select()
    {
        player.transform_(index);
        pause.close();
    }

}
