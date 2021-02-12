using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour
{
    private GameObject StartButton;
    private GameObject EndButton;
    private GameObject TutorialUI;
    private Text TutorialText;

    [SerializeField] bool IsGame;
    public static bool Hide = false;

    private void Start()
    {
        Time.timeScale = 1;
        Hide = false;

        if (IsGame)
        {
            StartButton.SetActive(Hide);
            EndButton.SetActive(Hide);
        }
    }

    private void Awake()
    {
        #region CONFIG
        StartButton = transform.Find("Start").gameObject;

        EndButton = transform.Find("End").gameObject;

        if (IsGame)
        {
            if (transform.Find("TutorialUI").gameObject != null)
            {
                TutorialUI = transform.Find("TutorialUI").gameObject;
                TutorialText = TutorialUI.transform.Find("Text").GetComponent<Text>();
            }
        }

        StartButton.AddComponent<Button>()
            .onClick
            .AddListener(delegate {
            
            SceneManager.LoadScene("SampleScene");
        });

        EndButton.AddComponent<Button>()
            .onClick
            .AddListener(delegate { 
            
            Application.Quit(); 
        
        });

        if (IsGame)
        {
            StartButton.SetActive(Hide);
            EndButton.SetActive(Hide);
        }

        #endregion
    }

    private void Update()
    {

        if (IsGame && Input.GetKeyDown(KeyCode.Escape))
        {
            Hide = !Hide;
            StartButton.SetActive(Hide);
            EndButton.SetActive(Hide);
            ChangeTimeScale(Hide);
        }
    }

    private void ChangeTimeScale(bool isFree)
    {
       Time.timeScale = !isFree ?  1 : 0; 

    }

    public void TutorialSwitch(bool toogle)
    {
        TutorialUI.SetActive(toogle);
    }

    public void UpdateTutorialText(string TextToAisgn)
    {
        TutorialText.text = TextToAisgn;
    }
}
