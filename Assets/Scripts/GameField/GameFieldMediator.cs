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
        dispatcher.AddListener(GameFieldEvent.REMOVE_BLOCKS, RemoveBlocksHandler);

        for(var i =0; i < GameFieldView.SpawnPoints.Length; i++)
            dispatcher.Dispatch(GameFieldEvent.CREATE_SHAPE);

        GameFieldView.Initialization();
    }

    private void OnShapeLanded(IEvent evnt)
    {
        GameObject shape = evnt.data as GameObject;
        shape.transform.parent = GameFieldView.transform;
        
        dispatcher.Dispatch(GameFieldEvent.CREATE_SHAPE);
    }

    private void RemoveBlocksHandler(IEvent evnt)
    {
        int? y = evnt.data as int?;
        if (y == null)
            return;
        
        for (var x = 0; x < GameFieldModel.FieldGrid.GetLength(0); x++)
        {
            Destroy(GameFieldModel.FieldGrid[x, (int)y].Block);
        }
    }

    private void OnShapeCreated(IEvent evnt)
    {
        GameFieldView.ShapeInit(evnt.data as GameObject, GameFieldModel.FieldGrid);
        dispatcher.Dispatch(GameFieldEvent.CHECK_FREE_SPACE, GameFieldView.SpawnPoints);
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(GameFieldEvent.CREATED_SHAPE, OnShapeCreated);
        dispatcher.RemoveListener(GameFieldEvent.LANDED_SHAPE, OnShapeLanded);
        dispatcher.RemoveListener(GameFieldEvent.REMOVE_BLOCKS, RemoveBlocksHandler);
    }
}
