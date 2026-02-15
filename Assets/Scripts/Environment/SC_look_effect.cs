using UnityEngine;

public class SC_look_effect : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform pupil;
    private SpriteRenderer pupilRenderer;

    [Header("Movement")]
    public float radius = 0.2f;              // Distance max du centre
    public float cylinderStrength = 1f;      // Intensité effet cylindre
    public float verticalLimit = 0.05f;      // Petit mouvement vertical

    [Header("Squash Effect")]
    public float squashStrength = 0.5f;      // Puissance écrasement horizontal
    public float stretchStrength = 0.3f;     // Compensation verticale
    public float minSquash = 0.2f;           // Limite minimale

    [Header("Smoothing")]
    public float smoothSpeed = 10f;          // Vitesse interpolation

    [Header("Visibility")]
    public float hideStartY = 0f;            // Début disparition
    public float hideEndY = 0.3f;            // Complètement caché

    private Vector3 initialLocalPos;
    private Vector3 targetLocalPos;
    private Vector3 targetScale;

    void Start()
    {
        initialLocalPos = pupil.localPosition;
        pupilRenderer = pupil.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if (player == null) return;

        // Direction stable top-down
        Vector3 toPlayer = player.position - pupil.position;
        Vector2 dir = new Vector2(toPlayer.x, toPlayer.y).normalized;
        // ===== Effet cylindre horizontal =====
        float curvedX = Mathf.Sin(dir.x * Mathf.PI * 0.5f) * cylinderStrength;

        // Petit mouvement vertical
        float y = Mathf.Clamp(dir.y * verticalLimit, -verticalLimit, verticalLimit);

        targetLocalPos = initialLocalPos + new Vector3(curvedX * radius, y, 0f);

        // ===== Écrasement latéral type cylindre =====
        float sideFactor = Mathf.Abs(dir.x);
        sideFactor *= sideFactor; // accentuation

        float squashX = Mathf.Clamp(1f - sideFactor * squashStrength, minSquash, 1f);
        float stretchY = 1f + sideFactor * stretchStrength;

        targetScale = new Vector3(squashX, stretchY, 1f);

        // ===== Interpolation douce =====
        pupil.localPosition = Vector3.Lerp(pupil.localPosition, targetLocalPos, Time.deltaTime * smoothSpeed);
        pupil.localScale = Vector3.Lerp(pupil.localScale, targetScale, Time.deltaTime * smoothSpeed);

        // ===== Disparition progressive quand derrière =====
        float visibility = Mathf.Clamp01(1f - Mathf.InverseLerp(hideStartY, hideEndY, dir.y));

        Color c = pupilRenderer.color;
        c.a = visibility;
        pupilRenderer.color = c;
    }
}