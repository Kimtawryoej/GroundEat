using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;
public enum floodFilltag { None = 0, LineColor, Wall }

public class FloodFill : SingleTone<FloodFill>
{
    #region Pos
    private Vector2Int savePos;
    private Vector2Int isVaildSavePos;
    private int isVaildSaveCount;
    private Vector2Int up = new Vector2Int(0, 1);
    private Vector2Int down = new Vector2Int(0, -1);
    private Vector2Int right = new Vector2Int(1, 0);
    private Vector2Int left = new Vector2Int(-1, 0);
    private Vector2Int[] dir = new Vector2Int[] { Vector2Int.up, Vector2Int.down, Vector2Int.left, Vector2Int.right };
    #endregion
    #region PlayerPos
    private HashSet<Vector2Int> BePosFill = new HashSet<Vector2Int>();
    #endregion
    #region FloodFill
    private Vector2Int point1;
    private Vector2Int point2;
    private Stack<Vector2Int> points1 = new Stack<Vector2Int>();
    private HashSet<Vector2Int> points1Set = new HashSet<Vector2Int>();
    private Stack<Vector2Int> points2 = new Stack<Vector2Int>();
    private HashSet<Vector2Int> points2Set = new HashSet<Vector2Int>();
    private HashSet<Vector2Int> selectPoint = new HashSet<Vector2Int>();
    #endregion
    public void FloodFill_S(Vector2Int starPos, Vector2Int CurrentPos)
    {

        //같은 반복문 조건문 캐싱 
        for (int i = 0; i < dir.Length; i++)
        {
            savePos = starPos + dir[i];
            if (isVaild(savePos))
            {
                points1.Push(savePos);
                points1Set.Add(savePos);
                points2.Push(starPos + dir[i + 1]);
                points2Set.Add(starPos + dir[i + 1]);
                break;
            }
        }

        while (points1.Count > 0 && points2.Count > 0)
        {
            point1 = points1.Pop();
            point2 = points2.Pop();
            for (int i = 0; i < dir.Length; i++)
            {
                savePos = point1 + dir[i];
                if (isVaild(savePos) && !points1Set.Contains(savePos))
                {
                    Debug.Log("원래 좌표" + starPos + "방향더한" + point1 + "=" + savePos.x + ":" + savePos.y + ":" + points1.Count + dir[i]);
                    points1.Push(savePos);
                    points1Set.Add(savePos);
                    //Texture.Instance.Background.sprite.texture.SetPixel(savePos.x, savePos.y, Texture.Instance.FillColor);
                    //Texture.Instance.Background.sprite.texture.Apply();
                }

                savePos = point2 + dir[i];
                if (isVaild(savePos) && !points2Set.Contains(savePos))
                {
                    points2.Push(savePos);
                    points2Set.Add(savePos);
                }
                //yield return null;
            }
        }
        Debug.Log("끝");
        selectPoint = points1.Count <= 0 ? points1Set : points2Set;
        foreach (Vector2Int point in selectPoint)
        {
            Texture.Instance.Background.sprite.texture.SetPixel(point.x, point.y, Texture.Instance.FillColor);
            Texture.Instance.Background.sprite.texture.Apply();
        }
        FloodFillSet(CurrentPos);

    }

    private bool isVaild(Vector2Int Pos)
    {
        if ((Pos.x >= 0 && Pos.y >= 0) && (Pos.x <= 1920 && Pos.y <= 1080))
        {
            if (Texture.Instance.resolution[Pos.x, Pos.y].Equals((int)floodFilltag.None))
            {
                return true;
            }
        }
        //if (Texture.Instance.resolution[Pos.x, Pos.y].Equals((int)floodFilltag.None))
        //{
        //    for (int i = 0; i < dir.Length; i++)
        //    {
        //        isVaildSavePos += dir[i];
        //        if (!Texture.Instance.resolution[isVaildSavePos.x, isVaildSavePos.y].Equals((int)floodFilltag.None))
        //        {
        //            isVaildSaveCount++;
        //        }
        //    }
        //}
        //if (isVaildSaveCount >= 2) { return true; }
        return false;
    }
    public void playerTransform(int x, int y)
    {
        if ((BePosFill.Count.Equals(0) && Texture.Instance.resolution[x, y].Equals((int)floodFilltag.None))
            || BePosFill.Count > 0 && !Texture.Instance.resolution[x, y].Equals((int)floodFilltag.LineColor))
        {
            Texture.Instance.Background.sprite.texture.SetPixel(x, y, Texture.Instance.FillColor);
            Texture.Instance.Background.sprite.texture.Apply();
            BePosFill.Add(new Vector2Int(x, y));
            if (Texture.Instance.resolution[BePosFill.Last().x, BePosFill.Last().y].Equals((int)floodFilltag.Wall))
            {
                FloodFill_S(BePosFill.First(), BePosFill.Last());
                //Debug.Log(BePosFill.First()+":"+BePosFill.Last());
            }
            Texture.Instance.resolution[x, y] = (int)floodFilltag.LineColor; //함수 캐싱
        }
    }
    private void FloodFillSet(Vector2Int SetPos)
    {
        foreach (Vector2Int item in BePosFill)
        {
            Texture.Instance.resolution[item.x, item.y] = (int)floodFilltag.Wall;
        }
        BePosFill.Clear();
        BePosFill.Add(SetPos);
        points1.Clear();
        points2.Clear();
        points1Set.Clear();
        points2Set.Clear();
    }
}