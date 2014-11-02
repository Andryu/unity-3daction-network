using UnityEngine;
using System.Collections;

public class EnemyGeneratorCtrl : MonoBehaviour {

    public GameObject enemyPrefab;
    // 敵を格納
    GameObject[] existEnemys;
    // アクティブの最大数
    public int maxEnemys = 2;

    // Use this for initialization
    void Start () {
        existEnemys = new GameObject[maxEnemys];
        StartCoroutine(Exec());
    }

    IEnumerator Exec() {
        while(true) {
            Generate();
            yield return new WaitForSeconds( 3.0f );
        }
    }

    void Generate() {
        for (int enemyCnt = 0; enemyCnt < existEnemys.Length ; enemyCnt++) {
            if (!existEnemys[enemyCnt]) {
                existEnemys[enemyCnt] = Instantiate(enemyPrefab, transform.position, transform.rotation) as GameObject;
                return;
            }
        }
    }
}
