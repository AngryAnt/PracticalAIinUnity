using UnityEngine;
using System;


public class UtilityBehaviour : MonoBehaviour
{
	public static float Max (params float[] values)
	{
		if (values.Length < 1)
		{
			return float.NaN;
		}

		float max = values[0];
		Array.ForEach (values, value => max = Mathf.Max (value, max));

		return max;
	}


	public static float Min (params float[] values)
	{
		if (values.Length < 1)
		{
			return float.NaN;
		}

		float min = values[0];
		Array.ForEach (values, value => min = Mathf.Min (value, min));

		return min;
	}
}
