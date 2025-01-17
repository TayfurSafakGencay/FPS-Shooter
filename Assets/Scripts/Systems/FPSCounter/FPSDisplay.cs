using UnityEngine;

namespace Systems.FPSCounter
{
	public class FPSDisplay : MonoBehaviour
	{
		public static FPSDisplay Instance { get; private set; }
		
		private float deltaTime = 0.0f;

		private void Awake()
		{
			if (Instance == null) Instance = this;
			else Destroy(gameObject);
		}

		private void Update()
		{
			deltaTime += (Time.unscaledDeltaTime - deltaTime) * 0.1f;
		}

		public float GetFPS()
		{
			return 1.0f / deltaTime;
		}
	}
}