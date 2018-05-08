using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum GameFieldEvent
{
    GAME_FIELD_INIT,
    CREATE_SHAPE,
    CREATED_SHAPE,
    CHECK_FREE_SPACE,
    TRY_LANDED_SHAPE,
    LANDING_FAILED,
    LANDED_SHAPE,
    REMOVE_BLOCKS,
    GAME_OVER
}
