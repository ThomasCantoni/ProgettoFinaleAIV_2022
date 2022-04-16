using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class KeyScript : MonoBehaviour
{
    [SerializeField]
    public int KeyID = 0;
    public Canvas canvasKey;
    AudioSource AS;
    public UnityEvent Event;
    private void Start()
    {
        AS = GetComponent<AudioSource>();
    }
    private void OnTriggerEnter(Collider other)
    {
        other.GetComponent<PlayerControllerSecondVersion>().PlayerData.keysTaken.Add(this.KeyID);
        if (KeyID == 0)
        {
            canvasKey.gameObject.SetActive(true);
        }
        else
        {
            canvasKey.gameObject.SetActive(false);
        }
        AS.Play();
        Transform[] children = GetComponentsInChildren<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            Destroy(children[i].gameObject);
        }
        StartCoroutine(DestroyKey());
    }
    private void Update()
    {
        if (canvasKey == null)
        {
            Transform t = GameObject.Find("Player second iteration").transform;
            canvasKey = t.GetChild(t.childCount - 1).GetComponent<Canvas>();
        }
    }
    public IEnumerator DestroyKey()
    {
        yield return new WaitForSeconds(AS.clip.length);
        Destroy(this.gameObject);
    }
}
