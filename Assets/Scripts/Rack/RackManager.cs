using UnityEngine;
using System.Collections;
using System.Collections.Generic;


public class RackManager : MonoBehaviour
{
    #region <<<< Serialize Fields >>>>

    [Header("Spawner")]

    [SerializeField] private Vector2 randomSpawnPoint;
    [SerializeField] private float spawnerZAxis;

    [Header("Spawner Properties")]

    [SerializeField] private float spawnDurationSpeed = 3f;
    [SerializeField] private float spawnDurationDecrease = 0.001f;
    [SerializeField] private byte maxRackValue = 5;


    [Header("Rack Properties")]

    [SerializeField] private GameObject[] RackObjects;
    [Range(0, 5), SerializeField] private float rackSpeed = 2.5f;


    [Header("Rack Object Pool")]

    [SerializeField] private byte maxObjectValue;


    #endregion <<<< XXX >>>>




    #region <<<< Private Fields >>>>

    private bool _isAttack = true;
    private Queue<Rack> _racks = new Queue<Rack>();
    private GameObject _racksParentObj;

    #endregion <<<< XXX >>>>





    #region <<<< Properties >>>>

    internal float GetRackSpeed => this.rackSpeed;
    internal const float rackSpeedMultiply = 75;


    #endregion <<<< XXX >>>>












    private void Awake()
    {
        this._racksParentObj = new GameObject("RackParentObject");
        this.CreateRacketObjectPool();
    }




    private void RackFire()
    {

    }





    /// <summary>
    /// Bu fonksiyon ile birlikte meteoor yagmurunun kac saniyede bir olacagini kontrol edebilirsiniz!
    /// </summary>
    /// <returns></returns>
    private IEnumerator IsAttackControl()
    {
        this._isAttack = false;
        yield return new WaitForSeconds(this.spawnDurationSpeed);
        this._isAttack = true;
    }








    #region <<<< Object Pool >>>>

    /// <summary>
    /// Bu fonksiyon ile meteorlar icin bir object pool olusturabilirsiniz!
    /// </summary>
    private void CreateRacketObjectPool()
    {
        // ~~ Variables ~~
        GameObject _createObject;
        Rack _createRack;

        for (int i = 0; i < maxObjectValue; i++)
        {
            _createObject = CreateRackObject();
            _createObject.TryGetComponent(out _createRack);
            
            if (_createRack == null)
            {
                Debug.LogError("<color=red>Error</color> Meteor objesi icersiinde <i>`Rack`</i> sinifi bulunamadi", _createObject);
            }
            else if (_createRack._rackManager == null)
                _createRack._rackManager = this;

            AddToPool(_createRack);
        }
    }


    /// <summary>
    /// Bu fonksiyon ile birlikte object pool icersiine bir raket ekleyebilirsiniz!
    /// </summary>
    /// <param name="rack"></param>
    private void AddToPool(Rack rack)
    {
        this._racks.Enqueue(rack);
        rack.gameObject.SetActive(false);
        rack.transform.SetParent(this._racksParentObj.transform);
    }

    /// <summary>
    /// Bu fonksiyon ile birlikte object pool icerisinden bir meteor cekebilirsiniz!
    /// </summary>
    /// <returns>Object pool icerisinde ilk sirada olan obje geri donderilecektir!</returns>
    private Rack GetRack()
    {
        // ~~ Variables ~~
        Rack _rack;

        _rack = this._racks.Dequeue();
        _rack.gameObject.SetActive(true);
        _rack.transform.SetParent(null);
        return _rack;
    }

    /// <summary>
    /// Bu fonksiyon ile bir meteor objesi olusturabilirsiniz!
    /// </summary>
    /// <returns>Olusturulan meteor geri donderilecektir!</returns>
    private GameObject CreateRackObject()
    {
        // ~~ Variables ~~
        byte _randomMeteorIndex;

        _randomMeteorIndex = (byte)Random.Range(0, this.RackObjects.Length);

        return Instantiate(this.RackObjects[_randomMeteorIndex]);
    }


    #endregion <<<< XXX >>>>
}