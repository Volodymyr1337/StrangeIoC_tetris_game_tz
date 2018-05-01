using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using UnityEngine;

public class PlaygroundView : View
{

    [SerializeField] private GameObject tempPrefab;
    private bool canMove;
    private GameObject tempGo;

    private float kx, ky;

    private Vector3 lastShapePos;


    public void Initialization(GameObject go)
    {
        tempGo = go;

        lastShapePos = new Vector3((kx + 0.021f)/2f, (ky + 0.021f) / 2f);
    }
    
    protected override void Awake()
    {
        base.Awake();
        SpriteRenderer sr = GetComponent<SpriteRenderer>();
        if (sr == null) return;

        transform.localScale = Vector3.one;

        float width = sr.sprite.bounds.size.x;
        float height = sr.sprite.bounds.size.y;

        float worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        float worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;
        kx = worldScreenWidth / width;
        ky = worldScreenHeight / height;
        transform.localScale = new Vector3(kx, ky, transform.localScale.z);
        Debug.Log(kx + " " + worldScreenWidth / width + " y: " + ky + " " + worldScreenHeight / height);
    }
    
    public void MoveTrue()
    {
        canMove = true;
        Debug.Log("move true");
    }

    public void MoveFalse()
    {
        canMove = false;
        Debug.Log("move false");
    }

    private void Update()
    {
        if (canMove)
        {
            var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            pos.z = 0f;

            tempGo.transform.position = PositionRounding(pos);
        }
    }

    private Vector3 PositionRounding(Vector3 position)
    {
        float xCord, yCord;

        if (position.x < (lastShapePos.x - 0.3f))
            xCord = lastShapePos.x - (kx+0.021f) ;
        else if (position.x > (lastShapePos.x + 0.3f))
            xCord = lastShapePos.x + (kx + 0.021f);
        else
            xCord = lastShapePos.x;

        if (position.y < (lastShapePos.y - 0.4f))
            yCord = lastShapePos.y - (ky + 0.021f);
        else if (position.y > (lastShapePos.y + 0.2f))
            yCord = lastShapePos.y + (ky + 0.021f);
        else
            yCord = lastShapePos.y;
        
        lastShapePos = new Vector3(xCord, yCord, 0f);
        
        return lastShapePos;
    }

    void OnMouseDown()
    {
        MoveTrue();
    }
    /* обнаружение figurki
       var pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector3.forward, 100f, 1<<8);
    */
    private void OnMouseUp()
    {
        MoveFalse();
    }

}
