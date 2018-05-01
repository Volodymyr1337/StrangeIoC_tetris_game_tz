using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.command.impl;
using strange.extensions.pool.api;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.context.api;

public class CreateShapeCommand : Command
{
    [Inject(ContextKeys.CONTEXT_DISPATCHER)]
    public IEventDispatcher Dispatcher { get; private set; }

    [Inject(Shape.I)]
    public IPool<GameObject> Pool_I { get; private set; }
    [Inject(Shape.J)]
    public IPool<GameObject> Pool_J { get; private set; }
    [Inject(Shape.L)]
    public IPool<GameObject> Pool_L { get; private set; }
    [Inject(Shape.O)]
    public IPool<GameObject> Pool_O { get; private set; }
    [Inject(Shape.S)]
    public IPool<GameObject> Pool_S { get; private set; }
    [Inject(Shape.T)]
    public IPool<GameObject> Pool_T { get; private set; }
    [Inject(Shape.Z)]
    public IPool<GameObject> Pool_Z { get; private set; }

    public override void Execute()
    {
        int rand = Random.Range(0, System.Enum.GetValues(typeof(Shape)).Length);
        IPool<GameObject> randPool;

        switch ((Shape)rand)
        {
            case Shape.I:
                randPool = Pool_I;
                break;
            case Shape.J:
                randPool = Pool_J;
                break;
            case Shape.L:
                randPool = Pool_L;
                break;
            case Shape.O:
                randPool = Pool_O;
                break;
            case Shape.S:
                randPool = Pool_S;
                break;
            case Shape.T:
                randPool = Pool_T;
                break;
            case Shape.Z:
                randPool = Pool_Z;
                break;
            default:
                randPool = Pool_I;
                break;
        }

        GameObject shape = randPool.GetInstance();
        
        Dispatcher.Dispatch(PlaygroundEvent.SHAPE_CREATED, shape);
    }
}