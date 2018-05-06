using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.pool.api;
using strange.extensions.dispatcher.eventdispatcher.api;

public class GameFieldMediator : EventMediator
{
    [Inject]
    public GameFieldView GameFieldView { get; private set; }
    
    [Inject]
    public GameFieldModel GameFieldModel { get; private set; }
    
    public override void OnRegister()
    {
        dispatcher.AddListener(GameFieldEvent.CREATED_SHAPE, OnShapeCreated);
        dispatcher.AddListener(GameFieldEvent.LANDED_SHAPE, OnShapeLanded);

        for(var i =0; i < GameFieldView.SpawnPoints.Length; i++)
            dispatcher.Dispatch(GameFieldEvent.CREATE_SHAPE);

        GameFieldView.Initialization();
    }

    private void OnShapeLanded(IEvent evnt)
    {
        GameObject shape = evnt.data as GameObject;
        shape.transform.parent = GameFieldView.transform;

        CalcShapeCoords(shape);

        dispatcher.Dispatch(GameFieldEvent.CREATE_SHAPE);
    }

    private void OnShapeCreated(IEvent evnt)
    {
        GameFieldView.ShapeInit(evnt.data as GameObject);
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(GameFieldEvent.CREATED_SHAPE, OnShapeCreated);
        dispatcher.RemoveListener(GameFieldEvent.LANDED_SHAPE, OnShapeLanded);
    }

    public void CalcShapeCoords(GameObject shape)
    {
        for (int i = 0; i < shape.transform.childCount; i++)
        {
            if (shape.transform.GetChild(i).name != "Block")
                continue;
            int firstBlockX = (int)((shape.transform.GetChild(i).transform.position.x + GameFieldModel.FieldSizeX / 2f) / GameFieldModel.ShapeSizeX);
            int firstBlockY = Mathf.RoundToInt(shape.transform.GetChild(i).transform.position.y / GameFieldModel.ShapeSizeY + GameFieldModel.FieldOffsetY);
            Debug.Log("fb (x/y): " + firstBlockX + "/" + firstBlockY);
        }
    }
}
