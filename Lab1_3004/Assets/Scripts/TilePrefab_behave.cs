using UnityEngine;

public class TilePrefab_behave : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Pickaxe"))
        {
            Debug.Log("Minor");
            Destroy(gameObject);
        }
    }
}
