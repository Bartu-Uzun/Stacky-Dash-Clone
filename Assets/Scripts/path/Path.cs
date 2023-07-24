using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    [SerializeField] private bool _isCollected = false;

    [SerializeField] private Direction _direction; //holds the direction of the path
    [SerializeField] private int _movementFactor = 1;
    [SerializeField] private float _distanceTravelledNormal = 0;
    [SerializeField] private float _distanceTravelledReverse = 10;
    [SerializeField] private float _distanceTravelled = 0;

    public enum Direction {Left, Right, Up, Down, Bridge};
    

    public Direction GetDirection() {
        return _direction;
    }
    public bool GetIsCollected() {

        return _isCollected;
    }
    public int GetMovementFactor() {

        return _movementFactor;
    }
    public float GetDistanceTravelled() {

        return _distanceTravelled;
    }

    public void SetIsCollected(bool isCollected) {

        _isCollected = isCollected;
    }
    public void ReverseDistanceTravelled() {

        if (_distanceTravelled == _distanceTravelledNormal) {
            _distanceTravelled = _distanceTravelledReverse;
        }
        else {
            _distanceTravelled = _distanceTravelledNormal;
        }
    }

    public void SetDirection(Direction direction) {

        _direction = direction;
    }
    public void ReverseMovementFactor() {

        _movementFactor *= -1;
    }
}
