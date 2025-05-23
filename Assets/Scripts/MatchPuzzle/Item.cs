using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public ItemType itemType;

    public int xIndex;
    public int yIndex;

    public bool isMatched;
    public bool isMoving;

    private Vector2 currentPos;
    private Vector2 targetPos;

    public Item(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    public void SetIndicies(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

    // MoveToTarget

    // MoveCouroutine

}

public enum ItemType
{
    Cookie,
    Candy,
    Donut,
    Cake,
    Muffin
}