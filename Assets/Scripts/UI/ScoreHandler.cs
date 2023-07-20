using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreHandler : MonoBehaviour
{

    public static ScoreHandler Instance;

    [SerializeField] private TMP_Text _scoreText;

    private int _score = 0;


    private void Awake() {
        
        if (Instance != null) {
            Destroy(this.gameObject);
        }

        else {
            Instance = this;
        }

        _scoreText.text = "0";
    }

    public int GetScore() {

        return _score;
    }

    public void IncrementScore() {

        _score++;

        _scoreText.text = _score.ToString();
    }
}
