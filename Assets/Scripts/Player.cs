using UnityEngine;

public class Player : Person
{
    
    public void Set()
    {
        InputController.Instance.SubscribeHorizontal(Move);
    }

}
