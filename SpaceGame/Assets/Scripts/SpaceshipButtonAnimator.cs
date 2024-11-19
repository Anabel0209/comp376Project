using DG.Tweening;
using TMPro;
using UnityEngine;

public class SpaceshipButtonAnimator : MonoBehaviour
{
    public RectTransform spaceshipButton; // Reference to the button's RectTransform
    public float duration = 3f; // Animation duration in seconds
    public float loopRadius = 100f; // Radius for the loop effect
    public GameObject planetImage; // The planet image to display when animation starts
    public TextMeshProUGUI landingText; // TextMeshPro object for "Click on spaceship to land"

    private void Start()
    {
        // Ensure initial states
        if (planetImage != null)
        {
            planetImage.SetActive(false); // Hide the planet initially
        }

        if (landingText != null)
        {
            landingText.gameObject.SetActive(false); // Hide the landing text initially
        }

        // Start the animation
        AnimateSpaceshipButton();
    }

    private void AnimateSpaceshipButton()
    {
        // Ensure spaceship starts from the bottom-left corner outside the canvas
        Vector2 startPosition = new Vector2(-Screen.width / 2f, -Screen.height / 2f);
        spaceshipButton.anchoredPosition = startPosition;

        // Target position at the center of the canvas
        Vector2 centerPosition = Vector2.zero;

        // Make the planet visible when the animation starts
        if (planetImage != null)
        {
            planetImage.SetActive(true);
        }

        // Sequence for creating the desired animation
        Sequence spaceshipSequence = DOTween.Sequence();
        spaceshipSequence.Append(spaceshipButton.DOAnchorPos(new Vector2(startPosition.x + loopRadius, startPosition.y + loopRadius), duration / 3).SetEase(Ease.InOutQuad)) // First loop motion
                         .Append(spaceshipButton.DOAnchorPos(new Vector2(centerPosition.x - loopRadius, centerPosition.y + loopRadius), duration / 3).SetEase(Ease.InOutQuad)) // Second loop motion
                         .Append(spaceshipButton.DOAnchorPos(centerPosition, duration / 3).SetEase(Ease.InOutQuad)) // Move to center
                         .OnComplete(OnAnimationComplete);
    }

    private void OnAnimationComplete()
    {
        // Show the landing text once the animation is complete
        if (landingText != null)
        {
            landingText.gameObject.SetActive(true);
        }

        Debug.Log("Animation Complete");
    }
}
