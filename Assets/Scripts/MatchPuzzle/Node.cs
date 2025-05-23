using UnityEngine;

public class Node : MonoBehaviour
{
    public bool isUsable;

    public GameObject item;

    public Node(bool isUsable, GameObject item)
    {
        this.isUsable = isUsable;
        this.item = item;
    }
}
