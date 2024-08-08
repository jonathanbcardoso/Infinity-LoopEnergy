using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// ConfigManager.cs
public class ConfigManager : MonoBehaviour
{
    [Header("                               ==== Config ====")]
    [SerializeField] private GameObject btnConfig;
    [SerializeField] private GameObject btnSound;
    [SerializeField] private GameObject panelIntro;

    [Header("                               ==== Levels ====")]
    [SerializeField] private GameObject btnLevels;
    [SerializeField] private List<Button> btnLevelsList = new();
    [SerializeField] private Button btnNextPuzzle;
    [SerializeField] private Button btnPrevPuzzle;
    [SerializeField] private GameObject panelLevels;

    [Header("                               ==== FPS ====")]
    [SerializeField] private GameObject btnFPS;
    [SerializeField] private TextMeshProUGUI textFPS;
    [SerializeField] private TextMeshProUGUI textTargetFrameRate;
    [SerializeField] private TextMeshProUGUI textScreenRefreshRate;
    [SerializeField] private GameObject panelFPS;
    [SerializeField] private List<Sprite> spriteIconList;
    [NonSerialized] private float deltaTime;
    [NonSerialized] private int currentFPS;

    [NonSerialized] private AudioSource audioClick;

    // Start is called before the first frame update
    private void Start()
    {
        btnConfig.SetActive(true);
        btnSound.SetActive(false);
        btnLevels.SetActive(false);
        btnFPS.SetActive(false);

        audioClick = this.GetComponent<AudioSource>();
        btnLevelsList = panelLevels.transform.GetComponentsInChildren<Button>().ToList();
    }
    void Update()
    {
        //targetFrameRate.text = "FR:" + Application.targetFrameRate.ToString();
        //screenRefreshRate.text = "RR:" + Math.Round(Screen.currentResolution.refreshRateRatio.value);

        deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
        float fps = 1.0f / deltaTime;
        textFPS.text = "FPS: " + Mathf.Ceil(fps).ToString();
    }

    public void ClickDeactivateSound()
    {
        audioClick.Play();
        btnSound.GetComponent<Animation>().Play("BUTTON_INCREASE_SIZE_EFFECT");

        if (AudioListener.volume != 0)
        {
            AudioListener.volume = 0;
        }
        else
        {
            AudioListener.volume = 1;
        }
    }

    public void ClickOpenLevelsMenu()
    {
        audioClick.Play();
        btnPrevPuzzle.interactable = false;
        btnNextPuzzle.interactable = false;

        btnLevels.GetComponent<Animation>().Play("BUTTON_INCREASE_SIZE_EFFECT");

        if (panelLevels.activeSelf == true)
        {
            panelLevels.SetActive(false);
        }
        else
        {
            panelLevels.SetActive(true);

            for (int i = 0; i < btnLevelsList.Count; i++)
            {
                btnLevelsList[i].interactable = true;

                if (PlayerPrefs.HasKey("last_unlocked_level") && PlayerPrefs.GetInt("last_unlocked_level") > i)
                {
                    btnLevelsList[i].GetComponent<Image>().color = new Color32(102, 224, 141, 166);
                }
                else if (PlayerPrefs.HasKey("last_unlocked_level") && PlayerPrefs.GetInt("last_unlocked_level") == i)
                {
                    btnLevelsList[i].GetComponent<Image>().color = new Color32(102, 113, 224, 85);
                }
                else
                {
                    btnLevelsList[i].GetComponent<Image>().color = new Color32(224, 102, 110, 85);
                    btnLevelsList[i].interactable = false;
                }
            }
        }
    }

    public void ClickOpenSubMenu()
    {
        audioClick.Play();
        btnConfig.GetComponent<Animation>().Play("BUTTON_ROTATE_SIZE_EFFECT");
        StartCoroutine(this.OpenSubMenu());
    }

    private IEnumerator OpenSubMenu()
    {
        yield return new WaitForSeconds(0.1f);

        if (btnSound.activeSelf == false)
        {
            btnFPS.SetActive(true);
            btnSound.SetActive(true);
            btnLevels.SetActive(true);
        }
        else
        {
            btnFPS.SetActive(false);
            btnSound.SetActive(false);
            btnLevels.SetActive(false);
        }
    }

    public void ClickOpenPuzzles()
    {
        //Deactivate menu and open puzzle;
        audioClick.Play();
        panelIntro.transform.GetChild(0).GetComponent<Animation>().Play();
        StartCoroutine(this.OpenPuzzles(0.5f));
    }

    public IEnumerator OpenPuzzles(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        panelIntro.SetActive(false);
        FindFirstObjectByType<PuzzleManager>().LoadPuzzle();
    }

    public void ClickChangeFPS()
    {
        audioClick.Play();
        btnFPS.GetComponent<Animation>().Play("BUTTON_INCREASE_SIZE_EFFECT");
        currentFPS += 1;

        switch (currentFPS)
        {
            case 1:
                Application.targetFrameRate = 15;
                btnFPS.transform.GetChild(0).GetComponent<Image>().sprite = spriteIconList[0];
                textFPS.gameObject.SetActive(true);
                break;
            case 2:
                Application.targetFrameRate = 30;
                btnFPS.transform.GetChild(0).GetComponent<Image>().sprite = spriteIconList[1];
                break;
            case 3:
                Application.targetFrameRate = 60;
                btnFPS.transform.GetChild(0).GetComponent<Image>().sprite = spriteIconList[2];
                break;
            case 4:
                currentFPS = 0;
                btnFPS.transform.GetChild(0).GetComponent<Image>().sprite = spriteIconList[3];
                textFPS.gameObject.SetActive(false);
                break;
            default:
                break;
        }
    }

}