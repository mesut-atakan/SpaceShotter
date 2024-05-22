using DG.Tweening;
using UnityEngine;



public class EnemyController : MonoBehaviour
{
    #region <<<< Serialize Fields >>>>
    [Header("Character Properties")]

    [SerializeField] private float targetSmooth = 2f;
    [SerializeField] private Transform target;


    [Header("Doch Mechanic")]

    [SerializeField] private bool dochMechanic = true;
    [SerializeField] private Vector3 centerPosition;
    [SerializeField] private Vector3 trigerScale;

    #endregion <<<< XXX >>>>




    #region <<<< Private Fields >>>>

    private bool isAbleDooch = true;
    private bool isCurrentDoch = false;
    #endregion <<<< XXX >>>>



    #region <<<< Properties >>>>

    private Vector3 GetCenterPosiiton => new Vector3(this.transform.position.x + centerPosition.x, this.transform.position.y, this.transform.position.z + this.centerPosition.z);

    #endregion <<<< XXX >>>>


    private void FixedUpdate()
    {
        // Move();
        DochMechanic();
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

        //this.transform.position = Vector3.Lerp(_startPos, _endPos, speed);
        this.transform.DOMove(_endPos, speed).OnComplete(() => this.isCurrentDoch = true);
    }





    /// <summary>
    /// Bu fonksiyon ile birlikte karakterin �n�nde bir nesne var m� yok mu kontrol edebilirsiniz!
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
                Move(10, 0.01f);
                this.isCurrentDoch = true;
                return true;
            }
        }
        return false;
    }
}