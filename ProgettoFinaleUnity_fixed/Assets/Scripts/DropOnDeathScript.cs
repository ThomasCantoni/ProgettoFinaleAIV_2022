using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;


public class DropOnDeathScript : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> ObjectsToDrop;
    public IHittable hittable;
    public float Timer = 0f;
    UnityAction onDeath;

    private void Start()
    {
        onDeath = Initiate;
        hittable = GetComponent<IHittable>();
        System.Type t = hittable.GetType();
       
        if(t == typeof(Chomper))
        {
            Chomper c = GetComponent<Chomper>();
            c.OnDeath.AddListener(onDeath);
        }
        if (t == typeof(Gunner))
        {
            Gunner c = GetComponent<Gunner>();
            c.OnDeath.AddListener(onDeath);
        }
    }

    public IEnumerator Drop()
    {
        yield return new WaitForSeconds(Timer);
        if (ObjectsToDrop.Count > 1)
        {
            for (int i = 0; i < ObjectsToDrop.Count; i++)
            {
                GameObject go = Instantiate(ObjectsToDrop[i]);
                go.transform.position = this.transform.position;
                go.transform.position += ((Vector3)Random.insideUnitCircle) * 1.2f;
            }
        }
        else
        {
            GameObject go = Instantiate(ObjectsToDrop[0]);
            go.transform.position = this.transform.position;

        }
    }
    public void Initiate()
    {
        StartCoroutine(Drop());
    }
}
