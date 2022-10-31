using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Morter : MonoBehaviour
{
    public float area = 10.0f;
    public float fireRate = 1.5f;
    public float bulletSpeed = 2.5f;

    public GameObject mainMorter;
    public GameObject bullet;
    public GameObject fireParticle;
    public Transform bulletSpawnPoint;
    public AudioClip fireAudio;


    public float closestDist = Mathf.Infinity;
    public GameObject closestEnemy;

    [SerializeField]
    private GameManager gameManager;

    public float lookAtOffset = 90f;

    public float x, y, t, g, Vx, Vy, Vx_offset;

    public bool isBoost = false;
    public GameObject boostParticle;
    public AudioClip boostAudio;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();

        StartCoroutine(FireIEnumerator());
    }

    void Update()
    {
        FindClosestEnemy();
        FaceEnemy();
    }

    public void FindClosestEnemy()
    {
        int layer_mask = LayerMask.GetMask("Enemy");
        Collider2D[] hitColliders = Physics2D.OverlapCircleAll(transform.position, area, layer_mask, -Mathf.Infinity, Mathf.Infinity);

        closestDist = Mathf.Infinity;

        foreach (Collider2D collider in hitColliders)
        {
            Enemy colliderEnemyScript = collider.gameObject.GetComponent<Enemy>();
            if(colliderEnemyScript.enemyType == Enemy.enemyTypes.FOOT_SOLDIOR)
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

    public void FaceEnemy()
    {
        if (closestEnemy == null) return;

        Vector2 lookDir = closestEnemy.transform.position - transform.position;
        float angle = Mathf.Atan2(lookDir.y, lookDir.x) * Mathf.Rad2Deg - lookAtOffset;

        mainMorter.transform.rotation = Quaternion.Euler(new Vector3(0, 0, angle));
    }

    IEnumerator FireIEnumerator()
    {
        float _fireRate = fireRate;

        if (isBoost) _fireRate = fireRate / 2;

        yield return new WaitForSeconds(_fireRate);

        Fire();

        StartCoroutine(FireIEnumerator());
    }

    public void Fire()
    {
        if (closestEnemy == null) return;

        GameObject bulletGO = Instantiate(bullet, bulletSpawnPoint.position, transform.rotation) as GameObject;
        GameObject fireGO = Instantiate(fireParticle, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as GameObject;

        Vector3 enemyDir = (closestEnemy.transform.position - bulletGO.transform.position).normalized;
        bulletGO.GetComponent<Rigidbody2D>().AddForce(enemyDir * bulletSpeed, ForceMode2D.Impulse);
        Destroy(fireGO, 2.0f);
        AudioSource.PlayClipAtPoint(fireAudio, Camera.main.transform.position, 0.01f);
    }

    //public void Fire()
    //{
    //    if (closestEnemy == null) return;

    //    GameObject bulletGO = Instantiate(bullet, bulletSpawnPoint.position, transform.rotation) as GameObject;
    //    GameObject fireGO = Instantiate(fireParticle, bulletSpawnPoint.position, bulletSpawnPoint.rotation) as GameObject;

    //    Vector3 enemyDir = (closestEnemy.transform.position - bulletGO.transform.position).normalized;


    //    #region PHYSICS
    //    x = bulletGO.transform.position.x - closestEnemy.transform.position.x;
    //    g = Physics.gravity.y * -1;
    //    Vx = x / t;
    //    Vy = y / t + 0.5f * g * t;
    //    if (x < 30) { t = 2f; }
    //    else { Vx_offset = 2.8f; }
    //    Vector3 V = new Vector3(Vx - Vx_offset, Vy, 0);
    //    #endregion



    //    bulletGO.GetComponent<Rigidbody2D>().AddForce(V * Time.deltaTime * bulletSpeed, ForceMode2D.Impulse);
    //    Destroy(fireGO, 2.0f);
    //    AudioSource.PlayClipAtPoint(fireAudio, Camera.main.transform.position, 0.01f);
    //}


    void OnMouseDown()
    {
        print(gameManager.isRemoveSelected());
        if (gameManager.isRemoveSelected())
        {
            Destroy(this.gameObject);
        }
        else if (gameManager.isBoostSelected())
        {
            StartCoroutine(Boost());
        }
    }

    IEnumerator Boost()
    {
        isBoost = true;
        Instantiate(boostParticle, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(boostAudio, Camera.main.transform.position, 0.01f);
        yield return new WaitForSeconds(5f);
        isBoost = false;
    }

    void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, area);
    }
}
