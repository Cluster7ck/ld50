using System;
using UnityEngine;

public class PowerUp
{
	string name = "PowerUp";
	public PowerUp()
	{
		
	}
	public virtual void Power () {
		Debug.Log(name);
    }
}
