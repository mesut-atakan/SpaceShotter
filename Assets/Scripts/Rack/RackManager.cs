using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class RackManager : MonoBehaviour
{
    #region <<<< Serialize Fields >>>>

    [Header("Spawner Properties")]

    [SerializeField] private GameObject[] objectPrefab;
    [SerializeField, Range(0, 10)] private float spawnDuration = 4;
    [SerializeField, Range(0, 5)] private float asteroidSpeed = 3;
    [SerializeField] private byte spawnMaxObject = 4;
    [SerializeField] private Vector2 spawnObjectClamp;
    [SerializeField] private float zAxis;


    [Header("Object Pool Properties")]
    [SerializeField] private int poolSize = 50;
    #endregion <<<< XXX >>>>





    #region <<<< Private Fields >>>>

    private ObjectPooling<Rack> objectPooling;
    private bool _isSpawner = true;
    [SerializeField] private List<Rack> _activeRacks = new List<Rack>();

    #endregion <<<< XXX >>>>





    #region <<<< Properties >>>>

    internal float asteroidSpeedMultiply { get; } = 40;
    internal float _speedDeflection { get; } = 3;
    internal float _asteroidSpeed => this.asteroidSpeed;

    #endregion <<<< XXX >>>>







    private void Awake()
    {
        this.objectPooling = new ObjectPooling<Rack>(this.objectPrefab, this.poolSize);
    }


    private void Start()
    {
        StartCoroutine(SpawnerCalculator());
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawLine(new Vector3(-100, 0, this.zAxis), new Vector3(100, 0, zAxis));
    }






    private IEnumerator SpawnerCalculator()
    {
        while(true)
        {
            yield return new WaitForSeconds(this.spawnDuration);
            this._activeRacks.Clear();
            Spawner();

        }
    }




    private void Spawner()
    {
        // ~~ Variables ~~
        Rack _createRack;

        for (int i = 0; i < this.spawnMaxObject; i++)
        {
            _createRack = this.objectPooling.Get();
            AddActiveRack(_createRack);
            _createRack.transform.position = GetPosition();
        }
    }



    /// <summary>
    /// Bu fonksiyon ile Active Rack listesi icerisine `Rack` sinifi ekleyebilirsiniz
    /// </summary>
    /// <param name="rack">Eklemek istediginiz rack sinifini parametre olarak giriniz!</param>
    private void AddActiveRack(Rack rack)
    {
        if (!this._activeRacks.Contains(rack))
        {
            this._activeRacks.Add(rack);
        }
    }












    
    /// <summary>
    /// Bu fonksiyon ile birlikte asteroid taslarinin birbirinden uzak bir sekilde olusmasini saglayabilirsiniz!
    /// </summary>
    /// <returns>Uygun pozisyon geri donderilecektir!</returns>
    private Vector3 GetPosition()
    {
        // ~~ Variables ~~
        Vector3 _pos;
        float _distance;
        bool _isPos = true;
        int _testValue = 0;
        int _maxTest = this.spawnMaxObject * 30;


        _pos = Vector3.zero;

        _pos.z = this.zAxis;
        do
        {
            _testValue++;
            _isPos = true;

            _pos.x = Random.Range(this.spawnObjectClamp.x, this.spawnObjectClamp.y);

            foreach (Rack _rack in this._activeRacks)
            {
                _distance = Mathf.Abs(_rack.transform.position.x - _pos.x);

                if (_distance < 4)
                {
                    _isPos = false;
                    break;
                }
            }
            if (_isPos)
                return _pos;
        } while (_testValue < _maxTest);

        Debug.LogError("<color=red>Error</color> Uygun Konum bulunamadi!", this.gameObject);
        return Vector3.zero;
    }
}