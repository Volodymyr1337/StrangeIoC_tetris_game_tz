using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using strange.extensions.dispatcher.eventdispatcher.api;
using strange.extensions.mediation.impl;
using strange.extensions.signal.impl;

public class BlockView : View
{
    [Inject]
    public IScaleFactor ScreenScaleFactor { get; private set; }

    public Signal<GameObject> OnMoouseDownSignal = new Signal<GameObject>();
    public Signal<GameObject> OnMoouseUpSignal = new Signal<GameObject>();

    private float xFactor
    {
        get { return ScreenScaleFactor.X; }
    }
    private float yFactor
    {
        get { return ScreenScaleFactor.Y; }
    }

    private Vector2 correctPos;
    private Vector2 defaultPos;

    private bool move = false;
    private GameObject HeldShape
    {
        get
        {
            return transform.parent.gameObject;
        }
    }
    public bool CanLanded;
    private float ratio = 0.021f;

    public Vector2 currPos;

    public void Initialization()
    {
        defaultPos = HeldShape.transform.position;
        correctPos = new Vector3((xFactor + ratio) / 2f, (yFactor + ratio) / 2f);
        CanLanded = true;
    }

    private void FixedUpdate()
    {
        currPos = transform.position;
        if (move)
        {
            Vector2 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            PositionRounding(pos);
            HeldShape.transform.position = pos;
        }
    }

    private Vector2 PositionRounding(Vector2 position)
    {
        float xCord, yCord;

        if (position.x < (correctPos.x - 0.3f))
            xCord = correctPos.x - (xFactor + ratio);
        else if (position.x > (correctPos.x + 0.3f))
            xCord = correctPos.x + (xFactor + ratio);
        else
            xCord = correctPos.x;

        if (position.y < (correctPos.y - 0.4f))
            yCord = correctPos.y - (yFactor + ratio);
        else if (position.y > (correctPos.y + 0.2f))
            yCord = correctPos.y + (yFactor + ratio);
        else
            yCord = correctPos.y;

        correctPos = new Vector2(xCord, yCord);

        return correctPos;
    }

    private void OnMouseDown()
    {
        if (this.transform.tag == ShapeState.Block.ToString())
        {
            move = true;
            OnMoouseDownSignal.Dispatch(HeldShape);
        }        
    }

    private void OnMouseUp()
    {
        if (this.transform.tag == ShapeState.Held.ToString())
        {
            move = false;
            OnMoouseUpSignal.Dispatch(HeldShape);
        }
    }

    public IEnumerator SetCorrectShapePos(System.Action callback)
    {
        HeldShape.transform.position = correctPos;
        yield return null;
        callback?.Invoke();
    }

    public void ResetShapePos()
    {
        HeldShape.transform.position = defaultPos;
    }
    
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.transform.tag == ShapeState.Landed.ToString())
        {
            CanLanded = false;
            Debug.Log(collision.transform.parent.name);
            Debug.Log(this.transform.parent.name + " can set: " + CanLanded);
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.transform.tag == ShapeState.Landed.ToString())
        {
            CanLanded = false;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.transform.tag == ShapeState.Landed.ToString())
        {
            CanLanded = true;
            Debug.Log(this.transform.parent.name + " can set: " + CanLanded);
        }
    }
}
