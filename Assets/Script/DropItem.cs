using UnityEngine;
using System.Collections;

public class DropItem : MonoBehaviour {

    public enum ItemKind {
        Attack,
        Heal,
    };
    public ItemKind kind;

    void OnTriggerEnter(Collider other) {
        if(other.tag == "Player") {
            // Get Item
            CharacterStatus status = other.GetComponent<CharacterStatus>();
            status.GetItem(kind);
            Destroy(gameObject);
        }
    }

    void Start() {
        Vector3 velocity = Random.insideUnitSphere * 2.0f + Vector3.up * 8.0f;
        rigidbody.velocity = velocity;
    }
}
