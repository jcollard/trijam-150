using System.Collections;
using System.Collections.Generic;
using CaptainCoder.Unity;
using UnityEngine;

public class GameInfoController : MonoBehaviour
{

    public static GameInfoController Instance;

    public int Sets;
    public int Cards = 81;
    public int Discards;

    public TextGroup SetsCompleted, CardsRemaining, CardsDiscarded, TimeText, ErrorText, HintText;
    public float StartAt;

    public GameObject GameScreen;
    public DeckController Deck;
    public BoardController GameBoard;
    public Canvas TitleScreen, GameScreenOverlay;
    public CanvasFader ErrorScreen, NiceScreen;

    void Awake()
    {
        Instance = this;
    }

    public void Update()
    {
        int timeElapsed = (int)(Time.time - StartAt);
        int minutes = timeElapsed / 60;
        int seconds = timeElapsed % 60;
        TimeText.SetText($"Time: {minutes}:{seconds:00}");
        SetsCompleted.SetText($"Groups Found: {Sets}");
        CardsRemaining.SetText($"Card Remaining: {Cards}");
        CardsDiscarded.SetText($"Cards Discarded: {Discards}");
    }

    public void StartGame()
    {
        StartAt = Time.time;
        Cards = 81;
        Sets = 0;
        Deck.Init();
        GameScreen.SetActive(true);
        GameScreenOverlay.gameObject.SetActive(true);
        TitleScreen.gameObject.SetActive(false);
        if (!SoundController.Instance.Music.isPlaying)
            SoundController.Instance.Music.Play();
        
    }

}
