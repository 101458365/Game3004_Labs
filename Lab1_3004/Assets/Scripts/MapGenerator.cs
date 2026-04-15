using System.Collections.Generic;
using UnityEngine;

public class MapGenerator : MonoBehaviour
{
    [Header("World Properties")]
    [Range(8, 64)]
    public int height = 8;
    [Range(8, 64)]
    public int width = 8;
    [Range(8, 64)]
    public int depth = 8;

    [Header("Tile Properties")]
    public GameObject threeDTilePrefab;
    public Transform tileParent;

    [Header("Scaling Values")]
    [Range(8, 64)]
    public float min = 16.00f;
    [Range(8, 64)]
    public float max = 24.00f;

    List<GameObject> grid = new List<GameObject>();
    List<GameObject> disabledTiles = new List<GameObject>();

    private int collisionCounter;
    private int startHeight, startWidth, startDepth;
    private float startMin, startMax;

    private void Start()
    {
        Initialize();
        Regenerate();
        OptimizingCollisionsAndMeshes();
    }

    private void Update()
    {
        if (startWidth != width || startDepth != depth || startHeight != height || startMin != min || startMax != max)
        {
            Initialize();
            Reset();
            Regenerate();
            OptimizingCollisionsAndMeshes();
        }
    }

    private void Reset()
    {
        foreach (GameObject tile in grid)
        {
            Destroy(tile);
        }

        grid.Clear();
        disabledTiles.Clear();
    }

    private void Initialize()
    {
        startWidth = width;
        startHeight = height;
        startDepth = depth;
        startMin = min;
        startMax = max;
    }

    private void Regenerate()
    {
        float randomScale = Random.Range(min, max);
        float offsetX = Random.Range(-1024.0f, 1024.0f);
        float offsetZ = Random.Range(-1024.0f, 1024.0f);

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                for (int z = 0; z < depth; z++)
                {
                    var perlinNoiseValue = Mathf.PerlinNoise(
                        (x + offsetX) / randomScale,
                        (z + offsetZ) / randomScale
                    ) * depth * 0.5f;

                    if (y < perlinNoiseValue)
                    {
                        var tile = Instantiate(threeDTilePrefab, new Vector3(x, y, z), Quaternion.identity);
                        tile.transform.SetParent(tileParent);
                        grid.Add(tile);
                    }
                }
            }
        }
    }

    private void OptimizingCollisionsAndMeshes()
    {
        disabledTiles.Clear();

        var normalArray = new Vector3[]
        {
            Vector3.up, Vector3.down,
            Vector3.forward, Vector3.back,
            Vector3.right, Vector3.left
        };

        foreach (GameObject tile in grid)
        {
            collisionCounter = 0;

            RaycastHit hit;

            for (int i = 0; i < normalArray.Length; i++)
            {
                bool isHitted = Physics.Raycast(
                    tile.transform.position,
                    normalArray[i],
                    out hit,
                    tile.transform.localScale.magnitude * 0.5f
                );

                if (isHitted && hit.collider.CompareTag("Tile"))
                {
                    TileBehaviour neighbor = hit.transform.GetComponent<TileBehaviour>();

                    if (neighbor != null)
                    {
                        neighbor.AddNeighborTile(tile);
                        collisionCounter++;
                    }
                }
            }

            if (collisionCounter > 5)
            {
                disabledTiles.Add(tile);
            }
        }

        foreach (GameObject tile in disabledTiles)
        {
            TileBehaviour tb = tile.GetComponent<TileBehaviour>();

            if (tb != null)
            {
                tb.InactivateTiles();
            }
            else
            {
                Debug.LogError($"Missing TileBehaviour on {tile.name}");
            }
        }
    }
}