using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerBulletPoolMgr : MonoBehaviour
{
    Queue<GameObject> Bullets;

    public void AddBullet(GameObject bullet)
    {
        Bullets.Enqueue(bullet);
    }

    public void OnCreation()
    {
        Bullets = new Queue<GameObject>(50);
    }

    public GameObject SpawnObj(Vector3 pos, Quaternion rot)
    {
        if (Bullets.Peek().activeSelf) return null;

        GameObject bullet = Bullets.Dequeue();
        bullet.SetActive(true);
        bullet.transform.position = pos;
        bullet.transform.rotation = rot;
        Bullets.Enqueue(bullet);
        return bullet;
    }
}
