using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance;

    private bool _levelFinished = false;

    [SerializeField] private GameObject _inGameCanvas;
    [SerializeField] private GameObject _endLevelCanvas;
    [SerializeField] private TMP_Text _calculatedScoreText;
    [SerializeField] private TMP_Text _totalScoreText;
    [SerializeField] private TMP_Text _highScoreText;
    [SerializeField] private GameObject _nextLevelButton;

    private int _times = 0;
    [SerializeField] private int _currentScore;
    [SerializeField] private int _totalScore;
    [SerializeField] private int _lastLevelIndex; //how many levels there are in the game
    private int _currentLevelIndex;
    private int _currentLevel;
    private int _highScore;

    private float _waitFor = 2;

    private void Awake() {
        
        if (Instance == null) {

            Instance = this;
            //DontDestroyOnLoad(base.gameObject);

        }
        else {

            Destroy(base.gameObject);
            
        }
        
        // set level values
        _lastLevelIndex = SceneManager.sceneCountInBuildSettings - 1; //one scene is just the level menu
        _currentLevelIndex = SceneManager.GetActiveScene().buildIndex;
        _currentLevel = _currentLevelIndex + 1;


        _highScore = GetPreviousHighScore(); // get previous highscore
        _totalScore = GetPreviousTotalScore(); // get previous total score

    }

    /* called when level is finished
    ** only called when either player is on an end birdge with no stacks left, or when player passed the finish line collider
    */
    public void LevelFinished(bool greatWin)
    {

        _levelFinished = true;

        CalculateScore();

        UpdateAnimation(greatWin);

        UpdatePlayerPrefs();

        UpdateScoreTexts(); // might give it to a UI manager

        UpdateActiveCanvas(); // might give it to a UI manager

    }

    /* disables inGameCanvas, enables endLevelCanvas */
    private void UpdateActiveCanvas() {

        _inGameCanvas.SetActive(false);
        _endLevelCanvas.SetActive(true);

        if (_currentLevelIndex == _lastLevelIndex) { // if this is the last level

            _nextLevelButton.SetActive(false);
        }

    }

    /* takes previous score from PlayerPrefs, adds score of the current level and updates PlayerPrefs */
    private void UpdatePlayerPrefs() {

        //get previous high score
        int prevHighScore = _highScore;

        //get previous total score
        int prevTotalScore = _totalScore;

        // check if a new highscore is achieved
        if (_currentScore > prevHighScore) {

            //update highscore
            PlayerPrefs.SetInt(TagManager.LEVEL_HIGHSCORE_KEY + _currentLevel.ToString(), _currentScore);

            //update total score
            _totalScore = _totalScore - prevHighScore + _currentScore;//update totalScore value of the GameManager class
            PlayerPrefs.SetInt(TagManager.TOTAL_SCORE_KEY, _totalScore);

            _highScore = _currentScore;
        }

       
    }

    // get previous high score for the current level from PlayerPrefs
    private int GetPreviousHighScore() {

        int prevLevelHighScore;

        if (PlayerPrefs.HasKey(TagManager.LEVEL_HIGHSCORE_KEY + _currentLevel.ToString())) {
            prevLevelHighScore = PlayerPrefs.GetInt(TagManager.LEVEL_HIGHSCORE_KEY + _currentLevel.ToString());
        }
        else {
            prevLevelHighScore = 0;
        }


        return prevLevelHighScore;

    }

    // get previous total score from PlayerPrefs
    private int GetPreviousTotalScore() {

        int prevTotalScore;
        if (PlayerPrefs.HasKey(TagManager.TOTAL_SCORE_KEY)) {
            prevTotalScore = PlayerPrefs.GetInt(TagManager.TOTAL_SCORE_KEY);
        }
        else {
            prevTotalScore = 0;
        }

        return prevTotalScore;

    }

    /* updates totalscore and player's score in the current level from the endLevelCanvas */
    private void UpdateScoreTexts()
    {
        _calculatedScoreText.text = "Your Score: " + _currentScore.ToString();
        _totalScoreText.text = "Total Score: " + _totalScore.ToString();
        _highScoreText.text = "High Score: " + _highScore.ToString();
    }

    /* update player animation according to the win type */
    private static void UpdateAnimation(bool greatWin)
    {
        
        if (greatWin)
        {
            PlayerAnimController.Instance.GreatWin();
        }
        else
        {
            PlayerAnimController.Instance.NormalWin();
        }
    }

    public void RestartLevel() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
    public void PlayNextLevel() {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }
    public void SeeLevelsMenu() {

        SceneManager.LoadScene(TagManager.LEVEL_MENU_NAME);

    }

    // gets final score, and multiplies with with the _times
    private void CalculateScore() {

        _currentScore = ScoreHandler.Instance.GetScore() * _times;

    }

    // _times is the x<number> value player achieves at the end of the level
    public void SetTimes(int times) {

        _times = times;
    }

    public bool GetLevelFinished() {

        return _levelFinished;
    }
    public int GetFinalScore() {

        return _currentScore;
    }


    
}
