using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    [SerializeField] private List<GameObject> puzzleList;
    [SerializeField] private int puzzleActive;


    private void Start()
    {
        puzzleList = new List<GameObject>();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            puzzleList.Add(this.transform.GetChild(i).gameObject);
        }
    }

    public void ClickToRotate(int index)
    {
        puzzleList[0].GetComponent<Puzzle>().RotatePiece(index);
    }
}
