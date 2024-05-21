using System.Collections;
using UnityEngine;



public class Bullet : MonoBehaviour, IInteraction
{
    #region <<<< Serialize Fields >>>>

    [SerializeField] internal GunController gunController;

    #endregion <<<< XXX >>>>



    #region <<<< Private Fields >>>>

    private Coroutine _returnToPoolCoroutine;

    #endregion <<<< XXX >>>>



    #region <<<< Properties >>>>

    internal bool _attack = false;
    internal bool _isPlayer = false;

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



    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<IInteraction>(out IInteraction _interaction))
        {
            if (_interaction is Bullet bullet && this._isPlayer != bullet._isPlayer)
            {
                _interaction.TakeDamage();
                
            }
            else if (!(_interaction as Bullet))
            {
                _interaction.TakeDamage();
                this.gunController._gameManager._scoreCalculator?.IncreaseScore(Camera.main.ScreenToViewportPoint(this.transform.position), 2, 1);
                this.gunController.ObjectPooling.ReturnToPool(this);
            }
        }
    }



    /// <summary>
    /// Bu fonksiyon ile birlikte kurþunlarýnýzýn daime ileri gitmesini saðlarsýnýz!
    /// </summary>
    private void BulletForward()
    {
        this.transform.Translate(Vector3.forward * this.gunController._bulletSpeed * this.gunController._bulletSpeedMultiply * Time.deltaTime);
    }




    // ~~ Interface ~~

    public void TakeDamage()
    {
        this.gunController.ObjectPooling.ReturnToPool(this);
        // Debug.Log("Return Pool <b>Bullet</b>");
    }

    public void Damage(Collider other)
    {
        
    }
}