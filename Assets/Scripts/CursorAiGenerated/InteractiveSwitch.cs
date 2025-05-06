using UnityEngine;

public class InteractiveSwitch : MonoBehaviour, ILoopable
{
    [Header("Switch Settings")]
    [SerializeField] private bool isActivated = false;
    [SerializeField] private GameObject[] connectedObjects;
    [SerializeField] private Sprite activatedSprite;
    [SerializeField] private Sprite deactivatedSprite;
    
    private SpriteRenderer spriteRenderer;
    private bool wasActivatedInPreviousLoop = false;
    
    private void Awake()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        UpdateVisuals();
    }
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            ToggleSwitch();
        }
    }
    
    private void ToggleSwitch()
    {
        isActivated = !isActivated;
        UpdateVisuals();
        UpdateConnectedObjects();
    }
    
    private void UpdateVisuals()
    {
        if (spriteRenderer != null)
        {
            spriteRenderer.sprite = isActivated ? activatedSprite : deactivatedSprite;
        }
    }
    
    private void UpdateConnectedObjects()
    {
        foreach (var obj in connectedObjects)
        {
            if (obj != null)
            {
                var loopable = obj.GetComponent<ILoopable>();
                if (loopable != null)
                {
                    if (isActivated)
                    {
                        loopable.OnLoopStart();
                    }
                    else
                    {
                        loopable.OnLoopEnd();
                    }
                }
            }
        }
    }
    
    public void OnLoopStart()
    {
        // Store the current state for the next loop
        wasActivatedInPreviousLoop = isActivated;
        
        // Reset to the state from the previous loop
        isActivated = wasActivatedInPreviousLoop;
        UpdateVisuals();
        UpdateConnectedObjects();
    }
    
    public void OnLoopEnd()
    {
        // No special behavior needed at loop end
    }
} 