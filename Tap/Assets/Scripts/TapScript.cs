using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Sprites;

public class TapScript : MonoBehaviour {

	int rand, boxCount, index; 
	int score, comboScore;
	int boxNumber;

	string boxNumberString;

	public GameObject boxs;
	public Text scoreText;

	Sprite green, red;

	void Start () {
		green = Resources.Load("Sprites/green", typeof(Sprite)) as Sprite;
		red = Resources.Load("Sprites/red", typeof(Sprite)) as Sprite;

		boxCount = boxs.transform.childCount;

		rand = Random.Range (1, boxCount+1);

		score = PlayerPrefs.GetInt("score");
		comboScore = PlayerPrefs.GetInt ("comboScore");
		scoreText.text = PlayerPrefs.GetInt("score").ToString();
		Debug.Log (comboScore);

		// scene manager index
		index = SceneManager.GetActiveScene().buildIndex;
	}

	void Update () {

		if (Input.GetMouseButtonUp (0)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
			if(hit != null && hit.collider != null){
				if (hit.collider.tag == "Box") {
					boxNumberString = hit.collider.name;
					boxNumber = int.Parse(boxNumberString);

					if (boxNumber%rand==0) {
						// comboScore
						if (comboScore < 16)
							comboScore += 1;
						else
							comboScore = 16;
						PlayerPrefs.SetInt ("comboScore", comboScore);
						if (comboScore == 3) {
							Debug.Log ("Triple lucky");
						} else if (comboScore == 4) {
							Debug.Log ("Mega lucky");
						} else if (comboScore == 5) {
							Debug.Log ("Extraordinary");
						} else if (comboScore == 6) {
							Debug.Log ("Staahp");
						} else if (comboScore == 7) {
							Debug.Log ("I said staahp");
						} else if (comboScore == 8) {
							Debug.Log ("Frenzy!");
						} else if (comboScore == 9) {
							Debug.Log ("Super Frenzy!");
						} else if (comboScore == 10) {
							Debug.Log ("Combo master");
						} else if (comboScore == 11) {
							Debug.Log ("Hold my beer");
						} else if (comboScore == 12) {
							Debug.Log ("Cant touch this");
						} else if (comboScore == 13) {
							Debug.Log ("Bananaexpert");
						} else if (comboScore == 14) {
							Debug.Log ("Button-muncher");
						} else if (comboScore == 15) {
							Debug.Log ("Tap Lord");
						} else if (comboScore >= 16) {
							Debug.Log ("Moom get the camera!");
						}

						// score
//						score += (boxCount);
						score += 1;						
						PlayerPrefs.SetInt ("score", score);
						
						Debug.Log ("True : " + boxNumber + "  % " + rand + " --- score : " + score);

						// pindah scene
						index = Random.Range(SceneManager.GetActiveScene().buildIndex, SceneManager.GetActiveScene().buildIndex+2);
						if (index > SceneManager.sceneCountInBuildSettings - 1) {
							index = SceneManager.sceneCountInBuildSettings - 1;
						}
					}						
					else{
						// comboScore
						comboScore = 0;
						PlayerPrefs.SetInt ("comboScore", comboScore);

						// score
//						score -= (boxCount);
						score -= 1;
						PlayerPrefs.SetInt ("score", score);

						Debug.Log ("False : " + boxNumber + "  % " + rand + " --- score : " + score);	

						// pindah scene
						index = Random.Range(SceneManager.GetActiveScene().buildIndex-2, SceneManager.GetActiveScene().buildIndex);
						if (index < 1) {												
							index = 1;
						}
					}
					// disable box dan ubah sprite masing-masing box
					for(int i = 0; i<boxCount; i++){
						boxs.transform.GetChild (i).GetComponent<BoxCollider2D> ().enabled = false;
						boxNumberString = boxs.transform.GetChild (i).name;
						boxNumber = int.Parse(boxNumberString);

						if (boxNumber % rand == 0) {							
							boxs.transform.GetChild (i).GetComponent<SpriteRenderer> ().sprite = green;
						} else {							
							boxs.transform.GetChild (i).GetComponent<SpriteRenderer> ().sprite = red;
						}
					}
					// simpan level terakhir
					PlayerPrefs.SetInt("lastLevel", index);
					StartCoroutine (LoadScene(index));
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
	