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


    [Header("Object Pool Properties")]
    [SerializeField] private int poolSize = 50;
    #endregion <<<< XXX >>>>





    #region <<<< Private Fields >>>>

    private ObjectPooling<Rack> objectPooling;

    #endregion <<<< XXX >>>>





    #region <<<< Properties >>>>

    internal const float asteroidSpeedMultiply = 100;

    #endregion <<<< XXX >>>>







    private void Awake()
    {
        this.objectPooling = new ObjectPooling<Rack>(this.objectPrefab, this.poolSize);
    }


}