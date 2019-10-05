using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Candle : MonoBehaviour
{
    //public CandleSlot CurrentSlot;

    public ParticleSystem FlameParticles;
    public Transform ScalableObjectsContainer;
    public int NoteID;

    private float Size;

    public void SetSize(float size)
    {
        this.Size = size;
        this.ScalableObjectsContainer.transform.localScale = new Vector3(this.ScalableObjectsContainer.transform.localScale.x, size, this.ScalableObjectsContainer.transform.localScale.z);
    }
}
