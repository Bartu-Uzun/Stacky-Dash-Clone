using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Path : MonoBehaviour
{

    private bool _isCollected = false;

    [SerializeField] private bool _isTwoSided = false;
    [SerializeField] private bool _hasPriority = false;

    [SerializeField] private List<GameObject> _priorityOverList;
    [SerializeField] private List<Direction> _priorityOverDirectionList; // the direction in which the path has a priority

    [SerializeField] private Direction _direction; //holds the direction of the path
    private int _movementFactor = 1;
    [SerializeField] private float _distanceTravelledNormal = 0; // just for debugging
    [SerializeField] private float _distanceTravelledReverse = 10; // fill from editor
    [SerializeField] private float _distanceTravelled = 0; // just for debugging

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
    public bool GetIsTwoSided() {

        return _isTwoSided;
    }

    // iterates through priorityList and priorityDirrectionList to check if path has a priority over input path in the input direction
    public bool GetHasPriority(GameObject path, Direction direction) {

        for (int i = 0; i < _priorityOverList.Count; i++) {

            if (path.Equals(_priorityOverList[i])) {

                if (direction == _priorityOverDirectionList[i]) {

                    return true;
                }
            }
        }

        return false;
    }

    // if path is added to pathManager's path list, than path's isCollected is true
    // else, it is false
    public void SetIsCollected(bool isCollected) {

        _isCollected = isCollected;
    }

    // called when path movement should be reversed
    public void ReverseDistanceTravelled() {

        if (_distanceTravelled == _distanceTravelledNormal) {
            _distanceTravelled = _distanceTravelledReverse;
        }
        else {
            _distanceTravelled = _distanceTravelledNormal;
        }
    }

    // called when path movement should be reversed
    public void SetDirection(Direction direction) {

        _direction = direction;
    }

    // called when path movement should be reversed
    public void ReverseMovementFactor() {

        _movementFactor *= -1;
    }
}
