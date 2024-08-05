using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// PuzzleManager.cs
public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> puzzleList;
    [SerializeField] private int activePuzzle;
    [SerializeField] private int playerScore;
    [SerializeField] private TextMeshProUGUI txtCurrentLevel;
    [SerializeField] private TextMeshProUGUI txtPlayerScore;
    [SerializeField] private Button btnNextPuzzle;
    [SerializeField] private Button btnPrevPuzzle;
    [SerializeField] private GameObject particleEffectPuzzleCompleted;
    [SerializeField] private GameObject panelScore;
    [NonSerialized] private List<int> completedPuzzles;
    [NonSerialized] private bool animationDone;

    // Start is called before the first frame update
    private void Start()
    {
        completedPuzzles = new List<int>();
        puzzleList = new List<GameObject>();
        animationDone = true;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            puzzleList.Add(this.transform.GetChild(i).gameObject);
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }

    public void LoadPuzzle()
    {
        if (PlayerPrefs.HasKey("level"))
        {
            activePuzzle = PlayerPrefs.GetInt("level");
            puzzleList[activePuzzle].SetActive(true);
            txtCurrentLevel.text = (activePuzzle + 1).ToString();
        }
        else
        {
            PlayerPrefs.SetInt("level", 0);
            puzzleList[0].SetActive(true);
            txtCurrentLevel.text = "1";
        }

        if (PlayerPrefs.HasKey("score"))
        {
            playerScore = PlayerPrefs.GetInt("score");
            txtPlayerScore.text = playerScore.ToString();
        }
        else
        {
            playerScore = 0;
            txtPlayerScore.text = "0";
        }
    }
    public void ClickToRotate(int index)
    {
        //Rotate pieces
        //Check if all pieces of the puzzle are completed on the right position and save the progress and Increase the score

        if (animationDone == true)
        {
            puzzleList[activePuzzle].GetComponent<Puzzle>().RotatePiece(index);

            if (completedPuzzles.Contains(activePuzzle) == false &&
                puzzleList[activePuzzle].GetComponent<Puzzle>().CheckPuzzleCompletion() == true)
            {
                puzzleList[activePuzzle].GetComponent<Animation>().Play("PUZZLE_INCREASE_SIZE_EFFECT");
                particleEffectPuzzleCompleted.SetActive(true);
                animationDone = false;
                completedPuzzles.Add(activePuzzle);
                playerScore += 100;
                txtPlayerScore.text = playerScore.ToString();
                PlayerPrefs.SetInt("level", activePuzzle);
                PlayerPrefs.SetInt("score", playerScore);

                StartCoroutine(this.ShowScoreBar(2.5f));
                StartCoroutine(this.PuzzleCompleted(5.3f));
            }
        }
    }

    private IEnumerator PuzzleCompleted(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        panelScore.SetActive(false);
        this.NextPuzzle();
        animationDone = true;
    }

    private IEnumerator ShowScoreBar(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        panelScore.SetActive(true);
    }

    private void NextPuzzle()
    {
        if (activePuzzle + 1 < puzzleList.Count)
        {
            puzzleList[activePuzzle].SetActive(false);
            puzzleList[activePuzzle + 1].SetActive(true);
            activePuzzle += 1;
            puzzleList[activePuzzle].GetComponent<Puzzle>().ShufflePieces();
            txtCurrentLevel.text = (activePuzzle + 1).ToString();
            completedPuzzles.Remove(activePuzzle);
        }
    }

    public void ClickNextPuzzle()
    {
        if (animationDone == true)
        {
            btnNextPuzzle.GetComponent<Animation>().Play();
            this.NextPuzzle();
        }
    }

    public void ClickPreviousPuzzle()
    {
        if (animationDone == true)
        {
            btnPrevPuzzle.GetComponent<Animation>().Play();

            if (activePuzzle - 1 >= 0)
            {
                puzzleList[activePuzzle].SetActive(false);
                puzzleList[activePuzzle - 1].SetActive(true);
                activePuzzle -= 1;
                puzzleList[activePuzzle].GetComponent<Puzzle>().ShufflePieces();
                txtCurrentLevel.text = (activePuzzle + 1).ToString();
                completedPuzzles.Remove(activePuzzle);
            }
        }
    }
}
