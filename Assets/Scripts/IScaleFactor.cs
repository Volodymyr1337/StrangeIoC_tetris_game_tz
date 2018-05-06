using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IScaleFactor
{
    float X { get; set; }
    float Y { get; set; }

    void Initialization(SpriteRenderer sr);
}
