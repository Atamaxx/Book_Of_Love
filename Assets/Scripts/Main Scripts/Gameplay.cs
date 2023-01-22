using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Gameplay : MonoBehaviour
{
    [SerializeField] private GameObject playerShadow;
    [SerializeField] private GameObject poiter;
    [SerializeField] private float distanceBetweenPlayers = 15f;
    [SerializeField] private PhysicsMaterial2D frictionMaterial;

    [SerializeField] private LayerMask platformLayer;
    [SerializeField] private float xOffset = 1.0f;
    [SerializeField] private float yOffset = 1.0f;
    [SerializeField] private int numberOfIterationsForColliderSearch;
    Vector2 newPlayerPosition;
    // Start is called before the first frame update
    void Start()
    {
       }

    // Update is called once per frame
    void FixedUpdate()
    {
        CreateShadowOfThePlayer();
    }

    public void CreateNewPlayer()
    {
        Vector2 oldPlayerPosition = GameObject.FindWithTag("Player").transform.position;

        newPlayerPosition = new(oldPlayerPosition.x - distanceBetweenPlayers, oldPlayerPosition.y);

        Vector2 spawnPoint = FindPointOnCollider(newPlayerPosition);

        spawnPoint = new(spawnPoint.x + xOffset, spawnPoint.y + yOffset);

        Instantiate(gameObject, spawnPoint, Quaternion.identity);
        Instantiate(poiter, spawnPoint, Quaternion.identity);

        //CreateShadowOfThePlayer();
    }


    public void CreateShadowOfThePlayer()
    {

    }

    


    public void MakePlayerEnemy()
    {
        GameObject lastPlayer;
        Component[] componentsToDisable;

        lastPlayer = GameObject.FindGameObjectWithTag("Player");

        lastPlayer.tag = "Main Enemy";
        lastPlayer.GetComponent<Rigidbody2D>().sharedMaterial = frictionMaterial;
        componentsToDisable = lastPlayer.GetComponents<MonoBehaviour>();

        foreach (Component component in componentsToDisable)
        {
            Destroy(component);
        }
        //Destroy(lastPlayer.GetComponent<Rigidbody2D>());

        //RaycastHit2D hitPlatforms = Physics2D.Raycast(transform.position, Vector2.down, 50 * 2, platformLayer);
        //transform.position = hitPlatforms.point; 
    }

    public Vector2 FindPointOnCollider(Vector2 originPoint)
    {
        float yLength = 50f;
        Vector2 direction = Vector2.down;

        RaycastHit2D hit = Physics2D.Raycast(originPoint, direction, yLength * 2, platformLayer);
        originPoint = new(originPoint.x, originPoint.y + yLength);
        Vector2 hitPoint = originPoint;


        //bool case_1 = false;
        //bool case_2 = false;

        //if (hit.collider != null && hit.collider.CompareTag("Platforms"))
        //    case_1 = true;

        //if (hit.collider == null)
        //    case_2 = true;


        if (hit.collider != null && hit.collider.CompareTag("Platforms"))
        {
            Debug.Log(hit.collider.name);
            hitPoint = hit.point;
            Debug.DrawLine(originPoint, hitPoint, Color.red, 20);

            return hitPoint;
        }

        for (int i = 0; i < numberOfIterationsForColliderSearch; i++)
        {
            if (hit.collider == null || !hit.collider.CompareTag("Platforms"))
            {
                originPoint = new(originPoint.x - i, originPoint.y);
                hit = Physics2D.Raycast(originPoint, direction, yLength * 2, platformLayer);
                hitPoint = hit.point;
                Debug.DrawLine(originPoint, hitPoint, Color.yellow, 20);
            }
            else
            {
                return hitPoint;
            }
        }
        return hitPoint;
    }
}
