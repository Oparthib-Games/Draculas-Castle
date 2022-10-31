using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Castle : MonoBehaviour
{
    public float castleHealth = 100f;
    public float maxCastleHealth = 100f;

    [SerializeField]
    private GameObject gameManagerGO;
    private GameManager gameManager;

    [SerializeField]
    private int currLevel = 0;
    [SerializeField]
    private int maxLevel = 6;
    [SerializeField]
    private int[] levelUpCosts = { 0, 100, 300, 500, 1000, 1500, 2000 };

    [SerializeField]
    private GameObject[] castleTowers;

    public GameObject HealthSlider;

    void Start()
    {
        gameManager = gameManagerGO.GetComponent<GameManager>();

        maxCastleHealth = castleHealth;

        //StartCoroutine(TEST());
    }

    //IEnumerator TEST()
    //{
    //    yield return new WaitForSeconds(2.0f);
    //    UpgradeCastle();
    //    StartCoroutine(TEST());
    //}

    void Update()
    {
        HealthSlider.GetComponent<Slider>().value = (castleHealth / maxCastleHealth);

        if(castleHealth <= 0)
        {
            SceneManager.LoadScene("Game-Over");
        }
    }

    public void UpgradeCastle()
    {
        if(currLevel == maxLevel)
        {
            Debug.LogWarning("Cannot upgrade further");

            return;
        }

        int nextLevel = currLevel + 1;

        if(gameManager?.getBloodStock() < levelUpCosts[nextLevel])
        {
            Debug.LogWarning("Not Enough BLOOD");
            return;
        }

        gameManager.useBloodStock(levelUpCosts[nextLevel]);
        castleHealth += levelUpCosts[nextLevel];
        maxCastleHealth += levelUpCosts[nextLevel];
        castleTowers[nextLevel]?.SetActive(true);
        if(nextLevel == 4)
        {
            castleTowers[nextLevel+1]?.SetActive(true);
            castleTowers[nextLevel+2]?.SetActive(true);
        }
        currLevel++;
    }

    public int getNextLevelCost() {
        if(currLevel == 4)
        {
            return 0;
        }
        return levelUpCosts[currLevel + 1];
    }
}
