using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SearchBullet : Bullet
{
    [HideInInspector]
    public Transform TargetToSearch;

    private Vector3 initialBulletForward;
    private bool firstFrame = true;

    protected override void OnStartLife()
    {
        base.OnStartLife();
        firstFrame = true;
    }

    protected override void UpdateLogic()
    {
        if (TargetToSearch == null)
        {
            TargetToSearch = GameObject.Find("Ellen PLAYER").transform;
        }

        if (firstFrame)
        {
            firstFrame = false;
            initialBulletForward = transform.forward;
        }
        Vector3 dirToTarget = (TargetToSearch.position + Vector3.up - transform.position).normalized;
        float dot = Vector3.Dot(initialBulletForward, dirToTarget);
        if (dot > 0.2f)
        {
            Quaternion toTargetRot = Quaternion.LookRotation(dirToTarget);
            transform.rotation = Quaternion.Slerp(transform.rotation, toTargetRot, 0.1f);
        }
        base.UpdateLogic();
    }
}
