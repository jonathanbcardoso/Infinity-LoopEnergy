using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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
    [SerializeField] private GameObject panelConfig;
    [SerializeField] private GameObject particleEffectPuzzleCompleted;
    [SerializeField] private GameObject panelScore;
    [NonSerialized] private List<int> completedPuzzles;
    [NonSerialized] private bool animationDone;
    [NonSerialized] private int lastUnlockedLevel;
    [NonSerialized] private bool hidePieces;
    [NonSerialized] private List<AudioSource> audioList;

    // Start is called before the first frame update
    private void Start()
    {
        audioList = this.GetComponents<AudioSource>().ToList();
        completedPuzzles = new List<int>();
        puzzleList = new List<GameObject>();
        animationDone = true;
        hidePieces = false;

        for (int i = 0; i < this.transform.childCount; i++)
        {
            puzzleList.Add(this.transform.GetChild(i).gameObject);
            this.transform.GetChild(i).gameObject.SetActive(false);
        }
    }
    /// <summary>
    /// Load the last unlocked puzzle the player is currently at and load the score. 
    /// </summary>
    public void LoadPuzzle()
    {
        //Load the last unlocked puzzle the player is currently at and load the score. 
        if (PlayerPrefs.HasKey("last_unlocked_level"))
        {
            activePuzzle = PlayerPrefs.GetInt("last_unlocked_level");
            puzzleList[activePuzzle].SetActive(true);
            txtCurrentLevel.text = (activePuzzle + 1).ToString();
            lastUnlockedLevel = activePuzzle;
        }
        else
        {
            PlayerPrefs.SetInt("last_unlocked_level", 0);
            puzzleList[0].SetActive(true);
            lastUnlockedLevel = 0;
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
    /// <summary>
    /// Rotate pieces
    /// Check if all pieces of the puzzle are completed on the right position and save the progress and Increase the score
    /// </summary>
    /// <param name="index"></param>
    public void ClickToRotate(int index)
    {
        //Rotate pieces
        //Check if all pieces of the puzzle are completed on the right position and save the progress and Increase the score

        if (animationDone == true)
        {
            puzzleList[activePuzzle].GetComponent<Puzzle>().RotatePiece(index);

            if (completedPuzzles.Contains(activePuzzle) == false &&
                puzzleList[activePuzzle].GetComponent<Puzzle>().CheckAllPiecesDone() == true)
            {
                audioList[1].Play(); //Play Puzzle Completed

                panelConfig.SetActive(false);
                puzzleList[activePuzzle].GetComponent<Animation>().Play("PUZZLE_INCREASE_SIZE_EFFECT");
                particleEffectPuzzleCompleted.SetActive(true);
                animationDone = false;
                completedPuzzles.Add(activePuzzle);
                playerScore += 100;
                txtPlayerScore.text = playerScore.ToString();

                if (activePuzzle + 1 < puzzleList.Count)
                {
                    if (activePuzzle + 1 > lastUnlockedLevel)
                    {
                        PlayerPrefs.SetInt("last_unlocked_level", activePuzzle + 1);
                        lastUnlockedLevel = activePuzzle + 1;
                    }

                    PlayerPrefs.SetInt("score", playerScore);
                }

                StartCoroutine(this.ShowScoreBar(1.6f));
                StartCoroutine(this.PuzzleCompleted(5.3f));
            }
        }
    }

    private IEnumerator PuzzleCompleted(float seconds)
    {
        //When puzzle is completed, Freeze all pieces, when all animations are done, go the next puzzle (if avaiable more puzzles)
        yield return new WaitForSeconds(seconds);
        panelScore.SetActive(false);
        puzzleList[activePuzzle].GetComponent<Puzzle>().FreezeAllPieces();
        this.NextPuzzle();
        animationDone = true;
        panelConfig.SetActive(true);
    }

    private IEnumerator ShowScoreBar(float seconds)
    {
        //Show Score Bar 0 to 100
        yield return new WaitForSeconds(seconds);
        panelScore.SetActive(true);
    }

    private void NextPuzzle()
    {
        //Go to the next Puzzle
        if (activePuzzle + 1 < puzzleList.Count)
        {
            puzzleList[activePuzzle].SetActive(false);
            puzzleList[activePuzzle + 1].SetActive(true);
            activePuzzle += 1;
            ShowPieces(activePuzzle);
            puzzleList[activePuzzle].GetComponent<Puzzle>().UnFreezeAllPieces();
            puzzleList[activePuzzle].GetComponent<Puzzle>().ShufflePieces();
            txtCurrentLevel.text = (activePuzzle + 1).ToString();
            completedPuzzles.Remove(activePuzzle);
        }
    }
    /// <summary>
    /// Go to the next puzzle and play Click Sound
    /// </summary>
    public void ClickNextPuzzle()
    {
        //Go to the next puzzle and play Click Sound
        audioList[0].Play();

        if (animationDone == true
            && lastUnlockedLevel >= activePuzzle + 1)
        {
            btnNextPuzzle.GetComponent<Animation>().Play();
            this.NextPuzzle();
        }
    }
    /// <summary>
    /// Go to the previous Puzzle and play Click Sound
    /// </summary>
    public void ClickPreviousPuzzle()
    {
        //Go to the previous Puzzle and play Click Sound        
        audioList[0].Play();

        if (animationDone == true)
        {
            btnPrevPuzzle.GetComponent<Animation>().Play();

            if (activePuzzle - 1 >= 0)
            {
                puzzleList[activePuzzle].SetActive(false);
                puzzleList[activePuzzle - 1].SetActive(true);
                activePuzzle -= 1;
                ShowPieces(activePuzzle);
                puzzleList[activePuzzle].GetComponent<Puzzle>().UnFreezeAllPieces();
                puzzleList[activePuzzle].GetComponent<Puzzle>().ShufflePieces();
                txtCurrentLevel.text = (activePuzzle + 1).ToString();
                completedPuzzles.Remove(activePuzzle);
            }
        }
    }
    /// <summary>
    /// Open choosed puzzle from the levels menu
    /// </summary>
    /// <param name="index"></param>
    public void ClickOpenPuzzle(int index)
    {
        audioList[0].Play();
        //Open choosed puzzle from the levels menu
        FindFirstObjectByType<ScrollRect>().gameObject.SetActive(false);
        puzzleList[activePuzzle].SetActive(false);
        activePuzzle = index;
        ShowPieces(activePuzzle);
        puzzleList[activePuzzle].GetComponent<Puzzle>().UnFreezeAllPieces();
        puzzleList[activePuzzle].SetActive(true);
        puzzleList[activePuzzle].GetComponent<Puzzle>().ShufflePieces();
        txtCurrentLevel.text = (activePuzzle + 1).ToString();
        completedPuzzles.Remove(activePuzzle);
    }

    private void ShowPieces(int index)
    {
        //Show All puzzle pieces from the active puzzle and turn on navigation through buttons
        puzzleList[index].GetComponent<Puzzle>().ShowPieces();
        btnPrevPuzzle.interactable = true;
        btnNextPuzzle.interactable = true;
        hidePieces = false;
    }
    /// <summary>
    /// Hide All puzzle Pieces from the active puzzle
    /// </summary>
    public void ClickHidePieces()
    {
        //Hide All puzzle Pieces from the active puzzle
        if (hidePieces == false)
        {
            puzzleList[activePuzzle].GetComponent<Puzzle>().HidePieces();
            hidePieces = true;
        }
        else
        {
            ShowPieces(activePuzzle);
        }
    }
}
