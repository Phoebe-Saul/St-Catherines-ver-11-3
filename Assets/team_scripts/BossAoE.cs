using UnityEngine;

//this for displaying an AoE radius in the editor because unity wants to not cooperate

public class BossAoE : MonoBehaviour
{
    [Header("Visual Debug")]
    [SerializeField] private Color gizmoColor = Color.cyan;

    private CircleCollider2D circleCollider;

    void Awake(){
        circleCollider = GetComponent<CircleCollider2D>();
    }

    void OnTriggerEnter2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            Debug.Log("Entered AoE (BossAoE.cs)");
        }
    }

    void OnTriggerExit2D(Collider2D collision){
        if(collision.CompareTag("Player")){
            Debug.Log("Left AoE (BossAoE.cs)");
        }
    }
    
    void OnDrawGizmors(){
        if(circleCollider == null){
            circleCollider = GetComponent<CircleCollider2D>();
        }
        if(circleCollider != null){
            Gizmos.color = gizmoColor;

            Vector3 center = transform.position + (Vector3)circleCollider.offset;

            Gizmos.DrawWireSphere(center, circleCollider.radius);
        }
    }
}
