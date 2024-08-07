using System.Collections.Generic;
using UnityEngine;

public class Piece : MonoBehaviour
{
    [SerializeField] public PieceType pieceType;
    [SerializeField] public bool isRotationDone;
    [SerializeField] public List<Piece> childPiecesList;
    [SerializeField] public List<Piece> parentPiecesList;
}

public enum PieceType
{
    FIRST,
    DEFAULT,
    STRAIGHT
}


