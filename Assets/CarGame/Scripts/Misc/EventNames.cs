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
}

public static class InputEvents
{
}

public static class MissionEvents
{
    public const string START_NEW_WAVE = "START_WAVE";
    public const string WAVE_FINISHED = "WAVE_FINISHED";
}

public static class SceneEvents
{
    public const string SCENE_LOAD_STARTED = "SCENE_LOAD_STARTED";
    public const string SCENE_LOAD_FINISHED = "SCENE_LOAD_FINISHED";
}