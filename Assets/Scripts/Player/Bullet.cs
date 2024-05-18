using System.Collections;
using UnityEngine;



public class Bullet : MonoBehaviour
{
    #region <<<< Serialize Fields >>>>

    [SerializeField] internal GunController gunController;

    #endregion <<<< XXX >>>>



    #region <<<< Private Fields >>>>

    private Coroutine _returnToPoolCoroutine;

    #endregion <<<< XXX >>>>



    #region <<<< Properties >>>>

    internal bool _attack = false;

    #endregion <<<< XXX >>>>





    private void OnEnable()
    {
        if (this._attack)
            StartCoroutine(ReturnToPool());
    }


    private void FixedUpdate()
    {
        if (this.gameObject.activeSelf)
            BulletForward();
    }



    /// <summary>
    /// Bu fonksiyon ile birlikte kurþunlarýnýzýn daime ileri gitmesini saðlarsýnýz!
    /// </summary>
    private void BulletForward()
    {
        this.transform.Translate(Vector3.forward * this.gunController._bulletSpeed * this.gunController._bulletSpeedMultiply * Time.deltaTime);
    }



    /// <summary>
    /// Bu fonksiyon ile birlikte bu kursunun belirli bir sure sonra obje havuzuna geri donmesini saglayabilirsiniz!
    /// </summary>
    /// <returns></returns>
    private IEnumerator ReturnToPool()
    {
        yield return new WaitForSeconds(this.gunController._returnToPoolDuration);
        this.gunController.AddPoolBullet(this);
    }
}