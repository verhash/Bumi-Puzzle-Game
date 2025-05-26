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
    public void MoveToTarget(Vector2 targetPos)
    {

        StartCoroutine(MoveCoroutine(targetPos));
    }

    // MoveCouroutine
    private IEnumerator MoveCoroutine(Vector2 targetPos)
    {
        isMoving = true;
        float duration = 0.2f;

        Vector2 startPos = transform.position;
        float elapsedTime = 0f;


        while (elapsedTime < duration)
        {
            float t = elapsedTime / duration;

            transform.position = Vector2.Lerp(startPos, targetPos, t);

            elapsedTime += Time.deltaTime;

            yield return null;
        }
        transform.position = targetPos;
        isMoving = false;
    }


}

public enum ItemType
{
    Cookie,
    Candy,
    Donut,
    Cake,
    Muffin
}