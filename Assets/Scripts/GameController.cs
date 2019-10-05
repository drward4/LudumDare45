using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public PlayerController PlayerController;
    public CameraController CameraController;
    public CameraRig StartingRig;

    public List<CandlePuzzleController> PuzzleControllers;

    private static GameController _Instance;

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
        }
    }


    private void HandleCameraTransitionCompleted(CameraRig targetRig)
    {
        if (targetRig == this.PlayerController.CameraRig)
        {
            this.PlayerController.FreezeMotion = false;
        }
    }


    private void HandlePlayerEnteredPuzzleArea(CandlePuzzleController puzzleController)
    {
        this.CameraController.TransitionToRig(puzzleController.CameraRig);
        this.PlayerController.FreezeMotion = true;
    }


    public static Vector3 GetCursorWorldPosition()
    {
        Plane plane = new Plane(Vector3.back, 0f);
        Ray ray = _Instance.CameraController.GameCamera.ScreenPointToRay(Input.mousePosition);
        float point = 0f;

        Vector3 targetPos = Vector3.zero;

        if (plane.Raycast(ray, out point))
            targetPos = ray.GetPoint(point);

        return targetPos;
    }



    private void Update()
    {
        if (this.PlayerController.FreezeMotion == true && Input.GetKeyUp(KeyCode.Escape) == true)
        {
            this.CameraController.TransitionToRig(this.PlayerController.CameraRig);
        }
    }
}
