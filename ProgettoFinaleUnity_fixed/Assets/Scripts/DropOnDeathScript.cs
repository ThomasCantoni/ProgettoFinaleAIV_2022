using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DropScript : MonoBehaviour
{
    [SerializeField]
    public List<GameObject> ObjectsToDrop;

    public float Timer = 0f;
    // Start is called before the first frame update
    public void Drop()
    {
        Timer -= Time.deltaTime;
        if(Timer <=0f)
        {
            if(ObjectsToDrop.Count > 1)
            {
                for (int i = 0; i < ObjectsToDrop.Count; i++)
                {
                    GameObject go = Instantiate(ObjectsToDrop[i]);
                    go.transform.position = this.transform.position;
                    go.transform.position += ((Vector3)Random.insideUnitCircle)*1.2f;
                }
            }
            else
            {
                GameObject go = Instantiate(ObjectsToDrop[0]);

            }
        }
    }
}
