using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameCompleteScreen : MonoBehaviour
{
    public float timeBetweenTexts;

    public bool canExit;

    public string mainMenuName = "MainMenu";

    public Text message, score, pressKey;


    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(ShowTextCo());
    }

    // Update is called once per frame
    //pour l'écran de fin quand le joueur a réussi les deux levels ainsi que le boss
    void Update()
    {
        if(canExit && Input.anyKeyDown)
        {
            SceneManager.LoadScene(mainMenuName);//quand on appuie sur n'importe quel boutton on revient au menu principal
        }
    }

    public IEnumerator ShowTextCo()
    {
        yield return new WaitForSeconds(timeBetweenTexts);
        message.gameObject.SetActive(true);//message de felicitiations etc...
        yield return new WaitForSeconds(timeBetweenTexts);
        score.text = "Final Score: " + PlayerPrefs.GetInt("CurrentScore"); //on rappel le score final -> Final score : XXXX
        score.gameObject.SetActive(true);
        yield return new WaitForSeconds(timeBetweenTexts);//temps d'attente avant d'afficher le message pour revenir au menu principal
        pressKey.gameObject.SetActive(true);
        canExit = true;
    }
}
