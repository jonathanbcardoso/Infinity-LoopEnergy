using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Puzzle : MonoBehaviour
{
    [SerializeField] private List<RectTransform> pieces;
    [SerializeField] private List<RectTransform> piecesInitialPosition;

    private void Start()
    {
        pieces = new List<RectTransform>();
        piecesInitialPosition = new List<RectTransform>();

        for (int i = 0; i < this.transform.childCount; i++)
        {
            pieces.Add(this.transform.GetChild(i).GetComponent<RectTransform>());
        }

        piecesInitialPosition = pieces;
    }

    public void RotatePiece(int index)
    {
        pieces[index].transform.eulerAngles = new Vector3(0f, 0f, pieces[index].eulerAngles.z + 90f);
    }
}
