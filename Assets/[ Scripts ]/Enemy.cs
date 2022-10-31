using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private enum states { CHASE, ATTACK, EXPLODE, DEAD }
    [SerializeField]
    private states currState;
    public enum enemyTypes { REGULAR_BAT, EXPLODING_BAT, FOOT_SOLDIOR, TORCH_SOLDIOR, GOLEM }
    [SerializeField]
    public enemyTypes enemyType;

    public GameObject explosionParticle;
    public float health = 20;

    public float area = 50.0f;
    public float closestDist = Mathf.Infinity;
    public GameObject closestEnemy;
    public float minDistFromPlayer = 0.5f;

    public float moveSpeed = 1.5f;
    public float attackRate = 1.5f;

    [Header("Enemy Bat")]
    public GameObject bullet;
    public float bulletSpeed = 2.5f;

    Rigidbody2D RB2D;
    Animator Anim;

    public GameObject bloodCoinGO;
    public int bloodCoin = 100;

    public GameObject footSoldierBullet;
    public GameObject footSoldierFlame;

    void Start()
    {
        RB2D = GetComponent<Rigidbody2D>();
        Anim = GetComponent<Animator>();
        StartCoroutine(AttackIEnumerator());
    }

    void Update()
    {
        if(health <= 0)
        {
            currState = states.DEAD;
            RB2D.gravityScale = 1;
            GameObject bulletCoinGO = Instantiate(bloodCoinGO, transform.position, Quaternion.identity) as GameObject;
            bulletCoinGO.GetComponent<BloodCoin>().bloodAmount = bloodCoin;
            Destroy(this.gameObject);
        }

        if(currState != states.DEAD)
        {
            FindClosestEnemy();

            if(closestEnemy != null)
            {
                if(currState == states.CHASE)
                {
                    ApproachPlayer();
                }
            }
        }
    }

    public void FindClosestEnemy()
    {
        int layer_mask = LayerMask.GetMask("Player");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, area, layer_mask, -Mathf.Infinity, Mathf.Infinity);

        closestDist = Mathf.Infinity;

        foreach (Collider2D collider in hitColliders)
        {
            if(enemyType == enemyTypes.REGULAR_BAT)
            {
                float distance = Vector2.Distance(collider.transform.position, transform.position);
                if (distance < closestDist)
                {
                    closestDist = distance;
                    closestEnemy = collider.gameObject;
                }
            }
            else if(enemyType == enemyTypes.FOOT_SOLDIOR)
            {
                if(collider.tag == "LandPLayer")
                {
                    float distance = Vector2.Distance(collider.transform.position, transform.position);
                    if (distance < closestDist)
                    {
                        closestDist = distance;
                        closestEnemy = collider.gameObject;
                    }
                }
            }
        }
    }

    public void ApproachPlayer()
    {
        Vector2 direction = closestEnemy.transform.position - transform.position;
        float distance = Vector2.Distance(closestEnemy.transform.position, transform.position);

        Vector3 reachPosition = closestEnemy.transform.position;

        transform.position = Vector3.MoveTowards(transform.position, reachPosition, moveSpeed * Time.deltaTime);

        if (distance <= minDistFromPlayer)
        {
            currState = states.ATTACK;
        }
    }

    IEnumerator AttackIEnumerator()
    {
        yield return new WaitForSeconds(attackRate);

        if(currState == states.ATTACK)
        {
            if (enemyType == enemyTypes.REGULAR_BAT) RegularBatAttack();
            if (enemyType == enemyTypes.FOOT_SOLDIOR) FootSoldierAttack();

            
        }

        StartCoroutine(AttackIEnumerator());
    }
    public void RegularBatAttack()
    {
        if (closestEnemy == null) return;

        GameObject bulletGO = Instantiate(bullet, transform.position, transform.rotation) as GameObject;
        //GameObject fireGO = Instantiate(fireParticle, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as GameObject;

        Vector3 enemyDir = (closestEnemy.transform.position - bulletGO.transform.position).normalized;
        bulletGO.GetComponent<Rigidbody2D>().AddForce(enemyDir * bulletSpeed, ForceMode2D.Impulse);
        //Destroy(fireGO, 2.0f);
        //AudioSource.PlayClipAtPoint(fireAudio, Camera.main.transform.position, 0.001f);
    }

    public void FootSoldierAttack()
    {
        Anim.SetBool("IsAttacking", true);


    }
    public void FootSoldierAttack_AnimEvent()
    {
        Instantiate(footSoldierBullet, footSoldierFlame.transform.position, Quaternion.identity);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "PlayerBullet")
        {
            GameObject explosionParticleGO = Instantiate(explosionParticle, collision.gameObject.transform.position, Quaternion.identity) as GameObject;
            float hitPoint = collision.gameObject.GetComponent<Projectile>().hitPoint;
            health -= hitPoint;
            Destroy(collision.gameObject);
        }
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, area);
    }
}
