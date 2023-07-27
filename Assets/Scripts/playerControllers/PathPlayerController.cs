using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PathCreation;

public class PathPlayerController : MonoBehaviour
{

    public static PathPlayerController Instance;

    [SerializeField] private float _speed = 5;
    
    private float _threshold = 0.005f;

    [SerializeField] private bool _shouldGoLeft = false;
    [SerializeField] private bool _shouldGoRight = false;
    [SerializeField] private bool _shouldGoUp = false;
    [SerializeField] private bool _shouldGoDown = false;

    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _isSliding = false;

    private bool _levelFinished = false;
    private bool _hasStoppedOnBridge = false;
    private bool _isOnTheEdge = false;
    [SerializeField] private float _currentDistanceTravelled;

    private float _currentPathMovementFactor;

    private GameObject _currentPath;
    [SerializeField] private PathCreator _currentPathCreator;
    private Vector3 _previousPos;

    [SerializeField] private MovementDirection _lastMovementDirection;

    private GameObject _edgePath;


    public enum MovementDirection {Left, Right, Up, Down}

    private void Awake() {
        
        if (Instance != null) {
            Destroy(this);
        }
        else {
            Instance = this;
        }

        _previousPos = transform.position;

        //DontDestroyOnLoad(gameObject); //NEED TO CHECK
    }

    /* Lesson: FixedUpdate is unreliable when reading functions like Input.GetKeyDown. To read them, we still
    ** need to use Update function so that the game does not miss any inputs.
    */
    private void Update()
    {
        if (!GameManager.Instance.GetLevelFinished()) {
            ReadInput();
        }

        if (_isMoving && _currentPathCreator != null) {
            Move();

        }

        if (_isOnTheEdge && !_isSliding) {
            EndOfBridgeMovement();
        }
        

    }

    private void Move() {

        _previousPos = transform.position;
        _currentDistanceTravelled += _speed * _currentPathMovementFactor * Time.deltaTime;
        transform.position = _currentPathCreator.path.GetPointAtDistance(_currentDistanceTravelled, EndOfPathInstruction.Stop);
        //transform.rotation = _currentPathCreator.path.GetRotationAtDistance(_currentDistanceTravelled, EndOfPathInstruction.Stop);

        if (Vector3.Distance(_previousPos, transform.position) <= _threshold) { // player has stopped
            Stop();
        }

    }

    public void SlideOnBridge(GameObject path) {

        if (_currentPath == null) {

            _currentPath = path;

            SetPathComponent();

            _isSliding = true;
            _isMoving = true;

            _currentPathCreator = _currentPath.GetComponent<PathCreator>();
        }
    }

    public void EndOfBridge(GameObject edgePath) {

        _edgePath = edgePath;
        _isOnTheEdge = true;
    }

    public void EndOfBridgeMovement() {

        _isOnTheEdge = false;

        _currentPath = _edgePath;

        Path pathComponent = SetPathComponent();

        _isSliding = false;
        _isMoving = true;

        _currentPathCreator = _currentPath.GetComponent<PathCreator>();


        Path.Direction pathDir = pathComponent.GetDirection();
        if (pathDir == Path.Direction.Down) {
            pathComponent.SetDirection(Path.Direction.Up);
        }
        else if (pathDir == Path.Direction.Up) {
            pathComponent.SetDirection(Path.Direction.Down);
        }
        else if (pathDir == Path.Direction.Right) {
            pathComponent.SetDirection(Path.Direction.Left);
        }
        else if (pathDir == Path.Direction.Left) {
            pathComponent.SetDirection(Path.Direction.Right);
        }
        
    }

    public void Stop() {

        bool doesCarryOn = false;

        _shouldGoDown = false;
        _shouldGoLeft = false;
        _shouldGoRight = false;
        _shouldGoUp = false;

        _isMoving = false;
        
        if (!_isSliding) {

            /* check all the paths player has access to
            ** if one of the paths has a matching direction with the _lastMovementDirection,
            ** player should start moving at that path
            */
            doesCarryOn = CheckIfPathCarriesOn(_currentPath);
        }

        
        if (!doesCarryOn) {

            _currentPath = null;
            _currentPathCreator = null;
        }

        _isSliding = false;
        
    }

    private bool CheckIfPathCarriesOn(GameObject pathToCheck) {

        

        Path.Direction checkDirection;
        Path.Direction reverseDirection;

        if (_lastMovementDirection == MovementDirection.Left) {

            checkDirection = Path.Direction.Left;
            reverseDirection = Path.Direction.Right;
        }
        else if (_lastMovementDirection == MovementDirection.Right) {

            checkDirection = Path.Direction.Right;
            reverseDirection = Path.Direction.Left;
        }
        else if (_lastMovementDirection == MovementDirection.Down) {

            checkDirection = Path.Direction.Down;
            reverseDirection = Path.Direction.Up;
        }
        else {

            checkDirection = Path.Direction.Up;
            reverseDirection = Path.Direction.Down;
        }

        bool flag = SetPlatformPathComponent(checkDirection, reverseDirection, pathToCheck);

        return flag;
    }

    // called whenever player should stop on a bridge
    public void BridgeStop(GameObject path) {
        
        _shouldGoDown = false;
        _shouldGoLeft = false;
        _shouldGoRight = false;
        _shouldGoUp = false;

        _isMoving = false;
        _isSliding = false;

        // We preserve the _currentPath
        
        _currentPath = path;
        _currentPathCreator = _currentPath.GetComponent<PathCreator>();

        _hasStoppedOnBridge = true;

        if (_levelFinished) {

            GameManager.Instance.LevelFinished(false); //level finished, not great win (player stopped on the end bridge)
        }
    }

    private void ReadInput()
    {

        if (!_isMoving) {

            if (!_hasStoppedOnBridge) { //player did not stop on the bridge

                //read inputs
                if (!_shouldGoLeft && _lastMovementDirection != MovementDirection.Left)   
                {
                    _shouldGoLeft = Input.GetKeyDown(KeyCode.LeftArrow) || MobileInput.Instance.swipeLeft;
                }
                if (!_shouldGoRight && _lastMovementDirection != MovementDirection.Right)
                {

                    _shouldGoRight = Input.GetKeyDown(KeyCode.RightArrow) || MobileInput.Instance.swipeRight;
                }
                if (!_shouldGoDown && _lastMovementDirection != MovementDirection.Down)
                {

                    _shouldGoDown = Input.GetKeyDown(KeyCode.DownArrow) || MobileInput.Instance.swipeDown;
                }
                if (!_shouldGoUp && _lastMovementDirection != MovementDirection.Up)
                {

                    _shouldGoUp = Input.GetKeyDown(KeyCode.UpArrow) || MobileInput.Instance.swipeUp;
                }

            }
            else { // player has stopped on a bridge

                if (_lastMovementDirection == MovementDirection.Right && !_shouldGoLeft) { // if last movement was right, player can move left

                    _shouldGoLeft = Input.GetKeyDown(KeyCode.LeftArrow) || MobileInput.Instance.swipeLeft;
                }
                if (_lastMovementDirection == MovementDirection.Left && !_shouldGoRight) { // if last movement was left, player can move right
                    
                    _shouldGoRight = Input.GetKeyDown(KeyCode.RightArrow) || MobileInput.Instance.swipeRight;
                }
                if (_lastMovementDirection == MovementDirection.Up && !_shouldGoDown) { // if last movement was up, player can move down

                    _shouldGoDown = Input.GetKeyDown(KeyCode.DownArrow) || MobileInput.Instance.swipeDown;
                }
                if (_lastMovementDirection == MovementDirection.Down && !_shouldGoUp) { // if last movement was down, player can move up

                    _shouldGoUp = Input.GetKeyDown(KeyCode.UpArrow) || MobileInput.Instance.swipeUp;
                }
            }

        }
        
    }

    private void FixedUpdate() {
        
        SetPath();
    }

    private void SetPath() {


        if (!_isMoving) {

            if (_hasStoppedOnBridge) {

                if (_shouldGoLeft) {

                    _lastMovementDirection = MovementDirection.Left; // save lastPressedKey value

                    SetBridgePathComponent();
                    _shouldGoLeft = false;

                }
                if (_shouldGoRight) {

                    _lastMovementDirection = MovementDirection.Right; // save lastPressedKey value

                    SetBridgePathComponent();
                    _shouldGoRight = false;
                }
                if (_shouldGoDown) {

                    _lastMovementDirection = MovementDirection.Down; // save lastPressedKey value

                    SetBridgePathComponent();
                    _shouldGoDown = false;
                }
                if (_shouldGoUp)
                {

                    _lastMovementDirection = MovementDirection.Up; // save lastPressedKey value

                    SetBridgePathComponent();
                    _shouldGoUp = false;
                }

            }
            else {

                // find movement direction, set path accordingly, update path's allowed direction
                if (_shouldGoLeft) {

                    SetPlatformPathComponent(Path.Direction.Left, Path.Direction.Right, _lastMovementDirection);

                    _lastMovementDirection = MovementDirection.Left; // save lastPressedKey value

                    _shouldGoLeft = false;
                }
                if (_shouldGoRight) {

                    SetPlatformPathComponent(Path.Direction.Right, Path.Direction.Left, _lastMovementDirection);

                    _lastMovementDirection = MovementDirection.Right; // save lastPressedKey value

                    _shouldGoRight = false;
                }
                if (_shouldGoDown) {

                    SetPlatformPathComponent(Path.Direction.Down, Path.Direction.Up, _lastMovementDirection);

                    _lastMovementDirection = MovementDirection.Down; // save lastPressedKey value

                    _shouldGoDown = false;
                    
                }
                if (_shouldGoUp) {

                    SetPlatformPathComponent(Path.Direction.Up, Path.Direction.Down, _lastMovementDirection);

                    _lastMovementDirection = MovementDirection.Up; // save lastPressedKey value

                    _shouldGoUp = false;
                    
                }
            }
            
            
        }
    }

    private void SetBridgePathComponent()
    {
        SetPathComponent();

        _isMoving = true;
        _isSliding = true;
        _hasStoppedOnBridge = false;
    }

    private void SetPlatformPathComponent(Path.Direction direction, Path.Direction reverseDirection, MovementDirection lastMovementDirection)
    {

        _currentPath = PathManager.Instance.GetPath(direction, reverseDirection, lastMovementDirection);

        if (_currentPath != null) {

            Path pathComponent = SetPathComponent();
            pathComponent.SetDirection(reverseDirection);
            _isMoving = true;

            _currentPathCreator = _currentPath.GetComponent<PathCreator>();

        }

    }

    private bool SetPlatformPathComponent(Path.Direction direction, Path.Direction reverseDirection, GameObject pathToCheck) {

        bool flag = false;

        _currentPath = PathManager.Instance.GetPath(direction, reverseDirection, pathToCheck);

        if (_currentPath != null) {

            Path pathComponent = SetPathComponent();
            pathComponent.SetDirection(reverseDirection);
            _isMoving = true;

            _currentPathCreator = _currentPath.GetComponent<PathCreator>();

            flag = true;

        }

        return flag;
    }

    private Path SetPathComponent()
    {
        Path pathComponent = _currentPath.GetComponent<Path>();
        _currentPathMovementFactor = pathComponent.GetMovementFactor();
        _currentDistanceTravelled = pathComponent.GetDistanceTravelled();
        pathComponent.ReverseMovementFactor();
        pathComponent.ReverseDistanceTravelled();

        return pathComponent;
    }

    public void AlertLevelFinished() {

        _levelFinished = true;
    }

    public bool GetIsSliding() {

        return _isSliding;
    }
    public bool GetHasStoppedOnBridge() {

        return _hasStoppedOnBridge;
    }
}
