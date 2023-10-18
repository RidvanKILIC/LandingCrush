using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class BaloonMovement : MonoBehaviour
{
    #region Varliables
    [Header("Movement Variables")]
    [SerializeField] float movementSpeed;
    [SerializeField] public float breakSensitivity;
    [SerializeField] public float maxVerticalSpeed;
    float moveX;
    float moveY;
    [Header("Baloon Movement Boundaries")]
    [SerializeField] public GameObject maxX;
    [SerializeField] public GameObject minX;
    [SerializeField] public GameObject maxY;
    [SerializeField] public GameObject minY;
    [SerializeField] public GameObject maxZ;
    [SerializeField] public GameObject minZ;
    [Header("References")]
    //PlayerInput playerInput;
    FloatingJoystick _floatingJoystick;
    BaloonAnimController _animController;
    Baloon _baloon;
    gameManager _gameManager;
    [Header("Controllers")]
    [SerializeField] float movementsFuelConsumption;
    [SerializeField] float brakesFuelConsumption;
    bool startDecreasingFuel;
    bool fly = false;
    bool fall = false;
    bool breakAvailable = false;
    #endregion
    // Start is called before the first frame update
    void Start()
    {
        Init();
    }
    /// <summary>
    /// Initializes References & Vaeiables
    /// </summary>
    void Init()
    {
        //playerInput = GetComponent<PlayerInput>();
        _floatingJoystick = GameObject.FindObjectOfType<FloatingJoystick>();
        _baloon = GetComponent<Baloon>();
        _animController = GetComponent<BaloonAnimController>();
        _gameManager = GameObject.FindObjectOfType<gameManager>();
    }
    // Update is called once per frame
    void FixedUpdate()
    {
        if(!_gameManager.landed && _gameManager.gameStarted)
            Move();
        if (breakAvailable)
            addForce();
    }
    void addForce()
    {
        //Debug.Log("Break");
        if(_baloon.rb.transform.position.y < maxY.transform.position.y)
        {
            _baloon.rb.AddRelativeForce(Vector3.up * Time.fixedDeltaTime * breakSensitivity);
            _baloon.rb.velocity = Vector3.ClampMagnitude(_baloon.rb.velocity, maxVerticalSpeed);
        }
        else
        {
            _baloon.rb.velocity = Vector3.zero;
            relase();
        }
        
        transform.position = clampPosition();
    }

    /// <summary>
    /// Gets inputs fron Player Input and applies to transform and decreases fuel
    /// </summary>
    void Move()
    {
        moveX = _floatingJoystick.Horizontal;
        moveY = _floatingJoystick.Vertical;
        //moveX = playerInput.actions["Move"].ReadValue<Vector2>().x;
        //moveY = playerInput.actions["Move"].ReadValue<Vector2>().y;

        if(moveX != 0 || moveY != 0)
        {
            float multiplier = Time.fixedDeltaTime / movementsFuelConsumption;
            //Debug.Log("Multiplier of movement:" + multiplier);
            //_baloon.setFuel(_baloon.getFuel() - (multiplier));


            //Debug.Log("X Value: " + moveX);
            //Debug.Log("Y Value: " + moveY);
        }

        //MovementAnim
        _animController.playMovementAnimation(moveX, moveY);

        Vector3 moveVector = new Vector3(moveX, 0f, moveY);
        transform.Translate(moveVector * movementSpeed * Time.fixedDeltaTime);


        transform.position = clampPosition();
        
        ///Down & Fly Anims
        //if (_baloon.getSpeed() > 30 && !fly)
        //{
        //    fly = true;
        //    fall = false;
        //    _animController.playHeightAnimations(_baloon.getSpeed());
        //}
        //if(_baloon.getSpeed() < 30 && !fall)
        //{
        //    fall = true;
        //    fly = false;
        //    _animController.playHeightAnimations(_baloon.getSpeed());
        //}
    }
    /// <summary>
    /// Clamps  the position of the object by usin boundary objects
    /// </summary>
    /// <returns></returns>
    Vector3 clampPosition()
    {
        //Clamp Position 
        var posX = Mathf.Clamp(transform.position.x, minX.transform.position.x, maxX.transform.position.x);
        var posY = Mathf.Clamp(transform.position.y, minY.transform.position.y, maxY.transform.position.y);
        var posZ = Mathf.Clamp(transform.position.z, minZ.transform.position.z, maxZ.transform.position.z);
        return new Vector3(posX, posY, posZ);
    }
    /// <summary>
    /// If fuel value freater than zero than starts decreaseFuel Coroutine
    /// </summary>
    public void brake()
    {
        if(_baloon.getFuel() > 0 && _baloon.rb.transform.position.y < maxY.transform.position.y)
        {
            _baloon.activateDeactivatefireFx(1);
            SoundManager.SInstance.playBaloonSound(SoundManager.SInstance.flame);
            //startDecreasingFuel = true;
            breakAvailable = true;
            //StartCoroutine(decreaseFuel());
        }

    }
    public void relase()
    {
        //_baloon.rb.drag = 0;
        // startDecreasingFuel = false;
        _baloon.activateDeactivatefireFx(0);
        SoundManager.SInstance.playBaloonSound();
        breakAvailable = false;
    }
    /// <summary>
    /// While presing brake button decreases Fuel
    /// </summary>
    /// <returns></returns>
    IEnumerator decreaseFuel() 
    {
       
        while (startDecreasingFuel /*&& _baloon.rb.drag < 1*/)
        {
            //_baloon.rb.drag += breakSensitivity;
           
            //Mathf.Clamp(_baloon.rb.drag, 0.2f, 1f);
            float multiplier = Time.fixedDeltaTime / brakesFuelConsumption;
            //Debug.Log("Multiplier of brake:" + multiplier);
            _baloon.setFuel(_baloon.getFuel() - (multiplier));
            yield return null;
        }
    }
}
