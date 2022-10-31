using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BloodCoin : MonoBehaviour
{
    public int bloodAmount = 10;

    [SerializeField]
    private GameManager gameManager;

    public GameObject bloodParticle;
    public AudioClip bloodAudio;

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        Destroy(this.gameObject, 10f);
    }

    void Update()
    {
        
    }

    void OnMouseDown()
    {
        Instantiate(bloodParticle, transform.position, Quaternion.identity);
        AudioSource.PlayClipAtPoint(bloodAudio, Camera.main.transform.position, 0.05f);
        gameManager.addBloodStock(bloodAmount);
        Destroy(this.gameObject);
    }
}
