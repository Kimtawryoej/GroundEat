using System;
using System.Linq;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region º¯¼ö
    #region Pos
    private Func<Vector3> xy;
    private Vector3 currentPos = new Vector3(0, 0, 10);
    private Vector2Int StarPos;
    #endregion
    private gathers gather;
    #endregion
    #region Method
    #region LifeCycle
    private void Start()
    {
        set();
    }
    private void Update()
    {
        Move();
    }
    #endregion

    #region CreateMethod
    private void set()
    {
        transform.position = GameSystem.Instance.cam.ScreenToWorldPoint(currentPos);
    }
    private bool Move()
    {
        xy = Input.GetButton("Horizontal") ? () => { gather.check = true; return new Vector3(Input.GetAxisRaw("Horizontal"), 0, 0); } : () => { gather.check = false; return new Vector3(0, Input.GetAxisRaw("Vertical"), 0); };
        currentPos = GameSystem.Instance.cam.WorldToScreenPoint(transform.position) + xy() * 1;
        if (GameSystem.Instance.moveCheck(currentPos))
        {
            transform.position = (GameSystem.Instance.cam.ScreenToWorldPoint(currentPos));
            FloodFill.Instance.PlayerTransform(Mathf.RoundToInt(currentPos.x),Mathf.RoundToInt(currentPos.y));
            return true;
        }
        return false;
    }
    #endregion
    #endregion
}
