using UnityEngine;
using System.Collections;

public class CreditsEventSystem : MonoBehaviour {
	/*
	 * 
	 * 
		ConceptByYihao,
		ManagedByEric,
		HackedByWill,
		ArtByLuc,
		DevelopedAtRPI,
		SpecialThanksToRuiz,
		SpecialThanksToChang,
		ThanksToOurFriendsAndFamily */
	// Use this for initialization
	void Start () {
		StartCoroutine (CreditsCoroutine ());
	}

	private IEnumerator CreditsCoroutine(){
		DialogueManager.Main.DisplayCreditsText (DialogueManager.CreditsEvent.ConceptByYihao);
		yield return new WaitForSeconds (3.0f);
        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds (3.0f);
		DialogueManager.Main.DisplayCreditsText (DialogueManager.CreditsEvent.ManagedByEric);
		yield return new WaitForSeconds (3.0f);

        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds (3.0f);
		DialogueManager.Main.DisplayCreditsText (DialogueManager.CreditsEvent.HackedByWill);
		yield return new WaitForSeconds (3.0f);

        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds (3.0f);
		DialogueManager.Main.DisplayCreditsText (DialogueManager.CreditsEvent.ArtByLuc);
		yield return new WaitForSeconds (3.0f);

        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds (3.0f);
		DialogueManager.Main.DisplayCreditsText (DialogueManager.CreditsEvent.MusicByRob);
		yield return new WaitForSeconds (3.0f);

        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds (3.0f);
		DialogueManager.Main.DisplayCreditsText (DialogueManager.CreditsEvent.ThanksToAffectiva);
		yield return new WaitForSeconds (3.0f);

        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds (3.0f);
		DialogueManager.Main.DisplayCreditsText (DialogueManager.CreditsEvent.DevelopedAtRPI);
		yield return new WaitForSeconds (3.0f);

        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds (3.0f);
		DialogueManager.Main.DisplayCreditsText (DialogueManager.CreditsEvent.SpecialThanksToRuiz);
		yield return new WaitForSeconds (3.0f);

        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds (3.0f);
		DialogueManager.Main.DisplayCreditsText (DialogueManager.CreditsEvent.SpecialThanksToChang);
		yield return new WaitForSeconds (3.0f);

        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds (3.0f);
		DialogueManager.Main.DisplayCreditsText (DialogueManager.CreditsEvent.ThanksToOurFriendsAndFamily);
		yield return new WaitForSeconds (6.0f);

        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds (6.0f);
		Application.Quit ();

	}


	// Update is called once per frame
	void Update () {
	
	}
}
