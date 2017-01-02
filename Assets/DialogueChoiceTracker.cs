using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class DialogueChoiceTracker : MonoBehaviour {
	private static int ms_anger = 0;
	private static int ms_disgust = 0;
	private static int ms_joy = 0;
	private static int ms_sadness = 0;
	private static int ms_surprise = 0;


	private static float ms_angerIntensity = 0;
	private static float ms_joyIntensity = 0;
	private static float ms_surpriseIntensity = 0;



	private static float ms_angerIntensityBoost = 0;
	private static float ms_joyIntensityBoost = 0;
	private static float ms_surpriseIntensityBoost = 0;


	private static List<Indicator> ms_angerIndicators;
	private static List<Indicator> ms_joyIndicators;
	private static List<Indicator> ms_surpriseIndicators;

    private const float c_baselineMultiplier = 5.0f;
	private const float c_emissionMultiplier = 1.0f;
	private const float c_intensityMultiplier = 1.0f;
	private const float c_boostMultiplier = 10.0f;
	private const float c_baseEmissionMultiplier = .1f;

	[SerializeField]private float m_leftBound = -10.0f;
	[SerializeField]private float m_rightBound = 10.0f;
	[SerializeField]private float m_top = -10.0f;
	[SerializeField]private float m_bottom = 10.0f;
	[SerializeField]private float m_forwardMin = 10.0f;
	[SerializeField]private float m_forwardMax = 20.0f;

	private const float c_newSpawnMinimum = 5.0f;
    
	private IEnumerator DisplayOnScreenFeedback(){
		while (true) {
			//Reset intensity points
			CalculateTotalIntensity ();
			float angerPoints = ms_angerIntensity;
			float joyPoints = ms_joyIntensity;
			float surprisePoints = ms_surpriseIntensity;

			float totalAngerPoints = angerPoints;
			float totalJoyPoints = joyPoints;
			float totalSurprisePoints = surprisePoints;

			// Distribute intensity of feedback
			// Randomly distribute anger
			for (int i = 0; i < ms_angerIndicators.Count; i++) {
				float nextPoints = Random.Range (0.0f,totalAngerPoints / ms_angerIndicators.Count);
				angerPoints -= nextPoints;
				ms_angerIndicators [i].GiveIntensity (nextPoints * c_emissionMultiplier);
			}

			// Randomly distribute joy
			for (int i = 0; i < ms_joyIndicators.Count; i++) {
				float nextPoints = Random.Range (0.0f,totalJoyPoints / ms_joyIndicators.Count);
				joyPoints -= nextPoints;
				ms_joyIndicators [i].GiveIntensity (nextPoints * c_emissionMultiplier);
			}

			// Randomly distribute surprise
			for (int i = 0; i < ms_surpriseIndicators.Count; i++) {
				float nextPoints = Random.Range (0.0f,totalSurprisePoints / ms_surpriseIndicators.Count);
				surprisePoints -= nextPoints;
				ms_surpriseIndicators [i].GiveIntensity (nextPoints * c_emissionMultiplier);
			}
            
			yield return new WaitForEndOfFrame ();
		}
	}

	/// <summary>
	/// Used for adding temporary intensity while the player is making an emotion
	/// </summary>
	/// <param name="em">The affected emotion</param>
	/// <param name="intensity">IHow much intensity we're adding</param>
	public static void AddIntensityBoost(DialogueManager.Emotion em, float intensity){
		//add the intensity to the respective indicators.
		switch (em) {
		case DialogueManager.Emotion.Anger:
			for (int i = 0; i < ms_angerIndicators.Count; i++) {
				float nextPoints = Random.Range (0.0f,intensity / ms_angerIndicators.Count);
				intensity -= nextPoints;
				ms_angerIndicators [i].GiveIntensity (nextPoints * c_boostMultiplier);
			}
			break;
		case DialogueManager.Emotion.Joy:
			// Randomly distribute joy
			for (int i = 0; i < ms_joyIndicators.Count; i++) {
				float nextPoints = Random.Range (0.0f,intensity / ms_joyIndicators.Count);
				intensity -= nextPoints;
				ms_joyIndicators [i].GiveIntensity (nextPoints * c_boostMultiplier);
			}
			break;
		case DialogueManager.Emotion.Surprise:
			// Randomly distribute surprise
			for (int i = 0; i < ms_surpriseIndicators.Count; i++) {
				float nextPoints = Random.Range (0.0f,intensity / ms_surpriseIndicators.Count);
				intensity -= nextPoints;
				ms_surpriseIndicators [i].GiveIntensity (nextPoints * c_boostMultiplier);
			}
			break;
		}
	}
    

	private IEnumerator DecayIntensityBoost ()
	{
		while (true) {
			ms_angerIntensityBoost = Mathf.Lerp (ms_angerIntensityBoost, 0.0f, Time.deltaTime);
			ms_joyIntensityBoost = Mathf.Lerp (ms_joyIntensityBoost, 0.0f, Time.deltaTime);
			ms_surpriseIntensityBoost = Mathf.Lerp (ms_surpriseIntensityBoost, 0.0f, Time.deltaTime);
			yield return new WaitForEndOfFrame ();
		}
	}

	/// <summary>
	/// Calculates the baseline intensity for each emotion based off of the itmes that each emotin has been called.
	/// 
	/// ms_angerIntensity
	/// ms_joyIntensity
	/// ms_surpriseIntensity
	/// </summary>
	public static void CalculateTotalIntensity(){
		//add the intensity to the respective indicators.
		ms_angerIntensity = (ms_anger * c_baselineMultiplier + ms_angerIntensityBoost) * c_intensityMultiplier;
		ms_joyIntensity = (ms_joy * c_baselineMultiplier + ms_joyIntensityBoost) * c_intensityMultiplier;
		ms_surpriseIntensity = (ms_surprise * c_baselineMultiplier + ms_surpriseIntensityBoost) * c_intensityMultiplier;
	}

	public void Start(){

		ms_angerIndicators = new List<Indicator>();
		ms_joyIndicators = new List<Indicator>();
		ms_surpriseIndicators = new List<Indicator>();

		Debug.Log (ms_anger);
		Debug.Log (ms_joy);
		Debug.Log (ms_surprise);

		//StartCoroutine (DisplayOnScreenFeedback());
		//StartCoroutine (DecayIntensityBoost ());
	}

}
