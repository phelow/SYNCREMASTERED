﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using Affdex;

public class SetInstructionSprite : MonoBehaviour {
	private static SetInstructionSprite ms_instance;

	public enum FaceState
	{
		NEUTRAL,
		GRIN,
		BLINK,
		DISGUST,
		OPEN,
		FOCUS,
		JOY,
		ANGER,
		SURPRISE
	}

	[SerializeField]private Sprite m_neutral;
	[SerializeField]private Sprite [] m_blink;
	[SerializeField]private Sprite m_joy;
	[SerializeField]private Sprite m_anger;
	[SerializeField]private Sprite m_surprise;
	[SerializeField]private Sprite [] m_focus;
	[SerializeField]private Sprite [] m_open;

	[SerializeField]private Sprite [] m_angerSprites;
	[SerializeField]private Sprite [] m_joySprites;
	[SerializeField]private Sprite [] m_surpriseSprites;
	[SerializeField]private Sprite [] m_waitingForInputPetalSprites;

	private Sprite m_currentFace;

	private static bool ms_isInUse = false;

	private IEnumerator m_facePrompt;
	private IEnumerator m_faceIcon;
	private const float c_fadeFaceTime = 1.0f;


	[SerializeField]private Image m_waitingForFaceIndicator;

	[SerializeField]private Image m_faceImage;
	[SerializeField]private Image m_angerImage;
	[SerializeField]private Image m_surpriseImage;
	[SerializeField]private Image m_joyImage;
	[SerializeField]private Image m_popInImage;

	[SerializeField]private Image [] m_angerScreen;
	[SerializeField]private Image [] m_joyScreen;
	[SerializeField]private Image [] m_surpriseScreen;
	 
	[SerializeField]private Transform m_startPosition;
	[SerializeField]private Transform m_endPosition;

	private Color c_fadedOut = new Color(1.0f,1.0f,1.0f,0.0f);
	private Color c_fadedOutGrey = new Color(.5f,.5f,.5f,.5f);
	private Color c_fadedIn = new Color(1.0f,1.0f,1.0f,1.0f);

	private const float c_fadeTime = 1.0f;

	private bool m_isFaceVisible = false;

	[SerializeField]private float m_maxSize = 5.0f;

	private const float c_mouthWaitTime = 2.0f;



	// Use this for initialization
	void Awake () {
		ms_instance = this;
		Debug.Log (ms_instance);
		Debug.Log (49);
		m_faceIcon = BlinkIcon();
		m_facePrompt = BlinkPrompt ();
		m_currentFace=m_faceImage.sprite;
		//StartCoroutine (PauseRoutine ());
	}

	public static void GrowIcon(float dt){
		ms_instance.m_waitingForFaceIndicator.GetComponent<RectTransform>().localScale = Vector3.Lerp (ms_instance.m_waitingForFaceIndicator.GetComponent<RectTransform>().localScale , Vector3.one * ms_instance.m_maxSize,dt);
	}

	public static void ShrinkIcon(float dt){
		ms_instance.m_waitingForFaceIndicator.GetComponent<RectTransform>().localScale = Vector3.Lerp (ms_instance.m_waitingForFaceIndicator.GetComponent<RectTransform>().localScale , Vector3.one,dt);
	}

	public static void FaceUndetected(){
		ms_instance.m_isFaceVisible = false;
	}

	public static void FaceDetected(){
		ms_instance.m_isFaceVisible = true;
	}

	private bool m_fading = false;

	public static void HideIcon(){
		ms_instance.m_waitingForFaceIndicator.color = ms_instance.c_fadedOut;
	}

	public static void PopInWaitingForFaceIndicator(){
		ms_instance.StartCoroutine (ms_instance.PopInFadeIn ());
		ms_instance.StartCoroutine (ms_instance.PopInAnimate ());
	}

	private IEnumerator ScaleUpForTime(float targetTime){
		float t = 0.0f;

		while (t < targetTime) {
			yield return new WaitForEndOfFrame ();
			float dt = Time.deltaTime;
			GrowIcon (dt);
			t += Time.deltaTime;
		}
		t = 0.0f;
		while (ms_instance.m_waitingForFaceIndicator.GetComponent<RectTransform>().localScale != Vector3.one) {
			yield return new WaitForEndOfFrame ();
			float dt = Time.deltaTime;
			ShrinkIcon (dt);
			t += Time.deltaTime;

		}
	}
	private IEnumerator ScaleUpPopIn;
	public static void ScaleUpPopInIndicator(){
		try{
			ms_instance.StopCoroutine(ms_instance.ScaleUpPopIn);
		
		}
		catch{

		}
		ms_instance.ScaleUpPopIn = ms_instance.ScaleUpForTime (1.0f);

		ms_instance.StartCoroutine(ms_instance.ScaleUpPopIn);
	}

	private IEnumerator PopInAnimate(){
		while (true) {
			for (int i = 0; i < m_waitingForInputPetalSprites.Length; i++) {
				m_popInImage.sprite = m_waitingForInputPetalSprites [i];
				yield return new WaitForSeconds (.1f);
			}
		}
	}

	private IEnumerator PopInFadeIn(){

		float t = 0.0f;
		while (t <= c_fadeTime * Time.timeScale) {
			t += Time.deltaTime;
			m_popInImage.color = Color.Lerp ( c_fadedOut,c_fadedIn,  t/ Time.timeScale);
			yield return new WaitForEndOfFrame ();
		}
	}

	private IEnumerator FadeInIndicator(){
		m_fading = true;
		float t = 0.0f;
		m_waitingForFaceIndicator.transform.position = m_startPosition.transform.position;

		while (t <= c_fadeTime ) {
			t += Time.deltaTime* (1/Time.timeScale);
			m_waitingForFaceIndicator.transform.position = Vector3.Lerp (m_startPosition.transform.position, m_endPosition.transform.position, (t / c_fadeTime) * Time.timeScale);
			m_waitingForFaceIndicator.color = Color.Lerp (c_fadedOut, c_fadedIn, t/ Time.timeScale);
			yield return new WaitForEndOfFrame ();
		}


		m_fading = false;
	}

	public void Start(){
		StartCoroutine (AngerEffectAnimation ());


		StartCoroutine (JoyEffectAnimation ());


		StartCoroutine (SurpriseEffectAnimation ());
	}
	/*
	 * ms_instance.m_joyImage.color = Color.Lerp ( ms_instance.c_fadedOutGrey,ms_instance.c_fadedIn, joy / totalEmotion);
		ms_instance.m_angerImage.color = Color.Lerp ( ms_instance.c_fadedOutGrey,ms_instance.c_fadedIn, anger / totalEmotion);
		ms_instance.m_surpriseImage.color = Color.Lerp (ms_instance.c_fadedOutGrey,ms_instance.c_fadedIn,  surprise / totalEmotion);
	 * */
	private IEnumerator AngerEffectInterpolation(){
		while (true) {
			yield return new WaitForEndOfFrame ();
		}
	}
	Color empty = new Color(0.0f,0.0f,0.0f,0.0f);
	private IEnumerator AngerEffectAnimation(){
		while (true) {
			for (int i = 0; i < m_angerScreen.Length; i++) {
				int cur = i;
				int next = (i + 1) % m_angerScreen.Length;

				float t = 0;
				while(t < m_lerpScreenTime){
					Color maxColor = ms_instance.m_angerImage.color;
					m_angerScreen [cur].color = Color.Lerp (m_angerScreen [cur].color, empty, Time.deltaTime);
					m_angerScreen [next].color = Color.Lerp (m_angerScreen [next].color,maxColor, Time.deltaTime);
					t += Time.deltaTime;
					yield return new WaitForEndOfFrame ();
				}
			}
		}

	}

	[SerializeField]private float m_lerpScreenTime = 5.0f;

	private IEnumerator JoyEffectAnimation(){
		while (true) {
			for (int i = 0; i < m_joyScreen.Length; i++) {
				int cur = i;
				int next = (i + 1) % m_joyScreen.Length;
				float t = 0;
				while(t < m_lerpScreenTime){
					Color maxColor = ms_instance.m_joyImage.color;
					m_joyScreen [cur].color = Color.Lerp (m_joyScreen [cur].color, empty, Time.deltaTime);
					m_joyScreen [next].color = Color.Lerp (m_joyScreen [next].color,maxColor,   Time.deltaTime);
					t += Time.deltaTime;
					yield return new WaitForEndOfFrame ();
				}
			}
		}

	}


	private IEnumerator SurpriseEffectAnimation(){
		while (true) {
			for (int i = 0; i < m_surpriseScreen.Length; i++) {
				int cur = i;
				int next = (i + 1) % m_surpriseScreen.Length;
				float t = 0;
				while(t < m_lerpScreenTime){
					Color maxColor = ms_instance.m_surpriseImage.color;
					m_surpriseScreen [cur].color = Color.Lerp (m_surpriseScreen [cur].color, empty, Time.deltaTime);
					m_surpriseScreen [next].color = Color.Lerp (m_surpriseScreen [next].color,maxColor,   Time.deltaTime);
					t += Time.deltaTime;
					yield return new WaitForEndOfFrame ();
				}
			}
		}

	}

	bool paused = false;

	public static void SetInUse(){
		ms_isInUse = true;
	}

	public static void SetOutOfUse(){
		ms_isInUse = false;
	}

	public void Update(){
		if (m_isFaceVisible) {
			m_faceImage.sprite = m_currentFace;
			m_angerImage.sprite = m_anger;
			m_joyImage.sprite = m_joy;
			m_surpriseImage.sprite = m_surprise;
		} else {
			m_faceImage.sprite = m_neutral;
			m_angerImage.sprite = m_neutral;
			m_joyImage.sprite = m_neutral;
			m_surpriseImage.sprite = m_neutral;
		}

		if (ms_isInUse == false) {
			//lerp to gray
			/*
			ms_instance.m_joyImage.color = Color.Lerp ( ms_instance.m_joyImage.color,ms_instance.c_fadedOutGrey,Time.deltaTime);
			ms_instance.m_angerImage.color = Color.Lerp ( ms_instance.m_angerImage.color,ms_instance.c_fadedOutGrey,Time.deltaTime);
			ms_instance.m_surpriseImage.color = Color.Lerp (ms_instance.m_surpriseImage.color,ms_instance.c_fadedOutGrey,Time.deltaTime);
			*/
		}


	}

	public IEnumerator PauseRoutine(){
		while (true) {
			if (paused == false && Input.GetKeyDown (KeyCode.P)) {
				paused = true;
				Time.timeScale = 0.00f;
			} else if (Input.GetKeyDown (KeyCode.P)) {
				paused = false;
				Time.timeScale = 1.0f;
			}
			yield return null;
		}
	}

	private IEnumerator FadeOutIndicator(){
		m_fading = true;
		float t = 0.0f;
		Color origColor = m_waitingForFaceIndicator.color;
		Color origColor2 = m_popInImage.color;

		while (t <= c_fadeTime) {
			t += Time.deltaTime * (1/Time.timeScale);
			m_waitingForFaceIndicator.color = Color.Lerp ( origColor, c_fadedOut, t/Time.timeScale);
			m_popInImage.color= Color.Lerp ( origColor2, c_fadedOut, t/Time.timeScale);
			yield return new WaitForEndOfFrame ();
		}

		m_fading = false;
	}

	public static void StartWaitingForEmotion(SetInstructionSprite.FaceState fps =SetInstructionSprite.FaceState.NEUTRAL){

		SetInstructionSprite.SetWaitingForFaceIndicator (fps);
		ms_instance.StartCoroutine (ms_instance.FadeInIndicator());
	}

	public static void StopWaitingForEmotion(){
		ms_instance.StartCoroutine (ms_instance.FadeOutIndicator());
	}

	/*
	 * 
	public enum EmotionState
	{
		JOY,
		SADNESS,
		ANGER,
		SURPRISE,
		DISGUST
	}
	 * */



	private IEnumerator OpenPrompt(){
		
		
		for(int i =0; i < 999; i = (i+1)%ms_instance.m_open.Length) {
			ms_instance.m_waitingForFaceIndicator.sprite = ms_instance.m_open [i];
			yield return new WaitForSeconds (c_mouthWaitTime);
		}
	}

	private IEnumerator FocusPrompt(){

		for(int i =0; i < 999; i = (i+1)%ms_instance.m_focus.Length) {
			ms_instance.m_waitingForFaceIndicator.sprite = ms_instance.m_focus [i];
			yield return new WaitForSeconds (.2f);
		}
	}

	private IEnumerator BlinkPrompt(){

		for(int i =0; i < 999; i = (i+1)%ms_instance.m_blink.Length) {
			ms_instance.m_waitingForFaceIndicator.sprite = ms_instance.m_blink [i];
			yield return new WaitForSeconds (.2f);
		}
	}

	private IEnumerator OpenIcon(){

		for(int i =0; i < 999; i = (i+1)%ms_instance.m_open.Length) {
			yield return new WaitForSeconds (.2f);
			//ms_instance.m_faceImage.sprite = ms_instance.m_open [i];
		}
	}

	private IEnumerator FocusIcon(){

		for(int i =0; i < 999; i = (i+1)%ms_instance.m_focus.Length) {
			//ms_instance.m_faceImage.sprite = ms_instance.m_focus [i];
			yield return new WaitForSeconds (.2f);
		}
	}

	private IEnumerator BlinkIcon(){

		for(int i =0; i < 999; i = (i+1)%ms_instance.m_blink.Length) {
			//ms_instance.m_faceImage.sprite = ms_instance.m_blink [i];
			yield return new WaitForSeconds (.2f);
		}
	}

	public static void SetWaitingForFaceIndicator(FaceState es){



		ms_instance.StartCoroutine (ms_instance.FadeInIndicator());
		ms_instance.StopCoroutine (ms_instance.m_facePrompt);

		switch (es) {
		case FaceState.JOY:
			ms_instance.m_currentFace = ms_instance.m_joy;
			break;
		case FaceState.ANGER:
			ms_instance.m_currentFace = ms_instance.m_anger;
			break;
		case FaceState.SURPRISE:
			ms_instance.m_currentFace = ms_instance.m_surprise;
			break;
		case FaceState.OPEN:
			ms_instance.m_facePrompt = ms_instance.OpenPrompt ();
			ms_instance.StartCoroutine (ms_instance.m_facePrompt);

			break;
		case FaceState.FOCUS:
			ms_instance.m_facePrompt = ms_instance.FocusPrompt ();
			ms_instance.StartCoroutine (ms_instance.m_facePrompt);
			break;
		case FaceState.BLINK:
			ms_instance.m_facePrompt = ms_instance.BlinkPrompt ();
			ms_instance.StartCoroutine (ms_instance.m_facePrompt);
			break;
		case FaceState.NEUTRAL:
			ms_instance.m_facePrompt = ms_instance.AnimatePetalAtWaitingForFaceIndicator ();
			ms_instance.StartCoroutine (ms_instance.m_facePrompt);
			break;
		}
	}

	public IEnumerator AnimatePetalAtWaitingForFaceIndicator(){
		while (true) {
			for (int i = 0; i < m_waitingForInputPetalSprites.Length; i++) {
				m_waitingForFaceIndicator.sprite = m_waitingForInputPetalSprites [i];
				yield return new WaitForSeconds (.5f * (1/Time.timeScale));
			}
		}
	}

	public static void SetFaceRatios(float joy, float anger, float surprise){
		float totalEmotion = joy + anger + surprise + 1.0f;

		ms_instance.m_joyImage.color = Color.Lerp ( ms_instance.c_fadedOutGrey,ms_instance.c_fadedIn, joy / totalEmotion);
		ms_instance.m_angerImage.color = Color.Lerp ( ms_instance.c_fadedOutGrey,ms_instance.c_fadedIn, anger / totalEmotion);
		ms_instance.m_surpriseImage.color = Color.Lerp (ms_instance.c_fadedOutGrey,ms_instance.c_fadedIn,  surprise / totalEmotion);
	}

	public static void SetEmotionDeciding(){
		ms_instance.StartCoroutine (ms_instance.LoseHalfOpacity ());
	}


	public static void SetFaceState(FaceState fs){



		ms_instance.StopCoroutine (ms_instance.m_faceIcon);
		switch (fs) {
		case FaceState.JOY:
			ms_instance.StartCoroutine(ms_instance.FaceStateCoroutine(ms_instance.m_joy));
			break;
		case FaceState.ANGER:
			ms_instance.StartCoroutine(ms_instance.FaceStateCoroutine(ms_instance.m_anger));
			break;
		case FaceState.SURPRISE:
			ms_instance.StartCoroutine(ms_instance.FaceStateCoroutine(ms_instance.m_surprise));
			break;
		}
	}

	private IEnumerator LoseHalfOpacity(){
		float t = 0.0f;
		while (t < c_fadeFaceTime/4) {
			t += Time.deltaTime;
			ms_instance.m_faceImage.color = Color.Lerp (c_fadedIn, c_fadedOut, t / c_fadeFaceTime);
			yield return new WaitForEndOfFrame ();
		}
	}

	private IEnumerator FaceStateCoroutine(Sprite m_image){
		//Fade out the image
		float t = 0.0f;
		while (t < c_fadeFaceTime) {
			t += Time.deltaTime;

			ms_instance.m_faceImage.color = Color.Lerp (c_fadedIn, c_fadedOut, t / c_fadeFaceTime);
			yield return new WaitForEndOfFrame ();
		}

		//Change the image
		ms_instance.m_currentFace = m_image;
		ms_instance.m_faceImage.sprite = m_image;


		//Fade in the image
		t = 0.0f;
		while (t < c_fadeFaceTime) {
			t += Time.deltaTime;

			ms_instance.m_faceImage.color = Color.Lerp (c_fadedOut, c_fadedIn, t / c_fadeFaceTime);
			yield return new WaitForEndOfFrame ();
		}
	}
}
