using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    public static bool gameRunning = false;
    public Canvas gameOverCanvas;
    private bool gameEnded = false;
    private DialogController dialogController;

    void Start()
    {

        dialogController = GameObject.FindWithTag("Dialog").GetComponent<DialogController>();
        if (SceneManager.GetActiveScene().name == "Level1")
        {
            StartCoroutine("StartFirstDialog");
        } else
        {
            gameRunning = true;
        }
    }

    public void Update()
    {
        if (gameEnded && Input.GetKeyDown("space"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void EndLevel()
    {
        gameRunning = false;
        gameEnded = true;
        gameOverCanvas.gameObject.SetActive(true);
    }

    public void Pause()
    {
        gameRunning = false;
    }

    public void Resume()
    {
        gameRunning = true;
    }

    public IEnumerator StartFirstDialog()
    {
        yield return new WaitForSeconds(1);
        dialogController.Dialog1_1();
        dialogController.Activate();
    }

    public void NextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    } 
}
