using UnityEngine;
using DG.Tweening;
using UnityEditor;
using TMPro;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


internal class ScoreCalculator : MonoBehaviour
{
    #region <<<< Serialize Fields >>>>

    [Header("UI Score")]
    [SerializeField] private Transform scoreBarParrent;
    [SerializeField] private Transform scoreBar;
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private Image scoreIncreaseImage;
    [SerializeField] private float scoreBarIncreaseAmount;

    [Header("3d Score")]

    [SerializeField] private TextMeshPro interactionScoreText;
    [SerializeField] private Vector2 distance;

    #endregion <<<< XXX >>>>



    #region <<<< Private Fields >>>>

    Tween _shakeAnim, _textPunchAnim, _textRotateShakeAnim, _scoreIncreaseImageMoveAnim, _imageColorAnim;
    #endregion <<<< XXX >>>>




    private float currentScoreAmount;




    private void Awake()
    {
        currentScoreAmount = 0;
        StartCoroutine(InteractionScoreText(Vector3.zero, 2, 1));
    }

    private void Update()
    {
    }


    public void IncreaseScore(Vector3 position, float damageAmount, float duration)
    {
        StartCoroutine(InteractionScoreText(this.transform.position, 2f, 0.7f));
        ScoreIncrease();
    }



    /// <summary>
    /// Bu fonksiyon ile karakteriniz bir skor kazandýðýnda skor hangi konumdan kazanýldýysa bu text o konuma gider ve kazanýlan skoru gösterir.
    /// </summary>
    /// <param name="position">Skor textinin gözükmesini istediðiniz pozisyonu giriniz!</param>
    /// <param name="damageAmount">Verilen hasarý veya kazanýlan skoru giriniz</param>
    /// <param name="duration">Efektin ne kadar süre sahnede kalmasý gerektiðini giriniz!</param>
    public IEnumerator InteractionScoreText(Vector3 position, float damageAmount, float duration)
    {
        // Variables
        Vector3 _textPosition;
        Sequence _interactionScoreTextAnimation;
        
        this.interactionScoreText.gameObject.SetActive(true); 
        _interactionScoreTextAnimation = DOTween.Sequence();
        _textPosition = position;
        _textPosition.x += this.distance.x;
        _textPosition.y += this.distance.y;
        _textPosition.z = 0;

        this.interactionScoreText.color = Color.white;
        this.interactionScoreText.transform.position = _textPosition;
        this.interactionScoreText.text = $"+{damageAmount.ToString()}x";

        yield return new WaitForSeconds(duration);
        Debug.Log("Deneme");

        _interactionScoreTextAnimation.Append(this.interactionScoreText.transform.DOMoveY(_textPosition.y + 5, 0.75f));
        _interactionScoreTextAnimation.Join(this.interactionScoreText.transform.DOPunchScale(-Vector3.one, 1f));
        _interactionScoreTextAnimation.Join(this.interactionScoreText.DOColor(new Color(1, 1, 1, 0), 1f));
        _interactionScoreTextAnimation.OnKill(() =>
        {
            this.interactionScoreText.gameObject.SetActive(false);
            this.interactionScoreText.transform.localScale = Vector3.one;
        });
    }





    public void ScoreIncrease()
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