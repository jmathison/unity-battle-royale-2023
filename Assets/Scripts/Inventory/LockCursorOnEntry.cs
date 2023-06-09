using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class LockCursorOnEntry : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private PlayerInput _playerInput;
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (_playerInput != null)
            _playerInput.actions["Fire"].Disable() ;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (_playerInput != null)
            _playerInput.actions["Fire"].Enable();
    }
}
