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

	public Type BulletType
	{
		get { return _type; }
	}

	public float Cooldown
	{
		get { return _cooldown; }
	}

	public static GameObject ShootByType(Type type)
	{
		var path = "Prefabs/Rock";
		switch (type)
		{
			case Type.Paper:
				path = "Prefabs/Paper";
				break;
			case Type.Scissors:
				path = "Prefabs/Scissor";
				break;
		}
		return Instantiate(Resources.Load(path)) as GameObject;
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
			needDestroy |= _type == Type.Paper && collideBullet.BulletType == Type.Scissors;
			needDestroy |= _type == Type.Scissors && collideBullet.BulletType == Type.Rock;
			needDestroy |= _type == Type.Rock && collideBullet.BulletType == Type.Paper;
			if (needDestroy)
			{
				Destroy(gameObject);
				coll.gameObject.GetComponent<Rigidbody2D>().AddForce(Vector2.right * -_forceMultiplier);
				return;
			}
		}

		if (coll.gameObject.CompareTag("Enemy"))
		{
			coll.gameObject.GetComponent<Enemy>().MakeDamage(10);
			Destroy(gameObject);
		}
		
		if (coll.gameObject.CompareTag("Player"))
		{
			coll.gameObject.GetComponent<Player>().MakeDamage(10);
			Destroy(gameObject);
		}
	}
}