using System;
using System.Linq;
using UnityEngine;

public class PlayerMove : MonoBehaviour
{
    #region 변수
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
            PlayerTransform(Mathf.RoundToInt(currentPos.x),Mathf.RoundToInt(currentPos.y));
            return true;
        }
        return false;
    }

    public void PlayerTransform(int x, int y) //플레이어가 움직임에 따라 선을 그리는 함수
    {
        // Texture.Instance.resolution[x, y] 맵 노드
        if ((FloodFill.Instance.BePosFill.Count.Equals(0) && Texture.Instance.resolution[x, y].Equals(FloodFilltag.None))
            || FloodFill.Instance.BePosFill.Count > 0 && !Texture.Instance.resolution[x, y].Equals(FloodFilltag.LineColor))
        {
            Texture.Instance.Background.sprite.texture.SetPixel(x, y, Texture.Instance.FillColor);
            Texture.Instance.Background.sprite.texture.Apply();
            FloodFill.Instance.BePosFill.Add(new Vector2Int(x, y));
            if (Texture.Instance.resolution[FloodFill.Instance.BePosFill.Last().x, FloodFill.Instance.BePosFill.Last().y].Equals(FloodFilltag.Wall))
            {
                FloodFill.Instance.FloodFill_S(FloodFill.Instance.BePosFill.First(), FloodFill.Instance.BePosFill.Last());
            }
            else
                Texture.Instance.resolution[x, y] = FloodFilltag.LineColor;
        }
    }
    #endregion

    #endregion
}
