using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthManager : MonoBehaviour
{
    public static HealthManager instance;

    public int currentHealth;
    public int maxHealth;

    public GameObject deathEffect;

    public float invincibleLength = 2f;
    private float invincCounter;
    public SpriteRenderer theSR;

    public int shieldPwr;
    public int shieldMaxPwr = 2; //puissance max du shield (2 protections)
    public GameObject theShield;

    private void Awake()
    {
        instance = this;
    }

    // Start is called before the first frame update
    void Start()
    {
        currentHealth = maxHealth;
        //on initialise les santés et les shields
        UIManager.instance.healthBar.maxValue = maxHealth;
        UIManager.instance.healthBar.value = currentHealth;

        UIManager.instance.shieldBar.maxValue = shieldMaxPwr;
        UIManager.instance.shieldBar.value = shieldPwr;
    }

    // Update is called once per frame
    void Update()
    {
        if(invincCounter >= 0)
        {
            invincCounter -= Time.deltaTime;

            if(invincCounter <= 0)
            {
                theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, 1f);
            }
        }
    }

    
    public void HurtPlayer()
    {
        if (invincCounter <= 0)
        {
            if (theShield.activeInHierarchy)
            {
                shieldPwr--; //retire un shield

                if(shieldPwr <= 0)
                {
                    theShield.SetActive(false);
                }
                UIManager.instance.shieldBar.value = shieldPwr;
            }
            else
            {
                currentHealth--;
                UIManager.instance.healthBar.value = currentHealth;

                if (currentHealth <= 0) //dans le cas où nous n'avons plus de vie, le joueur est tué
                {
                    Instantiate(deathEffect, transform.position, transform.rotation);
                    gameObject.SetActive(false);

                    GameManager.instance.KillPlayer();

                    WaveManager.instance.canSpawnWaves = false;
                }

                PlayerController.instance.doubleShotActive = false; //si l'on recupère le bonus D qui permet d'avoir un double shot à chaque tir
            }
        }
    }

    //quand on fait reapparaitre le joueur après avoir perdu une vie
    public void Respawn()
    {
        gameObject.SetActive(true);
        currentHealth = maxHealth;
        UIManager.instance.healthBar.value = currentHealth;

        invincCounter = invincibleLength;
        theSR.color = new Color(theSR.color.r, theSR.color.g, theSR.color.b, .5f);
    }

    //active le shield au maximum dès le début 2
    public void ActivateShield()
    {
        theShield.SetActive(true);
        shieldPwr = shieldMaxPwr;

        UIManager.instance.shieldBar.value = shieldPwr;
    }
}
