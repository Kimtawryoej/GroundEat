using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[System.Serializable]
public struct gathers
{
    public bool check;
    public int count;
}
public class LinkedStack<T> : MonoBehaviour
{
    private T OneDestory;
    private LinkedList<T> Stack = new LinkedList<T>();
    public void Add(T Object) { if (!Stack.Contains(Object)) { Stack.AddLast(Object); } }
    public T Push() { OneDestory = Stack.Last(); Stack.Remove(OneDestory); return OneDestory; }
    public bool Contains(T Object) { return Stack.Contains(Object); }
    public void Remove(T Object) { Stack.Remove(Object); }
    public void Clear() { Stack.Clear(); }
    public int Count() { return Stack.Count; }
}
public class GameSystem : SingleTone<GameSystem>
{
    public Camera cam;
    public Func<Vector3, bool> moveCheck { get; set; }
    public Vector3 Max, Min;

    private void Start()
    {
        set();
        Clamp();
    }
    private void set()
    {
        moveCheck = (Vector3 currentPos) => { if ((currentPos.x > Min.x && currentPos.y > Min.y) && (currentPos.x <= Max.x && currentPos.y <= Max.y)) { return true; } return false; };
    } //Interface

    private void Clamp()
    {
        Max = new Vector3(Screen.width, Screen.height, 10);
        Min = new Vector3(-1, -1, 10);
    }

}
