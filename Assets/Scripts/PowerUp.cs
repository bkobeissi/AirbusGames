using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerUp : MonoBehaviour
{
    public bool isShield;

    public bool isBoost;

    public bool isDoubleShot;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    //selon le bonus recupérer
    private void OnTriggerEnter2D(Collider2D other)
    {
        if(other.tag == "Player")
        {
            Destroy(gameObject);

            if(isShield) //s'il recupère un shield
            {
                HealthManager.instance.ActivateShield();
            }

            if(isBoost) //s'il recupère un boost
            {
                PlayerController.instance.ActivateSpeedBoost();
            }

            if(isDoubleShot) //s'il recupère un double shot
            {
                PlayerController.instance.doubleShotActive = true;
            }
        }
    }
}
