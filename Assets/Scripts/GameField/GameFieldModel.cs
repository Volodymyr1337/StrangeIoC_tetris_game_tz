using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameFieldModel
{
    public float FieldSizeX { get; private set; }
    public float FieldSizeY { get; private set; }

    public float ShapeSizeX { get; private set; }
    public float ShapeSizeY { get; private set; }

    public float FieldOffsetY { get; private set; }

    public void Initialization(float fieldSizeX, float fieldSizeY, float fieldOffsetY, float shapeSizeX, float shapeSizeY)
    {
        FieldSizeX = fieldSizeX;
        FieldSizeY = fieldSizeY;
        FieldOffsetY = fieldOffsetY;
        ShapeSizeX = shapeSizeX;
        ShapeSizeY = shapeSizeY;
    }
}
