using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.command.impl;

public class GameOverCommand : EventCommand
{

    [Inject]
    public GameFieldModel GameFieldModel { get; private set; }

    public override void Execute()
    {
        GameFieldModel.GameOver = true;
        GameObject gameOverCanvas = Resources.Load<GameObject>("GameOverCanvas");
        Object.Instantiate(gameOverCanvas);
    }
}
