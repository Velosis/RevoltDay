using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SaveDateNamespace
{
    [CreateAssetMenu(menuName = "Game_RevoltDay/SaveFile")]
    public class SaveData : ScriptableObject
    {
        public int tempInt;

        public void saveDelete()
        {
            tempInt = 0;
        }
    }
}


