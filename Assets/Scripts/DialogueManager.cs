using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Collections.Generic;

public class DialogueManager : MonoBehaviour
{

    //Used to access the display functions
    public static DialogueManager Main;

    private const bool c_typeWriterEffect = true;
    private const float c_typewriterSpeed = 1.0f;

    [SerializeField]
    private GameObject m_key;

    private IEnumerator m_revealSegment;

    //Reference to the Text object in the scene where the dialogue is displayed
    [SerializeField]
    Text txtDialogue;
    [SerializeField]
    Text txtDialogueShadow;
    [SerializeField]
    Text txtDialogueGlow;

    //Reference to the Game Object holding the text and image so it can be enabled/disabled
    [SerializeField]
    GameObject textContainer;
    CanvasGroup dialoguePopup;

    private static DialogueManager.Emotion m_emotion;
    private static bool inUse = false;
    private static bool m_facePassed = false;

    float fadeTime = 1f;

    private Color m_startColor;

    private const float c_timeMin = 0.3f;

    TextType m_dialog = TextType.Default;
    Color m_halfColor;
    private bool c_gameFestBuild = true;

    private bool m_dialogIconEnabled = false;

    public enum TextType
    {
        Default,
        Command,
        Dialog
    }

    public enum Emotion
    {
        Joy,
        Anger,
        Surprise
    }

    public enum CreditsEvent
    {
        ConceptByYihao,
        ManagedByEric,
        HackedByWill,
        ArtByLuc,
        MusicByRob,
        ArtByEnrico,
        DevelopedAtRPI,
        ThanksToAffectiva,
        SpecialThanksToRuiz,
        SpecialThanksToChang,
        ThanksToOurFriendsAndFamily
    }

    //Dictionaries containing dialogue lines, accessed via unique enums - one per scene
    Dictionary<TutorialEvents, string> tutorialDictionary;

    //Joy
    Dictionary<Scene0Events, string> scene0DictionaryJoy;
    Dictionary<Scene1Events, string> scene1DictionaryJoy;
    Dictionary<Scene2Events, string> scene2DictionaryJoy;
    Dictionary<Scene4Events, string> scene4DictionaryJoy;
    Dictionary<Scene5Events, string> scene5DictionaryJoy;
    Dictionary<GoodEndingEvents, string> goodEndingDictionaryJoy;
    Dictionary<BadEndingEvents, string> badEndingDictionaryJoy;

    //Anger
    Dictionary<Scene0Events, string> scene0DictionaryAnger;
    Dictionary<Scene1Events, string> scene1DictionaryAnger;
    Dictionary<Scene2Events, string> scene2DictionaryAnger;
    Dictionary<Scene4Events, string> scene4DictionaryAnger;
    Dictionary<Scene5Events, string> scene5DictionaryAnger;
    Dictionary<GoodEndingEvents, string> goodEndingDictionaryAnger;
    Dictionary<BadEndingEvents, string> badEndingDictionaryAnger;

    //Surprise
    Dictionary<Scene0Events, string> scene0DictionarySurprise;
    Dictionary<Scene1Events, string> scene1DictionarySurprise;
    Dictionary<Scene2Events, string> scene2DictionarySurprise;
    Dictionary<Scene4Events, string> scene4DictionarySurprise;
    Dictionary<Scene5Events, string> scene5DictionarySurprise;
    Dictionary<GoodEndingEvents, string> goodEndingDictionarySurprise;
    Dictionary<BadEndingEvents, string> badEndingDictionarySurprise;

    Dictionary<CreditsEvent, string> creditsDictionary;
    
    private float m_fadeInTime = 1.0f;

    [SerializeField]
    bool isBlack = true;

    void Start()
    {
        if (isBlack)
        {
            m_startColor = Color.black;

        }
        else {
            m_startColor = Color.white;
        }
    }

    //Instantiate needed assets
    void Awake()
    {
        //Hide the cursor
        UnityEngine.Cursor.visible = false;

        StartCoroutine(InterpolateDialog());
        m_revealSegment = RevealTextOneSegmentAtATime("");
        Main = GetComponent<DialogueManager>();

        dialoguePopup = textContainer.GetComponent<CanvasGroup>();
        dialoguePopup.alpha = 0f;

        tutorialDictionary = new Dictionary<TutorialEvents, string>();

        scene0DictionaryJoy = new Dictionary<Scene0Events, string>();
        scene1DictionaryJoy = new Dictionary<Scene1Events, string>();
        scene2DictionaryJoy = new Dictionary<Scene2Events, string>();
        scene4DictionaryJoy = new Dictionary<Scene4Events, string>();
        scene5DictionaryJoy = new Dictionary<Scene5Events, string>();
        goodEndingDictionaryJoy = new Dictionary<GoodEndingEvents, string>();
        badEndingDictionaryJoy = new Dictionary<BadEndingEvents, string>();

        scene0DictionaryAnger = new Dictionary<Scene0Events, string>();
        scene1DictionaryAnger = new Dictionary<Scene1Events, string>();
        scene2DictionaryAnger = new Dictionary<Scene2Events, string>();
        scene4DictionaryAnger = new Dictionary<Scene4Events, string>();
        scene5DictionaryAnger = new Dictionary<Scene5Events, string>();
        goodEndingDictionaryAnger = new Dictionary<GoodEndingEvents, string>();
        badEndingDictionaryAnger = new Dictionary<BadEndingEvents, string>();

        scene0DictionarySurprise = new Dictionary<Scene0Events, string>();
        scene1DictionarySurprise = new Dictionary<Scene1Events, string>();
        scene2DictionarySurprise = new Dictionary<Scene2Events, string>();
        scene4DictionarySurprise = new Dictionary<Scene4Events, string>();
        scene5DictionarySurprise = new Dictionary<Scene5Events, string>();
        goodEndingDictionarySurprise = new Dictionary<GoodEndingEvents, string>();
        badEndingDictionarySurprise = new Dictionary<BadEndingEvents, string>();

        creditsDictionary = new Dictionary<CreditsEvent, string>();

        BuildTutorialDictionary();
        BuildScene0Dictionary();
        BuildScene1Dictionary();
        BuildScene2Dictionary();
        BuildScene4Dictionary();
        BuildScene5Dictionary();
        BuildGoodEndingDictionary();
        BuildBadEndingDictionary();
        BuildCreditsDictionary();
    }

    public void CenterDialog()
    {

        txtDialogue.alignment = TextAnchor.MiddleLeft;
        txtDialogueShadow.alignment = TextAnchor.MiddleLeft;
        txtDialogueGlow.alignment = TextAnchor.MiddleLeft;
    }

    void BuildCreditsDictionary()
    {
        creditsDictionary.Add(CreditsEvent.ConceptByYihao, "Designed by Yihao Zhu");
        creditsDictionary.Add(CreditsEvent.ManagedByEric, "Written and produced by Eric Walsh");
        creditsDictionary.Add(CreditsEvent.HackedByWill, "Coded by William Pheloung");
        creditsDictionary.Add(CreditsEvent.ArtByLuc, "Art drawn by Luc Wong");
        creditsDictionary.Add(CreditsEvent.MusicByRob, "Music composed by Robert D. Bishop");
        if (c_gameFestBuild == false)
        {
            creditsDictionary.Add(CreditsEvent.ArtByEnrico, "Additional support from Enrico");
        }

        creditsDictionary.Add(CreditsEvent.ThanksToAffectiva, "Powered by Affectiva");
        creditsDictionary.Add(CreditsEvent.DevelopedAtRPI, "A game made at Rensselaer Polytechnic Institute");
        creditsDictionary.Add(CreditsEvent.SpecialThanksToRuiz, "With help from Professor Ruiz");
        creditsDictionary.Add(CreditsEvent.SpecialThanksToChang, "And help from Professor Chang");
        creditsDictionary.Add(CreditsEvent.ThanksToOurFriendsAndFamily, "Thanks to all our friends, family, and fraternity brothers at Alpha Sigma Phi and Sigma Chi who believed in us.");
    }

    void BuildTutorialDictionary()
    {

        tutorialDictionary.Add(TutorialEvents.Calibration, "SYNC uses the camera on your device to capture your facial expressions.\n\nPlease move yourself or your camera until you are comfortable and the icons remains solid, indicating it can pick up your face.\n");
        tutorialDictionary.Add(TutorialEvents.OpenMouthToContinue, "A symbol will appear on your screen when you need to act.\n\nIf you aren’t sure what you’re supposed to do, look to the text for help.\r\n\r\n Please open your mouth to continue.");
        tutorialDictionary.Add(TutorialEvents.PromptEmotions, "When you see the three face icons, you can move the main character to one of three different emotional states to direct the narrative: Happy, Angry, or Surprised.\n\nThe large face near the text shows the character's current emotional state, while the small faces below register YOUR emotional state while the main character's emotional state is being set.\n\nPlease open your mouth to continue.");
        tutorialDictionary.Add(TutorialEvents.PromptTutorial, "Register each emotional state (Joy, Anger, and Surprise) to continue.");
        tutorialDictionary.Add(TutorialEvents.MakeAnyFaceToStart, "Your choices over time will affect your experience, so choose wisely.\n\nRegister one of the three emotional states to start the game.\n");

    }

    //Populate all dialogue text needed for Scene 0
    void BuildScene0Dictionary()
    {
        scene0DictionaryJoy.Add(Scene0Events.OnSceneLoad, "Time is an eternal river, gradually carving out memories like eroded stone.");
        scene0DictionaryJoy.Add(Scene0Events.OnCommandToSmile, "I want to remember her as she was: her smile, her kiss, her touch.");
        scene0DictionaryJoy.Add(Scene0Events.OnDeadPetal, "Like a dying petal restored to life…");
        scene0DictionaryJoy.Add(Scene0Events.OnPetalRising, "Ascending on the wings of a divine breeze...");
        scene0DictionaryJoy.Add(Scene0Events.OnPetalInTree, "Until it rests once more in the boughs of its home…");
        scene0DictionaryJoy.Add(Scene0Events.PromptOnSceneEnd1, "What if I too could invert the world…");   //Prompt - the same for all emotions
        scene0DictionaryJoy.Add(Scene0Events.OnSceneEnd2, "...And return once more to her?");

        scene0DictionaryAnger.Add(Scene0Events.OnSceneLoad, "Time is a surging river, dragging me chained in its torrential wake.");
        scene0DictionaryAnger.Add(Scene0Events.OnCommandToSmile, "It’s not fair, none of this is fair! She should be sitting here, not me!");
        scene0DictionaryAnger.Add(Scene0Events.OnDeadPetal, "Like a dying petal denied its rest...");
        scene0DictionaryAnger.Add(Scene0Events.OnPetalRising, "Forced ever higher by an merciless breeze...");
        scene0DictionaryAnger.Add(Scene0Events.OnPetalInTree, "Until it trembles once more in its verdant cage...");
        scene0DictionaryAnger.Add(Scene0Events.PromptOnSceneEnd1, "What if I too could invert the world…"); //Prompt - the same for all emotions
        scene0DictionaryAnger.Add(Scene0Events.OnSceneEnd2, "...And redeem myself once more for her?");

        scene0DictionarySurprise.Add(Scene0Events.OnSceneLoad, "Time is a meandering river, a new revelation hidden beyond every serpentine bend.");
        scene0DictionarySurprise.Add(Scene0Events.OnCommandToSmile, "I can’t believe this is happening; I don’t want to imagine a world without her.");
        scene0DictionarySurprise.Add(Scene0Events.OnDeadPetal, "Like a dying petal mysteriously awoken…");
        scene0DictionarySurprise.Add(Scene0Events.OnPetalRising, "Tossed at random in a meaningless breeze...");
        scene0DictionarySurprise.Add(Scene0Events.OnPetalInTree, "Until it waits once more on the whim of fate...");
        scene0DictionarySurprise.Add(Scene0Events.PromptOnSceneEnd1, "What if I too could invert the world…");  //Prompt - the same for all emotions
        scene0DictionarySurprise.Add(Scene0Events.OnSceneEnd2, "...And find myself once more through her?");
    }

    //Populate all dialogue text needed for Scene 1
    void BuildScene1Dictionary()
    {
        scene1DictionaryJoy.Add(Scene1Events.OnSceneLoad, "What if I could go back to that peaceful city resting beneath its white pall?");
        scene1DictionaryJoy.Add(Scene1Events.OnShowCrashBeginning, "To a time when my dreams of a future still lived on through her.");
        scene1DictionaryJoy.Add(Scene1Events.OnReverseCar, "What if I could do it over? What if I could try again?");
        scene1DictionaryJoy.Add(Scene1Events.OnTimeTransition, "What if I...what if I could still save her?");
        scene1DictionaryJoy.Add(Scene1Events.OnCommandToPayAttention, "If my attention never wavers, then she will never die. I try my best not to blink.");
        scene1DictionaryJoy.Add(Scene1Events.OnCrash, "But of course, I have already done as much for her as I ever could.");
        scene1DictionaryJoy.Add(Scene1Events.OnShowCrashEnd, "There is no way to turn back the clock, yet still I can't help but wonder…"); //Prompt - the same for all emotions
        scene1DictionaryJoy.Add(Scene1Events.OnSceneEnd, "Did I at least make you happy?");

        scene1DictionaryAnger.Add(Scene1Events.OnSceneLoad, "What if I could go back to that wretched city rotting beneath its white pall?");
        scene1DictionaryAnger.Add(Scene1Events.OnShowCrashBeginning, "To the moment when my future with her was stolen in a flash of furious light.");
        scene1DictionaryAnger.Add(Scene1Events.OnReverseCar, "What if I could do it over? What if I could erase all my mistakes?");
        scene1DictionaryAnger.Add(Scene1Events.OnTimeTransition, "What if I...what if I could still fight for her?");
        scene1DictionaryAnger.Add(Scene1Events.OnCommandToPayAttention, "If my attention never wavers, then I will never fail her. I resist the urge to blink.");
        scene1DictionaryAnger.Add(Scene1Events.OnCrash, "But of course, I have already failed her as much as I ever could.");
        scene1DictionaryAnger.Add(Scene1Events.OnShowCrashEnd, "There is no way to turn back the clock, yet still I can't help but wonder…");  //Prompt - the same for all emotions
        scene1DictionaryAnger.Add(Scene1Events.OnSceneEnd, "Why couldn’t I have died instead?");

        scene1DictionarySurprise.Add(Scene1Events.OnSceneLoad, "What if I could go back to that ethereal city shifting beneath its white pall?");
        scene1DictionarySurprise.Add(Scene1Events.OnShowCrashBeginning, "To that blinking wreckage of a future that proved to be as ephemeral as the snow.");
        scene1DictionarySurprise.Add(Scene1Events.OnReverseCar, "What if I could do it over? What if I could uncover something new?");
        scene1DictionarySurprise.Add(Scene1Events.OnTimeTransition, "What if I...what if I could still rewrite her fate?");
        scene1DictionarySurprise.Add(Scene1Events.OnCommandToPayAttention, "If my attention never wavers, then who knows what will happen. I resist the urge to blink.");
        scene1DictionarySurprise.Add(Scene1Events.OnCrash, "But of course, I will never know how things might have been.");
        scene1DictionarySurprise.Add(Scene1Events.OnShowCrashEnd, "There is no way to turn back the clock, yet still I can't help but wonder…");    //Prompt - the same for all emotions
        scene1DictionarySurprise.Add(Scene1Events.OnSceneEnd, "What am I supposed to do now?");
    }

    //Populate all dialogue text needed for Scene 2
    void BuildScene2Dictionary()
    {
        scene2DictionaryJoy.Add(Scene2Events.OnSceneLoad, "She made me actually want to get up and dance.");
        scene2DictionaryJoy.Add(Scene2Events.OnIntro, "There’s no way I could have known to walk home that night instead of driving.");

        //neutral
        scene2DictionaryJoy.Add(Scene2Events.OnFirstConversation1, "\"But what about the cold? That wind is wicked and it’s practically a blizzard out there!\"");  //Prompt - the same for all emotions
        scene2DictionaryJoy.Add(Scene2Events.OnFirstConversation2, "I want to just sit and stare at her perfect face, memorizing every curving line.");
        //smile
        scene2DictionaryJoy.Add(Scene2Events.OnFirstConversation3, "Instead, I smile and ask if she really thinks I’d get us lost walking home.");
        scene2DictionaryJoy.Add(Scene2Events.OnSecondConversation1, "\"I mean, probably not, but why risk it? Besides, I thought you hated the cold?\""); //Prompt - the same for all emotions
        scene2DictionaryJoy.Add(Scene2Events.OnSecondConversation2, "She’s right of course; the cold reminds me of winters with my father.");   //Prompt - the same for all emotions
        //smile
        scene2DictionaryJoy.Add(Scene2Events.OnSecondConversation3, "But I tell her that that I could never be cold as long as I’m with her.");
        scene2DictionaryJoy.Add(Scene2Events.OnThirdConversation1, "\"Well...but what about the car? We drove here, remember?\"");    //Prompt - the same for all emotions
        //smile
        scene2DictionaryJoy.Add(Scene2Events.OnThirdConversation2, "When I tell her that all that matters to me tonight is her, she smiles and blushes.");
        scene2DictionaryJoy.Add(Scene2Events.OnThirdConversation3, "\"Ok, ok. You win Alex; I suppose freezing to death together IS kind of romantic.\"");    //Does not ask for input - follows previous line on timer
        scene2DictionaryJoy.Add(Scene2Events.OnWalkingOutside, "We walk and talk about things that don’t matter to anyone but us.");
        scene2DictionaryJoy.Add(Scene2Events.OnSnowflakeCommand, "When she stops to look up, opening her mouth, I laugh and join her.");
        scene2DictionaryJoy.Add(Scene2Events.OnSnowflakeCompletion, "\"I love you.\"");   //Prompt - the same for all emotions
        scene2DictionaryJoy.Add(Scene2Events.OnCarCrash, "The careening car took away my chance to say it back to her every day.");
        scene2DictionaryJoy.Add(Scene2Events.OnEndScene, "With no clear path forward, I long to think back further.");





        scene2DictionaryAnger.Add(Scene2Events.OnSceneLoad, "She made me stop caring what other people thought.");
        scene2DictionaryAnger.Add(Scene2Events.OnIntro, "How could I have been stupid enough to drive that night? We should have walked home instead.");
        //neutral
        scene2DictionaryAnger.Add(Scene2Events.OnFirstConversation1, "\"But what about the cold? That wind is wicked and it’s practically a blizzard out there!\"");   //Prompt - the same for all emotions
        scene2DictionaryAnger.Add(Scene2Events.OnFirstConversation2, "I want to shout a challenge to the entire world, daring them to even try to hurt her.");
        //frown
        scene2DictionaryAnger.Add(Scene2Events.OnFirstConversation3, "Instead, I glare and ask if she really thinks I’d get us lost walking home.");
        scene2DictionaryAnger.Add(Scene2Events.OnSecondConversation1, "\"I mean, probably not, but why risk it? Besides, I thought you hated the cold?\"");  //Prompt - the same for all emotions
        scene2DictionaryAnger.Add(Scene2Events.OnSecondConversation2, "She’s right of course; the cold reminds me of winters with my father.");     //Prompt - the same for all emotions
        //neutral
        scene2DictionaryAnger.Add(Scene2Events.OnSecondConversation3, "Not sure what else to say, I grumble that I’m allowed to change my mind.");
        scene2DictionaryAnger.Add(Scene2Events.OnThirdConversation1, "\"Well...but what about the car? We drove here, remember?\"");  //Prompt - the same for all emotions
        //frown
        scene2DictionaryAnger.Add(Scene2Events.OnThirdConversation2, "When I snap at her that the stupid car doesn’t matter, she frowns.");
        scene2DictionaryAnger.Add(Scene2Events.OnThirdConversation3, "\"Ok, ok. You win Alex; there’s no reason to get so upset over it.\"");    //Does not ask for input - follows previous line on timer
        scene2DictionaryAnger.Add(Scene2Events.OnWalkingOutside, "We talk and talk about things the world unfairly stole from us too soon.");
        scene2DictionaryAnger.Add(Scene2Events.OnSnowflakeCommand, "When she stops to look up, opening her mouth, I reluctantly join her.");
        scene2DictionaryAnger.Add(Scene2Events.OnSnowflakeCompletion, "\"I love you.\"");    //Prompt - the same for all emotions
        scene2DictionaryAnger.Add(Scene2Events.OnCarCrash, "The careening car meant she’d never hear it back as much as she deserved.");
        scene2DictionaryAnger.Add(Scene2Events.OnEndScene, "With no clear path forward, all I can do now is think back further.");




        scene2DictionarySurprise.Add(Scene2Events.OnSceneLoad, "She made me feel like I was loved, even when I was alone.");
        scene2DictionarySurprise.Add(Scene2Events.OnIntro, "Would we still be together if we’d walked home that night instead of driving?");
        //neutral
        scene2DictionarySurprise.Add(Scene2Events.OnFirstConversation1, "\"But what about the cold? That wind is wicked and it’s practically a blizzard out there!\""); //Prompt - the same for all emotions
        scene2DictionarySurprise.Add(Scene2Events.OnFirstConversation2, "I want to scream, grab her arm, and run away with her as fast and as far as I can.");
        //frown
        scene2DictionarySurprise.Add(Scene2Events.OnFirstConversation3, "Instead, I roll my eyes and ask if she really thinks I’d get us lost walking home.");
        scene2DictionarySurprise.Add(Scene2Events.OnSecondConversation1, "\"I mean, probably not, but why risk it? Besides, I thought you hated the cold?\"");    //Prompt - the same for all emotions
        scene2DictionarySurprise.Add(Scene2Events.OnSecondConversation2, "She’s right of course; the cold reminds me of winters with my father.");
        //neutral
        scene2DictionarySurprise.Add(Scene2Events.OnSecondConversation3, "I try to think of a clever response, but she was always the clever one. I just shrug.");
        scene2DictionarySurprise.Add(Scene2Events.OnThirdConversation1, "\"Well...but what about the car? We drove here, remember?\"");   //Prompt - the same for all emotions
        //smile
        scene2DictionarySurprise.Add(Scene2Events.OnThirdConversation2, "When I gasp and tell her I hadn’t considered the inanimate object’s feelings, she laughs.");
        scene2DictionarySurprise.Add(Scene2Events.OnThirdConversation3, "\"Ok, ok. You win Alex; I suppose the car will just have to settle for second best.\"");
        scene2DictionarySurprise.Add(Scene2Events.OnWalkingOutside, "We walk and talk about things that could’ve been in a future that never was.");
        scene2DictionarySurprise.Add(Scene2Events.OnSnowflakeCommand, "When she stops to look up, opening her mouth, I see no reason not to join her.");
        scene2DictionarySurprise.Add(Scene2Events.OnSnowflakeCompletion, "\"I love you.\"");  //Prompt - the same for all emotions
        scene2DictionarySurprise.Add(Scene2Events.OnCarCrash, "The careening car made it impossible for me to ever reply.");
        scene2DictionarySurprise.Add(Scene2Events.OnEndScene, "With no clear path forward, what else can I do but think back further?");
    }

    //Populate all dialogue text needed for Scene 4
    void BuildScene4Dictionary()
    {
        scene4DictionaryJoy.Add(Scene4Events.OnFirstJournal, "I tried again to save her, hoping against hope that this time, I’d succeed.");
        scene4DictionaryJoy.Add(Scene4Events.OnSecondJournal, "So I tried again. I’ll cherish every moment spent with her, but there’s no escaping reality.");
        scene4DictionaryJoy.Add(Scene4Events.OnThirdJournal, "So I tried again. Maybe I just need to learn to accept it, but it’s so hard to let her go.");
        scene4DictionaryJoy.Add(Scene4Events.OnSceneEnd, "Soon, there is only one memory left to save...");

        scene4DictionaryAnger.Add(Scene4Events.OnFirstJournal, "I tried again to save her, knowing she’d be stolen from me yet again.");
        scene4DictionaryAnger.Add(Scene4Events.OnSecondJournal, "So I tried again. I would do anything, sacrifice anything, if it meant bringing her back.");
        scene4DictionaryAnger.Add(Scene4Events.OnThirdJournal, "So I tried again. But it’s no use: I can’t fight back against Death itself.");
        scene4DictionaryAnger.Add(Scene4Events.OnSceneEnd, "Soon, there is only one memory left to vanquish...");

        scene4DictionarySurprise.Add(Scene4Events.OnFirstJournal, "I tried again to save her, convincing myself that things could be different.");
        scene4DictionarySurprise.Add(Scene4Events.OnSecondJournal, "So I tried again. But my hopes are never realized and change never comes.");
        scene4DictionarySurprise.Add(Scene4Events.OnThirdJournal, "So I tried again. I just don’t know what to do or think anymore.");
        scene4DictionarySurprise.Add(Scene4Events.OnSceneEnd, "Soon, there is only one memory left to explore...");
    }

    //Populate all dialogue text needed for Scene 5
    void BuildScene5Dictionary()
    {
        scene5DictionaryJoy.Add(Scene5Events.OnSceneLoad, "This used to be one of my favorite memories.");  //Prompt - the same for all emotions
        scene5DictionaryJoy.Add(Scene5Events.OnShowPark, "It still is, in fact, and always will be no matter what happens.");
        scene5DictionaryJoy.Add(Scene5Events.OnPlayerWaiting, "I wait, thinking about her and wondering if I should really do this.");
        scene5DictionaryJoy.Add(Scene5Events.OnLoverAppear, "\"Hey there. I noticed you just standing here by yourself, and you looked like you could maybe use some company. I’m Ivy.\"");    //Prompt - the same for all emotions
        scene5DictionaryJoy.Add(Scene5Events.OnPlayerDeciding, "This is my final chance to give up everything; if we’d never met, I would lose her completely, the good memories along with the bad.");
        scene5DictionaryJoy.Add(Scene5Events.OnCommandToSmile, "She stands there, smiling, and all I can think of is how good that smile used to make me feel. I know then what I have to do.");

        scene5DictionaryAnger.Add(Scene5Events.OnSceneLoad, "This used to be one of my favorite memories.");   //Prompt - the same for all emotions
        scene5DictionaryAnger.Add(Scene5Events.OnShowPark, "Now I can’t think of it without wanting to punch something.");
        scene5DictionaryAnger.Add(Scene5Events.OnPlayerWaiting, "I wait, fuming that it’s come to this, that this is the last recourse left to me.");
        scene5DictionaryAnger.Add(Scene5Events.OnLoverAppear, "\"Hey there. I noticed you just standing here by yourself, and you looked like you could maybe use some company. I’m Ivy.\"");     //Prompt - the same for all emotions
        scene5DictionaryAnger.Add(Scene5Events.OnPlayerDeciding, "This is my final chance to make things right; if we’d never met, that would’ve prevented all of this.");
        scene5DictionaryAnger.Add(Scene5Events.OnCommandToSmile, "She stands there, smiling, and all I can think of is how unfair it is that she’ll never smile like that again. I make my choice.");

        scene5DictionarySurprise.Add(Scene5Events.OnSceneLoad, "This used to be one of my favorite memories."); //Prompt - the same for all emotions
        scene5DictionarySurprise.Add(Scene5Events.OnShowPark, "There were so many ways that day could have gone.");
        scene5DictionarySurprise.Add(Scene5Events.OnPlayerWaiting, "I wait, trying to predict the full ramifications of this decision.");
        scene5DictionarySurprise.Add(Scene5Events.OnLoverAppear, "\"Hey there. I noticed you just standing here by yourself, and you looked like you could maybe use some company. I’m Ivy.\"");   //Prompt - the same for all emotions
        scene5DictionarySurprise.Add(Scene5Events.OnPlayerDeciding, "This is my final chance to start over; if we’d never met, everything would be so incredibly different.");
        scene5DictionarySurprise.Add(Scene5Events.OnCommandToSmile, "She stands there, smiling, and I marvel once more at the possibility of a new life without her. My course becomes clear.");
    }

    //Populate all dialogue text needed for the good ending where you move on
    void BuildGoodEndingDictionary()
    {
        goodEndingDictionaryJoy.Add(GoodEndingEvents.OnPlayerSmile, "I smile and know that this is the only way to truly honor her memory.");
        goodEndingDictionaryJoy.Add(GoodEndingEvents.OnAlexTalks, "\"Hi, I’m Alex. I WAS feeling a bit lost, but I think I’m good now.\"");
        goodEndingDictionaryJoy.Add(GoodEndingEvents.OnIvyResponds1, "\"Glad to hear it: being lost is never fun, especially on a day as gorgeous as today.\"");
        goodEndingDictionaryJoy.Add(GoodEndingEvents.OnIvyResponds2, "\"In fact, it’s SO gorgeous that I want a picture to remember it. Come on.\""); //Does not change across all 3 options
        goodEndingDictionaryJoy.Add(GoodEndingEvents.OnTakePicture, "I still have that picture of her: I don’t think she’s ever looked more radiant.");
        goodEndingDictionaryJoy.Add(GoodEndingEvents.OnReturnToStart1, "As long as I have my memories, her specter will linger, haunting me.");  //Does not change across all 3 options
        goodEndingDictionaryJoy.Add(GoodEndingEvents.OnReturnToStart2, "And that’s exactly how it should be.");
        goodEndingDictionaryJoy.Add(GoodEndingEvents.OnPetalBeginningToFall, "Like a blooming petal finally set free…");
        goodEndingDictionaryJoy.Add(GoodEndingEvents.OnPetalFalling, "Dancing and twirling as it descends…");
        goodEndingDictionaryJoy.Add(GoodEndingEvents.OnPetalLanding, "Until at long last it comes to rest…");
        goodEndingDictionaryJoy.Add(GoodEndingEvents.OnPetalDying1, "Perhaps I too can find my peace…");    //Does not change across all 3 options
        goodEndingDictionaryJoy.Add(GoodEndingEvents.OnPetalDying2, "In the memories I shared with her.");
        goodEndingDictionaryJoy.Add(GoodEndingEvents.Cheese, "\"Say Cheese!\"");

        goodEndingDictionaryAnger.Add(GoodEndingEvents.OnPlayerSmile, "I smile and feel something break inside me that should have broken a long time ago.");
        goodEndingDictionaryAnger.Add(GoodEndingEvents.OnAlexTalks, "\"Hi, I‘m Alex. I’m not lost! I’m just...figuring things out as I go.\"");
        goodEndingDictionaryAnger.Add(GoodEndingEvents.OnIvyResponds1, "\"Ah, a person after my own heart. What better time for soul-searching than on this gorgeous day?\"");
        goodEndingDictionaryAnger.Add(GoodEndingEvents.OnIvyResponds2, "\"In fact, it’s SO gorgeous that I want a picture to remember it. Come on.\"");   //Does not change across all 3 options
        goodEndingDictionaryAnger.Add(GoodEndingEvents.OnTakePicture, "I still have that picture of her: it’s beaten up and a little torn, but nothing stays perfect forever.");
        goodEndingDictionaryAnger.Add(GoodEndingEvents.OnReturnToStart1, "As long as I have my memories, her specter will linger, haunting me.");    //Does not change across all 3 options
        goodEndingDictionaryAnger.Add(GoodEndingEvents.OnReturnToStart2, "And that doesn’t bother me one bit.");
        goodEndingDictionaryAnger.Add(GoodEndingEvents.OnPetalBeginningToFall, "Like a blooming petal that’s lost it’s grip...");
        goodEndingDictionaryAnger.Add(GoodEndingEvents.OnPetalFalling, "Swirling about in a raging storm…");
        goodEndingDictionaryAnger.Add(GoodEndingEvents.OnPetalLanding, "Until at long last it settles down…");
        goodEndingDictionaryAnger.Add(GoodEndingEvents.OnPetalDying1, "Perhaps I too can find my peace…");  //Does not change across all 3 options
        goodEndingDictionaryAnger.Add(GoodEndingEvents.OnPetalDying2, "Without letting go of her.");
        goodEndingDictionaryAnger.Add(GoodEndingEvents.Cheese, "\"Say Cheese!\"");

        goodEndingDictionarySurprise.Add(GoodEndingEvents.OnPlayerSmile, "I smile and think about all the unknown paths forward now open to me, even as those behind me close for good.");
        goodEndingDictionarySurprise.Add(GoodEndingEvents.OnAlexTalks, "\"Hi, I’m Alex. Did I really look that lost?\"");
        goodEndingDictionarySurprise.Add(GoodEndingEvents.OnIvyResponds1, "\"I was worried you might start bawling any minute and ruin this gorgeous day.\"");
        goodEndingDictionarySurprise.Add(GoodEndingEvents.OnIvyResponds2, "\"In fact, it’s SO gorgeous that I want a picture to remember it. Come on.\"");    //Does not change across all 3 options
        goodEndingDictionarySurprise.Add(GoodEndingEvents.OnTakePicture, "I still have that picture of her: holding it, I can almost feel her with me again.");
        goodEndingDictionarySurprise.Add(GoodEndingEvents.OnReturnToStart1, "As long as I have my memories, her specter will linger, haunting me.");     //Does not change across all 3 options
        goodEndingDictionarySurprise.Add(GoodEndingEvents.OnReturnToStart2, "And I wouldn’t have it any other way.");
        goodEndingDictionarySurprise.Add(GoodEndingEvents.OnPetalBeginningToFall, "Like a blooming petal cast adrift…");
        goodEndingDictionarySurprise.Add(GoodEndingEvents.OnPetalFalling, "Tumbling down as the seasons change…");
        goodEndingDictionarySurprise.Add(GoodEndingEvents.OnPetalLanding, "Until at long last it embraces its fate…");
        goodEndingDictionarySurprise.Add(GoodEndingEvents.OnPetalDying1, "Perhaps I too can find my peace…");   //Does not change across all 3 options
        goodEndingDictionarySurprise.Add(GoodEndingEvents.OnPetalDying2, "In the choices I made with her.");
        goodEndingDictionarySurprise.Add(GoodEndingEvents.Cheese, "\");\n");
    }

    //Populate all dialogue text needed for the bad ending where you refuse to move on
    void BuildBadEndingDictionary()
    {
        badEndingDictionaryJoy.Add(BadEndingEvents.OnRefuseToSmile, "Despite my reservations, I press my lips together and avoid her eyes.");
        badEndingDictionaryJoy.Add(BadEndingEvents.OnIvyResponds, "\"Oh, um, ok. Sorry to bother you.\""); //Does not change across all 3 options
        badEndingDictionaryJoy.Add(BadEndingEvents.OnLoverGone, "Just that easily, I could have given her up in the name of some greater good.");
        badEndingDictionaryJoy.Add(BadEndingEvents.OnRewind1, "Even if I COULD change what’s already happened, doing so feels wrong.");
        badEndingDictionaryJoy.Add(BadEndingEvents.OnRewind2, "But here, now, for at least a moment, I take some small solace in the lie.");

        badEndingDictionaryAnger.Add(BadEndingEvents.OnRefuseToSmile, "I can’t even stand to look at her. I press my lips together and clench my fists.");
        badEndingDictionaryAnger.Add(BadEndingEvents.OnIvyResponds, "\"Oh, um, ok. Sorry to bother you.\"");  //Does not change across all 3 options
        badEndingDictionaryAnger.Add(BadEndingEvents.OnLoverGone, "Just that easily, I could have been rid of all this useless grief and guilt.");
        badEndingDictionaryAnger.Add(BadEndingEvents.OnRewind1, "If only I could do more to change the past than sit and think helplessly about it.");
        badEndingDictionaryAnger.Add(BadEndingEvents.OnRewind2, "But for now, whether right or wrong, thinking seems better than nothing.");

        badEndingDictionarySurprise.Add(BadEndingEvents.OnRefuseToSmile, "Knowing that being with her would only end in tragedy, I press my lips together and say nothing.");
        badEndingDictionarySurprise.Add(BadEndingEvents.OnIvyResponds, "\"Oh, um, ok. Sorry to bother you.\"");   //Does not change across all 3 options
        badEndingDictionarySurprise.Add(BadEndingEvents.OnLoverGone, "Just that easily, she could have been nothing but the faded memory of a dream.");
        badEndingDictionarySurprise.Add(BadEndingEvents.OnRewind1, "If only it were that easy to take the other road, to forge a different path.");
        badEndingDictionarySurprise.Add(BadEndingEvents.OnRewind2, "But instead, I’m trapped on this endless journey with no happy ending in sight.");
    }

    public IEnumerator TutorialCoroutine(TutorialEvents key, bool hold, Emotion emotion = Emotion.Joy, float HoldTime = 4.0f)
    {          
        if (!textContainer.activeSelf)
            textContainer.SetActive(true);

        string text = tutorialDictionary[key];
        
        StartCoroutine(SetFadeIn());
        yield return RevealTextOneSegmentAtATime(text);

        if (hold == false)
        {
            StartCoroutine(DelayToHideText(false, HoldTime));
        }
    }
    
    bool m_revelationMutex = false;

    private IEnumerator RevealTextOneSegmentAtATime(string remainingText)
    {
        Debug.Log("Starting to print text");
        string text = "";
        txtDialogue.text = text;
        txtDialogueShadow.text = text;
        txtDialogueGlow.text = text;
        //while not all of the text has been revealed
        while (remainingText.Length > 0)
        {
            //pick a key to reveal next
            int nextKeyLength = Random.Range(1, 5);
            if (nextKeyLength > remainingText.Length)
            {
                nextKeyLength = remainingText.Length;
            }
            int stop = nextKeyLength;

            string key = remainingText.Substring(0, stop);

            stop = remainingText.Length - nextKeyLength;
            if (stop < 0)
            {
                stop = 0;
            }

            remainingText = remainingText.Substring(nextKeyLength, stop);
            text += key;


            txtDialogue.text = text;
            txtDialogueShadow.text = text;
            txtDialogueGlow.text = text;
            //instantiate a random amount of keys

            int nKeys = Random.Range(0, 10);

            for (int i = 0; i < nKeys; i++)
            {
                GameObject go_key = GameObject.Instantiate(m_key);
                go_key.GetComponent<Key>().setText(key);
                go_key.transform.SetParent(txtDialogue.transform.parent);
                go_key.transform.localPosition = new Vector3(txtDialogue.transform.localPosition.x + Random.Range(-1000.0f, 1000.0f), txtDialogue.transform.localPosition.y + Random.Range(-200.0f, 200.0f), txtDialogue.transform.localPosition.z + Random.Range(-100.0f, 100.0f));
            }


            //wait a set amount of time
            yield return new WaitForSeconds(Random.Range(0.02f, .04f) * Time.timeScale);
        }
    }
    public IEnumerator Scene0Coroutine(Scene0Events key, bool hold, Emotion emotion = Emotion.Joy, float holdTime = 4.0f)
    {
        emotion = DialogueManager.GetCurrentEmotion();
        if (!textContainer.activeSelf)
            textContainer.SetActive(true);

        string text = scene0DictionaryJoy[key];
        yield return SetInstructionSprite.ms_instance.FadeInDialogImage();
        switch (emotion)
        {
            case Emotion.Anger:
                text = scene0DictionaryAnger[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.red, .5f);
                break;
            case Emotion.Joy:
                text = scene0DictionaryJoy[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.green, .5f);
                break;
            case Emotion.Surprise:
                text = scene0DictionarySurprise[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.yellow, .5f);
                break;
        }
        
        StartCoroutine(SetFadeIn());
        yield return RevealTextOneSegmentAtATime(text);
        yield return SetInstructionSprite.ms_instance.FadeOutDialogImage();
    }
    
    //Public function accessed through DialogueManager.Main to display text for a specific event in Scene 1
    public IEnumerator DisplayScene1Text(Scene1Events key, bool hold = false, Emotion emotion = Emotion.Joy, TextType dialogType = TextType.Default, float holdTime = 4.0f)
    {
        emotion = DialogueManager.GetCurrentEmotion();
        m_dialog = dialogType;

        yield return SetInstructionSprite.ms_instance.FadeInDialogImage();
        string text = scene1DictionaryJoy[key];
        EnableDialogIcon();
        switch (emotion)
        {
            case Emotion.Anger:
                text = scene1DictionaryAnger[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.red, .5f);
                break;
            case Emotion.Joy:
                text = scene1DictionaryJoy[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.green, .5f);
                break;
            case Emotion.Surprise:
                text = scene1DictionarySurprise[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.yellow, .5f);
                break;
        }

        StartCoroutine(SetFadeIn());
        yield return RevealTextOneSegmentAtATime(text);
        yield return SetInstructionSprite.ms_instance.FadeOutDialogImage();
    }



    //Public function accessed through DialogueManager.Main to display text for a specific event in Scene 2
    public IEnumerator DisplayScene2Text(Scene2Events key, bool hold = false, Emotion emotion = Emotion.Joy, TextType dialogType = TextType.Default, float holdTime = 4.0f)
    {
        emotion = DialogueManager.GetCurrentEmotion();
        m_halfColor = Color.yellow;
        m_dialog = dialogType;
        if (!textContainer.activeSelf)
            textContainer.SetActive(true);

        string text = scene2DictionaryJoy[key];
        yield return SetInstructionSprite.ms_instance.FadeInDialogImage();

        EnableDialogIcon();
        switch (emotion)
        {
            case Emotion.Anger:
                text = scene2DictionaryAnger[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.red, .5f);
                break;
            case Emotion.Joy:
                text = scene2DictionaryJoy[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.green, .5f);
                break;
            case Emotion.Surprise:
                text = scene2DictionarySurprise[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.yellow, .5f);
                break;
        }
        StartCoroutine(SetFadeIn());
        yield return RevealTextOneSegmentAtATime(text);
        yield return SetInstructionSprite.ms_instance.FadeOutDialogImage();

    }

    private IEnumerator InterpolateDialog()
    {
        Debug.Log("InterpolateDialog called");
        while (true)
        {
            if (m_dialog == TextType.Command)
            {

                EnableDialogIcon();
                Debug.Log("Interpolating");
                //Interpolate to half color
                float t = 0.0f;

                float fadeInTime = Random.Range(0.3f, 3.0f);
                float fadeOutTime = Random.Range(0.3f, 3.0f);
                Color rColor = new Color(Random.Range(0.0f, 1.0f), 0.0f, 0.0f);

                while (t < fadeInTime * Time.timeScale)
                {
                    t += Time.deltaTime;
                    txtDialogue.color = Color.Lerp(m_startColor, rColor, t / (fadeInTime * Time.timeScale));

                    yield return new WaitForEndOfFrame();
                }
                t = 0.0f;

                //Interpolate back to regular color
                while (t < fadeInTime * Time.timeScale)
                {
                    t += Time.deltaTime;
                    txtDialogue.color = Color.Lerp(rColor, m_startColor, t / (fadeInTime * Time.timeScale));
                    yield return new WaitForEndOfFrame();
                }
            }
            else if (m_dialog == TextType.Dialog)
            {
                Debug.Log("Interpolating");
                //Interpolate to half color
                float t = 0.0f;

                Color rColor = Color.Lerp(Color.yellow, Color.white, Random.Range(0.0f, 1.0f));//new Color (0.0f, Random.Range (0.0f, 1.0f), 0.0f);
                float fadeInTime = Random.Range(0.3f, 3.0f);
                float fadeOutTime = Random.Range(0.3f, 3.0f);

                while (t < fadeInTime * Time.timeScale)
                {
                    t += Time.deltaTime;
                    txtDialogue.color = Color.Lerp(m_startColor, rColor, t / (fadeInTime * Time.timeScale));
                    Debug.Log(txtDialogue.color);
                    yield return new WaitForEndOfFrame();
                }
                t = 0.0f;

                //Interpolate back to regular color
                while (t < fadeInTime * Time.timeScale)
                {
                    t += Time.deltaTime;
                    txtDialogue.color = Color.Lerp(rColor, m_startColor, t / (fadeInTime * Time.timeScale));
                    yield return new WaitForEndOfFrame();
                }

            }

            else {
                txtDialogue.color = m_startColor;
            }
            yield return new WaitForEndOfFrame();
        }
    }

    public IEnumerator FadeOutRoutine()
    {
        SetInstructionSprite.StopWaitingForEmotion();
        float t = m_fadeInTime;


        SetInstructionSprite.ms_instance.FadeOutDialogImage();


        while (t > 0.0f)
        {
            t -= Time.deltaTime;
            
            dialoguePopup.alpha = t / m_fadeInTime;
            
            yield return new WaitForEndOfFrame();

        }


        txtDialogue.text = "";
        txtDialogueShadow.text = "";
        txtDialogueGlow.text = "";
        m_dialogIconEnabled = false;
    }
    
    public void FadeOutIconOnly()
    {
        SetInstructionSprite.ms_instance.FadeOutDialogImage();
        m_dialogIconEnabled = false;
    }

    public void EnableDialogIcon()
    {
        m_dialogIconEnabled = true;
    }

    //Public function accessed through DialogueManager.Main to display text for a specific event in Scene 4
    public IEnumerator DisplayScene4Text(Scene4Events key, bool hold = true, Emotion emotion = Emotion.Joy, TextType dialogType = TextType.Default, float holdTime = 4.0f)
    {

        emotion = DialogueManager.GetCurrentEmotion();
        EnableDialogIcon();
        m_halfColor = Color.yellow;
        m_dialog = dialogType;
        if (!textContainer.activeSelf)
            textContainer.SetActive(true);

        string text = scene4DictionaryJoy[key];
        yield return SetInstructionSprite.ms_instance.FadeInDialogImage();

        switch (emotion)
        {
            case Emotion.Anger:
                text = scene4DictionaryAnger[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.red, .5f);
                break;
            case Emotion.Joy:
                text = scene4DictionaryJoy[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.green, .5f);
                break;
            case Emotion.Surprise:
                text = scene4DictionarySurprise[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.yellow, .5f);
                break;
        }

        StopCoroutine(m_revealSegment);
        m_revealSegment = RevealTextOneSegmentAtATime(text);
        StartCoroutine(m_revealSegment);
        txtDialogue.text = "";
        txtDialogueShadow.text = "";
        txtDialogueGlow.text = "";


        StartCoroutine(SetFadeIn());
        if (hold == false)
        {
            StartCoroutine(DelayToHideText(false, holdTime));
        }
    }

    //Public function accessed through DialogueManager.Main to display text for a specific event in Scene 5
    public IEnumerator DisplayScene5Text(Scene5Events key, bool slowmo = false, Emotion emotion = Emotion.Joy, bool hold = false, TextType dialog = TextType.Default, float holdTime = 4.0f)
    {
        emotion = DialogueManager.GetCurrentEmotion();
        EnableDialogIcon();
        m_dialog = dialog;
        m_slowmo = slowmo;
        if (!textContainer.activeSelf)
            textContainer.SetActive(true);
        string text = scene5DictionaryJoy[key];
        yield return SetInstructionSprite.ms_instance.FadeInDialogImage();
        switch (emotion)
        {
            case Emotion.Anger:
                text = scene5DictionaryAnger[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.red, .5f);
                break;
            case Emotion.Joy:
                text = scene5DictionaryJoy[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.green, .5f);
                break;
            case Emotion.Surprise:
                text = scene5DictionarySurprise[key];
                txtDialogue.color = Color.Lerp(txtDialogue.color, Color.yellow, .5f);
                break;
        }



        txtDialogue.text = "";
        txtDialogueShadow.text = "";
        txtDialogueGlow.text = "";
        StartCoroutine(SetFadeIn());
        yield return RevealTextOneSegmentAtATime(text);
        yield return SetInstructionSprite.ms_instance.FadeOutDialogImage();
    }

    //Public function accessed through DialogueManager.Main to display text for a specific event in the good ending
    public IEnumerator DisplayGoodEndingText(GoodEndingEvents key, bool slowmo = false, Emotion emotion = Emotion.Joy, bool hold = false, TextType tt = TextType.Default, float holdTime = 4.0f)
    {

        emotion = DialogueManager.GetCurrentEmotion();
        EnableDialogIcon();
        m_dialog = tt;
        if (!textContainer.activeSelf)
            textContainer.SetActive(true);

        string text = goodEndingDictionaryJoy[key];

        yield return SetInstructionSprite.ms_instance.FadeInDialogImage();
        switch (emotion)
        {
            case Emotion.Anger:
                text = goodEndingDictionaryAnger[key];
                break;
            case Emotion.Joy:
                text = goodEndingDictionaryJoy[key];
                break;
            case Emotion.Surprise:
                text = goodEndingDictionarySurprise[key];
                break;
        }


        txtDialogue.text = "";
        txtDialogueShadow.text = "";
        txtDialogueGlow.text = "";
        StartCoroutine(SetFadeIn());
        yield return RevealTextOneSegmentAtATime(text);
        yield return SetInstructionSprite.ms_instance.FadeOutDialogImage();
    }


    //public void Update()
    //{
    //    if (Input.GetKey(KeyCode.LeftShift) && Input.GetKey(KeyCode.RightShift) && Input.GetKey(KeyCode.Space))
    //    {
    //        Time.timeScale = 30.0f;
    //    }
        
    //}

    //Public function accessed through DialogueManager.Main to display text for a specific event in the bad ending
    public IEnumerator DisplayBadEndingText(BadEndingEvents key, bool slowmo = false, Emotion emotion = Emotion.Joy, bool hold = false, float holdTime = 4.0f)
    {

        emotion = DialogueManager.GetCurrentEmotion();
        EnableDialogIcon();
        if (!textContainer.activeSelf)
            textContainer.SetActive(true);

        string text = badEndingDictionaryJoy[key];
        yield return SetInstructionSprite.ms_instance.FadeInDialogImage();

        switch (emotion)
        {
            case Emotion.Anger:
                text = badEndingDictionaryAnger[key];
                break;
            case Emotion.Joy:
                text = badEndingDictionaryJoy[key];
                break;
            case Emotion.Surprise:
                text = badEndingDictionarySurprise[key];
                break;
        }

        txtDialogue.text = "";
        txtDialogueShadow.text = "";
        txtDialogueGlow.text = "";
        StartCoroutine(SetFadeIn());
        yield return RevealTextOneSegmentAtATime(text);
        yield return SetInstructionSprite.ms_instance.FadeOutDialogImage();
    }

    //Public function accessed through DialogueManager.Main to display text for a specific event in the bad ending
    public IEnumerator DisplayCreditsText(CreditsEvent key, bool slowmo = false, Emotion emotion = Emotion.Joy, bool hold = false, float holdTime = 4.0f)
    {        
        EnableDialogIcon();
        if (!textContainer.activeSelf)
            textContainer.SetActive(true);

        string text = creditsDictionary[key];


        txtDialogue.text = "";
        txtDialogueShadow.text = "";
        txtDialogueGlow.text = "";
        StartCoroutine(SetFadeIn());
        yield return RevealTextOneSegmentAtATime(text);
        yield return SetInstructionSprite.ms_instance.FadeOutDialogImage();
    }

    private static bool m_slowmo = false;
    public void ActivateSlowmo()
    {
        m_slowmo = true;
    }

    IEnumerator DelayToHideText(bool slowmo = false, float holdTime = 4.0f)
    {
        m_slowmo = slowmo;
        float t = 0.0f;
        while (t < holdTime)
        {
            t += Time.deltaTime * (1.0f / Time.timeScale);
            yield return new WaitForEndOfFrame();
        }
        yield return DialogueManager.Main.FadeOutRoutine();
    }

    public static bool CanGetCurrentEmotion()
    {
        return m_facePassed;
    }

    public static DialogueManager.Emotion GetCurrentEmotion()
    {
        return m_emotion;
    }

    private static Emotion m_lastEmotion = Emotion.Joy;

    public static Emotion GetLastEmotion()
    {
        return m_lastEmotion;
    }

    public static void SetCurrentEmotion(Emotion emotion)
    {
        m_emotion = emotion;
        m_facePassed = true;
        m_lastEmotion = emotion;
    }

    public static void SetLastEmotion(Emotion emotion)
    {
        m_lastEmotion = emotion;
    }


    public static void DisableCurrentEmotion()
    {
        m_facePassed = false;
    }


    IEnumerator SetFadeIn()
    {
        //TODO: show emotion text icon here
        
        float t = 0.0f;

        if (m_dialogIconEnabled)
        {
            SetInstructionSprite.ms_instance.FadeInDialogImage();
        }

        while (t < m_fadeInTime)
        {
            t += Time.deltaTime;
            
            dialoguePopup.alpha = t / m_fadeInTime;

            yield return new WaitForEndOfFrame();

        }


        yield return null;

    }
}
