using System.Collections;

public enum ManagerStatus
{
    NOT_INIT,
    LOADING,
    STARTED
}

public interface IGameManager
{
    IEnumerator Init();
    object GetData();
    void UpdateData(object data);
    bool IsReady();
    ManagerStatus GetStatus();
}
