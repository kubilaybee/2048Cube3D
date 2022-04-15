using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public static UIManager Instance;

    public GameObject UILevelFailed;
    public GameObject UIGamePlay;

    // GAME PLAY PARAMETRELERİ
    public Text highScoreText;
    public Text scoreText;
    public Text finalScoreText;

    [Header("UI MAIN MENU")]
    public GameObject UIMainMenu;

    public enum UIElements { None, UIMainMenu, UIGameplay, UILevelFail}

    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        defineTexts();
        ChangeUI(UIElements.UIMainMenu);
    }

    // DEFINE TEXTS
    public void defineTexts()
    {
        scoreText = UIGamePlay.transform.GetChild(0).gameObject.GetComponent<Text>();
        highScoreText = UIGamePlay.transform.GetChild(1).gameObject.GetComponent<Text>();
        finalScoreText = UILevelFailed.transform.GetChild(2).gameObject.GetComponent<Text>();
    }

    public void ChangeUI(UIElements uIElement)
    {
        CloseAllUIElements();

        switch (uIElement)
        {
            case UIElements.None:
                break;
            case UIElements.UIMainMenu:
                UIMainMenu.SetActive(true);
                break;
            case UIElements.UIGameplay:
                UIGamePlay.SetActive(true);
                break;
            case UIElements.UILevelFail:
                UILevelFailed.SetActive(true);
                break;
            default:
                break;
        }

    }

    public void CloseAllUIElements()
    {
        UIMainMenu.SetActive(false);
        UILevelFailed.SetActive(false);
        UIGamePlay.SetActive(false);
    }

    public void SetPlayButton()
    {
        GMScript.Instance.gameRestart();
        ChangeUI(UIElements.UIGameplay);
    }

}
