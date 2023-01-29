using UnityEngine;

public class AbsorbingBackground : MonoBehaviour
{
    public SpriteRenderer backgroundSprite;
    public SpriteRenderer playerSprite;
    private Vector3 backgroundPos;

    private int startingSortingOrder;
    private void Start()
    {
        backgroundSprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerSprite = other.GetComponent<SpriteRenderer>();
        startingSortingOrder = playerSprite.sortingOrder;

        // backgroundPos = new(other.transform.position.x, other.transform.position.y, transform.position.z);
        //other.gameObject.SetActive(false);
        playerSprite.sortingOrder = backgroundSprite.sortingOrder - 1;
        playerSprite.gameObject.SetActive(true);
        // backgroundSprite.transform.position = backgroundPos;


    }

    private void OnTriggerExit2D(Collider2D other)
    {
        if (!other.CompareTag("Player")) return;

        playerSprite.sortingOrder = startingSortingOrder;
    }
}
