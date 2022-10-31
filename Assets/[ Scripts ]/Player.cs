using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public GameObject explosionParticle;
    public float health = 20;

    [SerializeField]
    private GameManager gameManager;
    private Castle castle;

    public bool isDefense = true;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        castle = FindObjectOfType<Castle>();

        if(!isDefense) {
            GetComponent<SpriteRenderer>().enabled = false;
        }
    }

    void Update()
    {
        if(health <= 0 && isDefense) {
            Destroy(this.gameObject);
        }
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "EnemyBullet")
        {
            GameObject explosionParticleGO = Instantiate(explosionParticle, collision.gameObject.transform.position, Quaternion.identity) as GameObject;
            float hitPoint = collision.gameObject.GetComponent<Projectile>().hitPoint;
            health -= hitPoint;
            castle.castleHealth -= hitPoint;
            Destroy(collision.gameObject);
        }
    }
}
