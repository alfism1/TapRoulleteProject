using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Tapped : MonoBehaviour {
	private bool tapped;

	Sprite unpushed, pushed;
	GameObject lastTappedCircle;

	// Use this for initialization
	void Start () {
		tapped = false;

		unpushed = Resources.Load("Sprites/normalunpushed", typeof(Sprite)) as Sprite;
		pushed = Resources.Load("Sprites/normalpushed", typeof(Sprite)) as Sprite;
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetMouseButton(0)) {
			if(!tapped){
				Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
				RaycastHit2D hit = Physics2D.Raycast (pos, Vector2.zero);
				if (hit != null && hit.collider != null) {
					if (hit.collider.tag == "Circle") {
						lastTappedCircle = hit.collider.gameObject;
						lastTappedCircle.transform.GetComponent<SpriteRenderer> ().sprite = pushed;
					}
				} else {								
					lastTappedCircle.transform.GetComponent<SpriteRenderer> ().sprite = unpushed;
				}	
			}
		}

		if (Input.GetMouseButtonUp (0)) {
			Vector3 pos = Camera.main.ScreenToWorldPoint (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (pos, Vector2.zero);
			if (hit != null && hit.collider != null) {
				if (hit.collider.tag == "Circle") {
					tapped = true;
				}
			}
		}
	}
}
