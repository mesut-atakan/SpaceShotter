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

    [SerializeField] private int poolObjectValue = 300;
    [SerializeField] private float returnToPoolDuration = 10;
    #endregion <<<< XXX >>>>




    #region <<<< Private Fields >>>>

    private byte _level = 1;
    private bool _isFire = true;

    private GameObject _objectPoolParentObject;
    private Queue<Bullet> _bulletPool = new Queue<Bullet>();

    #endregion <<<< XXX >>>>




    #region <<<< Properties >>>>

    internal float _bulletSpeed => this.bulletSpeed;
    internal float _returnToPoolDuration => this.returnToPoolDuration;
    internal float _bulletSpeedMultiply { get; } = 100;

    #endregion <<<< XXX >>>>





    private void Awake()
    {
        _objectPoolParentObject = new GameObject("ObjectPool");
        CreatePool();
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
            _fireBullet = GetBullet;
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







    #region <<<< Bullet Pool >>>>

    /// <summary>
    /// Bu fonksiyon ile birlikte bir object pool olusturabilirsiniz!
    /// </summary>
    private void CreatePool()
    {
        // ~~ Variables ~~
        GameObject _createObject;
        Bullet _createBullet;

        for (int i = 0; i < this.poolObjectValue; i++)
        {
            _createObject = Instantiate(this.bulletPrefab);
            _createObject.TryGetComponent(out _createBullet);
            AddPoolBullet(_createBullet);

            if (_createBullet.gunController == null)
                _createBullet.gunController = this;
        }
    }


    /// <summary>
    /// Bu fonksiyon ile Object pool icerisine bir bullet ekleyebilirsiniz!
    /// </summary>
    /// <param name="bullet">Eklemek istediginiz bullet objesini giriniz!</param>
    public void AddPoolBullet(Bullet bullet)
    {
        this._bulletPool.Enqueue(bullet);
        bullet.gameObject.SetActive(false);
        bullet.transform.SetParent(this._objectPoolParentObject.transform);
    }


    /// <summary>
    /// Bu fonksiyon ile object pool icerisndeki siradaki objeyi cekebilirsiniz!
    /// </summary>
    /// <returns>Object pool icerisinden cekilen obje geri donderilecek!</returns>
    private Bullet GetBullet
    {
        get
        {
            Bullet _getBullet;

            _getBullet = this._bulletPool.Dequeue();
            _getBullet.transform.SetParent(null);
            _getBullet.gameObject.SetActive(true);
            return _getBullet;
        }
    }

    #endregion <<<< XXX >>>>
}