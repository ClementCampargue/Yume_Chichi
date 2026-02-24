using UnityEngine;

public class SC_look_effect : MonoBehaviour
{
    [Header("References")]
    private Transform player;
    public Transform pupil;
    public SpriteRenderer pupilRenderer;

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

    [Header("Rotation Progress")]
    public float rotationSpeed = 1f;        // Vitesse accumulation
    public float maxRotationValue = 5f;     // Cap positif
    public float minRotationValue = -5f;    // Cap négatif
    public float heightMultiplier = 0.2f;   // Hauteur par unité
    public float interactionThreshold = -4f; // Seuil interaction

    public SpriteRenderer targetSpriteRenderer;
    public SpriteRenderer targetSpriteRenderer2;

    private Material targetMaterial;
    private Material targetMaterial2;
    private int offsetID;
    private int offsetID2;
    private float rotationValue = 0f;
    private float previousAngle;
    private Vector3 targetInitialPos;

    void Start()
    {
        player = GameObject.Find("Player").transform;
        initialLocalPos = pupil.localPosition;

        Vector3 toPlayer = player.position - transform.position;
        previousAngle = Mathf.Atan2(toPlayer.y, toPlayer.x);

        if (targetSpriteRenderer != null)
        {
            targetMaterial = targetSpriteRenderer.material; // crée une instance
            offsetID = Shader.PropertyToID("offset_");
        }

        if (targetSpriteRenderer2 != null)
        {
            targetMaterial2 = targetSpriteRenderer2.material; // crée une instance
            offsetID2 = Shader.PropertyToID("offset_");
        }
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



        // ===== Détection rotation autour =====
        Vector3 toPlayerCenter = player.position - transform.position;
        float currentAngle = Mathf.Atan2(toPlayerCenter.y, toPlayerCenter.x);

        float deltaAngle = Mathf.DeltaAngle(previousAngle * Mathf.Rad2Deg, currentAngle * Mathf.Rad2Deg);
        previousAngle = currentAngle;

        // Accumulation selon sens
        rotationValue += deltaAngle * rotationSpeed * Time.deltaTime;

        // Clamp
        rotationValue = Mathf.Clamp(rotationValue, minRotationValue, maxRotationValue);

        // ===== Shader offset Y =====
        if (targetMaterial != null)
        {
            float heightOffset = rotationValue * heightMultiplier;

            Vector2 currentOffset = targetMaterial.GetVector(offsetID);
            currentOffset.y = heightOffset;

            targetMaterial.SetVector(offsetID, currentOffset);
        }
        if (targetMaterial2 != null)
        {
            float heightOffset = rotationValue * heightMultiplier;

            Vector2 currentOffset = targetMaterial2.GetVector(offsetID2);
            currentOffset.y = heightOffset;

            targetMaterial2.SetVector(offsetID2, currentOffset);
        }
        // ===== Interaction quand assez bas =====
        if (rotationValue <= interactionThreshold)
        {
            Debug.Log("Interaction possible !");
            // Ici tu peux appeler une fonction
            // Interact();
        }
    }
}