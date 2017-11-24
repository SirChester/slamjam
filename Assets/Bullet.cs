using UnityEngine;

public class Bullet : MonoBehaviour
{
	private void Awake()
	{
		GetComponent<Rigidbody2D>().AddForce(Vector2.right * 10000.0f);
	}

	void OnCollisionEnter2D(Collision2D coll)
	{
		Destroy(gameObject);
//		if (coll.gameObject.tag == "Enemy")
//			coll.gameObject.SendMessage("ApplyDamage", 10);
	}
}