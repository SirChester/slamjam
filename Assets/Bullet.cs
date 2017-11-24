using UnityEngine;
using UnityEngine.Networking;

public class Bullet : NetworkBehaviour
{
	private void Awake()
	{
		GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10000.0f);
		
		RectTransform room = FindObjectOfType<GameController>().GetComponent<RectTransform>();
		GetComponent<Transform>().SetParent(room);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		Destroy(gameObject);
//		if (coll.gameObject.tag == "Enemy")
//			coll.gameObject.SendMessage("ApplyDamage", 10);
	}
}