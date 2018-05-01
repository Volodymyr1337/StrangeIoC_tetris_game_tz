using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.pool.api;
using strange.extensions.dispatcher.eventdispatcher.api;

public class PlaygroundMediator : EventMediator
{
    [Inject]
    public PlaygroundView PlaygroundView { get; private set; }

    public override void OnRegister()
    {
        dispatcher.AddListener(PlaygroundEvent.SHAPE_CREATED, OnShapeCreated);
        dispatcher.Dispatch(PlaygroundEvent.CREATE_SHAPE);
    }

    private void OnShapeCreated(IEvent evnt)
    {
        GameObject shape = evnt.data as GameObject;
        shape.transform.parent = PlaygroundView.transform;
        shape.transform.localScale = Vector3.one;
        PlaygroundView.TestShapeInit(shape);
    }

    public override void OnRemove()
    {
        dispatcher.RemoveListener(PlaygroundEvent.SHAPE_CREATED, OnShapeCreated);
    }

}
