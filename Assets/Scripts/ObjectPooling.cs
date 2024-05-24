using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Profiling;



public class ObjectPooling<T> where T : Component
{
    #region <<<< Private Fields >>>>

    private Queue<T> objects = new Queue<T>();
    private T objPool;
    private GameObject[] objectPrefab;



    #endregion <<<< XXX >>>>





    #region <<<< Properties >>>>

    internal Transform parentTransform { get; set; }


    private int GetRandomIndex => UnityEngine.Random.Range(0, this.objectPrefab.Length);

    #endregion <<<< XXX >>>>





    public ObjectPooling(GameObject objectPrefab, int initialSize, GameObject parentTransform = null)
    {
        this.objectPrefab = new GameObject[1];
        this.objectPrefab[0] = objectPrefab;
        CreatePool(initialSize, parentTransform);
    }
    


    public ObjectPooling(GameObject[] objectPrefabs, int initialSize, GameObject parentTransform = null)
    {
        // ~~ Variables ~~
        this.objectPrefab = objectPrefabs;

        CreatePool(initialSize, parentTransform);
    }




    /// <summary>
    /// Bu fonksiyon ile birlikte Object Pool icerisinden bir obje cekebilirsiniz
    /// Eger object pool icerisinde herhangi bir eleman yoksa yeni obje olusturulacaktir!
    /// Cekilen objenin setActive acilacak ve parenti null olacaktir!
    /// </summary>
    /// <returns>Object pool icerisinden cekilen obje geri donderilecektir!</returns>
    public T Get(Action<T> OnComplate = null)
    {
        Profiler.BeginSample("Object Pooling GET");
        // ~~ Variables ~~
        T _obj;
        GameObject _createObject;

        if (this.objects.Count > 0)
        {
            _obj = this.objects.Dequeue();
        }
        else
        {
            Debug.LogWarning($"<color=yellow>Warning</color> Objets dizisinde herhangi bir obje yok ancak sifirdan bir obje olusuturuluyor!");
            _createObject = GameObject.Instantiate(this.objectPrefab[UnityEngine.Random.Range(0, this.objectPrefab.Length)]);
            _createObject.TryGetComponent<T>(out _obj);

            if (_obj == null)
            {
                Debug.LogError($"<color=red>Error</color> Olusturulan objenin icerisinde <T> turunde bir component bulunamadi! {_createObject.name}", _createObject);
                return null;
            }
        }

        OnComplate?.Invoke(_obj);
        /*
        _obj.transform.SetParent(null);
        _obj.gameObject.SetActive(true);
        */
        Profiler.EndSample();
        return _obj;
    }



    /// <summary>
    /// Bu fonksiyon ile birlikte Object pool icerisine obje atabilirsiniz!
    /// </summary>
    /// <param name="obj">atmak istediginiz sinifi parametre olarak giriniz!</param>
    public void ReturnToPool(T obj)
    {
        obj.gameObject.SetActive(false);
        obj.transform.SetParent(this.parentTransform);
        this.objects.Enqueue(obj);
    }


    /// <summary>
    /// Bu fonksiyon ile birlikte belirlediginiz bir objeyi bir sure sonra pool'a ekleyebilirsiniz!
    /// </summary>
    /// <param name="obj">Eklemek istediginiz objeyi parametre olarak ekleyiniz!</param>
    /// <param name="duration">Ne kadar sure sonra eklemek istediginizi giriniz!</param>
    public IEnumerator ReturnToPool(T obj, float duration)
    {
        yield return new WaitForSeconds(duration);
        ReturnToPool(obj);
    }




    


    private void CreatePool(int initialSize, GameObject parentObject)
    {
        // ~~ Variables ~~
        GameObject _createObject;
        int _index;
        bool _isArray;
        T _obj;

        if (parentObject == null)
            this.parentTransform = new GameObject("ParentObjects").transform;

        _index = 0;
        if (this.objectPrefab.Length > 1) _isArray = true;
        else _isArray = false;

        for (int i = 0; i < initialSize; i++)
        {
            if (_isArray)
                _index = GetRandomIndex;

            _createObject = GameObject.Instantiate(this.objectPrefab[_index]);
            _createObject.TryGetComponent<T>(out _obj);

            if (_obj == null)
            {
                Debug.LogError($"<color=red>Error</color> Olusturulan objenin icerisinde <T> turunde bir component bulunamadi! {_createObject.name}", _createObject);
                return;
            }
            ReturnToPool(_obj);
        }
    }
}