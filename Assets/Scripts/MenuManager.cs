using System;
using System.Collections;
using UnityEngine;

/// MenuManager.cs
public class MenuManager : MonoBehaviour
{
    [NonSerialized] private AudioSource audioClick;

    // Start is called before the first frame update
    private void Start()
    {
        audioClick = this.GetComponent<AudioSource>();
        this.gameObject.SetActive(true);
    }

    public void ClickOpenPuzzles()
    {
        audioClick.Play();
        //Deactivate menu and open puzzle;
        this.transform.GetChild(0).GetComponent<Animation>().Play();
        StartCoroutine(this.ClickOpenPuzzles(0.5f));
    }

    public IEnumerator ClickOpenPuzzles(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        this.gameObject.SetActive(false);
        FindFirstObjectByType<PuzzleManager>().LoadPuzzle();
    }
}
