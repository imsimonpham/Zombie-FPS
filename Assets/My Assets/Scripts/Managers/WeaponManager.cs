using UnityEngine;

public class WeaponManager : MonoBehaviour
{
    [SerializeField] private GameObject[] _weapons;
    [SerializeField] private int _currentWeaponIndex;
    private PlayerInputActions _playerInput;

    private void Awake()
    {
        //Player's Input
        _playerInput = new PlayerInputActions();
        _playerInput.Enable(); 
    }

    private void Start()
    {
        SwitchWeapon(_currentWeaponIndex);
    }

    private void Update()
    {
        HandleWeaponSwitching();
    }

    private void SwitchWeapon(int newWeaponIndex)
    {
        _weapons[_currentWeaponIndex].SetActive(false);
        _weapons[newWeaponIndex].SetActive(true);
        _currentWeaponIndex = newWeaponIndex;
        ShootingController gun = _weapons[_currentWeaponIndex].GetComponent<ShootingController>();
        if(gun != null)
        {
           UIManager.Instance.UpdateAmmoText(gun.GetCurrentAmmo());
        }
    }

    private void HandleWeaponSwitching()
    {
        if (_playerInput.Player.SwitchWeapon1.WasPressedThisFrame())
            SwitchWeapon(0);
        if (_playerInput.Player.SwitchWeapon2.WasPressedThisFrame())
            SwitchWeapon(1);
        if (_playerInput.Player.SwitchWeapon3.WasPressedThisFrame())
            SwitchWeapon(2);
        if (_playerInput.Player.SwitchWeapon4.WasPressedThisFrame())
            SwitchWeapon(3);
    }

    public void SetWeaponIndex(int index)
    {
        _currentWeaponIndex = index;
    }
}
