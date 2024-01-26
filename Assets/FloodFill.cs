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
    private Vector2Int savePos;
    private Vector2Int isVaildSavePos;
    private int isVaildSaveCount;
    private Vector2Int[] dir = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    #endregion
    #region PlayerPos
    private HashSet<Vector2Int> BePosFill = new HashSet<Vector2Int>();
    #endregion
    #region FloodFill
    private Vector2Int point1;
    private Stack<Vector2Int> points1 = new Stack<Vector2Int>();
    private HashSet<Vector2Int> points1Set = new HashSet<Vector2Int>();
    private Vector2Int point2;
    private Stack<Vector2Int> points2 = new Stack<Vector2Int>();
    private HashSet<Vector2Int> points2Set = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> selectPoint = new HashSet<Vector2Int>();
    #endregion
    float Count;

    public void FloodFill_S(Vector2Int starPos, Vector2Int CurrentPos) //플러드필
    {

        for (int i = 0; i < dir.Length; i++)
        {
            savePos = starPos + dir[i];
            if (IsVaild(savePos))
            {
                points1.Push(savePos);
                points1Set.Add(savePos);
                points2.Push(starPos + dir[i + 1]);
                points2Set.Add(starPos + dir[i + 1]);
                Debug.Log(starPos+":"+savePos+":"+ (starPos + dir[i + 1]));
                break;
            }
        }

        while (points1.Count > 0 && points2.Count > 0/*check*/)
        {
            point1 = points1.Pop();
            point2 = points2.Pop();
            for (int i = 0; i < dir.Length; i++)
            {
                savePos = point1 + dir[i];
                if (IsVaild(savePos) && !points1Set.Contains(savePos))
                {
                    points1.Push(savePos);
                    points1Set.Add(savePos);
                }

                savePos = point2 + dir[i];
                if (IsVaild(savePos) && !points2Set.Contains(savePos))
                {
                    points2.Push(savePos);
                    points2Set.Add(savePos);
                }
                Count++;
            }
        }
        selectPoint = !(points1.Count > 0) ? points1Set : points2Set;
        foreach (Vector2Int point in selectPoint)
        {
            Texture.Instance.Background.sprite.texture.SetPixel(point.x, point.y, Texture.Instance.FillColor);
        }
        Texture.Instance.Background.sprite.texture.Apply();
        Debug.Log(Count);
        FloodFillSet(CurrentPos);

    }

    private bool IsVaild(Vector2Int Pos) //범위 안에 있는지 확인하는 함수
    {
        if ((Pos.x >= 0 && Pos.y >= 0) && (Pos.x <= 1920 && Pos.y <= 1080))
        {
            if (Texture.Instance.resolution[Pos.x, Pos.y].Equals(FloodFilltag.None))
            {
                return true;
            }
        }
        return false;
    }
    public void PlayerTransform(int x, int y) //플레이어가 움직임에 따라 선을 그리는 함수
    {
        // Texture.Instance.resolution[x, y] 맵 노드
        if ((BePosFill.Count.Equals(0) && Texture.Instance.resolution[x, y].Equals(FloodFilltag.None))
            || BePosFill.Count > 0 && !Texture.Instance.resolution[x, y].Equals(FloodFilltag.LineColor))
        {
            Texture.Instance.Background.sprite.texture.SetPixel(x, y, Texture.Instance.FillColor);
            Texture.Instance.Background.sprite.texture.Apply();
            BePosFill.Add(new Vector2Int(x, y));
            if (Texture.Instance.resolution[BePosFill.Last().x, BePosFill.Last().y].Equals(FloodFilltag.Wall))
            {
                FloodFill_S(BePosFill.First(), BePosFill.Last());
            }
            else
                Texture.Instance.resolution[x, y] = FloodFilltag.LineColor;
        }
    }
    private void FloodFillSet(Vector2Int SetPos)
    {
        foreach (Vector2Int item in BePosFill)
        {
            Texture.Instance.resolution[item.x, item.y] = FloodFilltag.Wall;
        }
        Debug.Log("리셋");
        BePosFill.Clear();
        points1.Clear();
        points2.Clear();
        points1Set.Clear();
        points2Set.Clear();
    }
}