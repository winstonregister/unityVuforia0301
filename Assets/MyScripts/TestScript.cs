using UnityEngine;
using System.Collections;
using UnityEngine.UI;
public class TestScript : MonoBehaviour
{
    public Text txt;

    void Start()
    {

    }
    public void changeText()
    {
        txt.text = "ok";
    }


}