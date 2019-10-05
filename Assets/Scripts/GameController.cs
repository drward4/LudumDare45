using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    public PlayerController PlayerController;
    public CameraController CameraController;
    public CameraRig StartingRig;
    public List<CandlePuzzleController> PuzzleControllers;
    
    public Canvas GameCanvas;
    public GameObject StartPuzzleButton;

    private static GameController _Instance;
    private CandlePuzzleController CurrentProximityPuzzle;

    private void Awake()
    {
        _Instance = this;
    }


    private void Start()
    {
        this.CameraController.SetRig(this.StartingRig);
        this.CameraController.TransitionCompleted += this.HandleCameraTransitionCompleted;

        for (int i = 0; i < this.PuzzleControllers.Count; i++)
        {
            this.PuzzleControllers[i].PlayerEnteredArea += HandlePlayerEnteredPuzzleArea;
            this.PuzzleControllers[i].PlayerLeftArea += HandlePlayerLeftPuzzleArea;
        }

        this.StartPuzzleButton.SetActive(false);
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
        this.StartPuzzleButton.gameObject.SetActive(true);
        this.CurrentProximityPuzzle = puzzleController;
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
        if (this.PlayerController.FreezeMotion == true && Input.GetKeyUp(KeyCode.Escape) == true)
        {
            this.CameraController.TransitionToRig(this.PlayerController.CameraRig);
        }
    }
}
