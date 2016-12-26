public enum Scene1Events
{
    OnSceneLoad,                    //As the scene first loads, onto the image of the city
    OnShowCrashBeginning,           //As the scene switches to an image of the crashed car
    OnReverseCar,                   //As the scene switches to show car moving back in reverse
    OnTimeTransition,               //As the car finishes moving in reverse and time begins to move forward again
    OnCommandToPayAttention,        //COMMAND - As the inside of the car comes up, providing command to player to pay attention
    OnCrash,                        //As the player fails to pay attention and the car crashes
    OnShowCrashEnd,                 //As the scene switches back to showing the crashed car
    OnSceneEnd,                     //As the scene fades out
}
