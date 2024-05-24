using DG.Tweening;
using UnityEngine;


[RequireComponent (typeof (Rigidbody))]
internal class PlayerController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;
    [SerializeField] private Rigidbody rb;
    [SerializeField] private GunController gunController;
    [SerializeField] private Vector2 moveClamp_XAxis;
    [SerializeField] private Vector2 moveClamp_YAxis;

    [Space(15f)]

    [SerializeField] private float moveAxis;





    private const float _moveSpeedMultiply = 75;

    private void Awake()
    {
        if (this.rb == null)
            this.rb = GetComponent<Rigidbody>();
        if (this.gunController == null)
            this.gunController = GetComponent<GunController>();
    }


    private void FixedUpdate()
    {
        Movement();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) || Input.GetKeyDown(KeyCode.Space))
            this.gunController.Fire();
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(this.moveClamp_XAxis.x, 0, -100), new Vector3(this.moveClamp_XAxis.x, 0, 100));
        Gizmos.DrawLine(new Vector3(this.moveClamp_XAxis.y, 0, -100), new Vector3(this.moveClamp_XAxis.y, 0, 100));
        
        Gizmos.DrawLine(new Vector3(-100, 0, this.moveClamp_YAxis.x), new Vector3(100, 0, this.moveClamp_YAxis.x));
        Gizmos.DrawLine(new Vector3(-100, 0, this.moveClamp_YAxis.y), new Vector3(100, 0, this.moveClamp_YAxis.y));
    }




    private void Movement()
    {
        float _horizontal, _vertical;
        float _xAxisSpeed, _yAxisSpeed;
        _horizontal = Input.GetAxis("Horizontal") * 0.5f;
        _vertical = Input.GetAxis("Vertical") * 0.5f;

        if ((_horizontal > 0 && this.transform.position.x < this.moveClamp_XAxis.y) || _horizontal < 0 && this.transform.position.x > this.moveClamp_XAxis.x)
            _xAxisSpeed = _horizontal * moveSpeed * Time.deltaTime * _moveSpeedMultiply;
        else _xAxisSpeed = 0;

        if ((_vertical > 0 && this.transform.position.z < this.moveClamp_YAxis.x) || (_vertical < 0 && this.transform.position.z > this.moveClamp_YAxis.y))
            _yAxisSpeed = _vertical * moveSpeed * Time.deltaTime * _moveSpeedMultiply;
        else _yAxisSpeed = 0;

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