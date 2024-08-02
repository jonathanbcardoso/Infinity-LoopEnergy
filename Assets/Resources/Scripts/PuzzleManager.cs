using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// PuzzleManager.cs
public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> puzzleList;
    [SerializeField] private int activePuzzle;
    [SerializeField] private TextMeshProUGUI txtCurrentLevel;
    [SerializeField] private TextMeshProUGUI txtPlayerScore;
    [SerializeField] private int playerScore;
    [NonSerialized] private List<int> completedPuzzles;

    private void Start()
    {
        completedPuzzles = new List<int>();
        puzzleList = new List<GameObject>();

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
        //Check if all pieces of the puzzle are completed on the right position and save the progress
        //Increase the score

        puzzleList[activePuzzle].GetComponent<Puzzle>().RotatePiece(index);

        if (completedPuzzles.Contains(activePuzzle) == false &&
            puzzleList[activePuzzle].GetComponent<Puzzle>().CheckPuzzleCompletion() == true)
        {
            completedPuzzles.Add(activePuzzle);
            playerScore += 100;
            txtPlayerScore.text = playerScore.ToString();

            PlayerPrefs.SetInt("level", activePuzzle);
            PlayerPrefs.SetInt("score", playerScore);
            this.ClickNextPuzzle();
        }
    }

    public void ClickNextPuzzle()
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

    public void ClickPreviousPuzzle()
    {
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
