using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchBullet : Bullet
{
    [HideInInspector]
    public Transform TargetToSearch;

    protected override void UpdateLogic()
    {
        if (TargetToSearch == null)
        {
            TargetToSearch = GameObject.Find("Ellen PLAYER").transform;
        }
        Vector3 dirToTarget = (TargetToSearch.position + Vector3.up - transform.position).normalized;
        float dot = Vector3.Dot(transform.forward, dirToTarget);
        Debug.Log(dot);
        if (dot > 0.4f)
        {
            Quaternion toTargetRot = Quaternion.LookRotation(dirToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, toTargetRot, 0.04f);
        }
        base.UpdateLogic();
    }
}
