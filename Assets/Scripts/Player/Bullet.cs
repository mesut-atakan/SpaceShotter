using System.Collections;
using UnityEngine;



public class Bullet : MonoBehaviour, IInteractable
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



    /// <summary>
    /// Bu constructor ile yeni Bir Bullet Sinifi olusturabilirsiniz!
    /// </summary>
    /// <param name="gunController">Yeni bir bullet sinifi olusturmak icin `GunController` sinifini referans olarak verebilirsiniz!</param>
    public Bullet(GunController gunController)
    {
        this.gunController = gunController;
        this._attack = false;
    }





    private void OnEnable()
    {
        if (this.gunController == null)
            this.gunController = GameObject.FindGameObjectWithTag("GameController").GetComponent<GunController>() ?? FindObjectOfType<GunController>();
        if (this._attack)
            StartCoroutine(this.gunController.ObjectPooling.ReturnToPool(this, this.gunController._returnToPoolDuration));
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

    public void Interaction()
    {
        throw new System.NotImplementedException();
    }

    public void OnHit()
    {
        throw new System.NotImplementedException();
    }
}