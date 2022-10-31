using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [SerializeField]
    private GameObject gameManagerGO;
    private GameManager gameManager;

    public GameObject sprite;
    public GameObject spawnedItem;

    public GameObject canon;
    public GameObject morter;
    public GameObject arrow;
    public GameObject spawnParticle;

    public int canonCost;
    public int morterCost;
    public int arrowCost;

    public AudioClip spawnAudio;

    Animator anim;
    CircleCollider2D this_collider;

    void Start()
    {
        anim = GetComponent<Animator>();
        this_collider = GetComponent<CircleCollider2D>();
        gameManager = gameManagerGO.GetComponent<GameManager>();
    }

    void Update()
    {
        if(gameManager.isDefenseSelected())
        {
            if(spawnedItem == null)
            {
                sprite.SetActive(true);
                this_collider.enabled = true;
            }
        }
        else
        {
            sprite.SetActive(false);
            this_collider.enabled = false;
        }

        if (spawnedItem != null)
        {
            sprite.SetActive(false);
        }
    }

    private void SpawnDefense(GameObject spawnableDefense)
    {
        spawnedItem = Instantiate(spawnableDefense, transform.position, Quaternion.identity) as GameObject;
        Instantiate(spawnParticle, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(spawnAudio, Camera.main.transform.position, 0.01f);
    }

    void OnMouseDown()
    {
        if (gameManager.isDefenseSelected() && spawnedItem == null)
        {
            if (gameManager.isCanonSelected())
            {
                if(gameManager.getBloodStock() >= canonCost)
                {
                    gameManager.useBloodStock(canonCost);
                    SpawnDefense(canon);
                }
                else
                {
                    gameManager.ShowMessage("Not Enought Blood");
                }
            }
            else if (gameManager.isMorterSelected())
            {
                if (gameManager.getBloodStock() >= morterCost)
                {
                    gameManager.useBloodStock(morterCost);
                    SpawnDefense(morter);
                }
                else
                {
                    gameManager.ShowMessage("Not Enought Blood");
                }
            }
        }
    }

    void OnMouseOver()
    {
        sprite.GetComponent<SpriteRenderer>().color = Color.gray;
        anim.enabled = false;
    }
    void OnMouseExit()
    {
        sprite.GetComponent<SpriteRenderer>().color = Color.white;
        anim.enabled = true;
    }
}
