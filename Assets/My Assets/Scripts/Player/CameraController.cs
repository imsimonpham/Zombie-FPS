using UnityEngine;

public class CameraController : MonoBehaviour
{
    #region Variables
    [SerializeField] private Transform _player;
    [Range(0.0f, 50.0f)]
    [SerializeField] private float _lookSensitivity;
    [SerializeField] private float _minXAngle;
    [SerializeField] private float _maxXAngle;
    [SerializeField] private float _minYAngle;
    [SerializeField] private float _maxYAngle;
    [SerializeField] private float _smoothSpeed;

    private PlayerController _playerController;
    private float _xRot = 0f;
    private float _yRot = 0f;
    #endregion

    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        _playerController = _player.GetComponent<PlayerController>();
        if(_playerController == null)
        {
            Debug.LogError("Player is null");
        }
    }

    private void Update()
    {
        HandleLook();
    }

    private void HandleLook()
    {
        //get the look vector and limit camera view
        Vector2 lookVector = _playerController.GetLookVector();
        float mouseX = lookVector.x * _lookSensitivity / 100;
        float mouseY = lookVector.y * _lookSensitivity / 100;
        _yRot += mouseX;
        _xRot -= mouseY;
        _xRot = Mathf.Clamp(_xRot, _minXAngle, _maxXAngle);

        //Smooth out the look
        Quaternion targetRot = Quaternion.Euler(_xRot, _yRot, 0);
        _player.rotation = Quaternion.Slerp(_player.rotation, targetRot, _smoothSpeed * Time.deltaTime);
        transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, _smoothSpeed * Time.deltaTime);
    }
}
