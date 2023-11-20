using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    // Serialized fields visible in the Unity editor
    [SerializeField] private Transform gameTransform; // Transform to hold the game pieces
    [SerializeField] private Transform piecePrefab;   // Prefab for individual puzzle pieces

    // List to store references to all puzzle pieces
    private List<Transform> pieces;
    private int emptyLocation;    // Index of the empty location (hidden piece)
    private int size;             // Size of the puzzle grid (e.g., 3x3)
    private bool shuffling = false;  // Flag to check if the puzzle is currently shuffling
    private bool puzzleCompleted = false;  // Flag to check if the puzzle is completed

    // Create the game setup with size x size pieces.
    private void CreateGamePieces(float gapThickness)
    {
        float width = 1 / (float)size;  // Calculate the width of each puzzle piece
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                // Instantiate a puzzle piece
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);

                // Position the puzzle piece in the grid
                piece.localPosition = new Vector3(-1 + (2 * width * col) + width,
                                                  +1 - (2 * width * row) - width,
                                                  0);
                // Adjust the scale of the piece and set its name
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";

                // Determine the empty location and hide the last piece
                if ((row == size - 1) && (col == size - 1))
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                }
                else
                {
                    // Adjust UV coordinates for texture mapping
                    float gap = gapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];

                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));

                    mesh.uv = uv;
                }
            }
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        pieces = new List<Transform>();
        size = 4;  // Default puzzle size
        CreateGamePieces(0.01f);  // Create puzzle pieces with a small gap
        shuffling = true;
        StartCoroutine(WaitShuffle(0.5f));  // Start shuffling after a delay
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the puzzle is completed and not currently shuffling
        if (!shuffling && CheckCompletion() && !puzzleCompleted)
        {
            shuffling = true;
            puzzleCompleted = true;
            Debug.Log("¡Puzzle completado!");
            StartCoroutine(WaitShuffle(0.5f));  // Start shuffling after a delay
        }

        // Check for mouse click to interact with puzzle pieces
        if (Input.GetMouseButtonDown(0) && !puzzleCompleted)
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (pieces[i] == hit.transform)
                    {
                        // Attempt to swap the clicked piece with adjacent empty space
                        if (SwapIfValid(i, -size, size)) { break; }
                        if (SwapIfValid(i, +size, size)) { break; }
                        if (SwapIfValid(i, -1, 0)) { break; }
                        if (SwapIfValid(i, +1, size - 1)) { break; }
                    }
                }
            }
        }
    }

    // Attempt to swap the clicked piece with an adjacent empty space
    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            // Swap positions and update the empty location
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            (pieces[i].localPosition, pieces[i + offset].localPosition) = ((pieces[i + offset].localPosition, pieces[i].localPosition));
            emptyLocation = i;
            return true;
        }
        return false;
    }

    // Check if the puzzle is completed by comparing piece names with their indices
    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}")
            {
                return false;
            }
        }
        return true;
    }

    // Coroutine to wait for a specified duration before shuffling the puzzle
    private IEnumerator WaitShuffle(float duration)
    {
        yield return new WaitForSeconds(duration);
        Shuffle();
        shuffling = false;
    }

    // Shuffle the puzzle pieces to create a solvable state
    private void Shuffle()
    {
        if (!puzzleCompleted)
        {
            int count = 0;
            int last = 0;
            while (count < (size * size * size))
            {
                int rnd = Random.Range(0, size * size - 1);
                if (rnd == last) { continue; }
                last = emptyLocation;

                // Attempt to swap pieces randomly to shuffle the puzzle
                if (SwapIfValid(rnd, -size, size))
                {
                    count++;
                }
                else if (SwapIfValid(rnd, +size, size))
                {
                    count++;
                }
                else if (SwapIfValid(rnd, -1, 0))
                {
                    count++;
                }
                else if (SwapIfValid(rnd, +1, size - 1))
                {
                    count++;
                }
            }
        }
    }
}