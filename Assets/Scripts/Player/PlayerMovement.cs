using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.InputSystem.XR;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private float _slopeForce;
    [SerializeField] private float _slopeForceRayLength;
    [SerializeField] private float _speed = 15f;
    [SerializeField] private float _sensitivity = 5f;
    [SerializeField] private float _gravityForce = 9.8f;
    [SerializeField] private AnimationCurve jumpFallOff;
    [SerializeField] private float jumpMultiplier;
    private CharacterController _cc;
    private Transform _cameraTransform;
    private float _playerYVelocity;
    private bool _groundedPlayer;
    private bool _isJumping;
    // Start is called before the first frame update
    void Awake()
    {
        _cc = GetComponent<CharacterController>();
        _cameraTransform = GameObject.Find("PlayerCamera").transform;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        ApplyRotation();
        ApplyInput();
    }
    private void FixedUpdate()
    {
    }
    #region Mouse
    void ApplyRotation()
    {
        var mouseX = Input.GetAxisRaw("Mouse X") * _sensitivity * Time.deltaTime;
        var mouseY = -1 * Input.GetAxisRaw("Mouse Y") * _sensitivity * Time.deltaTime;
        mouseY = math.clamp(mouseY, -90f, 90f);
        transform.Rotate(Vector3.up * mouseX);
        _cameraTransform.Rotate(Vector3.right * mouseY);
    }
    #endregion
    #region Input
    void ApplyInput()
    {
        float xInput = Input.GetAxis("Horizontal");
        float yInput = Input.GetAxis("Vertical");
        var dirForward = new Vector3(_cameraTransform.forward.x, 0, _cameraTransform.forward.z).normalized;
        var dirRight = new Vector3(_cameraTransform.right.x, 0, _cameraTransform.right.z).normalized;

        _cc.SimpleMove(Vector3.ClampMagnitude(xInput * dirRight + yInput * dirForward, 1.0f) * _speed);
        if ((xInput != 0 || yInput != 0) && OnSlope())
            _cc.Move(Vector3.down * _cc.height / 2 * _slopeForce * Time.deltaTime);

        ApplyJump();
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
    void ApplyJump()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !_isJumping)
        {
            _isJumping = true;
            StartCoroutine(JumpEvent());
        }
    }
    private IEnumerator JumpEvent()
    {
        _cc.slopeLimit = 90.0f;
        float timeInAir = 0.0f;
        do
        {
            float xInput = Input.GetAxis("Horizontal");
            float yInput = Input.GetAxis("Vertical");
            var dirForward = new Vector3(_cameraTransform.forward.x, 0, _cameraTransform.forward.z).normalized;
            var dirRight = new Vector3(_cameraTransform.right.x, 0, _cameraTransform.right.z).normalized;
            var movementVec = Vector3.ClampMagnitude(xInput * dirRight + yInput * dirForward, 1.0f) * _speed * Time.deltaTime;
            float jumpForce = jumpFallOff.Evaluate(timeInAir);
            _cc.Move(movementVec + Vector3.up * jumpForce * jumpMultiplier * Time.deltaTime);
            timeInAir += Time.deltaTime;
            yield return null;
        } while (!_cc.isGrounded && _cc.collisionFlags != CollisionFlags.Above);

        _cc.slopeLimit = 45.0f;
        _isJumping = false;
    }
    #endregion 
}
