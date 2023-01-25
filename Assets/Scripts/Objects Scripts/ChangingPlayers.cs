using UnityEngine;

public class ChangingPlayers : MonoBehaviour
{
    [SerializeField] private GameObject player;

    Gameplay gameplayScript;


    private void Start()
    {
        gameplayScript = player.GetComponent<Gameplay>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        Collider2D collider2D = collision.collider;

        if (collider2D.CompareTag("Enemy"))
        {
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collider2D.CompareTag("Main Enemy"))
        {
            gameplayScript.ChangePlayers();
            Destroy(collision.gameObject);
            Destroy(gameObject);
        }

        if (collider2D.CompareTag("Platforms"))
        {
            Destroy(gameObject);
        }

    }


    


}