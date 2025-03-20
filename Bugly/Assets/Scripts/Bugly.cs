using UnityEngine;

namespace TEngine
{
    public class Bugly:MonoBehaviour
    {
        private void Awake()
        {
#if UNITY_IPHONE || UNITY_ANDROID
            BuglyManager.Instance.Init(Resources.Load<BuglyConfig>("BuglyConfig"));
            Destroy(this.gameObject);
#endif
        }
    }
}