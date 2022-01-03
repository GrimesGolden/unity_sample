using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerBehavior : MonoBehaviour
{
    public float MoveSpeed = 10f;
    public float RotateSpeed = 75f;

    // Two variables, one to store Bullet prefab, other holds bullet speed.
    public GameObject Bullet;
    public float BulletSpeed = 100f;

    // Bool checks to see if player should be shooting or jumping. 
    private bool _isShooting;
    private bool _isJumping;

    // Stores an instance of the GameBehavior class.
    private GameBehavior _gameManager;

    public float JumpVelocity = 5f;
    public float _vInput;
    public float _hInput;
    public float DistanceToGround = 0.1f;

    // Declare a new delegate type that returns void and takes no parameters.
    public delegate void JumpingEvent();

    // Create event of type JumpingEvent named playerJump.
    public event JumpingEvent playerJump;

    public LayerMask GroundLayer;

    private CapsuleCollider _col;
    private Rigidbody _rb;

    //______________________END VARIABLES___________________

    private void Start()
    {
        _rb = GetComponent<Rigidbody>();
        _col = GetComponent<CapsuleCollider>();

        // Assign to the GameManager objects component script.
        _gameManager = GameObject.Find("Game_Manager").
            GetComponent<GameBehavior>();
    }

    // Update is called once per frame
    void Update()
    {

        _isJumping |= Input.GetKeyDown(KeyCode.Space);

        _vInput = Input.GetAxis("Vertical") * MoveSpeed;

        _hInput = Input.GetAxis("Horizontal") * RotateSpeed;

        // Set the value of _isShooting using or operator, then check to see if left mouse button is down. 
        _isShooting |= Input.GetMouseButtonDown(0);

        /*
        this.transform.Translate(Vector3.forward * _vInput * Time.deltaTime);

    
        this.transform.Rotate(Vector3.up * _hInput * Time.deltaTime);
        */
    }

    void FixedUpdate()
    {
        if(IsGrounded() && _isJumping)
        {
            _rb.AddForce(Vector3.up * JumpVelocity,
                ForceMode.Impulse);

            // Call playerJump after force is applied.
            playerJump();

        }

        _isJumping = false;

        Vector3 rotation = Vector3.up * _hInput;

        Quaternion angleRot = Quaternion.Euler(rotation * Time.fixedDeltaTime);

        _rb.MovePosition(this.transform.position +
            this.transform.forward * _vInput * Time.fixedDeltaTime);

        _rb.MoveRotation(_rb.rotation * angleRot);

        // If is shooting returns true....
        if(_isShooting)
        {
            /* Use instantiate to assign a game object to newBullet by passing in the bullet prefab
             * We also use the player capsules position to place new bullet in front of the player (avoiding collisions)
             * Append it as GameObject to explicitly cast the returned object to the same type as newBullet, which is a GameObject. 
             */
            GameObject newBullet = Instantiate(Bullet,
                this.transform.position + new Vector3(1, 0, 0),
                 this.transform.rotation);

            // Call GetComponent() to return and store the Rigidbody component on newBullet.
            Rigidbody BulletRB =
                newBullet.GetComponent<Rigidbody>();

            // Use velocity property of Rigidbody component to set forward direction multiplied by BulletSpeed.
            BulletRB.velocity = this.transform.forward *
                BulletSpeed;

        }

        // Set _isShooting to false, to reset for next input event. 
        _isShooting = false;
    }

    void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.name == "Enemy")
            // If player collides with enemy, reduce HP by 1. 
        {
            _gameManager.HP -= 1;
        }
    }

    private bool IsGrounded()
    {
        Vector3 capsuleBottom = new Vector3(_col.bounds.center.x,
            _col.bounds.min.y, _col.bounds.center.z);

        bool grounded = Physics.CheckCapsule(_col.bounds.center,
            capsuleBottom, DistanceToGround, GroundLayer,
            QueryTriggerInteraction.Ignore);

        return grounded;
    }
}
