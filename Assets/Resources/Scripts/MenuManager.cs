using System.Collections;
using UnityEngine;

/// MenuManager.cs
public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        this.gameObject.SetActive(true);
    }

    public void ClickOpenPuzzles()
    {
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
