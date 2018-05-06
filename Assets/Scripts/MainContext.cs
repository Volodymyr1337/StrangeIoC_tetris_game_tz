using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;
using strange.extensions.context.impl;
using strange.extensions.pool.api;
using strange.extensions.pool.impl;
using strange.framework.api;
using strange.extensions.context.api;

public class MainContext : MVCSContext
{
    
    public MainContext(MonoBehaviour view) : base (view) { }
    
    protected override void mapBindings()
    {

        injectionBinder.Bind<IScaleFactor>().To<ScreenScaleFactor>().ToSingleton();
        injectionBinder.Bind<GameFieldModel>().ToSingleton();

        foreach (ShapeType shape in Enum.GetValues(typeof(ShapeType)))
        {
            injectionBinder.Bind<IPool<GameObject>>().To<Pool<GameObject>>().ToSingleton().ToName(shape);
        }

        mediationBinder.Bind<GameFieldView>().To<GameFieldMediator>();
        mediationBinder.Bind<BlockView>().To<BlockMediator>();
        
        commandBinder.Bind(ContextEvent.START).To<CreateGameFieldCommand>().To<SetupScreenScaleCommand>().InSequence().Once();
        commandBinder.Bind(GameFieldEvent.CREATE_SHAPE).To<CreateShapeCommand>().Pooled();
        commandBinder.Bind(GameFieldEvent.CREATED_SHAPE).To<InitGameFieldModelCommand>().Once();

        commandBinder.Bind(GameFieldEvent.TRY_LANDED_SHAPE).To<TryLandShapeCommand>().Pooled();
    }

    protected override void postBindings()
    {
        foreach (ShapeType shape in Enum.GetValues(typeof(ShapeType)))
        {
            IPool<GameObject> shapesPool = injectionBinder.GetInstance<IPool<GameObject>>(shape);
            shapesPool.instanceProvider = new ResourceInstanceProvider($"Shapes/{shape}", LayerMask.NameToLayer("Figure"));
            shapesPool.inflationType = PoolInflationType.INCREMENT;
        }

        base.postBindings();
    }
}
