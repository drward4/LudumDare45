using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void CandleSlotEvent(CandleSlot candleSlot);

public class CandleSlot : MonoBehaviour
{
    public CandleSlotEvent WasSelected;
    public Candle CurrentCandle;
    public Transform CandleContainer;

    private ClickHandler CandleClickHandler;

    private void Awake()
    {
        this.CandleClickHandler = this.GetComponentInChildren<ClickHandler>();
    }


    private void Start()
    {
        this.CandleClickHandler.WasClicked += this.HandleClick;
    }


    public void SetCurrentCandle(Candle candle)
    {
        candle.transform.SetParent(this.CandleContainer);
        candle.transform.localPosition = Vector3.zero;
        this.CurrentCandle = candle;
    }

    private void HandleClick()
    {
        if (this.WasSelected != null)
        {
            this.WasSelected(this);
        }
    }
}
