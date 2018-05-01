using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using strange.extensions.context.impl;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;
using strange.framework.api;


public class MainContext : MVCSContext
{
    
    public MainContext(MonoBehaviour view) : base (view) { }
    
    protected override void mapBindings()
    {
        foreach (Shape shape in Enum.GetValues(typeof(Shape)))
        {
            injectionBinder.Bind<IPool<GameObject>>().To<Pool<GameObject>>().ToSingleton().ToName(shape);
        }
        
        mediationBinder.Bind<PlaygroundView>().To<PlaygroundMediator>();

        commandBinder.Bind(PlaygroundEvent.CREATE_SHAPE).To<CreateShapeCommand>().Pooled();
    }

    protected override void postBindings()
    {
        foreach (Shape shape in Enum.GetValues(typeof(Shape)))
        {
            IPool<GameObject> shapesPool = injectionBinder.GetInstance<IPool<GameObject>>(shape);
            shapesPool.instanceProvider = new ResourceInstanceProvider($"Shapes/{shape}", LayerMask.NameToLayer("Figure"));
            shapesPool.inflationType = PoolInflationType.INCREMENT;
        }

        base.postBindings();
    }
}
