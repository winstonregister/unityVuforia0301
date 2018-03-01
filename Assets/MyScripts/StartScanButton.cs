using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class StartScanButton : MonoBehaviour
{


    //    public UILabel MyLabel;

    //    public UIInput MyInput;

    //public Button myButton;
    public InputField myInputField;
    //public Text inputText;
   // public Text verifyText;

    //public Image myImage;
    //public Sprite verifiHintSprite;
    public Sprite[] images;
    public Image img;

    void Start()
    {
        //Button btn = myButton.GetComponent<Button>();
        //  btn.onClick.AddListener(GoToNextScene);

        //InputField myInputField2 = myInputField.GetComponent<InputField>();
        // Text inputText = verifyText;      
        //inputText.text = myInputField.text;
        //  Debug.Log("hello world11"+inputText.text);
        //    this.GetComponent<Button>().name = 

        //InputFieldToLabel();
    }


    public void InputFieldToLabel()
    {
        //Text inputText; // = verifyText;
        //inputText.text = myInputField.text;
        Debug.Log("验证码：" + myInputField.text);
        
        if (myInputField.text == "123456")
        {
            //inputText.text = "验证码正确，欢迎使用AR卡片";
            SceneManager.LoadScene(1);
        }
        else 
        {
            //inputText.text = "验证码有误，请再次输入";
            //myImage.overrideSprite = Resources.Load("MyPic/verificationError", typeof(Sprite)) as Sprite; // Image/pic 在 Assets/Resources/
            //verifiHintSprite = Resources.Load("MyPic/verificationError", typeof(Sprite)) as Sprite;
            img.sprite = images[1];
        }

    }

    /*
        public void ChangeStrByInput()
        {
            if(MyInput != null)
            {
                GetComponent<UILabel>().text = MyInput.value;
            }                                                                                                                                           
        }
    */

    //  void GoToNextScene()
    //   {
    //       SceneManager.LoadScene(1);
    //   }
    //
}
