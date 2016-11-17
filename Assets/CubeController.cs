using UnityEngine;
using System.Collections;

public class CubeController : MonoBehaviour {

    //キューブの移動速度
    private float speed = -0.2f;
    //経過時間
    float time = 0f;
    float dif;
    
    //消滅位置
    private float deadLine = -10;
    //消滅時間
    private float deadTime = 3.8f;

    void OnCollisionEnter2D(Collision2D other)
    {
        if (other.gameObject.tag == "Cube"||other.gameObject.tag=="Ground")
        {
            AudioSource cubeAudio = GetComponent<AudioSource>();
            cubeAudio.Play();
            //一度音が鳴ったら二度と鳴らないようにしたい。このやりかたでは最初から音がしない。
            //cubeAudio.volume =0;
        }
    }


	// Use this for initialization
	void Start () {
       
    }
	
	// Update is called once per frame
	void Update () {
        //自滅用Timeカウント
        time += Time.deltaTime;
        dif = deadTime - time;

        //キューブ移動
        transform.Translate(this.speed, 0, 0);

        //画面外に出たら破棄する
        /*if文の時間条件は `deadTime == time` にすると失敗する。
        小数点が細かすぎて、等号で結ぶのは無理だと考えた。dif<0でOK */
        if (transform.position.x < this.deadLine || dif<0)
        {
            Destroy(gameObject);
            
        }
	
	}
}
