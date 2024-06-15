using UnityEngine;

public class LootDrop : MonoBehaviour
{

    protected void AddToLoot(int amount)
    {
        all_drop.botel_slime += amount;
        Debug.Log(all_drop.botel_slime);
    }
}
