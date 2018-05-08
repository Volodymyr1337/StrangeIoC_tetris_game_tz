using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.dispatcher.eventdispatcher.api;

public class BlockMediator : EventMediator
{
    [Inject]
    public BlockView BlockView { get; private set; }
    
    [Inject]
    public GameFieldModel GameFieldModel { get; private set; }

    public override void OnRegister()
    {
        BlockView.Initialization();

        BlockView.OnMoouseDownSignal.AddListener(OnMouseDownHandler);
        BlockView.OnMoouseUpSignal.AddListener(OnMouseUpHandler);
    }

    private void OnMouseDownHandler(GameObject shape)
    {
        if (GameFieldModel.GameOver)
            return;

        BlockView.Move = true;
        for (var i = 0; i < shape.transform.childCount; i++)
        {
            shape.transform.GetChild(i).tag = ShapeState.Held.ToString();
        }
    }

    private void OnMouseUpHandler(GameObject shape)
    {
        if (GameFieldModel.GameOver)
            return;

        BlockView.Move = false;
        BlockView.StartCoroutine(BlockView.SetCorrectShapePos(delegate ()
        {
            int blocksCanLandedCount = 0;
            var blockViews = shape.GetComponentsInChildren<BlockView>();
            foreach (var bv in blockViews)
            {
                if (bv.CanLanded)
                    blocksCanLandedCount++;
            }

            if (blocksCanLandedCount == blockViews.Length)
            {
                for (var i = 0; i < shape.transform.childCount; i++)
                {
                    shape.transform.GetChild(i).tag = ShapeState.Landed.ToString();
                    var sr = shape.transform.GetChild(i).GetComponent<SpriteRenderer>();
                    if (sr)
                        sr.sortingOrder = 1;
                }
                dispatcher.AddListener(GameFieldEvent.LANDING_FAILED, OnLandingFailed);
                dispatcher.Dispatch(GameFieldEvent.TRY_LANDED_SHAPE, shape);                
            }
            else
            {
                FailLanding(shape);
            }
        }));
    }

    private void FailLanding(GameObject shape)
    {
        for (var i = 0; i < shape.transform.childCount; i++)
            shape.transform.GetChild(i).tag = ShapeState.Block.ToString();

        BlockView.ResetShapePos(shape);
    }

    private void OnLandingFailed(IEvent evt)
    {
        GameObject shape = evt.data as GameObject;
        FailLanding(shape);
        dispatcher.RemoveListener(GameFieldEvent.LANDING_FAILED, OnLandingFailed);
    }
    
    public override void OnRemove()
    {
        BlockView.OnMoouseDownSignal.RemoveListener(OnMouseDownHandler);
        BlockView.OnMoouseUpSignal.RemoveListener(OnMouseUpHandler);       
    }
}
