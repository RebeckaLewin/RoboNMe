using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

//Handles the game flow, its state, as well as UI functionality outside the gameplay
//Plays the BG music
public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject quitMessage;
    private CmdParser parser;
    public static GameStates State { get; set; }

    [Header("MENU")]
    [SerializeField] private GameObject soundBtn;
    [SerializeField] private GameObject pauseBtn;

    [Header("AUDIO")]
    [SerializeField] private AudioSource source;
    [SerializeField] private AudioClip bgmBuild, bgmPlay;
    private bool soundOff = false;

    private GameStates prevState;
    private bool paused;

    // Start is called before the first frame update
    void Start()
    {
        parser = FindObjectOfType<CmdParser>();
        State = GameStates.buildPhase;

        Time.timeScale = 1;
        source = GetComponent<AudioSource>();
    }

    public void ChangeState(int index)
    {
        GameStates state = (GameStates)index;
        switch (state)
        {
            case GameStates.buildPhase:
                Time.timeScale = 1;
                break;
            case GameStates.playPhase:
                Time.timeScale = 1;
                source.Stop();
                StartCoroutine(StartPlayPhase());
                break;
            case GameStates.paused:
                Time.timeScale = 0;
                break;
        }
    }

    private IEnumerator StartPlayPhase()
    {
        yield return new WaitForSeconds(2f);
        source.clip = bgmPlay;
        source.Play();
        State = GameStates.playPhase;
        parser.StartProgram();
    }

    public void CheckIfQuit()
    {
        quitMessage.SetActive(true);
    }

    public void QuitGame()
    {
        Application.Quit();
    }

    public void TurnOffSound()
    {
        soundOff = !soundOff;
        var sources = (AudioSource[])Object.FindObjectsOfType(typeof(AudioSource));
        foreach(AudioSource s in sources)
        {
            if (soundOff)
                s.volume = 0;
            else
                s.volume = 1;

            soundBtn.GetComponent<MenuButtonScript>().ChangeSprite(soundOff);
        }
    }

    public void PauseGame()
    {
        paused = !paused;
        if (paused)
        {
            prevState = State;
            State = GameStates.paused;
            Time.timeScale = 0;
        }
        else { State = prevState; Time.timeScale = 1; }

        pauseBtn.GetComponent<MenuButtonScript>().ChangeSprite(paused);
    }

    public void ResetScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
