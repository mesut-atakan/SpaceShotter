using UnityEngine;



public class Rack : MonoBehaviour
{
    #region <<<< Serialize Fields >>>>

    [Header("Classes")]

    [SerializeField] private RackManager rackManager;
    #endregion <<<< XXX >>>>



    #region <<<< Private Fields >>>>


    #endregion <<<< XXX >>>>


    #region <<<< Properties >>>>

    internal RackManager _rackManager { get => this.rackManager; set => this.rackManager = value; }

    #endregion <<<< XXX >>>>
}