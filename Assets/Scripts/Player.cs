using UnityEngine;

public class Player : Character
{
	protected override void FixBulletPosition(GameObject bullet)
	{
		bullet.transform.localPosition = new Vector2(gameObject.transform.localPosition.x + 75.0f,
			gameObject.transform.localPosition.y - 12.5f);
	}
}