using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.pool.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.context.api;

public class CreateShapeCommand : EventCommand
{
    [Inject(ShapeType.I)]
    public IPool<GameObject> Pool_I { get; private set; }
    [Inject(ShapeType.J)]
    public IPool<GameObject> Pool_J { get; private set; }
    [Inject(ShapeType.L)]
    public IPool<GameObject> Pool_L { get; private set; }
    [Inject(ShapeType.O)]
    public IPool<GameObject> Pool_O { get; private set; }
    [Inject(ShapeType.S)]
    public IPool<GameObject> Pool_S { get; private set; }
    [Inject(ShapeType.T)]
    public IPool<GameObject> Pool_T { get; private set; }
    [Inject(ShapeType.Z)]
    public IPool<GameObject> Pool_Z { get; private set; }

    public override void Execute()
    {
        int rand = Random.Range(0, System.Enum.GetValues(typeof(ShapeType)).Length);
        IPool<GameObject> randPool;

        switch ((ShapeType)rand)
        {
            case ShapeType.I:
                randPool = Pool_I;
                break;
            case ShapeType.J:
                randPool = Pool_J;
                break;
            case ShapeType.L:
                randPool = Pool_L;
                break;
            case ShapeType.O:
                randPool = Pool_O;
                break;
            case ShapeType.S:
                randPool = Pool_S;
                break;
            case ShapeType.T:
                randPool = Pool_T;
                break;
            case ShapeType.Z:
                randPool = Pool_Z;
                break;
            default:
                randPool = Pool_I;
                break;
        }

        GameObject shape = randPool.GetInstance();
        dispatcher.Dispatch(GameFieldEvent.GAME_FIELD_INIT, shape);
        dispatcher.Dispatch(GameFieldEvent.CREATED_SHAPE, shape);
    }
}