using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;



public class GunController : MonoBehaviour
{
    #region <<<< Serialize Fields >>>>

    [Header("Gun Properties")]

    [SerializeField] private float fireDuration = 0.3f;
    [SerializeField] private bool flip = false;


    [Header("Bullet Properties")]

    [SerializeField] private GameObject bulletPrefab;
    [Range(0, 10)]
    [SerializeField] private float bulletSpeed = 5.0f;


    [Header("Guns")]

    [SerializeField] private Transform[] Guns;


    [Header("Object Pool Properties")]

    [SerializeField] private int poolSize = 300;
    [SerializeField] private float returnToPoolDuration = 10;


    [Header("Classes")]

    [SerializeField] private GameManager gameManager;
    #endregion <<<< XXX >>>>




    #region <<<< Private Fields >>>>

    private byte _level = 1;
    private bool _isFire = true;

    private GameObject _objectPoolParentObject;

    #endregion <<<< XXX >>>>




    #region <<<< Properties >>>>

    internal float _bulletSpeed => this.bulletSpeed;
    internal float _returnToPoolDuration => this.returnToPoolDuration;
    internal float _bulletSpeedMultiply { get; } = 30;
    internal ObjectPooling<Bullet> ObjectPooling { get; set; }
    internal GameManager _gameManager { get => this.gameManager; set => this.gameManager = value; }

    #endregion <<<< XXX >>>>





    private void Awake()
    {
#if UNITY_EDITOR
        Profiler.BeginSample("Create Object Pool");
#endif
        this.ObjectPooling = new ObjectPooling<Bullet>(this.bulletPrefab, this.poolSize);
#if UNITY_EDITOR
        Profiler.EndSample();
#endif
    }

    private void OnEnable()
    {
        this._isFire = true;
    }


    private void OnDisable()
    {
        this._isFire = false;
    }



    /// <summary>
    /// Bu fonksiyon ile birlikte ates edebilirsiniz!
    /// </summary>
    public void Fire()
    {
        if (!this._isFire) return;
        // Variables
        Bullet _fireBullet;
        foreach(Transform _gunTransform in this.Guns)
        {
            _fireBullet = this.ObjectPooling.Get(_fireBullet =>
            {
                _fireBullet._attack = true;
                if (this.flip && this.transform.rotation.y != 180)
                    _fireBullet.transform.rotation = Quaternion.Euler(0, 180, 0);
                _fireBullet.transform.SetParent(null);
                _fireBullet.gameObject.SetActive(true);
            });
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