using Cinemachine;
using UnityEngine;
using UnityEngine.InputSystem;
using StarterAssets;
using Unity.Netcode;

[RequireComponent(typeof(PlayerInput))]
public class CameraController : NetworkBehaviour
{

    public InputActionReference aimAction;

    private PlayerInput _playerInput;
    private ThirdPersonController _thirdPersonController;
    private GameObject _virtualCameraObject;
    private Animator _cameraAnimator;

    private bool _aiming = false;


    public void Start()
    {
        if (!IsOwner)
            return;
        _playerInput = GetComponent<PlayerInput>();
        _thirdPersonController = GetComponent<ThirdPersonController>();
        _virtualCameraObject = _thirdPersonController.cameraInstance;
        _cameraAnimator = _virtualCameraObject.GetComponent<Animator>();

        //_thirdPersonController.LockCameraPosition = true;
        //Cursor.lockState = CursorLockMode.None;
        Cursor.lockState = CursorLockMode.Locked;

        GameObject crosshairCanvas = GameObject.FindWithTag("Crosshair");
        if (crosshairCanvas != null)
        {
            Cinemachine3rdPersonAim aimComponent = _virtualCameraObject.GetComponent<Cinemachine3rdPersonAim>();
            aimComponent.AimTargetReticle = crosshairCanvas.GetComponent<RectTransform>();
        }
    }

    public void Update()
    {

        

    }

    void OnAim(InputValue value)
    {
        _aiming = value.isPressed;
        _cameraAnimator.SetBool("isAiming", _aiming);
    }

    public Vector3 getAimLocation()
    {
        Cinemachine3rdPersonAim aimComponent = _virtualCameraObject.GetComponent<Cinemachine3rdPersonAim>();
        RectTransform aimTransform = aimComponent.AimTargetReticle;
        RaycastHit hitInfo;
        if(Physics.Raycast(Camera.main.ScreenPointToRay(aimTransform.position), out hitInfo))
        {
            return hitInfo.point;
        }

        return Camera.main.ScreenToWorldPoint(aimTransform.position);
        
    }

    public void ChangeFollow(Transform cameraFollow)
    {
        CinemachineVirtualCamera vCam = _virtualCameraObject.GetComponent<CinemachineVirtualCamera>();

        vCam.Follow = cameraFollow;
    }

    public void ChangeLookAt(Transform cameraLookAt)
    {
        CinemachineVirtualCamera vCam = _virtualCameraObject.GetComponent<CinemachineVirtualCamera>();

        vCam.LookAt = cameraLookAt;
    }

    public void ChangeFocus(Transform cameraFollow, Transform cameraLookAt)
    {
        CinemachineVirtualCamera vCam = _virtualCameraObject.GetComponent<CinemachineVirtualCamera>();

        ChangeFollow(cameraFollow);
        ChangeLookAt(cameraLookAt);
    }


}
