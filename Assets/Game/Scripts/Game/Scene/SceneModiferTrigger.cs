using System.Collections;
using UnityEngine;

namespace Assets.Game.Scripts.Game.Scene
{
    public class SceneModiferTrigger : MonoBehaviour
    {
        public bool modify_disableDrawGun;
        public bool modify_disableDrawGun_value;

        public bool modifyCamera_fov;
        public float modifyCamera_fov_value;
        public float modifyCamera_fov_speed = 5;

    }
}