using UnityEngine;
using System.Collections;

public class PlayerCtrl : MonoBehaviour {

	const float RayCastMaxDistance = 100.0f;
	InputManager inputManager;

	// Use this for initialization
	void Start () {
		inputManager = FindObjectOfType<InputManager>();
	}
	
	// Update is called once per frame
	void Update () {
		Walking();	
	}

	void Walking() {
		if(inputManager.Clicked()) {
			Vector2 clickpos = inputManager.GetCursorPosition();

			// search RayCast target
			Ray ray = Camera.main.ScreenPointToRay(
				inputManager.GetCursorPosition());

			RaycastHit hitinfo;
			if(Physics.Raycast (ray, out hitinfo, RayCastMaxDistance, 1 << LayerMask.NameToLayer ("Ground"))) {
				SendMessage("SetDestination", hitinfo.point);
			}
		}
	}
}
