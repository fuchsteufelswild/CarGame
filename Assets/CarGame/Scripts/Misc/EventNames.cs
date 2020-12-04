public static class LoadingEvents
{
    public const string LOADING_STARTED = "LOADING_STARTED";
    public const string LOADING_PROGRESS = "LOADING_PROGRESS";
    public const string LOADING_FINISHED = "LOADING_FINISHED";
}

public static class SaveEvents
{
    public const string SAVE_GAME_STATE = "SAVE_GAME_STATE";
    public const string LOADING_SAVE_COMPLETED = "LOADING_SAVE_COMPLETED";
    public const string GAME_RESET = "GAME_RESET";
}

public static class InputEvents
{
    public const string TURN_RIGHT = "TURN_RIGHT";
    public const string TURN_LEFT = "TURN_LEFT";
}

// Should have respective InputEvents for being processed by MovementController
public static class InputButtonEvents
{
    public const string TURN_RIGHT_BUTTON = "TURN_RIGHT_BUTTON";
    public const string TURN_LEFT_BUTTON = "TURN_LEFT_BUTTON";
}

public static class MissionEvents
{
    public const string START_NEW_WAVE = "START_WAVE";
    public const string WAVE_FINISHED = "WAVE_FINISHED";

    public const string WAVE_COMPLETE_SIGNAL = "WAWE_COMPLETE_SIGNAL";
    public const string WAVE_TIMER_PROGRESS = "WAVE_TIMER_PROGRESS";

    public const string PLAYER_BUMP = "PLAYER_BUMP";

    public const string GAME_PAUSED = "GAME_PAUSED";
    public const string GAME_UNPAUSED = "GAME_UNPAUSED";
}

public static class SceneEvents
{
    public const string SCENE_LOAD_STARTED = "SCENE_LOAD_STARTED";
    public const string SCENE_LOAD_FINISHED = "SCENE_LOAD_FINISHED";
}