using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayerController PlayerController;
    public CameraController CameraController;
    public AudioSource MusicAudio;
    public AudioClip FinalSong;
    public CameraRig StartingRig;
    public CameraRig EndGameRig;
    public CameraRig PuzzleSolvedRig;
    public Piano Piano;
    public List<CandlePuzzleController> PuzzleControllers;
    
    public Canvas GameCanvas;
    public GameObject StartGamePanel;
    public GameObject EndGamePanel;
    public GameObject StartPuzzleButton;
    public GameObject ActivatePianoButton;
    public GameObject RestartGameButton;

    private static GameController _Instance;
    private CandlePuzzleController CurrentProximityPuzzle;
    private bool PlayerIsInPianoProximity ;

    private void Awake()
    {
        _Instance = this;
    }


    private void Start()
    {
        this.CameraController.SetRig(this.StartingRig);
        this.CameraController.TransitionCompleted += this.HandleCameraTransitionCompleted;

        CandlePuzzleController[] allPuzzles = FindObjectsOfType<CandlePuzzleController>();
        this.PuzzleControllers = new List<CandlePuzzleController>();

        for (int i = 0; i < allPuzzles.Length; i++)
        {
            allPuzzles[i].PlayerEnteredArea += this.HandlePlayerEnteredPuzzleArea;
            allPuzzles[i].PlayerLeftArea += this.HandlePlayerLeftPuzzleArea;
            allPuzzles[i].PuzzleWasSolved += this.HandlePuzzleSolved;
            allPuzzles[i].SheetMusicPiece.gameObject.GetComponent<CanvasRenderer>().SetAlpha(0f);
            allPuzzles[i].SheetMusicPiece.gameObject.SetActive(true);

            // FOR TESTING
            //allPuzzles[i].IsSolved = true;

            this.PuzzleControllers.Add(allPuzzles[i]);
        }

        this.Piano.PlayerEnteredArea += this.HandlePlayerEnteredPianoArea;
        this.Piano.PlayerLeftArea += this.HandlePlayerLeftPianoArea;

        this.StartGamePanel.SetActive(true);
        this.EndGamePanel.SetActive(false);
        this.ActivatePianoButton.SetActive(false);
        this.StartPuzzleButton.SetActive(false);
    }


    public static bool IsPlayerCollider(Collider collider)
    {
        return collider.gameObject.layer == LayerMask.NameToLayer("Player");
    }


    private IEnumerator DoPuzzleSolvedCinematic(CandlePuzzleController puzzleController)
    {
        yield return new WaitForSeconds(0.7f);

        this.CameraController.TransitionToRig(this.PuzzleSolvedRig);
        yield return new WaitForSeconds(1.3f);

        //puzzleController.SheetMusicPiece.gameObject.SetActive(true);

        puzzleController.SheetMusicPiece.CrossFadeAlpha(1f, 1.0f, true);
        yield return new WaitForSeconds(0.5f);

        this.MusicAudio.PlayOneShot(puzzleController.SolvedClip);
        yield return new WaitForSeconds(2.0f);

        this.CameraController.TransitionToRig(this.PlayerController.CameraRig);
    }


    private void HandleCameraTransitionCompleted(CameraRig targetRig)
    {
        if (targetRig == this.PlayerController.CameraRig)
        {
            this.PlayerController.FreezeMotion = false;
        }

        if (this.CurrentProximityPuzzle != null)
        {
            this.StartPuzzleButton.SetActive(false);
        }
    }


    public void StartPuzzlePressed()
    {
        if (this.CurrentProximityPuzzle != null)
        {
            this.StartPuzzle(this.CurrentProximityPuzzle);
        }
    }


    private void HandlePlayerEnteredPuzzleArea(CandlePuzzleController puzzleController)
    {
        if (puzzleController.IsSolved == false)
        {
            this.StartPuzzleButton.gameObject.SetActive(true);
            this.CurrentProximityPuzzle = puzzleController;
        }
    }


    private void HandlePlayerLeftPuzzleArea(CandlePuzzleController puzzleController)
    {
        this.StartPuzzleButton.gameObject.SetActive(false);
        this.CurrentProximityPuzzle = null;
    }


    private void StartPuzzle(CandlePuzzleController puzzleController)
    {
        this.CameraController.TransitionToRig(puzzleController.CameraRig);
        this.PlayerController.FreezeMotion = true;
    }


    private void HandlePuzzleSolved(CandlePuzzleController puzzleController)
    {
        if (this.PlayerController.SolvedPuzzlesInHand.Contains(puzzleController))
        {
            Debug.LogError("should not happen");
        }
        else
        {
            StartCoroutine(this.DoPuzzleSolvedCinematic(puzzleController));
            this.PlayerController.SolvedPuzzlesInHand.Add(puzzleController);
        }
    }


    public bool AllPuzzlesAreSolved()
    {
        bool isSolved = true;

        for (int i = 0; i < this.PuzzleControllers.Count; i++)
        {
            if (this.PuzzleControllers[i].IsSolved == false)
            {
                isSolved = false;
                break;
            }
        }

        return isSolved;
    }


    public void ActivatePianoPressed()
    {
        if (this.PlayerIsInPianoProximity == true && this.AllPuzzlesAreSolved())
        {
            this.MusicAudio.PlayOneShot(this.FinalSong);
            this.ActivatePianoButton.SetActive(false);
            this.PlayerController.FreezeMotion = true;
            this.EndGamePanel.SetActive(true);
            this.CameraController.TransitionToRig(this.EndGameRig);
        }
        else
        {
            Debug.LogError("should not happen");
        }
    }


    public void RestartGamePressed()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
    }


    private void HandlePlayerEnteredPianoArea()
    {
        this.PlayerIsInPianoProximity = true;

        Debug.Log("hi paino: " +this.AllPuzzlesAreSolved());

        if (this.AllPuzzlesAreSolved() == true)
        {
            this.ActivatePianoButton.SetActive(true);
        }
    }



    private void HandlePlayerLeftPianoArea()
    {
        this.PlayerIsInPianoProximity = false;

        this.ActivatePianoButton.SetActive(false);
    }


    //public static Vector3 GetCursorWorldPosition()
    //{
    //    Plane plane = new Plane(_Instance.CameraController.GameCamera.gameObject.transform.forward, 5f);
    //    Ray ray = _Instance.CameraController.GameCamera.ScreenPointToRay(Input.mousePosition);
    //    float point = 0f;

    //    Vector3 targetPos = Vector3.zero;

    //    if (plane.Raycast(ray, out point))
    //        targetPos = ray.GetPoint(point);

    //    return targetPos;
    //}


    private void Update()
    {
        if (this.CameraController.IsSetToRig(this.StartingRig) && Input.anyKey)
        {
            // Just for start of game.   TODO state machine time permitting
            this.StartGamePanel.SetActive(false);
            this.CameraController.TransitionToRig(this.PlayerController.CameraRig);
        }
        else if (this.CurrentProximityPuzzle != null && Input.GetKeyUp(KeyCode.Escape) == true)
        {
            this.CameraController.TransitionToRig(this.PlayerController.CameraRig);
        }
    }
}
