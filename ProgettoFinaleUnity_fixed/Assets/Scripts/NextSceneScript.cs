using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Events;
public class NextSceneScript : MonoBehaviour
{
    [SerializeField]
    public string SceneToLoad;
    public UnityAction<Collider> test;
    public int KeyToRemove=0;
    
    private void OnTriggerEnter(Collider other)
    {
        other.gameObject.GetComponent<PlayerControllerSecondVersion>().RemoveKey(KeyToRemove);
        SceneManager.LoadScene(SceneToLoad,LoadSceneMode.Single);
    }
}
