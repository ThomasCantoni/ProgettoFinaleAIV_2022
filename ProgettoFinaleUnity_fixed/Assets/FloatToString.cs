using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using TMPro;

public class FloatToString : MonoBehaviour
{
    // Start is called before the first frame update
    public void FloatToStr(float n)
    {
       
       GetComponent<TMP_Text>().text = n.ToString();
        
    }
}
