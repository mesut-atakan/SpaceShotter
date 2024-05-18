using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;


public class RackManager : MonoBehaviour
{
    #region <<<< Serialize Fields >>>>

    [Header("Spawner")]

    [SerializeField] private Vector2 randomSpawnPoint;
    [SerializeField] private float spawnerZAxis;

    [Header("Spawner Properties")]

    [SerializeField] private float spawnDurationSpeed = 3f;
    [SerializeField] private float spawnDurationDecrease = 0.001f;
    [SerializeField] private byte activeMaxRackValue = 5;


    [Header("Rack Properties")]

    [SerializeField] private GameObject[] RackObjects;
    [Range(0, 5), SerializeField] private float rackSpeed = 2.5f;


    [Header("Rack Object Pool")]

    [SerializeField] private byte maxObjectValue;


    #endregion <<<< XXX >>>>




    #region <<<< Private Fields >>>>

    private bool _isAttack = true;
    private const float _minDistance = 1.0f;
    private Queue<Rack> _racks = new Queue<Rack>();
    private GameObject _racksParentObj;
    private Rack[] _activeRacks;
    #endregion <<<< XXX >>>>





    #region <<<< Properties >>>>

    internal float GetRackSpeed => this.rackSpeed;
    internal const float rackSpeedMultiply = 75;


    #endregion <<<< XXX >>>>












    private void Awake()
    {
        this._activeRacks = new Rack[this.activeMaxRackValue];
        this._racksParentObj = new GameObject("RackParentObject");
        this.CreateRacketObjectPool();
    }



    private void Start()
    {
        this._isAttack = true;
    }




    private void FixedUpdate()
    {
        RackFire();
    }





    /// <summary>
    /// Bu fonksiyon ile birlikte asteroidinizin hangi pozisyonda spawn olacagini girebilirsiniz!
    /// </summary>
    private void RackFire()
    {
        if (this._isAttack)
        {
            // ~~ Varaiables ~~
            Rack _getRack;

            _getRack = GetRack();
            _getRack.transform.position = GetRackSpawnPosition();
            StartCoroutine(IsAttackControl());
        }
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





    /// <summary>
    /// Bu fonksiyon ile birlikte asteroildinizin spawnlanacagi noktayi cekebilirsiniz!
    /// Bir cok bugun onune gecmeye calisan moduler bir fonksiyon yazilmistir!
    /// Kullanmaniz tavsiye edilir!
    /// PERFORMANSI AZALTABILIR!!
    /// </summary>
    /// <returns></returns>
    private Vector3 GetRackSpawnPosition()
    {
        // ~~ Variables ~~
        Vector3 _spawnPos;
        Vector2 _randomSpawnPoint;
        bool _xAxisOK = false;
        ushort _trail = 0;

        _randomSpawnPoint = this.randomSpawnPoint;
        _spawnPos = Vector3.zero;
        _spawnPos.y = 0;
        _spawnPos.z = this.spawnerZAxis;

        do
        {
            if (_trail >= 100)
            {
                Debug.LogError($"<color=red><b>WARNING</b></color> Denem sayisi 100'u gecti!", this.gameObject);
                return Vector3.zero;
            }
            _trail++;
            _spawnPos.x = Random.Range(_randomSpawnPoint.x, _randomSpawnPoint.y);

            _xAxisOK = false;
            foreach (Rack _activeRack in this._activeRacks)
            {
                _xAxisOK = Vector3.Distance(_activeRack.transform.position, _spawnPos) > _minDistance;
                if (_xAxisOK == true)
                {
                    return _spawnPos;
                }
            }

            if (!_xAxisOK && _trail >= 5)
            {
                _randomSpawnPoint.x++;
                _randomSpawnPoint.y++;
            }

        }
        while (_xAxisOK);

        return Vector3.zero;
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
        RemoveActiveRack(rack);
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
        AddToActiveRack(_rack);
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






    #region <<<< Active Rack Array >>>>

    /// <summary>
    /// Bu fonksiyon ile ActiveRack dizisi icerisinde aradiginiz elemani bulabilirsiniz!
    /// </summary>
    /// <param name="_rack">Aradiginiz elemanin rack sinifini giriniz!</param>
    /// <returns>Aradiginiz rack sinifi bulunursa rack sinifi geri donderilir eger bulunamazsa bu fonksiyon geriye null degerini donderir!</returns>
    private Rack GetActiveRack(Rack _rack)
    {
        foreach(Rack _activeRacks in this._activeRacks)
        {
            // Active Rack dizisi icersiindeki tum elemanlarin parametre olarak girilen rack sinifi ile uyumlu olup olmadigi kontrol ediliyor!
            if (_activeRacks != null && _rack == _activeRacks)
            {
                // Eger Parametre olarak girilen rack sinifi dizi icerisindeki bir rack sinifi ile uyusuyorsa dizi icerisindeki rack sinifi geri donderiliyor!
                return _activeRacks;
            }
        }
        Debug.LogError("<color=red>Error!</color> Aranan Rack sinifi bulunamadi!", this.gameObject);
        return null; // Eger istenilen eleman bulunamzsa bu fonksiyon geriye null degerini donderiyor!
    }


    /// <summary>
    /// Bu fonksiyon ile birlikte aradiginiz rack sinifinin index numarasini girerseniz aradiginiz sinifi dizi icerisinden bulabilirsiniz!
    /// </summary>
    /// <param name="index">Aradiginiz rack sinifinin index numarasini giriniz</param>
    /// <returns>index numarasi ile eslesen rack sinifi geri donderilecek!</returns>
    private Rack GetActiveRack(int index)
    {
        if (index > this._activeRacks.Length)
        {
            Debug.LogError("parametre olarak girilen index numarasi dizinin uzunlugundan fazla oldugu icin hata olustu", this.gameObject);
            return null;
        }
        else if (this._activeRacks[index] == null)
        {
            Debug.Log("<color=yellow>Warning</color> Aranan obje bulundu ancak <b>NULL</b>", this.gameObject);
        }
        return this._activeRacks[index];
    }



    /// <summary>
    /// Bu fonksiyon ile birlikte Active Racket dizisi icersinde aradiginiz bir raketin index numarasini cekebilirsiniz!
    /// </summary>
    /// <param name="_rack">Aradiginiz raketin index numarasini giriniz!</param>
    /// <returns>Aradiginiz raket bulunursa index nuamrasi donderilir eger bulunamazsa `-1` degeri geri donderilir!</returns>
    private int IndexOfGetActiveRack(Rack _rack)
    {
        for (int i = 0; i < this._activeRacks.Length; i++)
        {
            if (this._activeRacks[i] == _rack)
            {
                return i;
            }
        }

        Debug.LogError("<color=red>Aranan Active Rack bulunamadi</color>", this.gameObject);
        return -1;
    }



    private void RemoveActiveRack(Rack rack)
    {
        this._activeRacks[IndexOfGetActiveRack(rack)] = null;
    }


    private bool AddToActiveRack(Rack rack)
    {
        for (int i = 0; i < this._activeRacks.Length; i++)
        {
            if (this._activeRacks[i] == null)
            {
                this._activeRacks[i] = rack;
                return true;
            }
        }
        Debug.LogError("<color=red>Error</color> Active Rack dizisi icersiinde istediginiz eleman eklenemedi!", this.gameObject);
        return false;
    }
    #endregion <<<< XXX >>>>
}