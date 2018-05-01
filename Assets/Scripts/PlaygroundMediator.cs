using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.mediation.impl;
using strange.extensions.pool.api;

public class PlaygroundMediator : EventMediator
{
    [Inject]
    public PlaygroundView PlaygroundView { get; private set; }

    [Inject(Shapes.Z)]
    public IPool<GameObject> Pool { get; private set; }

    public override void OnRegister()
    {
        GameObject go = Pool.GetInstance();
        go.SetActive(true);
        go.transform.parent = PlaygroundView.transform;
        go.transform.localScale = Vector3.one;
        PlaygroundView.Initialization(go);
    }

}
