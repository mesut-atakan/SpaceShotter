using UnityEngine;
using DG.Tweening;
using UnityEditor;
using TMPro;
using UnityEngine.UI;


internal class ScoreCalculator : MonoBehaviour
{
    [SerializeField] private Transform scoreBarParrent;
    [SerializeField] private Transform scoreBar;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image scoreIncreaseImage;
    [SerializeField] private float scoreBarIncreaseAmount;






    private float currentScoreAmount;




    private void Awake()
    {
        currentScoreAmount = 0;
        //ScoreIncrease();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
            ScoreIncrease();
    }




        Tween _shakeAnim, _textPunchAnim, _textRotateShakeAnim, _scoreIncreaseImageMoveAnim, _imageColorAnim;
    private void ScoreIncrease()
    {
        this.currentScoreAmount += this.scoreBarIncreaseAmount;
        this.scoreBar.transform.localScale = new Vector2(this.currentScoreAmount, this.scoreBar.localScale.y);
        _shakeAnim = this.scoreBarParrent.transform.DOShakeRotation(0.3f, 10, 12, 60).OnComplete(()
            => this.scoreBarParrent.DORotate(new Vector3(0,0,0), 0.08f));

        _textPunchAnim = this.scoreText.transform.DOPunchScale(new Vector3(0.2f, 0.2f, 0.2f), 0.2f).OnComplete(()
            => this.scoreText.transform.localScale = new Vector3(1, 1, 1));

        _textRotateShakeAnim = this.scoreText.transform.DOShakeRotation(0.2f, 8, 7, 40).OnComplete(() =>
            this.scoreText.transform.DORotate(new Vector3(0, 0, 0), 0.1f));


        _scoreIncreaseImageMoveAnim.Rewind();
        _imageColorAnim.Rewind();
        Color _imageColor = Color.white;
        _imageColor.a = 1;
        this.scoreIncreaseImage.color = _imageColor;
        scoreIncreaseImage.gameObject.SetActive(true);
        scoreIncreaseImage.transform.DOLocalMoveY(-150, 0);
        _scoreIncreaseImageMoveAnim = this.scoreIncreaseImage.transform.DOLocalMoveY(150, 0.4f).OnComplete(() => scoreIncreaseImage.transform.DOLocalMoveY(-150, 0.05f));
        _imageColorAnim = this.scoreIncreaseImage.DOColor(new Color(Color.white.r, Color.white.g, Color.white.b, 0), 0.3f).OnComplete(() => {
            this.scoreIncreaseImage.DOColor(new Color(Color.white.r, Color.white.g, Color.white.b, 1), 0.1f);
            this.scoreIncreaseImage.gameObject.SetActive(false);
        });
    }
}