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

	public int AmimationStatus = 0;

	public Action OnHpChanged;

	public Vector2 PositionOnBoard;

	private float _jumpAnimationTime = 0.5f;
	private float _attackAnimationTime = 0.7f;
	
	private int HorLimit = 2;
	private int VertLimit = 4;
	private Dictionary<Weapon.Type, float> _lastShots = new Dictionary<Weapon.Type, float>();

	private int _hp;
	private bool _movementLocked;
	private float _chargeTime;
	private Coroutine _movementCoroutine;
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

	public int ChargeIndex { get; private set; }

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
		if (_movementCoroutine != null)
		{
			StopCoroutine(_movementCoroutine);
			_movementCoroutine = null;
		}
		PositionOnBoard = new Vector2(1, 2);
		InitializePosition();
		Hp = _maxHp;
		_chargeTime = _chargeDefaultTime;
	}
	
	private IEnumerator PlayAnimation(int animationId)
	{
		var animator = GetComponent<Animator>();
		animator.SetInteger("State", animationId);
		AmimationStatus = animationId;
		float animationTime;
		if (animationId == 1)
		{
			animationTime = _jumpAnimationTime;
		} else if (animationId == 2)
		{
			animationTime = _attackAnimationTime;
		}
		else
		{
			animationTime = 0;
		}
		yield return new WaitForSeconds(animationTime);

		animator.SetInteger("State", 0);
		AmimationStatus = 0;
	}
	
	private IEnumerator SmoothChangePosition(Vector3 newPos)
	{
		var time = 0.0f;
		var tempPos = gameObject.transform.localPosition;
		while (time < _movementCooldown)
		{
			yield return null;
			var pos = gameObject.transform.localPosition;
			pos.x += (newPos.x - tempPos.x) * (time / _movementCooldown);
			pos.y += (newPos.y - tempPos.y) * (time / _movementCooldown);
			gameObject.transform.localPosition = pos;
			tempPos = gameObject.transform.localPosition;
			time += Time.deltaTime;
		}
		gameObject.transform.localPosition = newPos;
		_movementCoroutine = null;
	}

	private void ProcessMovement(Vector3 pos)
	{
		StartCoroutine(PlayAnimation(1));
		_movementCoroutine = StartCoroutine(SmoothChangePosition(pos));
		StartCoroutine(LockMovement());
	}

	public void Up()
	{
		if (PositionOnBoard.y > 0 && _movementCoroutine == null)
		{
			PositionOnBoard.y--;
			var pos = gameObject.transform.localPosition;
			pos.y += _step;
			ProcessMovement(pos);
		}
	}

	public void Down()
	{
		if (PositionOnBoard.y < VertLimit && _movementCoroutine == null)
		{
			PositionOnBoard.y++;
			var pos = gameObject.transform.localPosition;
			pos.y -= _step;
			ProcessMovement(pos);
		}
	}

	public void Left()
	{
		if (PositionOnBoard.x > 0 && _movementCoroutine == null)
		{
			PositionOnBoard.x--;
			var pos = gameObject.transform.localPosition;
			pos.x -= _step;
			ProcessMovement(pos);
		}
	}

	public void Right()
	{
		if (PositionOnBoard.x < HorLimit && _movementCoroutine == null)
		{
			PositionOnBoard.x++;
			var pos = gameObject.transform.localPosition;
			pos.x += _step;
			ProcessMovement(pos);
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
		ChargeIndex = 0;
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
			if (_chargeTime < _chargeDefaultTime / 2)
			{
				ChargeIndex = 1;
			}
		}
		ChargeIndex = 2;
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
