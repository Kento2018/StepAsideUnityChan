﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UnityChanController : MonoBehaviour {
	//アニメーションするためのコンポーネントを入れる
	private Animator myAnimator;
	//Unityちゃんを移動させるコンポーネントを入れる（追加）
	private Rigidbody myRigidbody;
	//前進するための力（追加）
	private float forwardForce = 800.0f;
	//左右に移動するための力（追加）
	private float turnForce = 500.0f;
	//ジャンプするための力（追加）
	private float upForce = 500.0f;
	//左右の移動できる範囲（追加）
	private float movableRange = 3.4f;
	//動きを減速させる係数（追加）
	private float coefficient = 0.95f;

	//ゲーム終了の判定（追加）
	private bool isEnd = false;
	private GameObject stateText;
	private GameObject scoreText;
	private int score = 0;
	private bool isLButtonDown = false;
	private bool isRButtonDown = false;

	// Use this for initialization
	void Start () {

		//アニメータコンポーネントを取得
		this.myAnimator = GetComponent<Animator>();

		//走るアニメーションを開始
		this.myAnimator.SetFloat ("Speed", 1);

		//Rigidbodyコンポーネントを取得（追加）
		this.myRigidbody = GetComponent<Rigidbody>();
		this.stateText = GameObject.Find ("GameResultText");
		this.scoreText = GameObject.Find ("ScoreText");
	}

	// Update is called once per frame
	void Update () {
		//ゲーム終了ならUnityちゃんの動きを減衰する（追加）
		if (this.isEnd) {
			this.forwardForce *= this.coefficient;
			this.turnForce *= this.coefficient;
			this.upForce *= this.coefficient;
			this.myAnimator.speed *= this.coefficient;
		}

		//Unityちゃんに前方向の力を加える（追加）
		this.myRigidbody.AddForce (this.transform.forward * this.forwardForce);
		//Unityちゃんを矢印キーまたはボタンに応じて左右に移動させる（追加）
		if ((Input.GetKey (KeyCode.LeftArrow) || this.isLButtonDown) && -this.movableRange < this.transform.position.x) {
			//左に移動（追加）
			this.myRigidbody.AddForce (-this.turnForce, 0, 0);
		} else if ((Input.GetKey (KeyCode.RightArrow) || this.isRButtonDown) && this.transform.position.x < this.movableRange) {
			//右に移動（追加）
			this.myRigidbody.AddForce (this.turnForce, 0, 0);
		} 
		//Jumpステートの場合はJumpにfalseをセットする（追加）
		if (this.myAnimator.GetCurrentAnimatorStateInfo(0).IsName ("Jump")) {
			this.myAnimator.SetBool ("Jump", false);
		}

		//ジャンプしていない時にスペースが押されたらジャンプする（追加）
		if (Input.GetKeyDown(KeyCode.Space) && this.transform.position.y < 0.5f) {
			//ジャンプアニメを再生（追加）
			this.myAnimator.SetBool ("Jump", true);
			//Unityちゃんに上方向の力を加える（追加）
			this.myRigidbody.AddForce (this.transform.up * this.upForce);
		}

	} //トリガーモードで他のオブジェクトと接触した場合の処理（追加）
	void OnTriggerEnter(Collider other) {

		//障害物に衝突した場合（追加）
		if (other.gameObject.tag == "CarTag" || other.gameObject.tag == "TrafficConeTag") {
			this.isEnd = true;
			this.stateText.GetComponent<Text>().text = "GAME OVER";
		}

		//ゴール地点に到達した場合（追加）
		if (other.gameObject.tag == "GoalTag") {
			this.isEnd = true;
			this.stateText.GetComponent<Text>().text = "CLEAR!!";
		} 
		if (other.gameObject.tag == "CoinTag") {
			this.score += 10;
			this.scoreText.GetComponent<Text> ().text = "Score " + this.score + "pt";
			GetComponent<ParticleSystem> ().Play ();
			Destroy (other.gameObject);
		}
	}
	public void GetMyJumpButtonDown() {
		if (this.transform.position.y < 0.5f) {
			this.myAnimator.SetBool ("Jump", true);
			this.myRigidbody.AddForce (this.transform.up * this.upForce);
		}
	}
	public void GetMyLeftButtonDown() {
		this.isLButtonDown = true;
	}
	public void GetMyLeftButtonUp() {
		this.isLButtonDown = false;
	}
	public void GetMyRightButtonDown() {
		this.isRButtonDown = true;
	}
	public void GetMyRightButtonUp() {
		this.isRButtonDown = false;
	}
}