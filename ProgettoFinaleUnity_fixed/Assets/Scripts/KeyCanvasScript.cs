using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KeyCanvasScript : MonoBehaviour
{
    
    public List<int> KeysTaken;
    
    public void AddKey(int ID)
    {
        if(KeysTaken.Contains(ID))
        {
            Debug.LogWarning("Canvas already has key!");
            return;
        }
            KeysTaken.Add(ID);
            transform.GetChild(ID).gameObject.SetActive(true);

    }
    public void RemoveKey(int ID)
    {
        if (KeysTaken.Contains(ID))
        {
           int index=  KeysTaken.IndexOf(ID);
            KeysTaken.RemoveAt(index);
            transform.GetChild(ID).gameObject.SetActive(false);
        }
    }
}
