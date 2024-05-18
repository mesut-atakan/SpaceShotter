using DG.Tweening;
using UnityEngine;



internal class CameraController : MonoBehaviour
{
    [SerializeField] private Transform target;
    [SerializeField] private new Camera camera;
    [SerializeField, Range(0.01f, 5f)] private float targetSmothness = 1.2f;
    [SerializeField] private float targetDistance = 10.0f;







    private Vector3 _targetPosition;

    private void Awake()
    {
        if (this.target == null)
            this.target = GameObject.FindGameObjectWithTag("Player")?.transform;
        if (this.camera == null)
            this.camera = Camera.main;
    }



    private void Start()
    {
        _targetPosition = new Vector3(this.camera.transform.position.x - this.target.position.x, this.camera.transform.position.y - this.target.position.y, this.camera.transform.position.z - this.target.position.z);
    }


    private void LateUpdate()
    {
        Target();
    }


    /// <summary>
    /// Bu fonksiyon ile birlikte kameranýzýn target objesini takip etmesini saðlayabilirsiniz!
    /// </summary>
    private void Target()
    {
        Vector3 _targetPos;
        _targetPos = new Vector3(this.target.position.x, this.target.position.y, this.target.position.z + this.targetDistance);
        this.camera.transform.position = Vector3.Lerp(this.camera.transform.position, this._targetPosition, this.targetSmothness * Time.deltaTime);
    }
}