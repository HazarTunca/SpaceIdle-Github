using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(PlayerInput))]
public class PlayerInputController : MonoBehaviour
{
#region variables

    public bool isTapped;
    
#endregion

#if UNITY_EDITOR_WIN
    public void OnWindows(InputValue value) => MouseTap(value.isPressed);
#endif

#if UNITY_ANDROID
    public void OnAndroid(InputValue value) => ScreenTap(value.isPressed);
#endif

    private void MouseTap(bool newAction) => isTapped = newAction;
    private void ScreenTap(bool newAction) => isTapped = newAction;
}
