using UnityEngine;

public class ShootingController : MonoBehaviour
{
    #region Variables
    [Header("Gun's Stats")]
    [SerializeField] private float _fireRate;
    [SerializeField] private float _fireRange;
    [SerializeField] private float _gunDamage;

    [Header("Shooting")]
    [SerializeField] private Transform _firePoint;    
    [SerializeField] private bool _auto;
    private float _nextFireTime = 0f;

    [Header("Ammunation")]
    [SerializeField] private int _maxAmmo;
    private int _currentAmmo;
    [SerializeField] private float _reloadTime;
    private bool _isReloading = false;

    [Header("Animations")]
    [SerializeField] private  Animator _gunAnimator;

    [Header("VFX")]
    [SerializeField] private ParticleSystem _muzzleFlash;
    [SerializeField] private ParticleSystem _bloodEffect;

    [Header("SFX")]
    [SerializeField] private AudioSource _audioSource;
    [SerializeField] private AudioClip _shootingSound;
    [SerializeField] private AudioClip _reloadSound;
    [SerializeField] private float _gunShotVolumeMultiplier;
    [SerializeField] private float _reloadVolumeMultiplier;

    private PlayerInputActions _playerInput;
    #endregion

    private void Start()
    {
        //Player's Input
        _playerInput = new PlayerInputActions();
        _playerInput.Enable();

        //Ammo
        _currentAmmo = _maxAmmo;
        UIManager.Instance.UpdateAmmoText(_currentAmmo);
    }

    private void Update()
    {
        HandleShooting();
        HandleReloading();
    }

    private void HandleShooting()
    {
        if (_isReloading) { return; }
        if(_auto)
        {
            if (_playerInput.Player.Shoot.IsPressed() && _nextFireTime <= 0f)
            {
                Shoot();
                _nextFireTime = _fireRate;
            }else
            {
                _gunAnimator.SetBool("Shoot", false);
            }
        }else
        {
            if (_playerInput.Player.Shoot.WasPressedThisFrame() && _nextFireTime <= 0f)
            {
                Shoot();
                _nextFireTime = _fireRate;
            }
            else
            {
                _gunAnimator.SetBool("Shoot", false);
            }
        }
       
        if (_nextFireTime > 0f)
            _nextFireTime -= Time.deltaTime;
    }

    private void Shoot()
    {
        if(!_gunAnimator.GetCurrentAnimatorStateInfo(0).IsName("Draw") && _currentAmmo > 0) {        
            RaycastHit hitInfo;
            if (Physics.Raycast(_firePoint.position, _firePoint.forward, out hitInfo, _fireRange))
            {
                PatrollingZombie patrollingZombie = hitInfo.collider.GetComponent<PatrollingZombie>();
                RegularZombie regZombie = hitInfo.collider.GetComponent<RegularZombie>();
                
                if (patrollingZombie != null)
                {
                    patrollingZombie.TakeDamage(_gunDamage);
                    ParticleSystem blood = Instantiate(_bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(blood.gameObject, blood.main.duration);
                }
                if (regZombie != null)
                {
                    regZombie.TakeDamage(_gunDamage);
                    ParticleSystem blood = Instantiate(_bloodEffect, hitInfo.point, Quaternion.LookRotation(hitInfo.normal));
                    Destroy(blood.gameObject, blood.main.duration);
                }         
            }
            _muzzleFlash.Play();
            _gunAnimator.SetBool("Shoot", true);
            _currentAmmo--;
            UIManager.Instance.UpdateAmmoText(_currentAmmo);
            _audioSource.PlayOneShot(_shootingSound, _gunShotVolumeMultiplier);
        }     
    }

    private void HandleReloading()
    {
        if(_currentAmmo <= 0 || _playerInput.Player.Reload.WasPressedThisFrame() && _currentAmmo < _maxAmmo)
        {
            Reload();
        }
    }

    private void Reload()
    {
        if(!_isReloading && _currentAmmo < _maxAmmo)
        {
            _gunAnimator.SetTrigger("Reload");
            _isReloading = true;
            _audioSource.PlayOneShot(_reloadSound, _reloadVolumeMultiplier);
            Invoke("FinishReloading", _reloadTime);
        }
    }

    private void FinishReloading()
    {
        _currentAmmo = _maxAmmo;
        UIManager.Instance.UpdateAmmoText(_currentAmmo);
        _isReloading = false;
        _gunAnimator.ResetTrigger("Reload");
    }

    public int GetMaxAmmo()
    {
        return _maxAmmo;
    }
}
