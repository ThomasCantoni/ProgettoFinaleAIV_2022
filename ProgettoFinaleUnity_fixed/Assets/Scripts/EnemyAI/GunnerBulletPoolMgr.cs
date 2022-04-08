using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GunnerBulletPoolMgr : MonoBehaviour
{
    Queue<GameObject> Bullets;
    int freffa;

    void Awake()
    {
        Bullets = new Queue<GameObject>(50);
    }

    public void AddBullet(GameObject bullet)
    {
        Bullets.Enqueue(bullet);
    }

    private void Update()
    {
        Debug.Log(Bullets.Count);
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
