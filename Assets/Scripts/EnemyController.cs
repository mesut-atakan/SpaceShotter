using UnityEngine;



public class EnemyController : MonoBehaviour
{
    #region <<<< Serialize Fields >>>>
    [Header("Character Properties")]

    [SerializeField] private float targetSmooth = 2f;
    [SerializeField] private Transform target;

    #endregion <<<< XXX >>>>




    #region <<<< Private Fields >>>>


    #endregion <<<< XXX >>>>



    private void FixedUpdate()
    {
        Move();
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
}