using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    [SerializeField] private int _level;

    [SerializeField] private TMP_Text _levelText;

    private void Start() {
        _levelText.text = _level.ToString();
    }


    public void OpenScene() {

        SceneManager.LoadScene("Level" + _level.ToString());
    }

}
