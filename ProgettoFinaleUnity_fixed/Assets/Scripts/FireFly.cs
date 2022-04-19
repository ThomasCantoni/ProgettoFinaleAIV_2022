using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class FireFly : MonoBehaviour, IHittable
{
    public GameObject fireFly;
    public GameObject TriggerToActivate;
    public int Health;
    public AnimationCurve MovementOverTime;
    public BoxCollider BoundingCollider;
    public float SpeedMultiplier;

    private Vector3 positionToGo = Vector3.zero;
    private Vector3 oldPositionToGo;
    private float timer;

    public virtual HittableType OnHit(Collider sender)
    {
        Health--;

        if (Health <= 0)
        {
            if (fireFly != null)
            {
                fireFly.SetActive(true);
            }
            this.gameObject.SetActive(false);
            if(TriggerToActivate != null)
            {
                TriggerToActivate.GetComponent<BoxCollider>().enabled = true;
            }
        }
        return HittableType.Other;
    }

    private void OnEnable()
    {
        if (positionToGo == Vector3.zero)
        {
            positionToGo = GetNewRandomPositionToGo(BoundingCollider);
            oldPositionToGo = transform.position;
        }
        timer = 0f;
    }

    private void Update()
    {
        timer += Time.deltaTime * SpeedMultiplier;
        if (timer > 1f)
        {
            oldPositionToGo = positionToGo;
            positionToGo = GetNewRandomPositionToGo(BoundingCollider);
            timer = 0f;
        }
        transform.position = Vector3.Lerp(oldPositionToGo, positionToGo, MovementOverTime.Evaluate(timer));
    }

    public Vector3 GetNewRandomPositionToGo(BoxCollider boxCollider)
    {
        Vector3 extents = boxCollider.size / 2f;
        Vector3 point = new Vector3(
            Random.Range(-extents.x, extents.x),
            Random.Range(-extents.y, extents.y),
            Random.Range(-extents.z, extents.z)
        );

        return boxCollider.transform.TransformPoint(point);
    }
}
