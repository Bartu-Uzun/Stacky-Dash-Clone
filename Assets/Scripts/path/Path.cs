using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    [SerializeField] private bool _isCollected = false;

    [SerializeField] private Direction _direction; //holds the direction of the path
    // Start is called before the first frame update

    public enum Direction {Left, Right, Up, Down};
    

    public Direction GetDirection() {
        return _direction;
    }
    public bool GetIsCollected() {

        return _isCollected;
    }

    public void SetIsCollected(bool isCollected) {

        _isCollected = isCollected;
    }

    public void SetDirection(Direction direction) {

        _direction = direction;


    }
}
