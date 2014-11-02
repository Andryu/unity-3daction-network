using UnityEngine;
using System.Collections;

public class EnemyCtrl : MonoBehaviour {

    // Add State
    CharacterStatus status;
    CharacterAnimation charaAnimation;
	CharacterMove characterMove;
    Transform attackTarget;

    // 待機時間は2秒とする
    public float waitBasetime = 2.0f;
    // 残り待機時間
    float waitTime;
    // 移動範囲5メートル
    public float walkRange = 1.5f;
    // 初期位置
    public Vector3 basePosition;

    enum State {
        Walking,
        Attacking,
        Died
    };

    State state     = State.Walking;
    State nextState = State.Walking;

    // Use this for initialization
    void Start () {
        // Add
        status = GetComponent<CharacterStatus>();
        charaAnimation = GetComponent<CharacterAnimation>();
		characterMove  = GetComponent<CharacterMove>();

        // AI
       basePosition = transform.position;
       waitTime     = waitBasetime;
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

        // state
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
        if (waitTime > 0.0f) {
            waitTime -= Time.deltaTime;
            if (waitTime <= 0.0f) {
                Vector2 randomValue = Random.insideUnitCircle * walkRange;
                Vector3 destinationPosition = basePosition + new Vector3(randomValue.x, 0.0f, randomValue.y);
                // 目的地の指定
                SendMessage("SetDestination", destinationPosition);
            }
        }else{
			if(characterMove.Arrived())
				waitTime = Random.Range(waitBasetime, waitBasetime * 2.0f);
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
