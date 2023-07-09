using UnityEngine;

public class Info : MonoBehaviour
{
    //private static Vector3 playerPosition;

    //public static Vector3 PlayerPosition
    //{
    //    get { return playerPosition; }
    //    set { playerPosition = value; }
    //}

    public static LayerMask PlayerLayer = LayerMask.GetMask("Player");
    public static LayerMask PlatformLayer = LayerMask.GetMask("Platforms");
    public static string PlayerTag = "Player";
}