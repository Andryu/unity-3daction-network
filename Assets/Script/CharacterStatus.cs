using UnityEngine;
using System.Collections;

public class CharacterStatus : MonoBehaviour {

  public int HP = 100;
  public int MaxHp = 100;

  public int Power = 10;

  public GameObject lastAttackTarget = null;

  // GUI お及びネットワーク
  // 名前
  public string CharacterName = "Player";

  // アニメーション
  // 状態
  public bool attacking = false;
  public bool died = false;

  // 攻撃強化
  public bool powerBoost = false;
  // 攻撃強化時間
  float powerBoostTime = 0.0f;

  public void GetItem(DropItem.ItemKind itemKind) {
      switch(itemKind) {
          case DropItem.ItemKind.Attack:
              powerBoostTime = 5.0f;
              break;
          case DropItem.ItemKind.Heal:
              // MaxHPの半分回復
              HP = Mathf.Min(HP + MaxHp / 2, MaxHp);
              break;
      }
  }

  // Update is called once per frame
  void Update () {
      powerBoost = false;
      if (powerBoostTime > 0.0f) {
          powerBoost     = true;
          powerBoostTime = Mathf.Max(powerBoostTime - Time.deltaTime, 0.0f);
      }
  }
}
