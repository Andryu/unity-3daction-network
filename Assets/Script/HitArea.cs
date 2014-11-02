using UnityEngine;
using System.Collections;

public class HitArea : MonoBehaviour {

  void Damage(AttackArea.AttackInfo attackinfo) {
    transform.root.SendMessage("Damage", attackinfo);
  }
}
