using System;
using UnityEngine;

public class Weapon : MonoBehaviour
{
	public enum Type
	{
		Rock,
		Paper,
		Scissors
	}

	[SerializeField] private Type _type;
	[SerializeField] private float _forceMultiplier;
	[SerializeField] private float _cooldown;
	[SerializeField] private int _damage;

	public Type BulletType
	{
		get { return _type; }
	}

	public float Cooldown
	{
		get { return _cooldown; }
	}

	public static GameObject ShootByType(Type type, string playerName)
	{
		return Instantiate(Resources.Load("Prefabs/" + playerName + "_" + type)) as GameObject;
	}
	
	private void Awake()
	{
		GetComponent<Rigidbody2D>().AddForce(Vector2.right * _forceMultiplier);
	}
	
	private void OnCollisionEnter2D(Collision2D coll)
	{
		var collideBullet = coll.gameObject.GetComponent<Weapon>();
		if (collideBullet != null)
		{
			var needDestroy = _type == collideBullet.BulletType;
			needDestroy |= collideBullet.BulletType == Type.Rock;
			needDestroy |= _type == Type.Scissors;
			if (needDestroy)
			{
				Destroy(gameObject);
				coll.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -_forceMultiplier);
				return;
			}
		}
		
		if (coll.gameObject.CompareTag("HealthKit"))
		{
			if (gameObject.layer == 9)
			{
				FindObjectOfType<Player>().MakeDamage(-25);
			}
			if (gameObject.layer == 10)
			{
				FindObjectOfType<Enemy>().MakeDamage(-25);
			}
			Destroy(coll.gameObject);
			Destroy(gameObject);
		}

		if (coll.gameObject.CompareTag("Enemy"))
		{
			coll.gameObject.GetComponent<Enemy>().MakeDamage(_damage);
			Destroy(gameObject);
		}
		
		if (coll.gameObject.CompareTag("Player"))
		{
			coll.gameObject.GetComponent<Player>().MakeDamage(_damage);
			Destroy(gameObject);
		}
	}
}