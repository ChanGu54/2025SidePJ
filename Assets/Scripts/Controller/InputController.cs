using UnityEngine;
using UnityEngine.Events;

public class InputController : MonoSingleton<InputController>
{

    private float horizontalInput;

    private UnityAction<float> onHorizontal;

    public void Initialize()
    {

    }

    public void Update()
    {
        HandleMovement();
    }

    private void HandleMovement()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        onHorizontal?.Invoke(horizontalInput);
    }

    #region Subscribe
    public void SubscribeHorizontal(UnityAction<float> action)
    {
        onHorizontal += action;
    }

    public void UnsubscribeHorizontal(UnityAction<float> action)
    {
        onHorizontal -= action;
    }
    #endregion
}
