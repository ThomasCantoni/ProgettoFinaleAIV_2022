using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyScript : MonoBehaviour
{
    [SerializeField]
    public int KeyID = 0;
    AudioSource AS;
    private void Start()
    {
        AS = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerControllerSecondVersion>().PlayerData.TakenKeysID.Add(this.KeyID);
        AS.Play();
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            Destroy(children[i].gameObject);
        }
        StartCoroutine(DestroyKey());
    }
    public IEnumerator DestroyKey()
    {
        yield return new WaitForSeconds(AS.clip.length);
        Destroy(this.gameObject);
    }
}
