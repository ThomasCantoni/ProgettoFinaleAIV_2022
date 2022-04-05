using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHPBarScript : MonoBehaviour
{
    public GameObject camera;
    void Start()
    {
        camera = GameObject.Find("Camera");
    }

    // Update is called once per frame
    void Update()
    {
        this.transform.LookAt(camera.transform);
    }
}
