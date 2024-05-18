using DG.Tweening;
using UnityEngine;


[RequireComponent (typeof (Rigidbody))]
internal class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody rb;

    [Space(15f)]

    [SerializeField] private float moveAxis;





    private const float _moveSpeedMultiply = 75;

    private void Awake()
    {
        if (this.rb == null)
            this.rb = GetComponent<Rigidbody>();
    }


    private void FixedUpdate()
    {
        Movement();
    }




    private void Movement()
    {
        float _horizontal, _vertical;
        float _xAxisSpeed, _yAxisSpeed;
        _horizontal = Input.GetAxis("Horizontal") * 0.5f;
        _vertical = Input.GetAxis("Vertical") * 0.5f;

        _xAxisSpeed = _horizontal * moveSpeed * Time.deltaTime * _moveSpeedMultiply;
        _yAxisSpeed = _vertical * moveSpeed * Time.deltaTime * _moveSpeedMultiply;
        this.rb.velocity = new Vector3(_xAxisSpeed, 0, _yAxisSpeed);

        MoveRotate();
    }




    private void MoveRotate()
    {
        float _horizontalAxis, _verticalAxis, _horizontal, _vertical;
        _horizontalAxis = 0;
        _horizontal = Input.GetAxisRaw("Horizontal");
        _vertical = Input.GetAxisRaw("Vertical");

        if (_horizontal < 0)
            _horizontalAxis = this.moveAxis;
        else if (_horizontal > 0)
            _horizontalAxis = -(this.moveAxis);
        else
            _horizontalAxis = 0;

        if (_vertical > 0)
            _verticalAxis = this.moveAxis;
        else if (_vertical < 0)
            _verticalAxis = -(this.moveAxis);
        else
            _verticalAxis = 0;
            
        this.transform.transform.DOLocalRotate(new Vector3(_verticalAxis, 0, _horizontalAxis), 0.6f);
    }
}