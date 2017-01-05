using UnityEngine;
using System.Collections;
using Affdex;

public class Scene51Script : MonoBehaviour
{
    private const float c_dialogInBetweenWaitTime = 7.0f;
    // Use this for initialization
    void Start()
    {
        StartCoroutine(SceneDialog());
    }

    private IEnumerator SceneDialog()
    {
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        yield return DialogueManager.Main.DisplayScene5Text(Scene5Events.OnSceneLoad, false);
        yield return new WaitForSeconds(3.0f);
        yield return DialogueManager.Main.FadeOutRoutine();
        yield return new WaitForSeconds(1.0f);
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();


        yield return new WaitForSeconds(1.0f);
        yield return DialogueManager.Main.DisplayScene5Text(Scene5Events.OnShowPark, false);
        
        yield return SetInstructionSprite.ms_instance.WaitForAnEmotionToBeSet();

        yield return new WaitForSeconds(1.0f);
        FadeInFadeOut.FadeOut();

    }
}
