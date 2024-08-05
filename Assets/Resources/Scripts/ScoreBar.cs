using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ScoreBar : MonoBehaviour
{
    [SerializeField] private Image imgScoreBar;
    [SerializeField] private Image imgScoreBarBKG;
    [SerializeField] private TextMeshProUGUI txtScoreIncreased;
    [SerializeField] private float slowFillBar = 2;
    [NonSerialized] private readonly float fillScoreBar = 100f;
    [NonSerialized] private float fill;
    [NonSerialized] private int increaseScorePoints;
    [NonSerialized] private bool isBarFilled;


    public void Start()
    {
        //Load image Bars 
        if (GameObject.FindWithTag("ScoreBar_BKG") != null)
            imgScoreBarBKG = GameObject.FindWithTag("ScoreBar_BKG").GetComponent<Image>();

        if (GameObject.FindWithTag("ScoreBar") != null)
            imgScoreBar = GameObject.FindWithTag("ScoreBar").GetComponent<Image>();
    }

    private void OnEnable()
    {
        fill = 0;
        txtScoreIncreased.text = "+ 0";
        imgScoreBar.fillAmount = 0f;
        increaseScorePoints = 0;
        isBarFilled = false;
    }

    private void Update()
    {
        //Fill the score bar 0 to 100
        if (isBarFilled == false && imgScoreBar.fillAmount < (0f + fillScoreBar))
        {
            fill = Time.deltaTime * (fillScoreBar / (12 * slowFillBar));
            imgScoreBar.fillAmount += fill;

            if (imgScoreBar.fillAmount >= 1)
            {
                imgScoreBar.fillAmount = 1f;
                isBarFilled = true;
            }
        }

        if (increaseScorePoints < 100)
        {
            increaseScorePoints += 1;
            txtScoreIncreased.text = "+ " + increaseScorePoints;
        }
    }
}