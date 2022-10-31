using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private enum cards { NONE, CANON, MORTER, BOOST, ARROW, UPGRADE, REMOVE }
    [SerializeField]
    private cards selectedCard;

    [SerializeField]
    private int bloodStock;
    public int maxBloodStock;
    public GameObject bloodTextGO;
    TextMeshProUGUI bloodText;
    
    public GameObject messageGO;
    TextMeshProUGUI messageText;

    public AudioClip buttonAudio;

    [SerializeField]
    private Castle castle;

    public GameObject[] BloodVial;


    public GameObject regularBat;
    public GameObject footSoldier;
    public GameObject[] footSoldierSpawner;
    public GameObject[] regularBatSpawner;
    public int currWave;

    public int wave_1_Bat = 3;

    public int wave_2_Bat = 3;
    public int wave_2_Foot = 2;

    public int wave_3_Bat = 5;
    public int wave_3_Foot = 3;

    public int wave_4_Bat = 7;
    public int wave_4_Foot = 5;

    public int wave_5_Bat = 7;
    public int wave_5_Foot = 7;

    void Start()
    {
        bloodText = bloodTextGO.GetComponent<TextMeshProUGUI>();
        messageText = messageGO.GetComponent<TextMeshProUGUI>();
        castle = FindObjectOfType<Castle>();

        StartCoroutine(IESpawner());
    }

    void Update()
    {
        bloodText.SetText(bloodStock.ToString());


        BloodVialManage();
    }

    public void SelectCard(string type)
    {
        if(type == "canon")
        {
            selectedCard = cards.CANON;
        }
        else if(type == "morter")
        {
            selectedCard = cards.MORTER;
        }
        else if(type == "boost")
        {
            selectedCard = cards.BOOST;
        }
        else if(type == "upgrade")
        {
            selectedCard = cards.UPGRADE;
            castle.UpgradeCastle();
        }
        else if(type == "remove")
        {
            selectedCard = cards.REMOVE;
        }
        AudioSource.PlayClipAtPoint(buttonAudio, Camera.main.transform.position, 0.1f);
    }
    public void DeSelectCard(string type)
    {
        print("Deselected...");
        if (type == "canon")
        {
            if(selectedCard == cards.CANON) selectedCard = cards.NONE;
        }
        else if (type == "morter")
        {
            if (selectedCard == cards.MORTER) selectedCard = cards.NONE;
        }
        else if (type == "boost")
        {
            if (selectedCard == cards.BOOST) selectedCard = cards.NONE;
        }
        else if (type == "upgrade")
        {
            if (selectedCard == cards.UPGRADE) selectedCard = cards.NONE;
        }
        else if (type == "remove")
        {
            if (selectedCard == cards.REMOVE) selectedCard = cards.NONE;
        }
    }
    public void HoverCard(string type)
    {
        print("Hovered...");
        if (type == "canon")
        {
            int canonCost = GameObject.FindObjectOfType<SpawnPoint>().canonCost;
            ShowMessage($"For Land & Air Enemies | Cost: {canonCost}");
        }
        else if (type == "morter")
        {
            int morterCost = GameObject.FindObjectOfType<SpawnPoint>().morterCost;
            ShowMessage($"For Land Enemies Only | Cost: {morterCost}");
        }
        else if (type == "boost")
        {
            ShowMessage("Click on Defense to Boost | Cost: 10");
        }
        else if (type == "upgrade")
        {
            int next_level_cost = castle.getNextLevelCost();
            if(next_level_cost == 0)
                ShowMessage($"Cannon upgrade further.");
            else
                ShowMessage($"Upgrade Castle | Cost: {next_level_cost}");
        }
        else if (type == "remove")
        {
            ShowMessage("Remove Defense");
        }
    }

    public void ShowMessage(string message)
    {
        messageText.SetText(message);

    }
    public void ClearMessage()
    {
        messageText.SetText("");
    }

    public void BloodVialManage()
    {
        float bloodStockF = bloodStock;
        float maxBloodStockF = maxBloodStock;
        float x = bloodStockF / maxBloodStockF;

        BloodVial[0].GetComponent<Slider>().value = 1 - x;
        BloodVial[1].GetComponent<Slider>().value = 1 - x;
        BloodVial[2].GetComponent<Slider>().value = 1 - x;
    }

    // =================================================================================+
    //                           COMMUNICATOR FOR OTHER SCRIPTS                         |
    // =================================================================================+
    public int getBloodStock()
    {
        return bloodStock;
    }
    public void useBloodStock(int blood_amount)
    {
        bloodStock -= blood_amount;
    }
    public void addBloodStock(int blood_amount)
    {
        bloodStock += blood_amount;
    }
    public bool isDefenseSelected()
    {
        return selectedCard == cards.CANON || selectedCard == cards.MORTER;
    }
    public bool isBoostSelected()
    {
        return selectedCard == cards.BOOST;
    }
    public bool isCanonSelected()
    {
        return selectedCard == cards.CANON;
    }
    public bool isMorterSelected()
    {
        return selectedCard == cards.MORTER;
    }
    public bool isRemoveSelected()
    {
        return selectedCard == cards.REMOVE;
    }

    IEnumerator IESpawner()
    {
        yield return new WaitForSeconds(2);
        print("Spawning: Wait 5 Seconds.");
        Spawner();
        StartCoroutine(IESpawner());
    }
    public void Spawner()
    {
        if(currWave > 5)
        {
            SceneManager.LoadScene("Game-Complete");
        }

        Enemy[] enemies = FindObjectsOfType<Enemy>();

        if(enemies.Length == 0)
        {
            currWave++;
        }

        if(currWave == 1)
        {
            if(wave_1_Bat > 0)
            {
                int spawnerNum = Random.Range(0, regularBatSpawner.Length);
                Instantiate(regularBat, regularBatSpawner[spawnerNum].transform.position, regularBatSpawner[spawnerNum].transform.rotation);
                wave_1_Bat--;
            }
        }

        if (currWave == 2)
        {
            int bat_or_foot = Random.Range(1, 3);
            print(bat_or_foot);
            if (wave_2_Bat > 0 && bat_or_foot == 1)
            {
                int spawnerNum = Random.Range(0, regularBatSpawner.Length);
                Instantiate(regularBat, regularBatSpawner[spawnerNum].transform.position, regularBatSpawner[spawnerNum].transform.rotation);
                wave_2_Bat--;
            }
            if (wave_2_Foot > 0 && bat_or_foot == 2)
            {
                int spawnerNum = Random.Range(0, footSoldierSpawner.Length);
                Instantiate(footSoldier, footSoldierSpawner[spawnerNum].transform.position, footSoldierSpawner[spawnerNum].transform.rotation);
                wave_2_Foot--;
            }
        }

        if (currWave == 3)
        {
            int bat_or_foot = Random.Range(1, 3);
            if (wave_3_Bat > 0 && bat_or_foot == 1)
            {
                int spawnerNum = Random.Range(0, regularBatSpawner.Length);
                Instantiate(regularBat, regularBatSpawner[spawnerNum].transform.position, regularBatSpawner[spawnerNum].transform.rotation);
                wave_3_Bat--;
            }
            if (wave_3_Foot > 0 && bat_or_foot == 2)
            {
                int spawnerNum = Random.Range(0, footSoldierSpawner.Length);
                Instantiate(footSoldier, footSoldierSpawner[spawnerNum].transform.position, footSoldierSpawner[spawnerNum].transform.rotation);
                wave_3_Foot--;
            }
        }

        if (currWave == 4)
        {
            int bat_or_foot = Random.Range(1, 3);
            if (wave_4_Bat > 0 && bat_or_foot == 1)
            {
                int spawnerNum = Random.Range(0, regularBatSpawner.Length);
                Instantiate(regularBat, regularBatSpawner[spawnerNum].transform.position, regularBatSpawner[spawnerNum].transform.rotation);
                wave_4_Bat--;
            }
            if (wave_4_Foot > 0 && bat_or_foot == 2)
            {
                int spawnerNum = Random.Range(0, footSoldierSpawner.Length);
                Instantiate(footSoldier, footSoldierSpawner[spawnerNum].transform.position, footSoldierSpawner[spawnerNum].transform.rotation);
                wave_4_Foot--;
            }
        }

        if (currWave == 5)
        {
            int bat_or_foot = Random.Range(1, 3);
            if (wave_5_Bat > 0 && bat_or_foot == 1)
            {
                int spawnerNum = Random.Range(0, regularBatSpawner.Length);
                Instantiate(regularBat, regularBatSpawner[spawnerNum].transform.position, regularBatSpawner[spawnerNum].transform.rotation);
                wave_5_Bat--;
            }
            if (wave_5_Foot > 0 && bat_or_foot == 2)
            {
                int spawnerNum = Random.Range(0, footSoldierSpawner.Length);
                Instantiate(footSoldier, footSoldierSpawner[spawnerNum].transform.position, footSoldierSpawner[spawnerNum].transform.rotation);
                wave_5_Foot--;
            }
        }
    }
}
