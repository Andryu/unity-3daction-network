using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {

    const float RayCastMaxDistance = 100.0f;
    InputManager inputManager;

    // Add State
    CharacterStatus status;
    CharacterAnimation charaAnimation;
    Transform attackTarget;
    public float attackRange = 1.5f;

	GameRuleCtrl gameRuleCtrl;

    enum State {
        Walking,
        Attacking,
        Died
    };

    State state     = State.Walking;
    State nextState = State.Walking;

    // Use this for initialization
    void Start () {
        inputManager = FindObjectOfType<InputManager>();

        // Add
        status = GetComponent<CharacterStatus>();
        charaAnimation = GetComponent<CharacterAnimation>();

		gameRuleCtrl = FindObjectOfType<GameRuleCtrl>();
    }

    // Update is called once per frame
    void Update () {
        switch(state) {
            case State.Walking:
                Walking();
                break;
            case State.Attacking:
                Attacking();
                break;
        }

        // stateの変更を行う
        if (state != nextState) {
			state  = nextState;
            switch(state) {
                case State.Walking:
                    WalkStart();
                    break;
                case State.Attacking:
                    AttackStart();
                    break;
                case State.Died:
                    Died();
                    break;
            }
        }
    }

    void ChangeState(State nextState) {
        this.nextState = nextState;
    }

    void WalkStart(){
        StateStartCommon();
    }

    void Walking() {
        if(inputManager.Clicked()) {
            Vector2 clickpos = inputManager.GetCursorPosition();

            // search RayCast target
            Ray ray = Camera.main.ScreenPointToRay(clickpos);

            RaycastHit hitInfo;
            if(Physics.Raycast (ray, out hitInfo, RayCastMaxDistance, (1 << LayerMask.NameToLayer ("Ground")) 
			   | (1<<LayerMask.NameToLayer("EnemyHit")))) {
                SendMessage("SetDestination", hitInfo.point);
            }

            // 敵がクリックされた
            if (hitInfo.collider.gameObject.layer == LayerMask.NameToLayer("EnemyHit")) {
                // check distance horizontal
                Vector3 hitPoint = hitInfo.point;
                hitPoint.y = transform.position.y;
                float distance = Vector3.Distance(hitPoint, transform.position);

                if (distance < attackRange) {
                    // attack
                    attackTarget = hitInfo.collider.transform;
                    ChangeState(State.Attacking);
                }else{
                    SendMessage("SetDestination", hitInfo.point);
                }
            }
        }
    }

    void AttackStart() {
        StateStartCommon();
        status.attacking = true;

        //
        Vector3 targetDirection = (attackTarget.position - transform.position).normalized;
        SendMessage("SetDirection", targetDirection);

        // stop move
        SendMessage("StopMove");
    }

    void Attacking() {
        if (charaAnimation.IsAttacked())
            ChangeState(State.Walking);
    }

    void Died() {
        status.died = true;
		gameRuleCtrl.GameOver();
    }

    void Damage(AttackArea.AttackInfo attackInfo) {
        status.HP -= attackInfo.attackPower;
        if (status.HP <= 0) {
            status.HP = 0;
            ChangeState(State.Died);
        }
    }

    void StateStartCommon() {
        status.attacking = false;
        status.died = false;
    }
}
