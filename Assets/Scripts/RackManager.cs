using UnityEngine;
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


    

    #endregion <<<< XXX >>>>




    #region <<<< Private Fields >>>>

    private bool _isAttack = true;

    #endregion <<<< XXX >>>>





    #region <<<< Properties >>>>

    internal float GetRackSpeed => this.rackSpeed;
    internal const float rackSpeedMultiply = 75;
    

    #endregion <<<< XXX >>>>









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
}