using UnityEngine.Events;

public class Events
{
    [System.Serializable] public class EventFadeComplete : UnityEvent<bool> { } //bool (true) = fade out
    [System.Serializable] public class EventGameState : UnityEvent<GameManager.GameState, GameManager.GameState> { }
    [System.Serializable] public class EventGameStateLose : UnityEvent<string> { }
    [System.Serializable] public class PlayerColorState: UnityEvent<ChangeColor.PlayerColor, ChangeColor.PlayerColor> { }
}
