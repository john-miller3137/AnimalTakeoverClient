using System.Collections;
using System.Collections.Generic;
using SharedLibrary;
using UnityEngine;

public class GameCardInfo : MonoBehaviour
{
    private int _id;
    private int _xMove;
    private int _yMove;
    private int _attackId;
    private byte _type;
    
    public GameCardInfo(int id, int xMove, int yMove, int attackId, byte type)
    {
        _id = id;
        _xMove = xMove;
        _yMove = yMove;
        _attackId = attackId;
        _type = type;
    }
    public int Id
    {
        get { return _id; }
        set { _id = value; }
    }
    public int XMove
    {
        get { return _xMove; }
        set { _xMove = value; }
    }
    public int YMove
    {
        get { return _yMove; }
        set { _yMove = value; }
    }
    public int AttackId
    {
        get { return _attackId; }
        set { _attackId = value; }
    }

    public byte Type
    {
        get { return _type; }
        set { _type = value; }
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
}
