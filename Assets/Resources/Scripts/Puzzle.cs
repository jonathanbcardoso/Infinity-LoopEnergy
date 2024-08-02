using System;
using System.Collections.Generic;
using UnityEngine;

/// Puzzle.cs
public class Puzzle : MonoBehaviour
{
    [SerializeField] private List<GameObject> pieces;
    [SerializeField] private List<bool> piecesDone;
    private System.Random randomN = new System.Random();

    private void Start()
    {
        //Fill lists 
        pieces = new List<GameObject>();
        piecesDone = new List<bool>();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            pieces.Add(this.transform.GetChild(i).gameObject);

            //Shuffle the initial position of the pieces
            pieces[i].transform.Rotate(0, 0, 90f * randomN.Next(1, 3));

            if (this.CheckRotationDone(i) == true)
            {
                piecesDone.Add(true);
            }
            else
            {
                piecesDone.Add(false);
            }
        }
    }

    public void RotatePiece(int index)
    {
        //Rotate pieces and check if it's the correct position
        if (this.CheckRotationDone(index) == false)
        {
            pieces[index].transform.eulerAngles = new Vector3(0, 0, pieces[index].transform.eulerAngles.z + 90); /// o_O

            if (this.CheckRotationDone(index) == true)
            {
                piecesDone[index] = true;
                //do animations and sounds here  
                this.CheckPuzzleCompletion();
            }
        }
    }

    private bool CheckRotationDone(int index)
    {
        //Check if the rotation is the correct position (0 is always the correct)
        if (Math.Round(pieces[index].transform.eulerAngles.z) == 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void ShufflePieces()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            //Shuffle the initial position of the pieces
            pieces[i].transform.Rotate(0, 0, 90f * randomN.Next(1, 3));

            if (this.CheckRotationDone(i) == true)
            {
                piecesDone[i] = true;
            }
            else
            {
                piecesDone[i] = false;
            }
        }
    }

    public bool CheckPuzzleCompletion()
    {
        //Check if the current puzzle open is completed
        if (piecesDone.Contains(false))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
}
