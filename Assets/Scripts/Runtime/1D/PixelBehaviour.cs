using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PixelBehaviour : MonoBehaviour
{
    [Header("Component")] 
    [SerializeField] private SpriteRenderer _sR;
    [SerializeField] private BoxCollider2D _b2D;
    [SerializeField] private Rigidbody2D _rb2D;
    public BoxCollider2D B2D => _b2D;

    [Header("Behaviour")]
    private PixelState _pixelState;
    [SerializeField] private List<PixelColor> pixelColors = new List<PixelColor>();
    private Action OntriggerEnterAction;

    [Header("Data")]
    private int _pixelPos; 
    public int PixelPos { set => _pixelPos = value;}

    [Header("Enemy")] 
    private float _timer = 0;
    [SerializeField] private float _cooldown = 3;

    private void Update()
    {
        if (_pixelState != PixelState.ENEMY && _pixelState != PixelState.PLAYER)
            return;

        if (_pixelState == PixelState.ENEMY)
        {
            _timer += Time.deltaTime;
            if (_timer >= _cooldown)
            {
                _timer = 0;
                _rb2D.MovePosition(new Vector3(transform.position.x - 1, 0, 0));
            }
        }

        if (_pixelState == PixelState.PLAYER)
        {
            if (Input.GetKeyDown(KeyCode.Q) || Input.GetKeyDown(KeyCode.A))
                PlayerMovement(-1);
            else if (Input.GetKeyDown(KeyCode.D))
                PlayerMovement(1);
            else if (Input.GetKeyDown(KeyCode.Space))
                PlayerMovement(2);
        }
    }

    private void PlayerMovement(int nextPos)
    {
        if (PixelManager.Instance.IsThereWall(_pixelPos + nextPos) || _pixelPos + nextPos < 0)
            return;
            
        _rb2D.MovePosition(new Vector3(transform.position.x + nextPos, 0, 0));
        _pixelPos += nextPos;
    }

    public void UpdateBehaviour(PixelState newState, Action triggeredAction = null)
    {
        _pixelState = newState;
        OntriggerEnterAction = triggeredAction;

        switch (_pixelState)
        {
            case PixelState.NOTHING:
                NewBehaviour(0, false);
                break;
            case PixelState.PLAYER:
                NewBehaviour(2, true, false);
                break;
            case PixelState.WALL:
                NewBehaviour(1, true, false);
                break;
            case PixelState.ENEMY:
                NewBehaviour(3, true);
                break;
            case PixelState.FINISH:
                NewBehaviour(1, true);
                break;
        }
    }

    private void NewBehaviour(int layerOrder, bool isColliderActivate, bool isTrigger = true)
    {
        _sR.sortingOrder = layerOrder;
        _sR.color = GetColor();

        _b2D.enabled = isColliderActivate;
        _b2D.isTrigger = isTrigger;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(OntriggerEnterAction != null)
            OntriggerEnterAction.Invoke();
    }

    private Color GetColor()
    {
        for (int i = 0; i < pixelColors.Count; i++)
        {
            if (pixelColors[i].State == _pixelState)
                return pixelColors[i].color;
        }

        return Color.yellow;
    }
}