using UnityEngine;



public class Rack : MonoBehaviour, IInteractable
{
    #region <<<< Serialize Fields >>>>



    #endregion <<<< XXX >>>>


    #region <<<< Private Fields >>>>

    private float _speed;

    #endregion <<<< XXX >>>>


    #region <<<< Properties >>>>

    internal RackManager _rackManager { get; set; }

    #endregion <<<< XXX >>>>






    private void Awake()
    {
        if (_rackManager == null)
            _rackManager = FindObjectOfType<RackManager>();
    }

    private void OnEnable()
    {
        float _deflection = Random.Range(1, this._rackManager._speedDeflection);

        this._speed = this._rackManager._asteroidSpeed * this._rackManager.asteroidSpeedMultiply * _deflection * Time.deltaTime;
        this._speed = Mathf.Abs(this._speed);
    }




    private void FixedUpdate()
    {
        Move();
    }



    /// <summary>
    /// Bu fonksiyon ile Asteroidin haraket etmesini saglayabilirsiniz!
    /// </summary>
    private void Move()
    {
        this.transform.Translate(-Vector3.forward * _speed);
    }

    public void Interaction()
    {
        
    }

    public void OnHit()
    {
        
    }
}