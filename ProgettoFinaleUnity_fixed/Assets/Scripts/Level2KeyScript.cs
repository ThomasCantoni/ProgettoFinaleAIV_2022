using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
public class Level2KeyScript : MonoBehaviour
{
    public int ID_KeyToRemove;
    public GameObject KeyToActivate;
    public ParticleSystem ParticleSystem;
    public AudioClip Loop, Insert;
    public AudioSource AS;
    public bool IsKeyInserted = false;
    public UnityEvent Actions;
    
    private void OnTriggerEnter(Collider other)
    {
        PlayerControllerSecondVersion PCSV = other.GetComponent<PlayerControllerSecondVersion>();
        if (PCSV.PlayerData.keysTaken.Contains(ID_KeyToRemove) && !IsKeyInserted)
        {
            IsKeyInserted = true;
            PCSV.RemoveKey(ID_KeyToRemove);
            ParticleSystem.Play();
            KeyToActivate.SetActive(true);
            AS.clip = Insert;
            AS.Play();
            Actions.Invoke();
            StartCoroutine(ChangeClip());
        }
        else
        {
            Debug.Log("Key not found, nothing to remove");
        }
    }
    public IEnumerator ChangeClip()
    {
        yield return new WaitForSeconds(Insert.length);
        AS.clip = Loop;
        AS.Play();
    }
}
