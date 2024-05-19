using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class GunController : MonoBehaviour
{
    #region <<<< Serialize Fields >>>>

    [Header("Gun Properties")]

    [SerializeField] private float fireDuration = 0.3f;


    [Header("Bullet Properties")]

    [SerializeField] private GameObject bulletPrefab;
    [Range(0, 10)]
    [SerializeField] private float bulletSpeed = 5.0f;


    [Header("Guns")]

    [SerializeField] private Transform[] Guns;


    [Header("Object Pool Properties")]

    [SerializeField] private int poolSize = 300;
    [SerializeField] private float returnToPoolDuration = 10;
    #endregion <<<< XXX >>>>




    #region <<<< Private Fields >>>>

    private byte _level = 1;
    private bool _isFire = true;

    private GameObject _objectPoolParentObject;

    #endregion <<<< XXX >>>>




    #region <<<< Properties >>>>

    internal float _bulletSpeed => this.bulletSpeed;
    internal float _returnToPoolDuration => this.returnToPoolDuration;
    internal float _bulletSpeedMultiply { get; } = 100;
    internal ObjectPooling<Bullet> ObjectPooling { get; set; }

    #endregion <<<< XXX >>>>





    private void Awake()
    {
        this.ObjectPooling = new ObjectPooling<Bullet>(this.bulletPrefab, this.poolSize);
    }

    private void OnEnable()
    {
        this._isFire = true;
    }


    private void OnDisable()
    {
        this._isFire = false;
    }





    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && this._isFire)
            Fire();
    }



    /// <summary>
    /// Bu fonksiyon ile birlikte ates edebilirsiniz!
    /// </summary>
    public void Fire()
    {
        Bullet _fireBullet;
        foreach(Transform _gunTransform in this.Guns)
        {
            _fireBullet = this.ObjectPooling.Get();
            _fireBullet.transform.position = _gunTransform.position;
        }
        StartCoroutine(IsFireControl());
    }



    /// <summary>
    /// Bu fonksiyon ile birlikte oyuncunun surekli olarak ates etmesini engelleyebilirsiniz!
    /// </summary>
    private IEnumerator IsFireControl()
    {
        this._isFire = false;
        yield return new WaitForSeconds(this.fireDuration);
        this._isFire = true;
    }
}