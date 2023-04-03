using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using Cinemachine;
using Unity.Netcode;

public class WeaponController : NetworkBehaviour
{
    PlayerInput playerInput;
    CameraController cameraController;

    [Tooltip("The point where the held item should rotate while aiming. If not set, held item will not rotate.")]
    public GameObject hingePoint;
    [Tooltip("The point where projectiles or traced shots should originate. If not set, origin will be the object's transform.")]
    public GameObject firePoint;

    public ParticleSystem[] MuzzleFlash;
    public ParticleSystem[] HitEffect;

    [Tooltip("Damage dealt to hit target")]
    public float weaponDamage = 25.0f;
    [Tooltip("Max range that weapon will do damage")]
    public float weaponMaxRange = Mathf.Infinity;
    // Damage falloff range start
    public float falloffStartDistance = Mathf.Infinity;

    // Max distance and scaling only applies if > start distance. Otherwise flat multiplier on falloff.
    public float falloffMaxDistance = 0;
    public float falloffMult = 0.5f;

    public float fireRate = 0.1f;

    public float verticalRecoilMin = 0f;
    public float verticalRecoilMax = 3.0f;
    public float verticalRecoilPerShot = 1f;

    public float horizontalRecoilMin = 0f;
    public float horizontalRecoilMax = 3.0f;
    public float horizontalRecoilPerShot = 1f;

    public float horizontalMovementRecoil = 0.5f;
    public float verticalMovementRecoil = 0.5f;

    public float recoilRecovery = 2.5f;

    public float tracerMaxLength = 20f;

    private float _fireTimer = Mathf.Infinity;
    //private float _burstTimer = 0;
    private bool _firing = false;
    private bool _moving = false;

    private LineRenderer lineRenderer;

    private float currentVerticalRecoil = 0f;
    private float currentHorizontalRecoil = 0f;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponentInParent<PlayerInput>();
        if (playerInput != null)
        {
            playerInput.actions["Fire"].performed += OnFire;
            playerInput.actions["Move"].performed += OnMove;
        }
        cameraController = GetComponentInParent<CameraController>();
        if (cameraController == null)
        {
            cameraController = GetComponent<CameraController>();
        }
        lineRenderer = GetComponent<LineRenderer>();
    }

    public override void OnDestroy()
    {
        if (playerInput != null)
        {
            playerInput.actions["Fire"].performed -= OnFire;
            playerInput.actions["Move"].performed -= OnMove;
        }
    }
    private void Shoot(Vector3 aimLocation)
    {
        Debug.Log("Shoot for " + transform.root.name);
        if (IsServer)
            Debug.Log("Server");
        else
            Debug.Log("Client");

        foreach (var particle in MuzzleFlash)
        {
            particle.Emit(1);
        }


        Transform fireTransform = (firePoint != null) ? firePoint.transform : gameObject.transform;
        Vector3 originalForward = fireTransform.forward;

        Vector3 firePosition = fireTransform.position;
        Vector3 fireVector = aimLocation - firePosition;
        fireTransform.forward = fireVector;

        float verticalRandomRecoil = Random.Range(-currentVerticalRecoil, currentVerticalRecoil);
        float horizontalRandomRecoil = Random.Range(-currentHorizontalRecoil, currentHorizontalRecoil);

        Vector3 currentEuler = fireTransform.eulerAngles;
        currentEuler.x += verticalRandomRecoil;
        currentEuler.y += horizontalRandomRecoil;

        fireTransform.eulerAngles = currentEuler;
        fireVector = fireTransform.forward;

        fireTransform.forward = originalForward;

        // Draw tracers
        lineRenderer.SetPosition(0, firePosition);
        lineRenderer.SetPosition(1, firePosition + fireVector * tracerMaxLength);
        lineRenderer.enabled = true;

        RaycastHit hitInfo;
        bool hit = Physics.Raycast(firePosition, fireVector, out hitInfo, weaponMaxRange);
        if (hit)
        {
            // Hit visuals
            foreach (var particle in HitEffect)
            {
                particle.transform.position = hitInfo.point;
                particle.transform.forward = hitInfo.normal;
                particle.Emit(1);
            }

            // This version should find a damageable target in the whole tree.
            IDamageable damageScript = findIDamageable(hitInfo.transform);

            //IDamageable damageScript = hitInfo.transform.root.GetComponent<IDamageable>();

            if (damageScript != null)
            {
                float finalDamage = weaponDamage;
                if (hitInfo.distance > falloffStartDistance)
                {
                    float falloffFinalMult = falloffMult;
                    float falloffScale = falloffMaxDistance - falloffStartDistance;
                    if (falloffScale > 0 && hitInfo.distance < falloffMaxDistance)
                    {
                        falloffFinalMult *= (hitInfo.distance - falloffStartDistance) / falloffScale;
                    }

                    finalDamage *= 1 - falloffFinalMult;
                }
                // Note - possibly add overloads for additional info passed in to Damage function e.g. hit location for extra headshot damage.
                damageScript.Damage(finalDamage);
            }

            if (hitInfo.distance < tracerMaxLength)
            {
                lineRenderer.SetPosition(1, hitInfo.point);
            }
        }
    }

    [ServerRpc(RequireOwnership = false)]
    private void ShootServerRpc(Vector3 aimLocation)
    {
        Debug.Log("Name: " + transform.root.gameObject.name);
        Shoot(aimLocation);
    }

    private IDamageable findIDamageable(Transform search)
    {
        IDamageable damageTarget = null;
        // Search up the tree to find damage script
        while (search && damageTarget == null)
        {
            damageTarget = search.gameObject.GetComponent<IDamageable>();
            search = search.parent;
        }
        return damageTarget;
    }

    // Update is called once per frame
    void Update()
    {
        if (IsOwner && IsClient)
        {
            float dt = Time.deltaTime;
            Vector3 aimLocation = cameraController.getAimLocation();

            // Count time between shots
        
            if (_fireTimer < fireRate)
            {
                _fireTimer += dt;
            }
            else
            {
                lineRenderer.enabled = false;
            }

            // Only do recoil / aiming if the held weapon has a hingePoint chosen.
            if (hingePoint != null)
            {
                // Aim the held item towards the crosshair's world location.
                hingePoint.transform.LookAt(aimLocation);
            }

            if (_firing)
            {
                if (_fireTimer >= fireRate)
                {
                    Shoot(aimLocation);
                    ShootServerRpc(aimLocation);
                    //Shoot(aimLocation);

                    // Add recoil
                    currentHorizontalRecoil += horizontalRecoilPerShot;
                    currentHorizontalRecoil = Mathf.Min(currentHorizontalRecoil, horizontalRecoilMax);

                    currentVerticalRecoil += verticalRecoilPerShot;
                    currentVerticalRecoil = Mathf.Min(currentVerticalRecoil, verticalRecoilMax);

                    _fireTimer = 0;
                    }
            }
            // Recoil recovery. Recoil won't go below movement recoil values if we're still moving.
            float currentMinHorizontal = horizontalRecoilMin;
            float currentMinVertical = verticalRecoilMin;

            if (_moving)
            {
                currentMinHorizontal += horizontalMovementRecoil;
                currentMinVertical += verticalMovementRecoil;
            }

            // Horizontal recoil recovery
            if (currentHorizontalRecoil > currentMinHorizontal)
            {
                currentHorizontalRecoil -= recoilRecovery * dt;
            }
            else
            {
                currentHorizontalRecoil = currentMinHorizontal;
            }

            // Vertical recoil recovery
            if (currentVerticalRecoil > currentMinVertical)
            {
                currentVerticalRecoil -= recoilRecovery * dt;
            }
            else
            {
                currentVerticalRecoil = currentMinVertical;
            }

        }
    }

    void OnFire(InputValue value)
    {
        _firing = value.isPressed;
    }

    void OnFire(InputAction.CallbackContext value)
    {
        _firing = value.action.IsPressed();
    }

    void OnMove(InputValue value)
    {
        _moving = value.Get<Vector2>() != Vector2.zero;
    }

    void OnMove(InputAction.CallbackContext value)
    {
        _moving = value.ReadValue<Vector2>() != Vector2.zero;
    }
}
