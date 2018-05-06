using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;

public class GameFieldView : View
{
    [Inject]
    public IScaleFactor ScreenScaleFactor { get; private set; }
    
    [SerializeField] private SpriteRenderer gameFieldSprt;
    public SpriteRenderer GameFieldSprt
    {
        get { return gameFieldSprt; }
    }
    [SerializeField] private GameObject[] spawnPoints;
    public GameObject[] SpawnPoints
    {
        get { return spawnPoints; }
    }
    
    public void Initialization()
    {
        transform.localScale = new Vector3(ScreenScaleFactor.X, ScreenScaleFactor.Y, transform.localScale.z);
        transform.position = Vector3.zero;
    }

    public void ShapeInit(GameObject shape)
    {
        if (spawnPoints == null)
            return;

        for (var i = 0; i < spawnPoints.Length; i++)
        {
            if (spawnPoints[i].transform.childCount == 0)
            {
                shape.transform.parent = spawnPoints[i].transform;
                shape.transform.localScale = Vector3.one;
                shape.transform.localEulerAngles = new Vector3(0f, 0f, Random.Range(-1f, 1f) > 0f ? (Random.Range(-1f, 1f) > 0f? 0f : 180f): 90f);
                shape.transform.position = spawnPoints[i].transform.position;
                break;
            }            
        }
    }
}
