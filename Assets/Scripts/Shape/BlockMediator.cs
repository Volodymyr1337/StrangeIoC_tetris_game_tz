using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.dispatcher.eventdispatcher.api;

public class BlockMediator : EventMediator
{
    [Inject]
    public BlockView BlockView { get; private set; }
    
    public override void OnRegister()
    {
        BlockView.Initialization();

        BlockView.OnMoouseDownSignal.AddListener(OnMouseDownHandler);
        BlockView.OnMoouseUpSignal.AddListener(OnMouseUpHandler);
    }

    private void OnMouseDownHandler(GameObject shape)
    {
        for(var i = 0; i < shape.transform.childCount; i++)
        {
            shape.transform.GetChild(i).tag = ShapeState.Held.ToString();
        }
    }

    private void OnMouseUpHandler(GameObject shape)
    {
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

                dispatcher.Dispatch(GameFieldEvent.LANDED_SHAPE, shape);
            }
            else
            {
                for (var i = 0; i < shape.transform.childCount; i++)
                    shape.transform.GetChild(i).tag = ShapeState.Block.ToString();
                BlockView.ResetShapePos();
            }
        }));
    }

    public override void OnRemove()
    {
        BlockView.OnMoouseDownSignal.RemoveListener(OnMouseDownHandler);
        BlockView.OnMoouseUpSignal.RemoveListener(OnMouseUpHandler);
    }
}
