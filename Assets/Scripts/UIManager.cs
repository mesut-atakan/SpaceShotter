using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;



internal class UIManager : MonoBehaviour
{
    [SerializeField] private Image leftPanel;
    [SerializeField] private Image rightPanel;






    [SerializeField] private UnityEvent awakeEvent;
    [SerializeField] private byte screenPartition = 4;





    private Vector2 GetQuality => new Vector2(Screen.width, Screen.height);


    private void Awake()
    {
        this.awakeEvent.Invoke();
    }




    public void SetQuality()
    {
        // Vector2 _quality = GetQuality;
        // this.leftPanel.rectTransform.sizeDelta = new Vector2((_quality.x / 3), _quality.y);
        // this.rightPanel.rectTransform.sizeDelta = new Vector2((_quality.x / 3), _quality.y);
        // Variables
        Vector2 _quality;
        _quality = GetQuality;

        this.leftPanel.rectTransform.sizeDelta = new Vector2(_quality.x / screenPartition, _quality.y);
        this.rightPanel.rectTransform.sizeDelta = new Vector2(_quality.x / screenPartition, _quality.y);
    }
}