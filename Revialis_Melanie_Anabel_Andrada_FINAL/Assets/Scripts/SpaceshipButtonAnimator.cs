using DG.Tweening;
using TMPro;
using UnityEngine;

public class SpaceshipButtonAnimator : MonoBehaviour
{
    public RectTransform spaceshipButton; 
    public float duration = 3f;
    public float loopRadius = 100f; 
    public GameObject planetImage; 
    public TextMeshProUGUI landingText; 

    private void Start()
    {
      
        if (planetImage != null)
        {
            planetImage.SetActive(false);
        }

        if (landingText != null)
        {
            landingText.gameObject.SetActive(false); 
        }

      
        AnimateSpaceshipButton();
    }

    private void AnimateSpaceshipButton()
    {
      
        Vector2 startPosition = new Vector2(-Screen.width / 2f, -Screen.height / 2f);
        spaceshipButton.anchoredPosition = startPosition;

      
        Vector2 centerPosition = Vector2.zero;

        if (planetImage != null)
        {
            planetImage.SetActive(true);
        }

        Sequence spaceshipSequence = DOTween.Sequence();
        spaceshipSequence.Append(spaceshipButton.DOAnchorPos(new Vector2(startPosition.x + loopRadius, startPosition.y + loopRadius), duration / 3).SetEase(Ease.InOutQuad)) 
                         .Append(spaceshipButton.DOAnchorPos(new Vector2(centerPosition.x - loopRadius, centerPosition.y + loopRadius), duration / 3).SetEase(Ease.InOutQuad)) 
                         .Append(spaceshipButton.DOAnchorPos(centerPosition, duration / 3).SetEase(Ease.InOutQuad)) 
                         .OnComplete(OnAnimationComplete);
    }

    private void OnAnimationComplete()
    {
        if (landingText != null)
        {
            landingText.gameObject.SetActive(true);
        }

        Debug.Log("Animation Complete");
    }
}
