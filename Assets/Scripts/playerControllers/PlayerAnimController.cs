using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimController : MonoBehaviour
{
    public static PlayerAnimController Instance;

    [SerializeField] private Animator _playerAnim;

    private void Awake() {
        
        if (Instance == null) {
            Instance = this;
        }
        else {
            Destroy(base.gameObject);
        }
    }

    public void GreatWin() {

        _playerAnim.SetBool(TagManager.GREAT_WIN_PARAM, true);


    }

    public void NormalWin() {

        _playerAnim.SetBool(TagManager.NORMAL_WIN_PARAM, true);

    }
}
