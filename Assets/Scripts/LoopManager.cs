using UnityEngine;
using System.Collections.Generic;

public class LoopManager : MonoBehaviour
{
    [Header("Loop Settings")]
    [SerializeField] private float loopDuration = 10f;
    [SerializeField] private int maxLoops = 3;
    
    [Header("Level Settings")]
    [SerializeField] private Transform levelContainer;
    [SerializeField] private GameObject[] levelPrefabs;
    
    private int currentLoop = 0;
    private float currentLoopTime = 0f;
    private bool isLooping = false;
    private List<GameObject> activeLevels = new List<GameObject>();
    private int currentLevelIndex = 0;
    
    private void Start()
    {
        LoadLevel(0);
    }
    
    private void Update()
    {
        if (isLooping)
        {
            currentLoopTime += Time.deltaTime;
            
            if (currentLoopTime >= loopDuration)
            {
                EndLoop();
            }
        }
    }
    
    public void StartLoop()
    {
        if (currentLoop < maxLoops)
        {
            isLooping = true;
            currentLoopTime = 0f;
            currentLoop++;
            
            // Notify all loopable objects
            foreach (var obj in FindObjectsOfType<MonoBehaviour>())
            {
                if (obj is ILoopable loopable)
                {
                    loopable.OnLoopStart();
                }
            }
        }
    }
    
    private void EndLoop()
    {
        isLooping = false;
        currentLoopTime = 0f;
        
        // Notify all loopable objects
        foreach (var obj in FindObjectsOfType<MonoBehaviour>())
        {
            if (obj is ILoopable loopable)
            {
                loopable.OnLoopEnd();
            }
        }
        
        // Check if level is complete
        if (CheckLevelCompletion())
        {
            LoadNextLevel();
        }
    }
    
    private void LoadLevel(int index)
    {
        // Clear existing level
        foreach (var level in activeLevels)
        {
            Destroy(level);
        }
        activeLevels.Clear();
        
        // Instantiate new level
        if (index < levelPrefabs.Length)
        {
            GameObject newLevel = Instantiate(levelPrefabs[index], levelContainer);
            activeLevels.Add(newLevel);
            currentLevelIndex = index;
        }
    }
    
    private void LoadNextLevel()
    {
        LoadLevel(currentLevelIndex + 1);
    }
    
    private bool CheckLevelCompletion()
    {
        // Check if all objectives are complete
        foreach (var objective in FindObjectsOfType<LevelObjective>())
        {
            if (!objective.IsComplete)
            {
                return false;
            }
        }
        return true;
    }
}

public interface ILoopable
{
    void OnLoopStart();
    void OnLoopEnd();
}

public abstract class LevelObjective : MonoBehaviour
{
    public bool IsComplete { get; protected set; }
    
    protected virtual void Start()
    {
        IsComplete = false;
    }
} 