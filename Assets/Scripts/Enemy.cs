using UnityEngine;

public class Enemy : Character
{
	protected override void FixBulletPosition(GameObject bullet)
	{
		bullet.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - 55.0f,
			gameObject.transform.localPosition.y - 12.5f);
	}
}