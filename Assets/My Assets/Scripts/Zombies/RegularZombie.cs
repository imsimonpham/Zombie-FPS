using System.Collections;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Rendering;

public class RegularZombie : MonoBehaviour
{
    #region Variables
    [Header("AI States")]
    [SerializeField] private ZombieState _currentState;
    [SerializeField] private enum ZombieState { Idle, Chase, Attack, Die}

    [Header("AI")]
    [SerializeField] private NavMeshAgent _agent;
    [SerializeField] private float _attackTimer;
    [SerializeField] private CapsuleCollider _capsuleCollider;
    [SerializeField] private Transform _attackPoint;
    [SerializeField] private float _attackRadius;

    [Header("AI Stats")]
    [SerializeField] private float _chaseDistance;
    [SerializeField] private float _attackDistance;
    [SerializeField] private float _attackCooldownTime;
    [SerializeField] private float _attackDamage;
    [SerializeField] private float _health;
    [SerializeField] private float _speed;
    private bool _isAttacking;
    private float _lastAttackTime;

    [Header("VFX")]
    private Volume _bloodScreenEffect;

    [Header("Animations")]
    [SerializeField] private Animator _animator;

    [Header("Player")]
    [SerializeField] private Transform _player;
    [SerializeField] private PlayerController playerController;
    [SerializeField] private LayerMask _hitMask;
    #endregion

    private void Awake()
    {
        _bloodScreenEffect = GameObject.FindGameObjectWithTag("Blood Screen Effect").GetComponent<Volume>();
        if (_bloodScreenEffect == null)
        {
            Debug.LogError("Blood Screen Effect is null");
        }
        _capsuleCollider = GetComponent<CapsuleCollider>();
    }

    private void Update()
    {
        switch (_currentState)
        {
            case ZombieState.Idle:
                HandleIdle();
                break;
            case ZombieState.Chase:
                HandleChase();
                break;
            case ZombieState.Attack:
                HandleAttack();
                break;
            case ZombieState.Die:
                HandleDeath();
                break;
        }
    }

    //Main Methods
    #region
    private void HandleIdle()
    {
        _attackTimer = 0;
        _animator.SetBool("isChasing", false);
        _animator.SetBool("isAttacking", false);
        _agent.speed = 0;
        if (PlayerIsInRange(_chaseDistance))
            _currentState = ZombieState.Chase;
    }

    private void HandleChase()
    {
        _attackTimer = 0;
        _animator.SetBool("isChasing", true);
        _animator.SetBool("isAttacking", false);
        _agent.SetDestination(_player.position);
        _agent.speed = _speed;
        if (PlayerIsInRange(_attackDistance))
            _currentState = ZombieState.Attack;
        if(!PlayerIsInRange(_chaseDistance))
            _currentState = ZombieState.Idle;
    }

    private void HandleAttack()
    {          
        if (!_isAttacking && _attackTimer >= _lastAttackTime)
        {
            _attackTimer += Time.deltaTime;
            _animator.SetBool("isAttacking", true);
            _agent.SetDestination(transform.position);
            AttackPlayer();
        }
        if (!PlayerIsInRange(_attackDistance))
            _currentState = ZombieState.Chase;
    }

    private void HandleDeath()
    {
        _animator.SetBool("isChasing", false);
        _animator.SetBool("isAttacking", false);
        _animator.SetBool("isDead", true);
        _agent.enabled = false;
        _capsuleCollider.enabled = false;
        this.enabled = false;
        //increase score
        Debug.Log("Zombie died");
    }
    #endregion

    //SubMethods
    #region
    void AttackPlayer()
    {
        Collider[] hitColliders = Physics.OverlapSphere(_attackPoint.position, _attackRadius, _hitMask);
        if (hitColliders.Length > 0 && _animator.GetCurrentAnimatorStateInfo(0).IsName("Attack"))
        {
            StartCoroutine(AttackRoutine());
            StartCoroutine(BloodScreenEffectRoutine());
        }
    }

    public void TakeDamage(float dmg)
    {
        if (_currentState == ZombieState.Die) { return; }
        _health -= dmg;
        if (_health <= 0)
        {
            _health = 0;
            Die();
        }
    }

    private IEnumerator AttackRoutine()
    {
        _isAttacking = true;
        playerController.TakeDamage(_attackDamage);
        yield return new WaitForSeconds(_attackCooldownTime);
        _isAttacking = false;
        _lastAttackTime = _attackTimer;
    }

    private IEnumerator BloodScreenEffectRoutine()
    {
        _bloodScreenEffect.enabled = true;
        yield return new WaitForSeconds(0.5f);
        _bloodScreenEffect.enabled = false;
    }

    private bool PlayerIsInRange(float range) { return Vector3.Distance(transform.position, _player.transform.position) <= range; }

    private void Die() { _currentState = ZombieState.Die; }

    void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(_attackPoint.transform.position, _attackRadius);
    }
    #endregion
}
