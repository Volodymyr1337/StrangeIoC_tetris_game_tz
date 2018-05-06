using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.command.impl;

public class SetupScreenScaleCommand : Command
{
    [Inject]
    public IScaleFactor ScreenScaleFactor { get; private set; }

    public override void Execute()
    {
        GameFieldView field = Object.FindObjectOfType<GameFieldView>();
        ScreenScaleFactor.Initialization(field.GameFieldSprt);
    }
}
