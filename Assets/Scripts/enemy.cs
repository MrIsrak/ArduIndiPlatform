using UnityEngine;

public class Enemy : MonoBehaviour, IEnemy
{
    public int health = 5; // Переименовано с heal на health
    protected bool dead = false;
    protected Animator anim;
    protected Rigidbody2D rb;
    protected Vector3 spawnPosition;

    public GameObject text_1;
    public GameObject text_2;

    public int Health => health;

    protected virtual void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        anim = GetComponent<Animator>();
    }

    public virtual void TakeDamage()
    {
        health--;
        print(health);
        anim.Play("Enemy Hit");

        if (health <= 0)
        {
            Die();
        }
        else
        {
            spawnPosition = rb.position;
            Instantiate(text_1, spawnPosition, Quaternion.identity);
        }
    }

    public virtual void TakeUltDamage()
    {
        health -= 2;
        print(health);
        anim.Play("Enemy Hit");

        if (health <= 0)
        {
            Die();
        }
        else
        {
            spawnPosition = rb.position;
            Instantiate(text_2, spawnPosition, Quaternion.identity);
        }
    }

    protected virtual void Die()
    {
        dead = true;
        anim.Play("Enemy Death");
        GetComponent<Collider2D>().enabled = false;
    }
}
