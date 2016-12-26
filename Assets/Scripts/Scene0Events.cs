
public enum Scene0Events {
    OnSceneLoad,        //As the game first starts and the scene fades in
    OnCommandToSmile,   //COMMAND - As the petal loads, providing first instruction to the player to bring petal back to life
    OnDeadPetal,        //As the player begins to smile to restore the petal
    OnPetalRising,      //As the petal is finally restored and begins to rise up into the air
    OnPetalInTree,      //As the petal reaches the tree limbs at the top
    PromptOnSceneEnd1,        //As the petal settles into the tree and the scene begins to end
    OnSceneEnd2,        //Final bit of dialogue as scene fades to black
}
