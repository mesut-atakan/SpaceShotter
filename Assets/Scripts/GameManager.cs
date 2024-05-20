using UnityEngine;



public class GameManager : MonoBehaviour
{
    #region <<<< Serialize Fields >>>>

    [Header("Classes")]

    [SerializeField] private GunController gunController;
    [SerializeField] private ScoreCalculator scoreCalculator;

    #endregion <<<< XXX >>>>






    #region <<<< Properties >>>>

    internal GunController _gunController { get => this.gunController; set => this.gunController = value; }
    internal ScoreCalculator _scoreCalculator { get => this.scoreCalculator; set => this.scoreCalculator = value; }
    #endregion <<<< XXX >>>>
}