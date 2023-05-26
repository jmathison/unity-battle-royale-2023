using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterController : MonoBehaviour
{
    // Start is called before the first frame update

    public float damageOnTouch = 1000;
    public float risingSpeed = 0f;

    public Vector3 moveDirection = new Vector3(0, 1, 0);

    private Vector3 startingScale;

    void Start()
    {
        startingScale = transform.localScale;
    }

    void OnTriggerStay(Collider collider)
    {
        if(transform.localScale == startingScale) {
            IDamageable damageTarget = findIDamageable(collider.gameObject.transform);

            // If we found a damage script
            if (damageTarget != null) {
                damageTarget.Damage(damageOnTouch);
            }
        }
        else {
            transform.localScale = startingScale;
        }
    }

    // Update is called once per frame
    void Update()
    {
        float dt = Time.deltaTime;

        Vector3 positionChange = moveDirection.normalized * risingSpeed * dt;

        gameObject.transform.position += positionChange;
        
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
}
