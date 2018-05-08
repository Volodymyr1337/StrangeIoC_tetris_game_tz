using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.command.impl;

public class CheckFreeSpaceCommand : EventCommand
{
    [Inject]
    public GameFieldModel GameFieldModel { get; private set; }

    private int CantSetShapesCount;

	public override void Execute()
    {
        CantSetShapesCount = 0;
        GameObject[] spawnPoints = evt.data as GameObject[];
        foreach (var spawnPoint in spawnPoints)
        {
            if (spawnPoint.transform.childCount <= 0)
                continue;

            GameObject shape = spawnPoint.transform.GetChild(0).gameObject;

            List<Vector2> possibleCoords = new List<Vector2>();
            SpriteRenderer firstBlock = null;

            foreach (var block in shape.GetComponentsInChildren<SpriteRenderer>())
            {
                if (firstBlock == null)
                {
                    firstBlock = block;
                    possibleCoords.Add(new Vector2(0, 0));
                }
                else
                {
                    int sign_x = block.transform.position.x > firstBlock.transform.position.x ? 1 : -1;
                    int sign_y = block.transform.position.y > firstBlock.transform.position.y ? 1 : -1;
                    var from = new Vector3(block.transform.position.x, 0f, 0f);
                    var to = new Vector3(firstBlock.transform.position.x, 0f, 0f);
                    float distance = Vector3.Distance(from, to);
                    int counterX = 1;
                    while (distance > (firstBlock.bounds.size.x * counterX))
                    {
                        counterX++;
                    }
                    counterX--;
                    var from_y = new Vector3(0f, block.transform.position.y, 0f);
                    var to_y = new Vector3(0f, firstBlock.transform.position.y, 0f);
                    float distance_y = Vector3.Distance(from_y, to_y);
                    int counter_y = 1;
                    while (distance_y > (firstBlock.bounds.size.x * counter_y))
                    {
                        counter_y++;
                    }
                    counter_y--;
                    counterX *= sign_x;
                    counter_y *= sign_y;
                    possibleCoords.Add(new Vector2(counterX, counter_y));
                }
            }
            bool possible = false;
            for (var y = 0; y < GameFieldModel.FieldGrid.GetLength(1); y++)
            {
                for (var x = 0; x < GameFieldModel.FieldGrid.GetLength(0); x++)
                {
                    int canPlace = 0;
                    foreach (var cord in possibleCoords)
                    {
                        int xPos = x + (int)cord.x;
                        int yPos = y + (int)cord.y;

                        if (xPos >= 0 && xPos < GameFieldModel.FieldGrid.GetLength(0) && yPos >= 0 && yPos < GameFieldModel.FieldGrid.GetLength(1) && GameFieldModel.FieldGrid[xPos, yPos].Block == null)
                        {
                            canPlace++;
                        }
                    }
                    if (canPlace == possibleCoords.Count)
                    {
                        possible = true;
                        break;
                    }
                }
                if (possible)
                    break;
            }
            if (!possible)
            {
                CantSetShapesCount++;
                if (CantSetShapesCount >= spawnPoints.Length)
                    dispatcher.Dispatch(GameFieldEvent.GAME_OVER);
            }
        }
    }
}
