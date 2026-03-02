using UnityEngine;

public class SC_look_effect : MonoBehaviour
{
    [Header("References")]
    private Transform player;
    public Transform pupil;
    public SpriteRenderer pupilRenderer;

    [Header("Movement")]
    public float radius = 0.2f;
    public float cylinderStrength = 1f;
    public float verticalLimit = 0.05f;

    [Header("Squash Effect")]
    public float squashStrength = 0.5f;
    public float stretchStrength = 0.3f;
    public float minSquash = 0.2f;

    [Header("Smoothing")]
    public float smoothSpeed = 10f;

    [Header("Visibility")]
    public float hideStartY = 0f;
    public float hideEndY = 0.3f;

    [Header("Rotation Progress")]
    public float rotationSpeed = 1f;
    public float maxRotationValue = 5f;
    public float minRotationValue = -5f;
    public float heightMultiplier = 0.2f;
    public float interactionThreshold = -4f;

    public SpriteRenderer targetSpriteRenderer;
    public SpriteRenderer targetSpriteRenderer2;

    private Material targetMaterial;
    private Material targetMaterial2;
    private int offsetID;
    private int offsetID2;
    private float rotationValue = 0f;
    private float previousAngleDeg;
    private Vector3 initialLocalPos;
    private Vector3 targetLocalPos;
    private Vector3 targetScale;

    public Transform object_trs;
    public SC_interactive_object interactive_object;

    private Vector3 startPos;
    private bool startPosSet = false;
    private bool interactionActivated = false; // nouveau flag pour bloquer rotation

    void Start()
    {
        player = SC_Player_controller.instance.transform;
        if (player == null)
        {
            Debug.LogError("Player not found!");
            return;
        }

        initialLocalPos = pupil.localPosition;

        Vector3 toPlayer = player.position - transform.position;
        previousAngleDeg = Mathf.Atan2(toPlayer.y, toPlayer.x) * Mathf.Rad2Deg;

        if (targetSpriteRenderer != null)
        {
            targetMaterial = targetSpriteRenderer.material;
            offsetID = Shader.PropertyToID("offset_");
        }

        if (targetSpriteRenderer2 != null)
        {
            targetMaterial2 = targetSpriteRenderer2.material;
            offsetID2 = Shader.PropertyToID("offset_");
        }
    }

    void Update()
    {
        if (player == null) return;

        // ===== Mouvement du regard =====
        Vector3 toPlayer = player.position - pupil.position;
        Vector2 dir = new Vector2(toPlayer.x, toPlayer.y).normalized;

        // Position locale cible
        Vector2 localOffset = dir * radius;
        localOffset.y = Mathf.Clamp(localOffset.y, -verticalLimit, verticalLimit);
        targetLocalPos = initialLocalPos + new Vector3(localOffset.x * cylinderStrength, localOffset.y, 0f);

        // Squash/stretch
        float sideFactor = Mathf.Abs(dir.x);
        sideFactor *= sideFactor;
        float squashX = Mathf.Clamp(1f - sideFactor * squashStrength, minSquash, 1f);
        float stretchY = 1f + sideFactor * stretchStrength;
        targetScale = new Vector3(squashX, stretchY, 1f);

        // Interpolation douce
        pupil.localPosition = Vector3.Lerp(pupil.localPosition, targetLocalPos, Time.deltaTime * smoothSpeed);
        pupil.localScale = Vector3.Lerp(pupil.localScale, targetScale, Time.deltaTime * smoothSpeed);

        // Disparition progressive
        float relativeY = player.position.y - transform.position.y;
        float visibility = Mathf.Clamp01(1f - Mathf.InverseLerp(hideStartY, hideEndY, relativeY));
        Color c = pupilRenderer.color;
        c.a = visibility;
        pupilRenderer.color = c;

        // ===== Rotation autour =====
        if (!interactionActivated) // bloquer rotation si interaction activée
        {
            Vector3 toPlayerCenter = player.position - transform.position;
            float currentAngleDeg = Mathf.Atan2(toPlayerCenter.y, toPlayerCenter.x) * Mathf.Rad2Deg;

            float deltaAngle = Mathf.DeltaAngle(previousAngleDeg, currentAngleDeg);
            previousAngleDeg = currentAngleDeg;

            rotationValue += deltaAngle * rotationSpeed;
            rotationValue = Mathf.Clamp(rotationValue, minRotationValue, maxRotationValue);
        }

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
            float heightOffset = rotationValue * heightMultiplier * 1.5f;
            Vector2 currentOffset = targetMaterial2.GetVector(offsetID2);
            currentOffset.y = heightOffset;
            targetMaterial2.SetVector(offsetID2, currentOffset);
        }

        // ===== Déplacement objet =====
        if (object_trs != null)
        {
            if (!startPosSet)
            {
                startPos = object_trs.position;
                startPosSet = true;
            }

            float heightOffset = rotationValue * heightMultiplier * 1.5f;
            Vector3 newPos = startPos;
            newPos.y -= heightOffset;
            object_trs.position = newPos;
        }

        // ===== Interaction =====
        if (rotationValue >= interactionThreshold && !interactionActivated)
        {
            interactive_object.enabled = true;
            interactionActivated = true; // bloquer la rotation maintenant
        }
    }
}