using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class AudioScript : MonoBehaviour {

    public AudioSource ads;
    //public AudioSource ads2;
    public Button audioButton;
    public Sprite play, mute;
  

    // Use this for initialization
    void Start () {

        //Button btn = audioButton;
        //btn.onClick.AddListener(playAudio);
        //GameObject GOb = GameObject.Find("playButton");
        //Button btn = GOb.GetComponent<Button>();
       
    }
	
    public void playAudio()
    {
        Button btn = audioButton;
        if (!ads.isPlaying) {
            ads.Play();
            btn.GetComponent<Image>().sprite = mute;
        }
        else
        {
            ads.Stop();
            btn.GetComponent<Image>().sprite = play;
        }
        // SceneManager.LoadScene(1);
        //ScreenCapture.CaptureScreenshot();
        //ScreenCapture.CaptureScreenshot(Application.streamingAssetsPath + "/ScreenShot.png", 0);
       //ads2.Play();

    }

    // Update is called once per frame
    void Update () {
	    
	}
}
