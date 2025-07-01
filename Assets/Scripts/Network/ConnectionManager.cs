using UnityEngine;

public enum ServerType {
    DEV,
    PROD
}

public class ConnectionManager : MonoBehaviour
{
    [SerializeField] ServerType serverType;
    public string devServerHostname;
    public string prodServerHostname;

    public string Hostname => serverType == ServerType.DEV ? devServerHostname : prodServerHostname;

    public static ConnectionManager instance;

    void Awake(){
        if(instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this);
    }

}
