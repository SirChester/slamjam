using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Character : MonoBehaviour
{
	
	
	[SerializeField] private float _movementCooldown;
	[SerializeField] private float _invulnerabilityCooldown = 0.3f;
	[SerializeField] private Weapon[] _weapons;
	[SerializeField] private Vector2 _initialPos;
	[SerializeField] private float _step;
	[SerializeField] private Transform _bulletPlace;
	[SerializeField] private float _chargeDefaultTime;
	[SerializeField] private int _maxHp;

	public Action OnHpChanged;

	public Vector2 PositionOnBoard;

	private float _jumpAnimationTime = 0.5f;
	
	private int HorLimit = 2;
	private int VertLimit = 4;
	private Dictionary<Weapon.Type, float> _lastShots = new Dictionary<Weapon.Type, float>();

	private int _hp;
	private bool _movementLocked;
	private float _chargeTime;
	private Coroutine _chargeCoroutine;

	public int Hp
	{
		get { return _hp; }
		set
		{
			_hp = value;
			if (OnHpChanged != null)
			{
				OnHpChanged();
			}
		}
	}

	public int MaxHp
	{
		get { return _maxHp; }
		set { _maxHp = value; }
	}

	public bool CanMove 
	{
		get { return !_movementLocked; }
		private set { _movementLocked = value; }
	}

	public float ChargeTime
	{
		get { return _chargeTime; }
	}

	public bool HasInvulnerability { get; set; }
//	public float HasInvulnerability { get; private set; }

	private void Awake()
	{
		for (var i = 0; i < _weapons.Length; ++i)
		{
			_lastShots.Add(_weapons[i].BulletType, 0.0f);
		}
		ResetChar();
	}
	
	public void ResetChar()
	{
		PositionOnBoard = new Vector2(1, 2);
		InitializePosition();
		Hp = _maxHp;
		_chargeTime = _chargeDefaultTime;
	}
	
	private IEnumerator PlayAnimation(int animationId)
	{
		var animator = GetComponent<Animator>();
		animator.SetInteger("State", 1);
		float animationTime;
		if (animationId == 1)
		{
			animationTime = _jumpAnimationTime;
		}
		else
		{
			animationTime = 0;
		}
		yield return new WaitForSeconds(animationTime);

		animator.SetInteger("State", 0);
	}

	public void Up()
	{
		if (PositionOnBoard.y > 0)
		{
			PositionOnBoard.y--;
			var pos = gameObject.transform.localPosition;
			pos.y += _step;
			gameObject.transform.localPosition= pos;

			StartCoroutine(PlayAnimation(1));
			StartCoroutine(LockMovement());
		}
	}

	public void Down()
	{
		if (PositionOnBoard.y < VertLimit)
		{
			PositionOnBoard.y++;
			var pos = gameObject.transform.localPosition;
			pos.y -= _step;
			gameObject.transform.localPosition= pos;
			StartCoroutine(PlayAnimation(1));
			StartCoroutine(LockMovement());
		}
	}

	public void Left()
	{
		if (PositionOnBoard.x > 0)
		{
			PositionOnBoard.x--;
			var pos = gameObject.transform.localPosition;
			pos.x -= _step;
			gameObject.transform.localPosition= pos;
			StartCoroutine(PlayAnimation(1));
			StartCoroutine(LockMovement());
		}
	}

	public void Right()
	{
		if (PositionOnBoard.x < HorLimit)
		{
			PositionOnBoard.x++;
			var pos = gameObject.transform.localPosition;
			pos.x += _step;
			gameObject.transform.localPosition= pos;
			StartCoroutine(PlayAnimation(1));
			StartCoroutine(LockMovement());
		}
	}
	
	private void Update()
	{
		GetComponent<SpriteRenderer>().enabled = !HasInvulnerability || Time.realtimeSinceStartup % 0.32 < 0.16;
	}
	
	private void InitializePosition()
	{
		gameObject.transform.localPosition = new Vector3(_initialPos.x, _initialPos.y, 0.0f);
	}
	
	public void ShootByIndex(int idx)
	{
		var obj = Weapon.ShootByType(_weapons[idx].BulletType, gameObject.name);
		obj.transform.position = _bulletPlace.position;
		var weapon = obj.GetComponent<Weapon>();
		_lastShots[weapon.BulletType] = Time.realtimeSinceStartup;
		StartCoroutine(PlayAnimation(2));
	}
	
	public void StartShootWithCharge()
	{
		_movementLocked = true;
		_chargeCoroutine = StartCoroutine(ShootWithChargeCoroutine());
	}
	
	public void StopShootWithCharge()
	{
		_movementLocked = false;
		_chargeTime = _chargeDefaultTime;
		if (_chargeCoroutine != null)
		{
			StopCoroutine(_chargeCoroutine);
		}
	}

	private IEnumerator ShootWithChargeCoroutine()
	{
		while (_chargeTime > 0)
		{
			yield return null;
			_chargeTime -= Time.deltaTime;
		}
		var obj = Weapon.ShootByType(_weapons[1].BulletType, gameObject.name);
		obj.transform.position = _bulletPlace.position;
		var weapon = obj.GetComponent<Weapon>();
		_lastShots[weapon.BulletType] = Time.realtimeSinceStartup;
		_movementLocked = false;
		_chargeTime = _chargeDefaultTime;
		StartCoroutine(PlayAnimation(2));
	}
	
	public void MakeDamage(int damage)
	{
		if (HasInvulnerability)
		{
			return;
		}
		
		Hp -= damage;
		StartCoroutine(ApplyInvul());
	}

	private IEnumerator ApplyInvul()
	{
		HasInvulnerability = true;
		yield return new WaitForSeconds(_invulnerabilityCooldown);
		HasInvulnerability = false;
	}

	private IEnumerator LockMovement()
	{
		_movementLocked = true;
		yield return new WaitForSeconds(_movementCooldown);
		_movementLocked = false;
	}

	public bool CanShootByIndex(int idx)
	{
		return Time.realtimeSinceStartup - _lastShots[_weapons[idx].BulletType] > _weapons[idx].Cooldown;
	}
	
	protected virtual void FixBulletPosition(GameObject bullet)
	{
	}
}
