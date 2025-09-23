using UnityEngine;
using UnityEngine.UI;

public class SC_select_button : MonoBehaviour
{
    public Button toSelect;
    void OnEnable()
    {
        string[] joysticks = Input.GetJoystickNames();

        if (joysticks.Length > 0 && !string.IsNullOrEmpty(joysticks[0]))
        {
            toSelect.Select();
        }
    }

}
