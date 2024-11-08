using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Windows;

public class EntityMovement : MonoBehaviour
{
    [SerializeField] private float _slopeForce;
    [SerializeField] private float _slopeForceRayLength;
    [SerializeField] private float _speed = 6f;
    [SerializeField] private float _angerRange = 100f;
    private CharacterController _cc;
    private GameObject _player;
    private Vector3 movement = Vector3.zero;
    private bool _groundedPlayer;
    private bool _isJumping;
    // Start is called before the first frame update
    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        FindPlayerPosition();
    }
    void FindPlayerPosition()
    {
        var playerPosition = _player.transform.position;
        var distance = Vector3.Distance(transform.position, playerPosition);
        if (distance <= _angerRange)
            Chase(playerPosition);
        else
            Wander();
        _cc.SimpleMove(movement);
        if ((movement.x != 0 || movement.y != 0) && OnSlope())
            _cc.Move(Vector3.down * _cc.height / 2 * _slopeForce * Time.deltaTime);
    }
    void Chase(Vector3 playerPosition)
    {
        transform.LookAt(playerPosition);
        var fwd = transform.forward.normalized;
        movement = fwd * _speed;
    }
    void Wander()
    {
        movement = Vector3.zero;
    }
    bool OnSlope()
    {
        if (_isJumping)
            return false;
        RaycastHit hit;
        if (Physics.Raycast(transform.position, Vector3.down, out hit, _cc.height / 2 * _slopeForceRayLength))
        {
            if (hit.normal != Vector3.up)
            {
                return true;
            }
        }
        return false;
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawLine(transform.position, transform.position + Vector3.forward * 10);
        Gizmos.color = Color.yellow;
        Gizmos.DrawSphere(transform.position, _angerRange);
    }
}
