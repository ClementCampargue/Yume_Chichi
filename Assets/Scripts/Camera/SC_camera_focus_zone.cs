using UnityEngine;

public class SC_camera_focus_zone : MonoBehaviour
{

    private SC_camera_controller cam_controller;
    public Transform target_cam;        // Point sur lequel la caméra doit se focus
    public bool trigger_multiple_times; // Si la zone peut se réactiver
    private bool focus_active = false;

    void Start()
    {
        cam_controller = SC_camera_controller.instance;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && !focus_active)
        {
            StartFocus();
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player") && focus_active)
        {
            EndFocus();
        }
    }

    public void StartFocus()
    {
        focus_active = true;
        cam_controller.start_cutscene(target_cam); // Déclenche le focus smooth
    }

    public void EndFocus()
    {
        cam_controller.end_cutscene(); // Retour automatique vers le joueur

        if (trigger_multiple_times)
        {
            focus_active = false; // Réactive le trigger si nécessaire
        }
        else
        {
            this.enabled = false; // Désactive le trigger après usage
        }
    }
}