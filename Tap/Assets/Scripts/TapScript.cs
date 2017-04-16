using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Sprites;

public class TapScript : MonoBehaviour {

	int rand, circleCount, index; 
	int score, comboScore;
	int circleNumber, circleSiblingIndex;

	string circleNumberString;

	public GameObject circles;
	public Text scoreText;

	Sprite green, red;
	Sprite greenPushed, redPushed;

	// efek tap
	private bool tapped;
	Sprite unpushed, pushed;
	GameObject lastTappedCircle;

	void Start () {		
		green = Resources.Load("Sprites/greenunpushed", typeof(Sprite)) as Sprite;
		red = Resources.Load("Sprites/redunpushed", typeof(Sprite)) as Sprite;
		greenPushed = Resources.Load("Sprites/greenpushed", typeof(Sprite)) as Sprite;
		redPushed = Resources.Load("Sprites/redpushed", typeof(Sprite)) as Sprite;

		circleCount = circles.transform.childCount;

		rand = Random.Range (1, circleCount+1);

		score = PlayerPrefs.GetInt("score");
		comboScore = PlayerPrefs.GetInt ("comboScore");
		scoreText.text = PlayerPrefs.GetInt("score").ToString();

		// scene manager index
		index = SceneManager.GetActiveScene().buildIndex;

		// efek tap
		tapped = false;
		unpushed = Resources.Load("Sprites/normalunpushed", typeof(Sprite)) as Sprite;
		pushed = Resources.Load("Sprites/normalpushed", typeof(Sprite)) as Sprite;
	}

	void Update () {		
		if (Input.GetMouseButton(0)) {							
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (pos, Vector2.zero);
			if (hit != null && hit.collider != null) {
				if (hit.collider.tag == "Circle") {
					lastTappedCircle = hit.collider.gameObject;
					tapped = !tapped;
					if (tapped) {
						lastTappedCircle.transform.GetComponent<SpriteRenderer> ().sprite = pushed;
					}
				}
			} else {				
				if (tapped) {
					tapped = false;
					lastTappedCircle.transform.GetComponent<SpriteRenderer> ().sprite = unpushed;
				}
			}
		}

		else if (Input.GetMouseButtonUp (0)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (pos, Vector2.zero);
			if (hit != null && hit.collider != null) {
				if (hit.collider.tag == "Circle") {
					circleSiblingIndex = hit.collider.gameObject.transform.GetSiblingIndex ();
					circleNumberString = hit.collider.name;
					circleNumber = int.Parse (circleNumberString);

					if (circleNumber % rand == 0) {
						// comboScore
						if (comboScore < 16)
							comboScore += 1;
						else
							comboScore = 16;
						PlayerPrefs.SetInt ("comboScore", comboScore);
//						if (comboScore == 3) {
//							Debug.Log ("Triple lucky");
//						} else if (comboScore == 4) {
//							Debug.Log ("Mega lucky");
//						} else if (comboScore == 5) {
//							Debug.Log ("Extraordinary");
//						} else if (comboScore == 6) {
//							Debug.Log ("Staahp");
//						} else if (comboScore == 7) {
//							Debug.Log ("I said staahp");
//						} else if (comboScore == 8) {
//							Debug.Log ("Frenzy!");
//						} else if (comboScore == 9) {
//							Debug.Log ("Super Frenzy!");
//						} else if (comboScore == 10) {
//							Debug.Log ("Combo master");
//						} else if (comboScore == 11) {
//							Debug.Log ("Hold my beer");
//						} else if (comboScore == 12) {
//							Debug.Log ("Cant touch this");
//						} else if (comboScore == 13) {
//							Debug.Log ("Bananaexpert");
//						} else if (comboScore == 14) {
//							Debug.Log ("Button-muncher");
//						} else if (comboScore == 15) {
//							Debug.Log ("Tap Lord");
//						} else if (comboScore >= 16) {
//							Debug.Log ("Moom get the camera!");
//						}

						// score
						score += 1;						
						PlayerPrefs.SetInt ("score", score);

//						Debug.Log ("True : " + circleNumber + "  % " + rand + " --- score : " + score);

						// index pindah scene
						index = Random.Range (SceneManager.GetActiveScene ().buildIndex, SceneManager.GetActiveScene ().buildIndex + 2);
						if (index > SceneManager.sceneCountInBuildSettings - 1) {
							index = SceneManager.sceneCountInBuildSettings - 1;
						}
					} else {
						// comboScore
						comboScore = 0;
						PlayerPrefs.SetInt ("comboScore", comboScore);

						// score
						score -= 1;
						PlayerPrefs.SetInt ("score", score);

//						Debug.Log ("False : " + circleNumber + "  % " + rand + " --- score : " + score);	

						// index pindah scene
						index = Random.Range (SceneManager.GetActiveScene ().buildIndex - 2, SceneManager.GetActiveScene ().buildIndex);
						if (index < 1) {												
							index = 1;
						}
					}
					// disable circle dan ubah sprite masing-masing circle
					for (int i = 0; i < circleCount; i++) {																		
						circles.transform.GetChild (i).GetComponent<CircleCollider2D> ().enabled = false;
						circleNumberString = circles.transform.GetChild (i).name;
						circleNumber = int.Parse (circleNumberString);

						if (circleNumber % rand == 0) {
							if (i == circleSiblingIndex)
								circles.transform.GetChild (i).GetComponent<SpriteRenderer> ().sprite = greenPushed;
							else
								circles.transform.GetChild (i).GetComponent<SpriteRenderer> ().sprite = green;
						} else {							
							if (i == circleSiblingIndex)
								circles.transform.GetChild (i).GetComponent<SpriteRenderer> ().sprite = redPushed;
							else
								circles.transform.GetChild (i).GetComponent<SpriteRenderer> ().sprite = red;
						}
					}
					// simpan level terakhir
					PlayerPrefs.SetInt ("lastLevel", index);
					StartCoroutine (LoadScene (index));
				}
			}
		}
		else if(Input.GetKeyDown(KeyCode.Escape)){
			SceneManager.LoadScene(0);	// back to home
		}
	}

	IEnumerator LoadScene(int index){
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene (index);
	}
}
