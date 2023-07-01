using UnityEngine;
using UnityEngine.UI;

public class CanvasDraw : MonoBehaviour
{

    public Vector2 PointSize;
    public GameObject canvas; // Reference to the canvas
    private ScrollRect scrollRect;
    public float scrollSpeed = 100f; // Adjust the scrolling speed as per your preference

    public float contentWidth;
    public float pointPos;


    void Start()
    {
        scrollRect = canvas.GetComponent<ScrollRect>();
        contentWidth = scrollRect.content.rect.width;
    }

    void Update()
    {
        foreach (Transform child in canvas.transform)
        {
            // Check if the point is outside the canvas bounds
            if (child.localPosition.x < -contentWidth)
            {
                Destroy(child.gameObject);
            }
        }

        // Draw the player's position as a point on the canvas
        GameObject point = new();
        point.transform.SetParent(canvas.transform);
        RectTransform rectTransform = point.AddComponent<RectTransform>();
        rectTransform.anchoredPosition = Info.PlayerPosition; // Assuming the player.Position is a Vector2
        rectTransform.sizeDelta = PointSize; // Set the size of the point

        // You can also add an Image component to the point object to make it visible
        Image image = point.AddComponent<Image>();
        image.color = Color.red; // Set the color of the point

        // Scroll the canvas to the left
        scrollRect.content.localPosition += scrollSpeed * Time.deltaTime * Vector3.left;

        pointPos = scrollRect.content.localPosition.x;///////////////DELETE


        // Check if the content has reached the end
        if (scrollRect.content.localPosition.x <= -contentWidth)
        {
            // Reset the content position to loop it
            scrollRect.content.localPosition = Vector3.zero;
        }
    }
}
