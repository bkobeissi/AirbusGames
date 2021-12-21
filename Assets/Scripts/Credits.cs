using UnityEngine;
using UnityEngine.SceneManagement;

public class Credits : MonoBehaviour
{
    //chargement du menu principal
   public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    //si l'on appuie sur Esc retour au menu principal meme si une animation
    //fait en sorte de revenir au menu principal dès sa fin
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenu();
        }
    }
}
