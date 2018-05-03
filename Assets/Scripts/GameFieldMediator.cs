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

    public override void OnRegister()
    {
        dispatcher.AddListener(GameFieldEvent.SHAPE_CREATED, OnShapeCreated);
        dispatcher.Dispatch(GameFieldEvent.CREATE_SHAPE);
    }

    private void OnShapeCreated(IEvent evnt)
    {
        GameObject shape = evnt.data as GameObject;
        shape.transform.parent = GameFieldView.transform;
        shape.transform.localScale = Vector3.one;
        GameFieldView.TestShapeInit(shape);
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(GameFieldEvent.SHAPE_CREATED, OnShapeCreated);
    }

}
