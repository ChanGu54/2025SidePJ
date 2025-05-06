using UnityEngine;

public class ExitObjective : LevelObjective
{
    [Header("Exit Settings")]
    [SerializeField] private bool requiresKey = false;
    [SerializeField] private string requiredKeyId = "";
    
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            if (!requiresKey)
            {
                IsComplete = true;
            }
            else
            {
                var inventory = other.GetComponent<PlayerInventory>();
                if (inventory != null && inventory.HasKey(requiredKeyId))
                {
                    IsComplete = true;
                }
            }
        }
    }
}

public class PlayerInventory : MonoBehaviour
{
    private System.Collections.Generic.List<string> collectedKeys = new System.Collections.Generic.List<string>();
    
    public void AddKey(string keyId)
    {
        if (!collectedKeys.Contains(keyId))
        {
            collectedKeys.Add(keyId);
        }
    }
    
    public bool HasKey(string keyId)
    {
        return collectedKeys.Contains(keyId);
    }
} 