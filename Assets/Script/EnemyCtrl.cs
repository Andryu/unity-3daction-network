using UnityEngine;
using System.Collections;

public class EnemyCtrl : MonoBehaviour {
    enum State {
        Walking,
        Chasing,
        Attacking,
        Died
    };

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

    // State
    State state     = State.Walking;
    State nextState = State.Walking;

    // Item
    public GameObject[] dropItemPrefab;

	GameRuleCtrl gameRuleCtrl;

    // Target設定
    public void SetAttackTarget(Transform target) {
      attackTarget = target;
    }

    // Use this for initialization
    void Start () {
    // Status
        status = GetComponent<CharacterStatus>();
        charaAnimation = GetComponent<CharacterAnimation>();
        characterMove  = GetComponent<CharacterMove>();

        // AI
        basePosition = transform.position;
        waitTime     = waitBasetime;

		gameRuleCtrl = FindObjectOfType<GameRuleCtrl>();
    }

    // Update is called once per frame
    void Update () {
        switch(state) {
            case State.Walking:
                Walking();
                break;
            case State.Chasing:
                Chasing();
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

    void dropItem() {
    if (dropItemPrefab.Length == 0) return;
        GameObject dropItem = dropItemPrefab[Random.Range(0, dropItemPrefab.Length)];
        Instantiate(dropItem, transform.position, Quaternion.identity);
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

            if( attackTarget )
                ChangeState(State.Chasing);
        }
    }

    void ChasingStart(){
        StateStartCommon();
    }

    void Chasing(){
        // 移動先をプレイヤーに設定
        SendMessage("SetDestination", attackTarget.position);
        // 2m以内に近づいたら攻撃
        if (Vector3.Distance(attackTarget.position, transform.position) <= 2.0f)
            ChangeState(State.Attacking);
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
        if (charaAnimation.IsAttacked()) {
            ChangeState(State.Walking);
            // 待機時間を再設定
            waitTime = Random.Range(waitBasetime, waitBasetime * 2.0f);
            // ターゲットリセット
            attackTarget = null;
        }
    }

    void Died() {
        status.died = true;
        dropItem();
		Destroy(gameObject);
		if (gameObject.tag == "Boss")
			gameRuleCtrl.GameClear();
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
