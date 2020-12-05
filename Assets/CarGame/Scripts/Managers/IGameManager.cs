/* Interface for managers that requires Initialization step
 * and may require saving/restoring in case of save/load action.
 */ 
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
