using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace KvaGames.Asteroids
{
	[RequireComponent(typeof(Renderer))]
	public class ShipShield : MonoBehaviour
	{
		[SerializeField]
		private Color inactiveEmmition;
		[SerializeField]
		private Color safeEmmition;
		[SerializeField]
		private Color dangerEmmition;

		[SerializeField]
		private Renderer rend;
		[SerializeField]
		private Material mat;
		private Color currentColor;
		[SerializeField]
		private ShipShieldState state = ShipShieldState.inactive;

		public ShipShieldState State
		{
			get
			{
				return state;
			}

			set
			{
				state=value;
				if (value == ShipShieldState.inactive)
					currentColor = inactiveEmmition;
				else if(value == ShipShieldState.safe)
					currentColor = safeEmmition;
				else if(value == ShipShieldState.danger)
					currentColor = dangerEmmition;
			}
		}

		private void Awake( )
		{
			rend = rend ?? GetComponent<Renderer>();
			mat = mat ?? rend.material;
		}

		// Update is called once per frame
		void Update( )
		{
			Color c = Color.Lerp(mat.GetColor("_EmissionColor"), currentColor, Time.deltaTime*5);
			if(c.ToString() != currentColor.ToString())
				mat.SetColor("_EmissionColor", c);
		}
		private void OnValidate( )
		{
			State = state;
			rend = rend ?? GetComponent<Renderer>();
			mat = mat ?? rend.material;
			if(Application.isEditor)
				mat.SetColor("_EmissionColor", currentColor);
		}

	}
	public enum ShipShieldState { inactive, safe, danger}
}