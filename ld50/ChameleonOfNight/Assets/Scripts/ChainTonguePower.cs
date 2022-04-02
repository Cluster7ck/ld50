using System;
using System.Collections.Generic;
using UnityEngine;


public class ChainTonguePower : PowerUp
{
	private string name = "ChainTongue";
	private float range = 0.2f;

	public ChainTonguePower(int level = 1)
	{
		range *= level * 0.2f;
	}

	public override void Power() {
		Debug.Log(name);
		List<Enemy> enemies = GameObject.g
    }
}
