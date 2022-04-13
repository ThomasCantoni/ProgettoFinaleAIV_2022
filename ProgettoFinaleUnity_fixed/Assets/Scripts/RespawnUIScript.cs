using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Animations.Rigging;

public class RespawnUIScript : MonoBehaviour
{
    public GameObject Player;
    PlayerControllerSecondVersion PCSV;
    InverseKinematicsTest IKT;
    RigBuilder Rb;
    public Canvas UICanvas;
    public Canvas DeathCanvas;

    // Start is called before the first frame update
    void Start()
    {
        PCSV = Player.GetComponent<PlayerControllerSecondVersion>();
        IKT = Player.GetComponent<InverseKinematicsTest>();
        Rb = Player.GetComponent<RigBuilder>();
    }
    public void Respawn()
    {
        //PCSV.enabled = true;
        //IKT.enabled = true;
        
        PCSV.Respwan();
        PCSV.Anim.SetBool("isDeath", false);
        Cursor.visible = false;
        
        PCSV.GetComponent<CharacterController>().enabled = true;
        UICanvas.gameObject.SetActive(true);
        DeathCanvas.gameObject.SetActive(false);
    }

}
