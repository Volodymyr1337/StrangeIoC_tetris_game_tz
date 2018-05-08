using UnityEngine;
using strange.extensions.command.impl;

public class InitGameFieldModelCommand : EventCommand
{
    [Inject]
    public GameFieldModel GameFieldModel { get; private set; }

    [Inject]
    public IScaleFactor ScreenScaleFactor { get; private set; }

    public override void Execute()
    {
        GameObject shape = evt.data as GameObject;
        GameFieldView fieldView = Object.FindObjectOfType<GameFieldView>();
        BoxCollider2D fieldBox = fieldView.GetComponent<BoxCollider2D>();
        float fieldSizeX = fieldBox.size.x * ScreenScaleFactor.X;
        float fieldSizeY = fieldBox.size.y * ScreenScaleFactor.Y;
        
        var blockSprite = shape.transform.GetComponentInChildren<SpriteRenderer>();

        float shapeSizeX = blockSprite.size.x * ScreenScaleFactor.X * blockSprite.transform.localScale.x;
        float shapeSizeY = blockSprite.size.y * ScreenScaleFactor.Y * blockSprite.transform.localScale.y;

        int gridSizeX = (int)(fieldSizeX / shapeSizeX);
        int gridSizeY = (int)(fieldSizeY / shapeSizeY);

        shapeSizeX = fieldSizeX / gridSizeX;
        shapeSizeY = fieldSizeY / gridSizeY;

        GameFieldModel.Initialization(fieldSizeX, fieldSizeY, fieldBox.offset.y, shapeSizeX, shapeSizeY, new BlockStruct[gridSizeX, gridSizeY]);
    }
}
