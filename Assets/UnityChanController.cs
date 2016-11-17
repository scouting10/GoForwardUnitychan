using UnityEngine;
using System.Collections;

public class UnityChanController : MonoBehaviour {
  
    //アニメーションをするためのコンポ―ネント
    Animator animator;
    //移動用コンポーネント
    Rigidbody2D rigid2D;
    //ジャンプの速度減衰
    private float dump = 0.8f;
    //ジャンプの速度
    float jumpVelocity = 20;
    float cubeVelocity = 15;
    //踏み加速速度
    float stumpVelocity = 1;

    //ゲームオーバーになる位置
    private float deadLine = -9;

    //地面の位置
    private float groundLevel = -3.0f;


    //ボックス接触判定
    /*cubeとUnityちゃんの位置関係から、Unityちゃんの下のCubeだけを対象とする。
    また、踏むと少し前に出られるゲーム性に変更する。
     */
    bool withCube;
    float withCubePos;
    void OnCollisionEnter2D(Collision2D other)
    {
        withCubePos = this.transform.position.y - other.transform.position.x;
        if (other.gameObject.tag == "Cube" && withCubePos > 1)
        {
            this.rigid2D.velocity = new Vector2(stumpVelocity, 0);
            withCube = true;
            
        }
    }
    void OnCollisionExit2D(Collision2D other)
    {
        if (other.gameObject.tag == "Cube")
        {
            withCube = false;
           
        }
    }



    // Use this for initialization
    void Start () {
        //アニメータのコンポーネントを取得する
        this.animator = GetComponent<Animator>();
        //Rigidbody2Dのコンポーネントを取得
        this.rigid2D = GetComponent<Rigidbody2D>();
        
	
	}
	
	// Update is called once per frame
	void Update () {
        //Horizontal＝１にて、右方向へ走るアニメを再生させ続ける。
        this.animator.SetFloat("Horizontal", 1);

        //着地判定
        bool isGround = (transform.position.y > this.groundLevel) ? false : true;
        this.animator.SetBool("isGround", isGround);

        
        //ジャンプ時に足音音量は０
        GetComponent<AudioSource>().volume = (isGround) ? 1 : 0;

        //着地状態でクリックされた場合。
        //velocity.yにjumpVelocityが入る。
        //”マウスを押し続けている間上向きの力がかかる”のではなく、”一度代入された速度がクリックやめる瞬間までいじられない”だけ。だから、クリックし続けている（介入している）ように見えて、ジャンプした瞬間からUnityちゃんには何の入力も入っていない。
        if (Input.GetMouseButtonDown(0) && isGround)
        {
            //上方向の力
            this.rigid2D.velocity = new Vector2(0, this.jumpVelocity);
        }

        //ボックスに触ってる時も飛べる
        if (Input.GetMouseButtonDown(0) && withCube)
        {
            this.rigid2D.velocity = new Vector2(0, this.cubeVelocity);
        }
        
        //クリックをやめた時。
        /*クリックやめたら[上向き（y>0）のvelocityだけ]減速。velocityを代入（たとえば、"velocity.y = 0" など）するのでなく、「上向きの」velocityだけを減衰させることで、慣性を感じる自然な挙動になる。
        ちなみに、下向きの力はproject settingsのgravityである。*/
        if (Input.GetMouseButton(0) == false)
        {
            //この条件を消すと、落下速度にも減衰がかかってしまう。「着地するまでクリックし続け」なければ、Unityちゃんがホバリングする。
            if (this.rigid2D.velocity.y > 0)
            {
                this.rigid2D.velocity *= this.dump;
            }
        }

        //デッドラインでゲームオーバー処理
        if (transform.position.x < this.deadLine)
        {
            //UIControllerのGameOver関数を呼び出す
            GameObject.Find("Canvas").GetComponent<UIController>().GameOver();

            //ユニティちゃんをDestroy
            Destroy(gameObject);
        }
	}
}
