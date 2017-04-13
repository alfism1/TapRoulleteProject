using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using UnityEngine.Sprites;

public class TapScript : MonoBehaviour {

	int rand, boxCount, index; 
	int score;
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
		scoreText.text = PlayerPrefs.GetInt("score").ToString();

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
						score += (boxCount);
						PlayerPrefs.SetInt ("score", score);
						
						Debug.Log ("True : " + boxNumber + "  % " + rand + " --- score : " + score);

						// pindah scene
						index = Random.Range(SceneManager.GetActiveScene().buildIndex, SceneManager.GetActiveScene().buildIndex+2);
						if (index > SceneManager.sceneCountInBuildSettings - 1) {
							index = SceneManager.sceneCountInBuildSettings - 1;
						}
					}						
					else{
						score -= (boxCount);
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
					StartCoroutine (LoadScene(index));
				}
			}
		}
	}

	IEnumerator LoadScene(int index){
		yield return new WaitForSeconds(1);
		SceneManager.LoadScene (index);
	}
}
	