using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;

/// Puzzle.cs
public class Puzzle : MonoBehaviour
{
    [SerializeField] private List<GameObject> pieces;
    [SerializeField] private List<bool> piecesDone;
    [SerializeField] private Color32 piecesColor;
    [SerializeField] private Color32 piecesColorDefault;
    private System.Random randomN = new System.Random();

    // Start is called before the first frame update
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

        pieces[index].transform.eulerAngles = new Vector3(0, 0, pieces[index].transform.eulerAngles.z + 90); /// o_O
        piecesDone[index] = false;

        if (this.CheckRotationDone(index) == true)
        {
            //if piece is done, change the collor to highlight the piece
            pieces[index].GetComponent<SpriteRenderer>().color = piecesColor;
            piecesDone[index] = true;
            this.CheckPuzzleCompletion();
        }
        else
        {
            //if piece is not done, change the collor to default collor
            pieces[index].GetComponent<SpriteRenderer>().color = piecesColorDefault;
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
            pieces[i].GetComponent<SpriteRenderer>().color = piecesColorDefault;
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
