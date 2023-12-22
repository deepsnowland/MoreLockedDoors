using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using Il2Cpp;
using UnityEngine;

namespace MoreLockedDoors.Locks
{
    internal class CustomLockSaveDataProxy
    {

        [JsonConverter(typeof(JsonStringEnumConverter))]
        public LockState m_LockState { get; set; }
        public List<string> m_ItemsUsedToForceLock { get; set; }
        public string m_LockedAudio { get; set; }
        public string m_Pair { get; set; }

        public CustomLockSaveDataProxy(LockState lockState, List<string> itemsUsedToForceLock, string lockedAudio, string pair)
        {
            m_LockState = lockState;
            m_ItemsUsedToForceLock = itemsUsedToForceLock;
            m_LockedAudio = lockedAudio;
            m_Pair = pair;
        }

        public CustomLockSaveDataProxy()
        {
        }

    }
}
