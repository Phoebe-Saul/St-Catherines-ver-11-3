using UnityEngine;

public class Projectile : MonoBehaviour
{
    [SerializeField] private float lifetime = 5f;

    void Start(){
        Destroy(gameObject, lifetime);
    }

    void OnTriggerEnter2D(Collider2D collision){
        //prevent projectile instantly destroying from collision with boss
        if(collision.CompareTag("ThroneBoss")) return;

        if(collision.CompareTag("Player")){
            Debug.Log("MC hit!");
        }

        Debug.Log("Projectile collision success: " + collision.gameObject.name + ", " + collision.tag);

        Destroy(gameObject);
    }
}
