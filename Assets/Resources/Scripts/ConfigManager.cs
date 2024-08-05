using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// ConfigManager.cs
public class ConfigManager : MonoBehaviour
{
    [SerializeField] private GameObject btnConfig;
    [SerializeField] private GameObject btnSound;
    [SerializeField] private GameObject btnLevels;
    [SerializeField] private AudioListener gameAudio;

    // Start is called before the first frame update
    void Start()
    {
        btnConfig.SetActive(true);
        btnSound.SetActive(false);
        btnLevels.SetActive(false);
    }

    public void ClickDeactivateSound()
    {
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
        //
    }

    public void ClickOpenSubMenu()
    {
        btnConfig.GetComponent<Animation>().Play("BUTTON_ROTATE_SIZE_EFFECT");
        StartCoroutine(this.OpenSubMenu());
    }

    private IEnumerator OpenSubMenu()
    {
        yield return new WaitForSeconds(0.5f);

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