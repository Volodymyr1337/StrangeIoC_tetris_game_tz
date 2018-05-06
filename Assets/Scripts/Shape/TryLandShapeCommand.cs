using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.command.impl;

public class TryLandShapeCommand : EventCommand
{
    [Inject]
    public GameFieldModel GameFieldModel { get; private set; }

    
    public override void Execute()
    {
        GameObject shape = evt.data as GameObject;

        Debug.Log("TryLandShapeCommand: " + shape.name);

        Dictionary<Vector2, GameObject> blockCoords = new Dictionary<Vector2, GameObject>();

        for (int i = 0; i < shape.transform.childCount; i++)
        {
            if (shape.transform.GetChild(i).name != "Block" || (shape.transform.GetChild(i).transform.position.x) < -(GameFieldModel.FieldSizeX / 2f))
                continue;

            int firstBlockX = (int)((shape.transform.GetChild(i).transform.position.x + GameFieldModel.FieldSizeX / 2f) / GameFieldModel.ShapeSizeX);
            int firstBlockY = Mathf.RoundToInt(shape.transform.GetChild(i).transform.position.y / GameFieldModel.ShapeSizeY + GameFieldModel.FieldOffsetY);

            if (firstBlockX >= 0 && firstBlockX < GameFieldModel.FieldGrid.GetLength(0) && firstBlockY >= 0 && firstBlockY < GameFieldModel.FieldGrid.GetLength(1) && GameFieldModel.FieldGrid[firstBlockX, firstBlockY].Block == null)
            {
                blockCoords.Add(new Vector2(firstBlockX, firstBlockY), shape.transform.GetChild(i).gameObject);
            }
        }
        // -1 потому что 1 обьект - Pivot
        if ((shape.transform.childCount - 1) == blockCoords.Count)
        {
            foreach (var coords in blockCoords)
            {
                GameFieldModel.FieldGrid[(int)coords.Key.x, (int)coords.Key.y].Block = coords.Value;
            }
            for(var y = 0; y < GameFieldModel.FieldGrid.GetLength(0); y++)
            {
                int counter = 0;
                for (var x = 0; x < GameFieldModel.FieldGrid.GetLength(1); x++)
                {
                    if (GameFieldModel.FieldGrid[x,y].Block != null)
                        counter++;
                }

                if (counter == GameFieldModel.FieldGrid.GetLength(1))
                    dispatcher.Dispatch(GameFieldEvent.REMOVE_BLOCKS, y);
                /*
                for (var x = 0; x < GameFieldModel.FieldGrid.GetLength(1); x++)
                {
                    GameFieldModel.FieldGrid[x, y].Block
                }
                */
            }
            dispatcher.Dispatch(GameFieldEvent.LANDED_SHAPE, shape);
        }
        else
        {
            dispatcher.Dispatch(GameFieldEvent.LANDING_FAILED, shape);
        }
    }
}
