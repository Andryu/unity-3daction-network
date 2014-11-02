using UnityEngine;
using System.Collections;

public class AttackAreaActivator : MonoBehaviour {
    Collider[] attackAreaColliders; // 攻撃用判定コライダ

    void Start(){
        // 子供のGameObjecyにAttackAreaAcriptがついているGameObjectを探す
        AttackArea[] attackAreas = GetComponentsInChildren<AttackArea>();
        attackAreaColliders = new Collider[attackAreas.Length];

        for(int attackAreaCnt = 0 ; attackAreaCnt < attackAreas.Length; attackAreaCnt++) {
            attackAreaColliders[attackAreaCnt] = attackAreas[attackAreaCnt].collider;
            attackAreaColliders[attackAreaCnt].enabled = false;
        }
    }

    // アニメーションイベントのStartAttackHitを受け取ってコライダを有効にする
    void StartAttackHit() {
        foreach (Collider attackAreaCollider in attackAreaColliders)
            attackAreaCollider.enabled = true;
    }

    // アニメションイベントのEndAttackHitを受け取ってコライダを無効にする
    void EndAttackHit() {
        foreach (Collider attackAreaCollider in attackAreaColliders)
            attackAreaCollider.enabled = false;
    }
}
