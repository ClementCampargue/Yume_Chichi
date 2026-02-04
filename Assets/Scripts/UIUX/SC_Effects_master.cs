using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class SC_Effects_master : MonoBehaviour
{
    public GameObject defaultButton;
    bool IsUsingGamepad()
    {
        return Gamepad.current != null;
    }
    void OnEnable()
    {
        if (IsUsingGamepad())
        {
            EventSystem.current.SetSelectedGameObject(null);
            EventSystem.current.SetSelectedGameObject(defaultButton);
        }
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
