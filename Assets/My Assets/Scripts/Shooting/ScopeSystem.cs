using Unity.VisualScripting;
using UnityEngine;

public class ScopeSystem : MonoBehaviour
{
    [Header("Scope")]
    [SerializeField] private GameObject _scopeScreen; 
    [SerializeField] private float _scopeFOV;
    [SerializeField] private float _scopeSensitivity;
    [SerializeField] private GameObject _unscopeCrosshair;
    private float _unscopeSensitivity;
    private bool _isScoped = false;

    [Header("Camera")]
    [SerializeField] private Camera _mainCamera;
    private float _originalFOV;
    private float _zoomSpeed = 100f;

    [Header("Gun and Animation")]
    [SerializeField] private Animator _gunAnimator;
    [SerializeField] private ShootingController _gun;
    private PlayerInputActions _playerInput;

    private void Start()
    {
        //Player's Input
        _playerInput = new PlayerInputActions();
        _playerInput.Enable();
        _originalFOV = _mainCamera.fieldOfView;
    }

    private void Update()
    {
        UpdateUnscopeSensitivity();
        HandleScope();
    }

    void HandleScope()
    {
        if (_playerInput.Player.Scope.IsPressed()) _isScoped = true;
        else _isScoped = false;

        if (_isScoped && !_gunAnimator.GetCurrentAnimatorStateInfo(0).IsName("Draw") && !_gunAnimator.GetCurrentAnimatorStateInfo(0).IsName("Reload"))
        {
            _unscopeCrosshair.SetActive(false);
            _scopeScreen.SetActive(true);
            _mainCamera.fieldOfView -= _zoomSpeed * Time.deltaTime;
            _mainCamera.GetComponent<CameraController>().SetLookSensitivity(_scopeSensitivity);
            _mainCamera.cullingMask &= ~(1 << LayerMask.NameToLayer("Player"));
            if (_mainCamera.fieldOfView < _scopeFOV)
                _mainCamera.fieldOfView = _scopeFOV;
        }
        else
        {
            _unscopeCrosshair.SetActive(true);
            _scopeScreen.SetActive(false);
            _mainCamera.fieldOfView += _zoomSpeed * Time.deltaTime;
            _mainCamera.cullingMask |= (1 << LayerMask.NameToLayer("Player"));
            if (_mainCamera.fieldOfView > _originalFOV)
            {
                _mainCamera.fieldOfView = _originalFOV;
                _mainCamera.GetComponent<CameraController>().SetLookSensitivity(_unscopeSensitivity);
            }
        }
    }

    void UpdateUnscopeSensitivity() { _unscopeSensitivity = _mainCamera.GetComponent<CameraController>().GetUnscopeSensitity(); }
}
