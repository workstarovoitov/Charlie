using UnityEngine;

class ItemsSpawner : MonoBehaviour
{	
	[SerializeField] private ItemSpawnObject[] objects;

	public void SpawnItem()
    {
		BoxCollider2D bc = GetComponent<BoxCollider2D>();
		for (int i = 0; i < objects.Length; i++)
		{
			for (int k = 0; k < objects[i].Quantity; k++)
            {	
				if (Random.Range(0f, 1f) <= objects[i].SpawnChance)
				{
					Instantiate(objects[i].Item, new Vector3(transform.position.x + Random.Range(-1.5f, 1.5f ), transform.position.y + Random.Range(1f, 2f), 0f), Quaternion.Euler(Vector3.zero));
                }
            }
		}
        
    }
}

[System.Serializable]
public class ItemSpawnObject
{
	[SerializeField] private GameObject item;
	public GameObject Item
	{
		get => item;
	}

	[SerializeField] private int quantity;
	public float Quantity
	{
		get => quantity;
	}
	[SerializeField] [Range(0f, 1f)] private float spawnChance;
	public float SpawnChance
    {
		get => spawnChance;
    }
}