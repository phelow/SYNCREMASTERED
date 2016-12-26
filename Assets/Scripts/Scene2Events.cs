public enum Scene2Events
{
    OnSceneLoad,                    //As the scene first starts
    OnIntro,                       //As camera zooms out to reveal entire table (or third intro image in sequence)
    OnFirstConversation1,           //VOICEOVER: IVY (LOVER) - As you see lover across table and the conversation begins
    OnFirstConversation2,           //As first part of the conversation continues
    OnFirstConversation3,           //COMMAND - As first conversation command is given - show disgust
    OnSecondConversation1,          //VOICEOVER: IVY (LOVER) - As the lover responds to first interaction
    OnSecondConversation2,          //As main character reflects on first response
    OnSecondConversation3,          //As main character ponders what to do next
    OnSecondConversationCommand,    //COMMAND - As second conversation command is given - show joy
    OnThirdConversation1,           //VOICEOVER: IVY (LOVER) - As lover responds to second interaction
    OnThirdConversation2,           //As main character mocks the lover
    OnThirdConversation3,          //VOICEOVER: IVY (LOVER) - As lover finishes responding to third interaction
    OnWalkingOutside,               //As scene transitions to show them moving outside the restaurant
    OnSnowflakeCommand,             //COMMAND - As scene shifts to look up at the sky and command for snowflake mini-game is given
    OnSnowflakeCompletion,          //VOICEOVER: IVY (LOVER) - As the snowflake mini-game is completed
	OnCarCrash,
    OnEndScene,                    //As that transition scene concludes
}
