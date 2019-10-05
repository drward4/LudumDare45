using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CameraControllerEvent(CameraRig targetRig);

public class CameraController : MonoBehaviour
{
    public Camera GameCamera;
    public float TransitionTime = 3f;

    public CameraControllerEvent TransitionCompleted;

    private CameraRig CurrentRig;
    private CameraRig PreviousRig;
    private float TransitionTimeRemaining;


    public void SetRig(CameraRig rig)
    {
        this.CurrentRig = rig;
        this.PreviousRig = rig;
        this.TransitionTimeRemaining = 0f;
        this.GameCamera.transform.SetParent(this.CurrentRig.Anchor, true);
        this.GameCamera.transform.position = this.CurrentRig.Anchor.position;
        this.GameCamera.transform.rotation = Quaternion.LookRotation(this.CurrentRig.Lookat.position - this.CurrentRig.Anchor.position);
    }


    public void TransitionToRig(CameraRig rig)
    {
        this.PreviousRig = this.CurrentRig;
        this.CurrentRig = rig;
        this.GameCamera.transform.SetParent(this.CurrentRig.Anchor, true);

        if (this.TransitionTimeRemaining > 0f)
        {
            // in the middle of a transition so back out
            float d = this.TransitionTimeRemaining / this.TransitionTime;
            this.TransitionTimeRemaining = Mathf.Clamp01((1f - d) * this.TransitionTime);
        }
        else
        {
            this.TransitionTimeRemaining = this.TransitionTime;
        }
    }

    public float D;

    private void Update()
    {
        if (this.TransitionTimeRemaining > 0f)
        {
            this.TransitionTimeRemaining -= Time.deltaTime;

            float d = Mathf.Clamp01(1f - (this.TransitionTimeRemaining / this.TransitionTime));
            this.D = d;
            this.GameCamera.transform.position = Vector3.Lerp(this.PreviousRig.Anchor.position, this.CurrentRig.Anchor.position, d);

            Quaternion rotationA = Quaternion.LookRotation(this.PreviousRig.Lookat.position - this.PreviousRig.Anchor.position);
            Quaternion rotationB = Quaternion.LookRotation(this.CurrentRig.Lookat.position - this.CurrentRig.Anchor.position);

            this.GameCamera.transform.rotation = Quaternion.Lerp(rotationA, rotationB, d);

            if (this.TransitionTimeRemaining <= 0f && this.TransitionCompleted != null)
            {
                this.TransitionCompleted(this.CurrentRig);
            }
        }
    }
}
