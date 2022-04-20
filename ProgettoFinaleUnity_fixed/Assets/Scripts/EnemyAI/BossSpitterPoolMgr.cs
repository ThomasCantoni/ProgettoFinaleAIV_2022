using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossSpitterPoolMgr : MonoBehaviour
{
    public int ActiveSpitter
    {
        get
        {
            int count = 0;
            for (int i = 0; i < transform.childCount; i++)
            {
                if (transform.GetChild(i).gameObject.activeSelf)
                    count++;
            }
            return count;
        }
    }

    public Queue<GameObject> Spitters;

    public void AddSpitter(GameObject bullet)
    {
        Spitters.Enqueue(bullet);
    }

    public void OnCreation()
    {
        Spitters = new Queue<GameObject>(50);
    }

    public GameObject SpawnObj(Vector3 pos, Vector3 forward)
    {
        if (Spitters.Peek().activeSelf) return null;

        GameObject spitter = Spitters.Dequeue();
        spitter.SetActive(true);
        spitter.transform.position = pos;
        spitter.transform.forward = forward;
        Spitters.Enqueue(spitter);
        return spitter;
    }

    public void KillAllSpitters()
    {
        for (int i = 0; i < transform.childCount; i++)
        {
            GameObject spitter = transform.GetChild(i).gameObject;
            Destroy(spitter);
        }
        Spitters.Clear();
    }
}
