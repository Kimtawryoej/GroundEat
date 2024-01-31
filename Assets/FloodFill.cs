using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public enum FloodFilltag { None = 0, LineColor, Wall }

public class FloodFill : SingleTone<FloodFill>
{
    #region Pos
    private Vector2Int isVaildSavePos;
    private int isVaildSaveCount;
    private Vector2Int[] dir = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    #endregion
    #region PlayerPos
    public HashSet<Vector2Int> BePosFill { get; set; } = new HashSet<Vector2Int>();
    #endregion
    FloodFilltag[,] t;
    private void Start()
    {
        t = Texture.Instance.resolution;
    }
    float Count; // 실행횟수체크 변수

    public void FloodFill_S(Vector2Int starPos, Vector2Int CurrentPos) //플러드필
    {
        #region FloodFill
        PointData pointData1 = new PointData();
        PointData pointData2 = new PointData();
        Vector2Int savePos;
        HashSet<Vector2Int> selectPoint = new HashSet<Vector2Int>();
        #endregion
        for (int i = 0; i < dir.Length; i++)
        {
            savePos = starPos + dir[i];
            if (IsVaild(savePos))
            {
                pointData1.points.Push(savePos);
                pointData1.pointsSet.Add(savePos);
                pointData2.points.Push(starPos + dir[i + 1]);
                pointData2.pointsSet.Add(starPos + dir[i + 1]);
                break;
            }
        }

        while (pointData1.points.Count > 0 && pointData2.points.Count > 0/*check*/)
        {
            pointData1.point = pointData1.points.Pop();
            pointData2.point = pointData2.points.Pop();
            for (int i = 0; i < dir.Length; i++)
            {
                savePos = pointData1.point + dir[i];
                if (IsVaild(savePos) && !pointData1.pointsSet.Contains(savePos)) //해시 충돌 일어남
                {
                    pointData1.points.Push(savePos);
                    pointData1.pointsSet.Add(savePos);
                }

                savePos = pointData2.point + dir[i];
                if (IsVaild(savePos) && !pointData2.pointsSet.Contains(savePos))
                {
                    pointData2.points.Push(savePos);
                    pointData2.pointsSet.Add(savePos);
                }
                Count++;
            }
        }
        selectPoint = !(pointData1.points.Count > 0) ? pointData1.pointsSet : pointData2.pointsSet;
        foreach (Vector2Int point in selectPoint)
        {
            Texture.Instance.Background.sprite.texture.SetPixel(point.x, point.y, Texture.Instance.FillColor);
        }
        Texture.Instance.Background.sprite.texture.Apply();
        FloodFillSet(CurrentPos);

    }

    private bool IsVaild(Vector2Int Pos) //범위 안에 있는지 확인하는 함수
    {
        if ((Pos.x >= 0 && Pos.y >= 0) && (Pos.x <= Screen.width && Pos.y <= Screen.height))
        {
            if (t[Pos.x, Pos.y] == FloodFilltag.None)
            {
                return true;
            }
        }
        return false;
    }

    private void FloodFillSet(Vector2Int SetPos)
    {
        foreach (Vector2Int item in BePosFill)
        {
            Texture.Instance.resolution[item.x, item.y] = FloodFilltag.Wall;
        }
        BePosFill.Clear();
    }
}
public class PointData
{
    public Vector2Int point { get; set; }
    public Stack<Vector2Int> points { get; set; } = new Stack<Vector2Int>();
    public HashSet<Vector2Int> pointsSet { get; set; } = new HashSet<Vector2Int> { };
}