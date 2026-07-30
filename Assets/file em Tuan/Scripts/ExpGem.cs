using UnityEngine;

public class ExpGem : MonoBehaviour
{
    public int expValue = 10;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerLevel pl = other.GetComponent<PlayerLevel>();
            if (pl != null) pl.AddExp(expValue);
            Destroy(gameObject);
        }
    }
}