using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// A helper component to detect when the mobile UI brake button is pressed and held.
/// </summary>
public class BrakeButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool IsPressed { get; private set; }

    public void OnPointerDown(PointerEventData eventData)
    {
        IsPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        IsPressed = false;
    }

    private void OnDisable()
    {
        IsPressed = false;
    }
}
