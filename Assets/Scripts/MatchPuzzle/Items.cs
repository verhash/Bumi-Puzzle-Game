using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Items : MonoBehaviour
{
    public ItemType itemType;

    public int xIndex;
    public int yIndex;

    public bool isMatched;
    public bool isMoving;

    private Vector2 currentPos;
    private Vector2 targetPos;

    public Items(int x, int y)
    {
        xIndex = x;
        yIndex = y;
    }

}

public enum ItemType
{
    Cookie,
    Candy,
    Donut
}