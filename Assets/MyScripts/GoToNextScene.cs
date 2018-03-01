using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class GoToNextScene: MonoBehaviour
{


    //    public UILabel MyLabel;

    //    public UIInput MyInput;

    public Button myButton;
    //public InputField myInputField;
    //public Text inputText;
    //public Text verifyText;

    void Start()
    {
        Button btn = myButton.GetComponent<Button>();
        btn.onClick.AddListener(GoToNext);
    }


    void GoToNext()
    {
        SceneManager.LoadScene(1);
    }

}
