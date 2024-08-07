using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

/// ConfigManager.cs
public class ConfigManager : MonoBehaviour
{
    [SerializeField] private GameObject btnConfig;
    [SerializeField] private GameObject btnSound;
    [SerializeField] private GameObject btnLevels;
    [SerializeField] private GameObject panelLevels;
    [SerializeField] private List<Button> btnLevelsList = new();
    [SerializeField] private Button btnNextPuzzle;
    [SerializeField] private Button btnPrevPuzzle;
    [NonSerialized] private AudioSource audioClick;


    // Start is called before the first frame update
    private void Start()
    {
        btnConfig.SetActive(true);
        btnSound.SetActive(false);
        btnLevels.SetActive(false);

        audioClick = this.GetComponent<AudioSource>();
        btnLevelsList = panelLevels.transform.GetComponentsInChildren<Button>().ToList();
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
            btnSound.SetActive(true);
            btnLevels.SetActive(true);
        }
        else
        {
            btnSound.SetActive(false);
            btnLevels.SetActive(false);
        }
    }

}