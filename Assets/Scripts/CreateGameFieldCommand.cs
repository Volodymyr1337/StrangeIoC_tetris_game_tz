using strange.extensions.command.impl;
using UnityEngine;
using System;

public class CreateGameFieldCommand : Command
{

    public override void Execute()
    {
        GameObject gameField = Resources.Load<GameObject>("GameField");
        if (gameField)
        {
            GameObject.Instantiate(gameField);
        }
    }
}
