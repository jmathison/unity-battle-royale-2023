using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CircleController : MonoBehaviour
{
    public float shrinkTime = 30f;
    public float timeBetweenCircles = 60f;

    public float damagePerTick = 2f;
    public float damageTickRate = 1f;

    public float newCircleRatio = 0.5f;

    public float finalCircleRadius = 5f;

    [SerializeField]
    private GameObject sphereGraphics;
    private SphereCollider sphere;

    private float circleTimer = 0;
    private float shrinkTimer = 0;
    private bool shrinking = false;
    private float damageTimer = 0;

    private Vector3 startPosition;
    private Vector3 targetPosition;

    private float startRadius;
    private float targetRadius;

    private float graphicsMultiplier;

    private List<IDamageable> targetsOutsideCircle = new();

    private IDamageable[] targetsOutsideRing;
    // Start is called before the first frame update
    void Start()
    {
        sphere = GetComponent<SphereCollider>();
        boundsSettings();
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        if (!shrinking)
        {
            circleTimer += dt;
            if (circleTimer >= timeBetweenCircles)
            {
                shrinking = true;
                shrinkTimer = 0;
                pickTargetCircle();
            }
        }
        else
        {
            shrinkTimer += dt;
            float t = shrinkTimer / shrinkTime;
            Vector3 newPos = Vector3.Slerp(startPosition, targetPosition, t);
            float newRadius = Mathf.Lerp(startRadius, targetRadius, t);

            gameObject.transform.position = newPos;
            sphere.radius = newRadius;
            
            if(shrinkTimer >= shrinkTime)
            {
                gameObject.transform.position = targetPosition;
                sphere.radius = targetRadius;
                shrinking = false;
                circleTimer = 0;
            }

        }

        updateCircleGraphics();

        // damage anyone in the damage list if it's time
        damageTimer += dt;
        if(damageTimer >= damageTickRate)
        {
            damageTimer = 0;
            foreach(IDamageable target in targetsOutsideCircle)
            {
                target.Damage(damagePerTick);
            }
            targetsOutsideCircle.RemoveAll(i => i.IsDead());
        }
    }

    void pickTargetCircle()
    {

        float newRadius = Mathf.Max(sphere.radius * newCircleRatio, finalCircleRadius);
        Vector2 newCircle = randomPointInRadius(0, sphere.radius - newRadius);
        startPosition = gameObject.transform.position;
        startRadius = sphere.radius;

        targetPosition = startPosition;
        targetPosition.x += newCircle.x;
        targetPosition.z += newCircle.y;

        targetRadius = sphere.radius * newCircleRatio;

        
    }

    void updateCircleGraphics()
    {
        sphereGraphics.transform.localScale = Vector3.one * graphicsMultiplier * sphere.radius;
    }

    void boundsSettings()
    {
        float graphicsRadius = sphereGraphics.GetComponent<Renderer>().localBounds.size.x / 2;

        graphicsMultiplier = 1 / graphicsRadius;
    }

    Vector2 randomPointInRadius(float minRadius, float maxRadius)
    {
        Vector2 randomVector = Random.insideUnitCircle;

        return randomVector.normalized * minRadius + randomVector * (maxRadius - minRadius);
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

    private void OnTriggerEnter(Collider other)
    {
        IDamageable target = findIDamageable(other.transform);
        if (target != null)
        {
            targetsOutsideCircle.RemoveAll(x => x == target);
        }
    }

    private void OnTriggerExit(Collider other)
    {
        IDamageable target = findIDamageable(other.transform);
        if (target != null)
        {
            targetsOutsideCircle.Add(target);
        }
    }

}
