using UnityEngine;

public class PlayerController : MonoBehaviour
{
    #region Variables
    //Input
    private PlayerInputActions _playerInput;
    private Vector3 _moveDirection;

    [Header("Player's Health & Damage")]
    [SerializeField] private float _currentHealth;
    private float _maxHealth = 200;  

    [Header("Player's Movement & Gravity")]
    [SerializeField] private float _movementSpeed;
    [SerializeField] private Transform _groundCheck;
    [SerializeField] private LayerMask _groundMask;
    [SerializeField] private float _groundDistance;
    [SerializeField] private float _gravityMultiplier;
    [SerializeField] private bool _isGrounded;
    private Vector3 _velocity;
    private float _gravity = -9.81f;
    private CharacterController _characterController;
    private bool _isMoving;

    [Header("Jumping")]
    [SerializeField] private float _jumpForce;
    [SerializeField] private float _gravityOffsetMultiplier;

    [Header("SFX")]
    [SerializeField] private AudioSource _leftFoodAudioSource;
    [SerializeField] private AudioSource _rightFoodAudioSource;
    [SerializeField] private AudioClip[] _footstepSounds;
    [SerializeField] float _footstepInterval;
    private float _nextFootstepTime;
    private bool _isLeftFootStep = true;
    #endregion

    private void Start()
    {
        //Character Controller
        _characterController = GetComponent<CharacterController>();

        //Player's Input
        _playerInput = new PlayerInputActions();
        _playerInput.Enable();

        //Player's Health
        _currentHealth = _maxHealth;
    }

    private void Update()
    {
        _isGrounded = Physics.CheckSphere(_groundCheck.position, _groundDistance, _groundMask);
        HandleGravity();
        HandleMovement();
        HandleFootstepSound();
        HandleJump();
    }

    //Gravity and Movement
    #region
    void HandleGravity()
    {
        if(_isGrounded && _velocity.y < 0) _velocity.y = -1f;
        else _velocity.y += _gravity * _gravityMultiplier * Time.deltaTime;
    }

    void HandleMovement()
    {
        Vector2 inputVector = _playerInput.Player.Move.ReadValue<Vector2>();
        if (inputVector != Vector2.zero)
        {
            _isMoving = true;
        }else
        {
            _isMoving = false;
        }
        _moveDirection = inputVector.y * transform.forward + inputVector.x * transform.right;
        _characterController.Move(_moveDirection * _movementSpeed * Time.deltaTime);
        _characterController.Move(_velocity * Time.deltaTime);
    }

    void HandleJump()
    {
        if (_playerInput.Player.Jump.WasPressedThisFrame() && _isGrounded)
        {
            _velocity.y = Mathf.Sqrt(_jumpForce * _gravity * _gravityOffsetMultiplier);
        }
    }
    #endregion

    //Audio
    #region
    void HandleFootstepSound()
    {
        if (_isGrounded && _isMoving && Time.time >= _nextFootstepTime)
        {
            PlayFootstepSound();
            _nextFootstepTime = _footstepInterval + Time.time;
        }    
    }
    void PlayFootstepSound()
    {
        AudioClip footstepClip = _footstepSounds[Random.Range(0, _footstepSounds.Length)];
        if (_isLeftFootStep)
        {
            _leftFoodAudioSource.PlayOneShot(footstepClip, 0.005f);
        }
        else
        {
            _rightFoodAudioSource.PlayOneShot(footstepClip, 0.005f);
        }
        _isLeftFootStep = !_isLeftFootStep;
    }
    #endregion

    //Health and Damage
    #region
    public void TakeDamage(float dmg)
    {
        _currentHealth -= dmg;
        UIManager.Instance.UpdateHealthBar(_currentHealth);
        if (_currentHealth <= 0)
        {
            _currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        //death screen
        Debug.Log("Player Died");
    }
    #endregion

    //Get Functions
    #region
    public Vector2 GetLookVector()
    {
        return _playerInput.Player.Look.ReadValue<Vector2>();
    }

    public float GetMaxHealth()
    {
        return _maxHealth;
    }
    #endregion
}
