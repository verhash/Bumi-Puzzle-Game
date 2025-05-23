using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class MatchBoard : MonoBehaviour
{
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

    // LayoutArray
    //public ArrayLayout arrayLayout;

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
        matchBoard = new Node[width, height];

        spacingX = (float)(width - 1) / 2;
        spacingY = (float)(height - 1) / 2;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                Vector2 position = new Vector2(x - spacingX, y - spacingY);

                /*if (arrayLayout.rows[y].row[x])
                {
                    matchBoard[x,y] = new Node(false, null);
                }*/


                int randomIndex = Random.Range(0, itemPrefabs.Length);

                GameObject item = Instantiate(itemPrefabs[randomIndex], position, Quaternion.identity);
                item.GetComponent<Items>().SetIndicies(x, y);
                matchBoard[x, y] = new Node(true, item);
            }
        }
    }

    public bool CheckBoard()
    {
        Debug.Log("CheckBoard");
        bool hasMatched = false;

        List<Items> itemsToRemove = new();

        for (int x = 0; x < width; x++)
        {
            for (int y = 0; y < height; y++)
            {
                // Checking if potion node is usable
                if (matchBoard[x, y].isUsable)
                {
                    // Get item class in node
                    Items item = matchBoard[x, y].item.GetComponent<Items>();

                    // Ensre it's not matched
                    if (!item.isMatched)
                    {
                        // Run matching logic
                        MatchResult matchedItems = IsConnected(item);
                    }
                }
            }
        }
        return false;
    }

    // IsConnected method

    MatchResult IsConnected(Items item)
    {
        List<Items> connectedItems = new();
        ItemType itemType = item.itemType;

        connectedItems.Add(item);

        // Check right

        // Check left

        // Have we made a 3 match? (Horizontal match)

        // Checking for more than 3 (long horizontal match

        // Clean out the connectedItems

        // Readd our initial item

        // Check up

        // Check down

        // Have we made a 3 match? (Vertical match)

        // Checking for more than 3 (long vertical match)
    }

    // CheckDirection

    void CheckDirection(Items item, Vector2Int direction, List<Items> connectedItems)
    { 
        ItemType itemType = item.itemType;
        int x = item.xIndex + direction.x;
        int y = item.yIndex + direction.y;

        //Check that we're within the boundaries of the board
        while (x >= 0 && x < width && y >= 0 && y < height)
        {

        }
    }
}

public class MatchResult
{
    List<Items> connectedItems;
    MatchDirection direction;
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