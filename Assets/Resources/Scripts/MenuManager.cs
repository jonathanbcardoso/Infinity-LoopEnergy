using UnityEngine;

public class MenuManager : MonoBehaviour
{
    // Start is called before the first frame update
    private void Start()
    {
        this.gameObject.SetActive(true);
    }

    public void OpenChallenges()
    {
        //Deactivate the Menu
        this.gameObject.SetActive(false);
        FindFirstObjectByType<PuzzleManager>().LoadPuzzle();
    }

    //private IEnumerator OpenLoadScreen(float seconds)
    //{

    //}
}
