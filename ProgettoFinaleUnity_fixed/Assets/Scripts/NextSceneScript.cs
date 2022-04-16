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
    
    private void OnTriggerEnter(Collider other)
    {
        SceneManager.LoadScene(SceneToLoad,LoadSceneMode.Single);
    }
}
