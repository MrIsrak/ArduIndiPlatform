using UnityEngine;

public class drop_slime : LootDrop
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "Player")
        {
            int lootAmount = Random.Range(1, 3);
            AddToLoot(lootAmount);
            Destroy(gameObject);
        }
        
    }
}
