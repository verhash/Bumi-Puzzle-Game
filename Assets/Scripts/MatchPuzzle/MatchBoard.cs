using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchBoard : MonoBehaviour
{
    [SerializeField] private Item selectedItem;
    [SerializeField] private bool isProcessingMove;

    // Define size of the baord
    public int width = 6;
    public int height = 8;

    // Define spacing for the board
    public float spacingX;
    public float spacingY;

    // Get a reference to the item prefabs
    public GameObject[] itemPrefabs;

    // Get ref to the collection nodes 
    private Node[,] matchBoard;
    public GameObject matchBoardGO;

    public List<GameObject> itemsToDestroy = new();

    // LayoutArray
    public ArrayLayout arrayLayout;

    public static MatchBoard Instance;

    private void Awake()
    {
        Instance = this;
    }

    void Start()
    {
        InitializeBoard();
    }

    void InitializeBoard()
    {

        DestroyItems();
        matchBoard = new Node[width, height];

        spacingX = (float)(width - 1) / 2;
        spacingY = (float)(height - 1) / 2;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 position = new Vector2(x - spacingX, y - spacingY);

                if (arrayLayout.rows[y].row[x])
                {
                    matchBoard[x, y] = new Node(false, null);
                }
                else
                {
                    int randomIndex = Random.Range(0, itemPrefabs.Length);

                    GameObject item = Instantiate(itemPrefabs[randomIndex], position, Quaternion.identity);
                    item.GetComponent<Item>().SetIndicies(x, y);
                    matchBoard[x, y] = new Node(true, item);
                    itemsToDestroy.Add(item);
                }
            }
        }

        if (CheckBoard())
        {
            Debug.Log("We have matches let's re-create the board");
            InitializeBoard();
        }
        else
        {
            Debug.Log("There are no matches, it's time to start the game!");
        }
    }

    private void DestroyItems()
    {
        if(itemsToDestroy != null)
        {
            foreach (GameObject item in itemsToDestroy)
            {
                Destroy(item);
            }

            itemsToDestroy.Clear();
        }
    }

    public bool CheckBoard()
    {
        Debug.Log("CheckBoard");
        bool hasMatched = false;

        List<Item> itemsToRemove = new();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Checking if potion node is usable
                if (matchBoard[x, y].isUsable)
                {
                    // Get item class in node
                    Item item = matchBoard[x, y].item.GetComponent<Item>();

                    // Ensre it's not matched
                    if (!item.isMatched)
                    {
                        // Run matching logic
                        MatchResult matchedItems = IsConnected(item);

                        if (matchedItems.connectedItems.Count >= 3)
                        {
                            // TODO - Complex matching

                            itemsToRemove.AddRange(matchedItems.connectedItems);

                            foreach (Item matchedItem in matchedItems.connectedItems)
                                matchedItem.isMatched = true;

                            hasMatched = true;
                        }
                    }
                }
            }
        }
        return hasMatched;
    }

    // IsConnected method

    MatchResult IsConnected(Item item)
    {
        List<Item> connectedItems = new();
        ItemType itemType = item.itemType;

        connectedItems.Add(item);

        // Check right
        CheckDirection(item, new Vector2Int(1, 0), connectedItems);

        // Check left
        CheckDirection(item, new Vector2Int(-1, 0), connectedItems);

        // Have we made a 3 match? (Horizontal match)
        if (connectedItems.Count == 3)
        {
            Debug.Log("I have a normal horizontal match, item of my match is " + connectedItems[0].itemType);

            return new MatchResult
            {
                connectedItems = connectedItems,
                direction = MatchDirection.Horizontal
            };
        }

        // Checking for more than 3 (long horizontal match
        else if (connectedItems.Count > 3)
        {
            Debug.Log("I have a long horizontal match, item of my match is " + connectedItems[0].itemType);

            return new MatchResult
            {
                connectedItems = connectedItems,
                direction = MatchDirection.LongHorizontal
            };
        }

        // Clear out the connectedItems
        connectedItems.Clear();

        // Read our initial item
        connectedItems.Add(item);

        // Check up
        CheckDirection(item, new Vector2Int(0, 1), connectedItems);
        // Check down
        CheckDirection(item, new Vector2Int(0, -1), connectedItems);

        // Have we made a 3 match? (Vertical match)
        if (connectedItems.Count == 3)
        {
            Debug.Log("I have a normal vertical match, item of my match is " + connectedItems[0].itemType);

            return new MatchResult
            {
                connectedItems = connectedItems,
                direction = MatchDirection.Vertical
            };
        }

        // Checking for more than 3 (long vertical match)
        else if (connectedItems.Count > 3)
        {
            Debug.Log("I have a long vertical match, item of my match is " + connectedItems[0].itemType);

            return new MatchResult
            {
                connectedItems = connectedItems,
                direction = MatchDirection.LongVertical
            };
        }
        else
        {
            return new MatchResult
            {
                connectedItems = connectedItems,
                direction = MatchDirection.None
            };
        }
    }

    // CheckDirection

    void CheckDirection(Item item, Vector2Int direction, List<Item> connectedItems)
    { 
        ItemType itemType = item.itemType;
        int x = item.xIndex + direction.x;
        int y = item.yIndex + direction.y;

        // Check that we're within the boundaries of the board
        while (x >= 0 && x < width && y >= 0 && y < height)
        {
            if (matchBoard[x, y].isUsable)
            {
                Item neighbourItem = matchBoard[x, y].item.GetComponent<Item>();

                if(!neighbourItem.isMatched && neighbourItem.itemType == itemType)
                {
                    connectedItems.Add(neighbourItem);

                    x += direction.x;
                    y += direction.y;
                }
                else
                {
                    break;
                }
            }
            else
            {
                break;
            }
        }
    }

    #region Swapping Items

    // Select item
    public void SelectItem(Item item)
    {
        // If we dont have an item currently selected, then set the item I just clicked to my selectedItem
        if (selectedItem == null)
        {
            Debug.Log(item);
            selectedItem = item;
        }

        // If we select the same item twice, then make selectedItem null
        else if (selectedItem == item)
        {
            selectedItem = null;
        }

        // If selectedItem is not null and is not the current item, attempt a swap

        // selectedItem back to null
        else if (selectedItem != item)
        {
            SwapItem(selectedItem, item);
            selectedItem = null;
        }

    }

    // Swap item - logic

    private void SwapItem(Item item, Item targetItem)
    {
        // !IsAdjacent don't do anything
        /*if (!IsAdjacent(currentItem, targetItem))
        {
            return;
        }*/

        // DoSwap

        isProcessingMove = true;

        //startCorouting ProcessMatches
    }

    // Do swap

    // IsAdjacent
    private bool IsAdjacent(Item currentItem, Item targetItem)
    {
        return Mathf.Abs(currentItem.xIndex - targetItem.xIndex) + Mathf.Abs(currentItem.yIndex - targetItem.yIndex) == 1;
    }

    // ProcessMatches

    #endregion

}

public class MatchResult
{
    public List<Item> connectedItems;
    public MatchDirection direction;
}

public enum MatchDirection
{
    Vertical,
    Horizontal,
    LongVertical,
    LongHorizontal,
    Super,
    None
}