using UnityEngine;

public class Carrot : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D _collision)
    {
        if (!_collision.GetComponent<Player>()) return;
        FindObjectOfType<GameManager>().AddCarrot(1U);
        Destroy(gameObject);
    }
}
