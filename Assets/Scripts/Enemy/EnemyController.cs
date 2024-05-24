using DG.Tweening;
using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;



public class EnemyController : MonoBehaviour, IInteraction
{
    #region <<<< Serialize Fields >>>>
    [Header("Character Properties")]

    [SerializeField] private float targetSmooth = 2f;
    [SerializeField] private Transform target;


    [Header("Doch Mechanic")]

    [SerializeField] private bool dochMechanic = true;
    [SerializeField] private Vector3 centerPosition;
    [SerializeField] private Vector3 trigerScale;
    [SerializeField] private float dochIsAbleDuration = 3.5f;
    [SerializeField] private float dochDistance = 12f;
    [SerializeField] private float dochDuration = 0.3f;


    [Header("Classes")]

    [SerializeField] private GunController gunController;

    #endregion <<<< XXX >>>>




    #region <<<< Private Fields >>>>

    private bool isAbleDooch = true;
    private bool isCurrentDoch = false;
    #endregion <<<< XXX >>>>



    #region <<<< Properties >>>>

    private Vector3 GetCenterPosiiton => new Vector3(this.transform.position.x + centerPosition.x, this.transform.position.y, this.transform.position.z + this.centerPosition.z);

    private bool GetIsAttack => (this.transform.position.x - target.transform.position.x) < 5f;

    #endregion <<<< XXX >>>>


    private void FixedUpdate()
    {
        Move();
        DochMechanic();
        Attack();
    }



    private void OnDrawGizmos()
    {
        if (this.dochMechanic)
        {
            Gizmos.color = Color.blue;
            Gizmos.DrawWireCube(GetCenterPosiiton, trigerScale);
        }
    }


    public void Move()
    {
        if (this.isCurrentDoch == true) return;
        // Variables
        Vector3 _startPos, _targetPos;

        _startPos = this.transform.position;
        _targetPos = this.transform.position;
        _targetPos.x = target.position.x;
        this.transform.position = Vector3.Lerp(_startPos, _targetPos, this.targetSmooth * Time.deltaTime);
    }

    public void Move(float xPos, float speed = 0)
    {
        // Variables
        Vector3 _startPos;
        Vector3 _endPos;


            if (speed == 0)
                speed = this.targetSmooth * Time.deltaTime;

            _startPos = this.transform.position;
            _endPos = this.transform.position;
            _endPos.x += xPos;

        this.transform.DOMove(_endPos, speed).OnComplete(() => this.isCurrentDoch = false);
    }





    /// <summary>
    /// Bu fonksiyon ile birlikte karakterin önünde bir nesne var mý yok mu kontrol edebilirsiniz!
    /// </summary>
    private bool DochMechanic()
    {
        // Variables
        RaycastHit _hit;

        if (dochMechanic && !isCurrentDoch && Physics.BoxCast(GetCenterPosiiton, trigerScale, Vector3.forward, out _hit))
        {
            if (_hit.collider.tag == "Bullet")
            {
                Debug.Log($"Temas {_hit.collider.name}", _hit.collider.gameObject);
                if (Random.Range(0,2) == 0)
                    Move(dochDistance, dochDuration);
                else
                    Move(-dochDistance, dochDuration);
                this.isCurrentDoch = true;
                StartCoroutine(IsDochControl());
                return true;
            }
        }
        return false;
    }


    private IEnumerator IsDochControl()
    {
        this.dochMechanic = false;
        yield return new WaitForSeconds(this.dochIsAbleDuration);
        this.dochMechanic = true;
    }


    /// <summary>
    /// Bu fonksiyon ile AI ateþ etmesini saglayabilirsiniz!
    /// </summary>
    private void Attack()
    {
        if (GetIsAttack && !this.isCurrentDoch)
        {
            this.gunController.Fire();
        }
    }

    public void Damage(Collider other)
    {
        if (other.CompareTag("Bullet"))
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }
    }

    public void TakeDamage()
    {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);

    }
}