using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

/// Puzzle.cs
public class Puzzle : MonoBehaviour
{
    [SerializeField] private List<GameObject> pieces;
    [SerializeField] private Color32 piecesColor;
    [SerializeField] private Color32 piecesColorDefault;
    [NonSerialized] public bool isShuffleDone;
    private Piece tmpPiece;
    private readonly System.Random randomN = new();
    private AudioSource audioPieceDone;

    // Start is called before the first frame update
    private void Start()
    {
        //Fill lists and Variables
        audioPieceDone = this.GetComponent<AudioSource>();
        pieces = new List<GameObject>();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            pieces.Add(this.transform.GetChild(i).gameObject);
        }

        this.ShufflePieces();
    }

    /// <summary>
    /// Rotate pieces and check if it's the correct position
    /// </summary>
    /// <param name="index"></param> 
    public void RotatePiece(int index)
    {
        //Rotate pieces and check if it's the correct position

        if (pieces[index].GetComponent<Piece>().pieceType != PieceType.FIRST && isShuffleDone == true)
        {
            pieces[index].GetComponent<Piece>().isRotationDone = false;
            pieces[index].transform.eulerAngles = new Vector3(0, 0, pieces[index].transform.eulerAngles.z + 90); /// o_O

            if (this.CheckRotationDone(index) == true)
            {
                pieces[index].GetComponent<Piece>().isRotationDone = true;

                for (int i = 0; i < pieces[index].GetComponent<Piece>().parentPiecesList.Count; i++)
                {
                    if (pieces[index].GetComponent<Piece>().parentPiecesList[i].isRotationDone
                        && pieces[index].GetComponent<Piece>().parentPiecesList[i].GetComponent<SpriteRenderer>().color == piecesColor)
                    {
                        //if piece is done and parent piece is done, then change the collor to highlight the piece
                        audioPieceDone.Play();
                        pieces[index].GetComponent<SpriteRenderer>().color = piecesColor;
                        this.SetChildPiecesDone(index);
                    }
                }
            }
            else
            {
                //if piece is not done, change the collor to default collor
                pieces[index].GetComponent<SpriteRenderer>().color = piecesColorDefault;
                this.SetChildPiecesNotDone(index);
            }
        }
    }

    private bool CheckRotationDone(int index)
    {
        //Check if the piece is on the correct rotation

        if (Math.Round(pieces[index].transform.eulerAngles.z) == 0
            || (pieces[index].GetComponent<Piece>().pieceType == PieceType.STRAIGHT && Math.Round(pieces[index].transform.eulerAngles.z) == 180))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void SetChildPiecesNotDone(int index)
    {
        //Change color to NOT DONE status if the child piece is in the right position
        for (int i = 0; i < pieces[index].GetComponent<Piece>().childPiecesList.Count; i++)
        {
            pieces[index].GetComponent<Piece>().childPiecesList[i].GetComponent<SpriteRenderer>().color = piecesColorDefault;
        }
    }

    private void SetChildPiecesDone(int index)
    {
        //Change color to DONE status if the child piece is in the right position
        for (int i = 0; i < pieces[index].GetComponent<Piece>().childPiecesList.Count; i++)
        {
            tmpPiece = pieces[index].GetComponent<Piece>().childPiecesList[i];

            if (tmpPiece.isRotationDone == true
                && tmpPiece.parentPiecesList.Find(x => x.isRotationDone == true)
                && tmpPiece.parentPiecesList.Find(x => x.isRotationDone == true).GetComponent<SpriteRenderer>().color == piecesColor)
            {
                pieces[index].GetComponent<Piece>().childPiecesList[i].GetComponent<SpriteRenderer>().color = piecesColor;
            }
        }
    }

    /// <summary>
    /// Shuffle the initial position of the pieces and change colors if needed
    /// </summary>
    public void ShufflePieces()
    {
        //Shuffle the initial position of the pieces and change colors if needed
        isShuffleDone = false;

        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].GetComponent<SpriteRenderer>().color = piecesColorDefault;
            pieces[i].GetComponent<Piece>().isRotationDone = false;

            switch (pieces[i].GetComponent<Piece>().pieceType)
            {
                case PieceType.STRAIGHT:
                    pieces[i].transform.Rotate(0, 0, 90f * randomN.Next(1, 1));
                    break;
                case PieceType.FIRST:
                    pieces[i].GetComponent<SpriteRenderer>().color = piecesColor;
                    pieces[i].GetComponent<Piece>().isRotationDone = true;
                    break;
                default:
                    pieces[i].transform.Rotate(0, 0, 90f * randomN.Next(1, 3));
                    break;
            }

            if (CheckRotationDone(i) && pieces[i].GetComponent<Piece>().pieceType != PieceType.FIRST)
            {
                pieces[i].transform.Rotate(0, 0, 90f * randomN.Next(1, 1));
                pieces[i].GetComponent<Piece>().isRotationDone = false;
                pieces[i].GetComponent<SpriteRenderer>().color = piecesColorDefault;
            }
        }

        isShuffleDone = true;
    }
    /// <summary>
    /// Check if the all pieces in the active puzzle are completed
    /// </summary>
    /// <returns></returns>
    public bool CheckAllPiecesDone()
    {
        //Check if the all pieces in the active puzzle are completed
        if (pieces.Find(x => x.GetComponent<Piece>().isRotationDone == false))
        {
            return false;
        }
        else
        {
            return true;
        }
    }
    public void FreezeAllPieces()
    {
        //Freeze all pieces on the active puzzle 
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].GetComponent<Button>().interactable = false;
        }
    }
    /// <summary>
    /// UnFreeze all pieces on the active puzzle 
    /// </summary>
    public void UnFreezeAllPieces()
    {
        //UnFreeze all pieces on the active puzzle 
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].GetComponent<Button>().interactable = true;
        }
    }
    /// <summary>
    ///Show All hided pieces from the active puzzle
    /// </summary>
    public void ShowPieces()
    {
        //Show All hided pieces from the active puzzle
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].SetActive(true);
        }
    }

    /// <summary>
    /// Hide All pieces from the active puzzle
    /// </summary>
    public void HidePieces()
    {
        //Hide All pieces from the active puzzle
        for (int i = 0; i < pieces.Count; i++)
        {
            pieces[i].SetActive(false);
        }
    }
}
