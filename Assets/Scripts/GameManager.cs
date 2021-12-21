using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentLives = 3;

    public float respawnTime = 2f;

    public int currentScore;
    private int highScore = 500;

    public bool levelEnding;

    private int levelScore;

    public float waitForLevelEnd = 5f;

    public string nextLevel;

    private bool canPause;

    private void Awake()
    {
        instance = this;
    }

    void Start()
    {
        //vie du joueur (x3 en début de partie dans notre cas
        currentLives = PlayerPrefs.GetInt("CurrentLives");
        UIManager.instance.livesText.text = "x " + currentLives;

        //high score du joueur
        highScore = PlayerPrefs.GetInt("HighScore");
        UIManager.instance.hiScoreText.text = "Hi-Score: " + highScore;

        //score actuel pendant la partie
        currentScore = PlayerPrefs.GetInt("CurrentScore");
        UIManager.instance.scoreText.text = "Score: " + currentScore;

        canPause = true;
    }

    void Update()
    {
        //quand le level est terminée(remportée), le joueur se fait pousser vers la droite
        if(levelEnding)
        {
            PlayerController.instance.transform.position += new Vector3(PlayerController.instance.boostSpeed * Time.deltaTime, 0f, 0f);
        }

        //si le bouton Esc est actionné, le menu pause s'ouvre
        if(Input.GetKeyDown(KeyCode.Escape) && canPause)
        {
            PauseUnpause();
        }
    }

    //si le joueur perd une vie (il pourra rejouer s'il est >0 sinon game over
    public void KillPlayer()
    {
        currentLives--;
        UIManager.instance.livesText.text = "x " + currentLives;

        if (currentLives > 0)
        {
            //respawn code
            StartCoroutine(RespawnCo());

        } else
        {
            //game over code
            UIManager.instance.gameOverScreen.SetActive(true);
            WaveManager.instance.canSpawnWaves = false;

            MusicController.instance.PlayGameOver();
            PlayerPrefs.SetInt("HighScore", highScore);

            canPause = false;
        }
    }

    //respawn du joueur suite à la perte d'une vie
    public IEnumerator RespawnCo()
    {

        yield return new WaitForSeconds(respawnTime);
        HealthManager.instance.Respawn();

        WaveManager.instance.ContinueSpawning();
    }

    //ajout du score pour le joueur
    public void AddScore(int scoreToAdd)
    {
        currentScore += scoreToAdd;
        levelScore += scoreToAdd;
        UIManager.instance.scoreText.text = "Score: " + currentScore;

        if(currentScore > highScore)
        {
            highScore = currentScore;
            UIManager.instance.hiScoreText.text = "Hi-Score: " + highScore;
            //PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    //fin d'un level
    public IEnumerator EndLevelCo()
    {
        UIManager.instance.levelEndScreen.SetActive(true);
        PlayerController.instance.stopMovement = true;
        levelEnding = true;
        MusicController.instance.PlayVictory();

        canPause = false;

        yield return new WaitForSeconds(.5f);

        UIManager.instance.endLevelScore.text = "Level Score: " + levelScore; //rappel du score du level
        UIManager.instance.endLevelScore.gameObject.SetActive(true);

        yield return new WaitForSeconds(.5f);

        PlayerPrefs.SetInt("CurrentScore", currentScore);
        UIManager.instance.endCurrentScore.text = "Total Score: " + currentScore;
        UIManager.instance.endCurrentScore.gameObject.SetActive(true);

        if(currentScore == highScore) //dans le cas où un nouveau high score est fait par le joueur
        {
            yield return new WaitForSeconds(.5f);
            UIManager.instance.highScoreNotice.SetActive(true);
        }

        PlayerPrefs.SetInt("HighScore", highScore);
        PlayerPrefs.SetInt("CurrentLives", currentLives);

        yield return new WaitForSeconds(waitForLevelEnd);

        SceneManager.LoadScene(nextLevel);
    }

    //menu pause pendant une partie
    public void PauseUnpause()
    {
        if(UIManager.instance.pauseScreen.activeInHierarchy)
        {
            UIManager.instance.pauseScreen.SetActive(false);
            Time.timeScale = 1f;
            PlayerController.instance.stopMovement = false;
        } else
        {
            UIManager.instance.pauseScreen.SetActive(true);
            Time.timeScale = 0f;
            PlayerController.instance.stopMovement = true;
        }
    }
}
