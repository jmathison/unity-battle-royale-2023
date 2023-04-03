using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : NetworkBehaviour
{
    [SerializeField] private Transform _interactionPoint;
    [SerializeField] private float _interactionPointRadius = 0.5f;
    [SerializeField] private LayerMask _interactableMask;
    [SerializeField] private InteractionPromptUI _interactionPromptUI;
    [SerializeField] private InputActionReference _interactionInput;

    private readonly Collider[] _colliders = new Collider[3];
    private int _numFound;

    private IInteractable _interactable;

    // Start is called at the start of the game
    private void Start()
    {
        _interactionInput.action.performed += (context) => OnInteractPressed();

    }

    void OnInteractPressed()
    {
        if (_interactable != null)
        {
            _interactable.Interact(this);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (!IsOwner && IsClient)
            return;
        _numFound = Physics.OverlapSphereNonAlloc(_interactionPoint.position, _interactionPointRadius, _colliders, _interactableMask);

        if (_numFound > 0)
        {
            _interactable = _colliders[0].GetComponent<IInteractable>();

            if (_interactable != null && !_interactionPromptUI.IsDisplayed)
                 _interactionPromptUI.SetUp(_interactable.InteractionPrompt);
        } 
        else
        {
            if (_interactable != null)
            {
                _interactable = null;
            }
            if (_interactionPromptUI)
            {
                _interactionPromptUI.Close();
            }
        }

    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(_interactionPoint.position, _interactionPointRadius);
    }
}
