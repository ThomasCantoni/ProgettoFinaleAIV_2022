using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class CheckpointScript : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        //Debug.Log(other.gameObject.name);
        PlayerControllerSecondVersion PCSV = other.GetComponent<PlayerControllerSecondVersion>();
        PCSV.UseLatestData = true;
        PlayerData newData = new PlayerData(PCSV);
        SaveManager.SavePlayer(newData);
        other.GetComponent<PlayerControllerSecondVersion>().PlayerData = SaveManager.LastSave;
        this.GetComponent<AudioSource>().Play();
       // System.Diagnostics.Stopwatch s = new System.Diagnostics.Stopwatch();
        //s.Start();
        GameObject go = FindGameObjectWithInactiveTag("Game Saved");
        //s.Stop();
        //Debug.LogError(s.ElapsedMilliseconds * 0.001);
        go.SetActive(true);
        StartCoroutine(DisableText(go));
    }
    public GameObject FindGameObjectWithInactiveTag(string tag)
    {

        Transform[] t = Resources.FindObjectsOfTypeAll<Transform>() as Transform[];
        for (int i = 0; i < t.Length; i++)
        {
            if (t[i].hideFlags == HideFlags.None)
            {
                if (t[i].CompareTag(tag))
                {
                    return t[i].gameObject;
                }
            }
        }
        return null;
    }
    public IEnumerator DisableText(GameObject g)
    {
        yield return new WaitForSeconds(2f);
        g.SetActive(false);
    }
}
