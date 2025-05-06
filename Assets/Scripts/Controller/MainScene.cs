using UnityEngine;

public class MainScene : MonoSingleton<MainScene>
{
    [SerializeField] private Player player;
    void Start()
    {
        InputController.Instance.Initialize();
        player.Set();
    }

    // Update is called once per frame
    
}
