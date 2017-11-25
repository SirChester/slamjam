using UnityEngine;

public class Bullet : MonoBehaviour
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
	
	private Vector2 _direction;

	public Type BulletType
	{
		get { return _type; }
	}

	public float Cooldown
	{
		get { return _cooldown; }
	}

	private Rigidbody2D _rb;

	private void Awake()
	{
		_rb = GetComponent<Rigidbody2D>();
	}

	public void PushTo(bool enemy)
	{
		_direction = enemy ? Vector2.right : Vector2.left;
		_rb.AddForce(_direction * _forceMultiplier);
	}

	private void OnCollisionEnter2D(Collision2D coll)
	{
		var collideBullet = coll.gameObject.GetComponent<Bullet>();
		if (collideBullet != null)
		{
			var needDestroy = _type == collideBullet.BulletType;
			needDestroy |= _type == Type.Paper && collideBullet.BulletType == Type.Scissors;
			needDestroy |= _type == Type.Scissors && collideBullet.BulletType == Type.Rock;
			needDestroy |= _type == Type.Rock && collideBullet.BulletType == Type.Paper;
			if (needDestroy)
			{
				Destroy(gameObject);
				coll.gameObject.GetComponent<Rigidbody2D>().AddForce(_direction * -_forceMultiplier);
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