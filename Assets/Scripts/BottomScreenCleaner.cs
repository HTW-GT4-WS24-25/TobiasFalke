using System.Collections;
using UnityEngine;

public class BottomScreenCleaner : MonoBehaviour
{
    private const float Delay = 2f;
    private void OnTriggerEnter2D(Collider2D other)
    {
        Destroy(other.gameObject, Delay);
    }
}
