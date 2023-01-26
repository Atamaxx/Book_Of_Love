using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private GameObject playerShadow;
    [SerializeField] private GameObject poiter;
    [SerializeField] private float distanceBetweenPlayers = 15f;

    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float xOffset = 1.0f;
    [SerializeField] private float yOffset = 1.0f;
    [SerializeField] private int numberOfIterationsForColliderSearch;
    
    //Vector2 newPlayerPosition;
    private GameObject mainPlayer;
    private GameObject mainEnemy;

    void Start()
    {
        mainPlayer = GameObject.FindWithTag("Player");
        mainEnemy = GameObject.FindWithTag("Main Enemy");
    }

    void FixedUpdate()
    {
        CreateShadowOfThePlayer();
    }


    public void ChangePlayers()
    {
        Vector2 oldPlayerPosition = mainPlayer.transform.position;

        MoveMainPlayer(oldPlayerPosition);
        MoveMainEnemy(oldPlayerPosition);

    }
    public void MoveMainEnemy(Vector2 oldPlayerPosition)
    {
        Vector2 newPlayerPosition = new(oldPlayerPosition.x, oldPlayerPosition.y);
        mainEnemy.transform.position = FindPointOnCollider(newPlayerPosition);
        //CreateShadowOfThePlayer();
    }
    public void MoveMainPlayer(Vector2 oldPlayerPosition)
    {
        Vector2 newPlayerPosition = new(oldPlayerPosition.x - distanceBetweenPlayers, oldPlayerPosition.y);
        mainPlayer.transform.position = FindPointOnCollider(newPlayerPosition);

    }

    public void CreateShadowOfThePlayer()
    {

    }


    public Vector2 FindPointOnCollider(Vector2 originPoint)
    {
        float yLength = 50f;
        Vector2 direction = Vector2.down;

        RaycastHit2D hit = Physics2D.Raycast(originPoint, direction, yLength * 2, platformLayer);
        originPoint = new(originPoint.x, originPoint.y + yLength);
        Vector2 hitPoint = originPoint;


        if (hit.collider != null && hit.collider.CompareTag("Platforms"))
        {
            Debug.Log(hit.collider.name);
            hitPoint = hit.point;
            Debug.DrawLine(originPoint, hitPoint, Color.red, 20);

            return new(hitPoint.x + xOffset, hitPoint.y + yOffset);
        }

        for (int i = 0; i < numberOfIterationsForColliderSearch; i++)
        {
            if (hit.collider == null || !hit.collider.CompareTag("Platforms"))
            {
                originPoint = new(originPoint.x - i, originPoint.y);
                hit = Physics2D.Raycast(originPoint, direction, yLength * 2, platformLayer);
                hitPoint = hit.point;
                //Debug.DrawLine(originPoint, hitPoint, Color.yellow, 20);
            }
            else
            {
                return new(hitPoint.x + xOffset, hitPoint.y + yOffset);
            }
        }
        return new(hitPoint.x + xOffset, hitPoint.y + yOffset);
    }
}
