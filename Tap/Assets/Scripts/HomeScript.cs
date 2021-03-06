﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class HomeScript : MonoBehaviour {

	public Sprite red;
	public Sprite green;

	// Use this for initialization
	void Start () {
		PlayerPrefs.DeleteAll ();

		// mulai comboScore dari awal
		PlayerPrefs.SetInt ("comboScore", 0);

		// jika health belum diset
		if( PlayerPrefs.GetInt("setHealth") == 0 ){
			PlayerPrefs.SetInt ("health", 500);
			PlayerPrefs.SetInt ("setHealth", 1);
		}
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButtonUp (0)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero);
			if(hit != null && hit.collider != null){
				if (hit.collider.tag == "Circle") {
					hit.collider.gameObject.GetComponent<SpriteRenderer>().sprite = green;
					if( PlayerPrefs.GetInt("lastLevel") > 0 )
						StartCoroutine(LoadScene(PlayerPrefs.GetInt("lastLevel")));
					else
						StartCoroutine(LoadScene(1));
				}
			}
		}
		else if(Input.GetKeyDown(KeyCode.Escape)){
			Application.Quit ();
		}
	}

	IEnumerator LoadScene(int index){
		yield return new WaitForSeconds(0.5f);
		SceneManager.LoadScene (index);
	}
}
