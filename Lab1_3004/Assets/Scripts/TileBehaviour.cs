using System.Collections.Generic;
using UnityEngine;

public enum TileType
{
    Grass,
    Dirt,
    Stone,
    Coal,
    Iron,
    Gold,
    Diamond
}

[System.Serializable]
public struct TileWeight
{
    public TileType tileType;
    public int weight;
}

public class TileBehaviour : MonoBehaviour
{
    public bool isActiveTile = true;
    public List<GameObject> neighborTiles = new List<GameObject>();
    public TileType tileType = TileType.Grass;

    [Header("Tile Spawn Weights")]
    public List<TileWeight> tileWeights = new List<TileWeight>()
    {
        new TileWeight { tileType = TileType.Dirt, weight = 50 },
        new TileWeight { tileType = TileType.Stone, weight = 40 },
        new TileWeight { tileType = TileType.Coal, weight = 30 },
        new TileWeight { tileType = TileType.Iron, weight = 20 },
        new TileWeight { tileType = TileType.Gold, weight = 10 },
        new TileWeight { tileType = TileType.Diamond, weight = 5 }
    };

    MeshRenderer meshRenderer;
    BoxCollider boxCollider;

    private void Awake()
    {
        meshRenderer = GetComponent<MeshRenderer>();
        boxCollider = GetComponent<BoxCollider>();
    }

    private void Start()
    {
        UpdateTile(tileType);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickaxe"))
        {
            if (isActiveTile)
            {
                foreach (GameObject tile in neighborTiles)
                {
                    if (tile != null)
                    {
                        TileBehaviour tb = tile.GetComponent<TileBehaviour>();

                        if (tb != null && !tb.isActiveTile)
                        {
                            tb.ActivateTiles();
                        }
                    }
                }
            }

            Destroy(gameObject);
        }
    }

    public void InactivateTiles()
    {
        isActiveTile = false;

        if (boxCollider != null)
            boxCollider.enabled = false;

        if (meshRenderer != null)
            meshRenderer.enabled = false;
    }

    public void ActivateTiles()
    {
        isActiveTile = true;

        if (boxCollider != null)
            boxCollider.enabled = true;

        if (meshRenderer != null)
            meshRenderer.enabled = true;

        UpdateTile(GetRandomTile());
    }

    TileType GetRandomTile()
    {
        int totalWeight = 0;

        foreach (var t in tileWeights)
            totalWeight += t.weight;

        int rand = Random.Range(0, totalWeight);

        foreach (var t in tileWeights)
        {
            if (rand < t.weight)
                return t.tileType;

            rand -= t.weight;
        }

        return TileType.Stone; // fallback (should never hit)
    }

    private void UpdateTile(TileType tileType)
    {
        this.tileType = tileType; 

        switch (tileType)
        {
            case TileType.Grass:
                meshRenderer.material.color = Color.green;
                break;
            case TileType.Dirt:
                meshRenderer.material.color = new Color(0.545f, 0.271f, 0.075f);
                break;
            case TileType.Stone:
                meshRenderer.material.color = new Color(0.45f, 0.45f, 0.45f);
                break;
            case TileType.Coal:
                meshRenderer.material.color = new Color(0.05f, 0.05f, 0.05f);
                break;
            case TileType.Iron:
                meshRenderer.material.color = new Color(0.6f, 0.65f, 0.7f);
                break;
            case TileType.Gold:
                meshRenderer.material.color = Color.yellow;
                break;
            case TileType.Diamond:
                meshRenderer.material.color = Color.cyan;
                break;
        }
    }

    public void AddNeighborTile(GameObject tile)
    {
        neighborTiles.Add(tile);
    }
}