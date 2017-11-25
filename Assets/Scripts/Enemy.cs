using UnityEngine;

public class Enemy : Character
{
	protected override void InitializePosition()
	{
		PositionOnBoard = new Vector2(2, 0);
	}

	protected override void SetPosition()
	{
		var pos = new Vector2(PositionOnBoard.x * 50.0f, PositionOnBoard.y * -50.0f);
		GetComponent<RectTransform>().anchoredPosition = pos;
		base.SetPosition();
	}

	protected override void FixBulletPosition(GameObject bullet)
	{
		bullet.transform.localPosition = new Vector2(gameObject.transform.localPosition.x - 55.0f,
			gameObject.transform.localPosition.y - 12.5f);
	}
}