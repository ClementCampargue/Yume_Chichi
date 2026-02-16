using System.Collections.Generic;
using UnityEngine;

public class SC_hand_arm : MonoBehaviour
{
    public Transform hand;
    public float pointSpacing = 0.1f;

    private LineRenderer line;
    private List<Vector3> points = new List<Vector3>();

    void Start()
    {
        line = GetComponent<LineRenderer>();

        points.Clear();
        points.Add(hand.position);

        line.positionCount = 1;
        line.SetPosition(0, hand.position);
    }

    void Update()
    {
        if (hand == null) return;

        Vector3 currentHandPos = hand.position;

        // Ajouter un point si assez éloigné du dernier
        if (points.Count == 0 || Vector3.Distance(currentHandPos, points[points.Count - 1]) > pointSpacing)
        {
            points.Add(currentHandPos);
        }

        // Supprimer les points progressivement depuis la fin si la main se rapproche
        while (points.Count > 1 && Vector3.Distance(currentHandPos, points[points.Count - 2]) < pointSpacing)
        {
            points.RemoveAt(points.Count - 1);
        }

        line.positionCount = points.Count;
        line.SetPositions(points.ToArray());
    }
}