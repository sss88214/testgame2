using UnityEngine;
using System.Collections;

public class Shooter2D : MonoBehaviour {
	bool isThis=false;
	float m_time = 0.5f;
	bool m_select = false;
	bool m_CanPress = true;
	bool m_CanShoot = false;
	public GameObject SelectObject;
	public GameObject m_Torque;
	public GameObject m_point;
	Vector3 direction;
	float magnitide;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		CheckObject2D ();
		Shooter ();
	}

	void CheckObject2D(){


		if (Input.GetMouseButton (0)&&m_CanPress) {



			Ray ray = UICamera.mainCamera.ScreenPointToRay (Input.mousePosition);
			RaycastHit2D hit = Physics2D.Raycast (ray.origin, ray.direction, 10, 32);
			if (hit.collider!=null){
				if (hit.collider.tag == "ball") {
					m_select = true;
					m_CanPress=false;
					Debug.Log("GG");
					SelectObject = hit.transform.gameObject;
					if (SelectObject.GetComponent<Rigidbody2D>().isKinematic == false) SelectObject.GetComponent<Rigidbody2D>().isKinematic = true;
					m_Torque.transform.position = SelectObject.transform.position;

				}

			}else{
				//m_click = false;
				Debug.Log("null");
				m_CanPress=false;
			}
			//Debug.Log(hit.collider.name);
			//Debug.Log(m_CanPress);

		}
		if (Input.GetMouseButtonUp (0)) {
			m_CanPress=true;
			m_select=false;
			if (SelectObject){
				if (m_Torque.GetComponent<BoxCollider>().enabled==true) m_Torque.GetComponent<BoxCollider>().enabled=false;
				if (SelectObject.GetComponent<Rigidbody2D>().isKinematic == true) SelectObject.GetComponent<Rigidbody2D>().isKinematic = false;
				if (m_point.activeSelf == true) m_point.SetActive(false);
				if (m_CanShoot) SelectObject.GetComponent<Rigidbody2D>().velocity = direction * magnitide * 10f;
				SelectObject =null;
			}
		}

	}

	void Shooter(){
		if (SelectObject != null) {
			if (m_Torque.GetComponent<BoxCollider>().enabled==false) m_Torque.GetComponent<BoxCollider>().enabled=true;
			Ray ray = UICamera.mainCamera.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit;
			if (Physics.Raycast(ray,out hit)){
				if (hit.collider.tag=="Torque"){
					Debug.Log("in");
					if (Vector3.Distance(SelectObject.transform.position,hit.point)>0.05f){
						if (m_CanShoot==false) m_CanShoot=true;
						if (m_point.activeSelf == false) m_point.SetActive(true);
						direction = SelectObject.transform.position-hit.point;
						direction.Normalize();

						m_point.transform.position = hit.point;

						m_point.transform.localPosition =SelectObject.transform.localPosition+ new Vector3(Vector3.ClampMagnitude(m_point.transform.localPosition - SelectObject.transform.localPosition, 50f).x,Vector3.ClampMagnitude(m_point.transform.localPosition - SelectObject.transform.localPosition, 50f).y, 0f);

						magnitide = Vector3.Distance(SelectObject.transform.position,m_point.transform.position);
						Debug.Log(magnitide);
						Debug.Log(Vector3.Distance(SelectObject.transform.position,hit.point));
						//Debug.Log(m_point.transform.localPosition);
						//Debug.Log(SelectObject.transform.localPosition);
					}else if (Vector3.Distance(SelectObject.transform.position,hit.point)<=0.05f){
						if (m_point.activeSelf == true) m_point.SetActive(false);
						if (m_CanShoot==true)m_CanShoot=false;
					}
				}
			}
		}
	}



}
