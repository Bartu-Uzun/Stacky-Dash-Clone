using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RigidbodyPlayerController : MonoBehaviour
{

    public static RigidbodyPlayerController Instance;
    [SerializeField] private float _speed = 1600;
    [SerializeField] private float _bridgeSpeed = 1700;

    [SerializeField] private float _threshold = 3.5f;

    private Rigidbody _rb;


    [SerializeField] private bool _shouldGoLeft = false;
    [SerializeField] private bool _shouldGoRight = false;
    [SerializeField] private bool _shouldGoUp = false;
    [SerializeField] private bool _shouldGoDown = false;

    [SerializeField] private bool _isMoving = false;
    [SerializeField] private bool _stoppedOnBridge = false;
    [SerializeField] private bool _hasEnteredToANewBridge = false;
    
    private Vector3 _bridgeDir;

    private void Awake() {
        
        if (Instance != null) {
            Destroy(this);
        }
        else {
            Instance = this;
        }

        //DontDestroyOnLoad(gameObject); //NEED TO CHECK
    }

    // Start is called before the first frame update
    void Start()
    {

        _rb = GetComponent<Rigidbody>();
        
    }

    // Lesson: Always use FixedUpdate if you are using physics 
    void FixedUpdate()
    {
        Move();

        CheckVelocity();
    }

    private void Move()
    {

        if (!_isMoving) { // if player is not moving, move according to the input

            if (_hasEnteredToANewBridge) { //if player has entered a new bridge, move along bridge's direction


                _rb.velocity = _bridgeDir * _bridgeSpeed * Time.deltaTime;

                _hasEnteredToANewBridge = false;

                _isMoving = true;

                CharacterRotationController.Instance.Rotate(_bridgeDir); // rotate character

                
            }

            else if (_shouldGoLeft)
            {

                _rb.velocity = Vector3.left * _speed * Time.deltaTime;
                _shouldGoLeft = false;

                _isMoving = true;

                _stoppedOnBridge = false;

                CharacterRotationController.Instance.Rotate(Vector3.left); // rotate character

                
            }

            else if (_shouldGoRight)
            {

                _rb.velocity = Vector3.right * _speed * Time.deltaTime;
                _shouldGoRight = false;

                _isMoving = true;

                _stoppedOnBridge = false;

                CharacterRotationController.Instance.Rotate(Vector3.right); // rotate character
            }

            else if (_shouldGoDown)
            {

                _rb.velocity = Vector3.back * _speed * Time.deltaTime;
                _shouldGoDown = false;

                _isMoving = true;

                _stoppedOnBridge = false;

                CharacterRotationController.Instance.Rotate(Vector3.back); // rotate character
            }
            else if (_shouldGoUp)
            {

                _rb.velocity = Vector3.forward * _speed * Time.deltaTime;
                _shouldGoUp = false;

                _isMoving = true;

                _stoppedOnBridge = false;

                CharacterRotationController.Instance.Rotate(Vector3.forward); // rotate character
            }


        }
        
    }

    /* Lesson: FixedUpdate is unreliable when reading functions like Input.GetKeyDown. To read them, we still
    ** need to use Update function so that the game does not miss any inputs.
    */
    private void Update()
    {
        if (!GameManager.Instance.GetLevelFinished()) {
            ReadInput();
        }
        

    }

    private void ReadInput()
    {

        if (!_isMoving) {

            if (!_stoppedOnBridge) { //player did not stop on the bridge

                //read inputs
                if (!_shouldGoLeft)
                {
                    _shouldGoLeft = Input.GetKeyDown(KeyCode.LeftArrow) || MobileInput.Instance.swipeLeft;
                }
                if (!_shouldGoRight)
                {

                    _shouldGoRight = Input.GetKeyDown(KeyCode.RightArrow) || MobileInput.Instance.swipeRight;
                }
                if (!_shouldGoDown)
                {

                    _shouldGoDown = Input.GetKeyDown(KeyCode.DownArrow) || MobileInput.Instance.swipeDown;
                }
                if (!_shouldGoUp)
                {

                    _shouldGoUp = Input.GetKeyDown(KeyCode.UpArrow) || MobileInput.Instance.swipeUp;
                }

            }

            else { //player has stopped on the bridge, they can only go back

                if (_bridgeDir.x == -1 && !_shouldGoRight) { //last movement is left, can only move right

                    _shouldGoRight = Input.GetKeyDown(KeyCode.RightArrow) || MobileInput.Instance.swipeRight;

                }
                else if (_bridgeDir.z == 1 && !_shouldGoDown) { //last movement is up, can only move down

                    _shouldGoDown = Input.GetKeyDown(KeyCode.DownArrow) || MobileInput.Instance.swipeDown;

                }
                else if (_bridgeDir.z == -1 && !_shouldGoUp) { //last movement is down, can only move up

                    _shouldGoUp = Input.GetKeyDown(KeyCode.UpArrow) || MobileInput.Instance.swipeUp;

                }
                



            }

            

        }
        
    }

    public void Stop() { //called whenever player is stopped

        _shouldGoDown = false;
        _shouldGoLeft = false;
        _shouldGoRight = false;
        _shouldGoUp = false;

        _isMoving = false;
    }

    // called whenever player should stop on a bridge
    public void BridgeStop() {
        _rb.velocity = Vector3.zero;

        Stop();

        _stoppedOnBridge = true;
    }

    public void ChestStop() {

        _rb.velocity = Vector3.zero;

        Stop();
    }

    //checks the velocity of the player. If it is less than the threshold, it is casted as 0
    private void CheckVelocity() {

        if (_rb.velocity.magnitude <= _threshold && _isMoving) {

            Stop();
        }
    }

    //gets called whenever player enters a new bridge. bridgeDir is the direction of the bridge, and is normalized
    public void EnterToANewBridge(Vector3 bridgeDir) {

        Stop();

        _hasEnteredToANewBridge = true;

        _bridgeDir = bridgeDir;

    }
}
