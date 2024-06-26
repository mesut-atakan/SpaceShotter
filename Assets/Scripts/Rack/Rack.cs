using UnityEngine;



public class Rack : MonoBehaviour, IInteraction
{
    #region <<<< Serialize Fields >>>>



    #endregion <<<< XXX >>>>


    #region <<<< Private Fields >>>>

    private float _speed;

    #endregion <<<< XXX >>>>


    #region <<<< Properties >>>>

    internal RackManager _rackManager { get; set; }
    internal bool _isActive { get; set; }

    #endregion <<<< XXX >>>>






    private void Awake()
    {
        if (_rackManager == null)
            _rackManager = FindObjectOfType<RackManager>();
    }

    private void OnEnable()
    {
        if (this._isActive)
        {
            StartCoroutine(this._rackManager.ObjectPooling.ReturnToPool(this, this._rackManager.GetReturnToPoolDuration));

            float _deflection = Random.Range(1, this._rackManager._speedDeflection);

            this._speed = this._rackManager._asteroidSpeed * this._rackManager.asteroidSpeedMultiply * _deflection * Time.deltaTime;
            this._speed = Mathf.Abs(this._speed);

        }
    }


    private void OnDisable()
    {
        this._isActive = false;
    }




    private void FixedUpdate()
    {
        Move();
    }


    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteraction>(out IInteraction _interaction))
            _interaction.TakeDamage();
    }



    /// <summary>
    /// Bu fonksiyon ile Asteroidin haraket etmesini saglayabilirsiniz!
    /// </summary>
    private void Move()
    {
        this.transform.Translate(-Vector3.forward * _speed);
    }

    public void Damage(Collider other)
    {
    }

    public void TakeDamage()
    {
        this._rackManager.ObjectPooling.ReturnToPool(this);
        this._rackManager.RemoveActiveRack(this);
        // Debug.Log("Return Pool <b>Rack</b>");
    }
}