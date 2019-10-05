using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    public Camera GameCamera;
    //public CameraRig CurrentRig;
    //public CameraRig PreviousRig;

    private static GameController _Instance;

    private void Awake()
    {
        _Instance = this;
    }


    public static Vector3 GetCursorWorldPosition()
    {
        Plane plane = new Plane(Vector3.back, 0f);
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        float point = 0f;

        Vector3 targetPos = Vector3.zero;

        if (plane.Raycast(ray, out point))
            targetPos = ray.GetPoint(point);

        return targetPos;
    }


    // TODO move camera rig stuff to separate component
    //public float testCameraTimeRem = 5f;
    //public float testCameraTime = 5f;

    private void Update()
    {
        //if (this.testCameraTimeRem > 0f)
        //{
        //    this.testCameraTimeRem -= Time.deltaTime;

        //    float d = Mathf.Clamp01(1f - (this.testCameraTimeRem / this.testCameraTime));
        //    this.GameCamera.transform.position = Vector3.Lerp(this.PreviousRig.Anchor.position, this.CurrentRig.Anchor.position, d);

        //    Quaternion rotationA = Quaternion.LookRotation(this.PreviousRig.Lookat.position - this.PreviousRig.Anchor.position);
        //    Quaternion rotationB = Quaternion.LookRotation(this.CurrentRig.Lookat.position - this.CurrentRig.Anchor.position);

        //    this.GameCamera.transform.rotation = Quaternion.Lerp(rotationA, rotationB, d);
        //}


    }
}
