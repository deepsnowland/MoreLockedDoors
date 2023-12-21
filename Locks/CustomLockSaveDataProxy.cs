using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Il2Cpp;
using Il2CppSystem.Security.Cryptography.X509Certificates;

namespace MoreLockedDoors.Locks
{
    internal class CustomLockSaveDataProxy
    {

        public LockState m_LockState { get; set; }
        public ObjectGuid m_GUID { get; set; }
        public List<GearItem> m_ItemsUsedToForceLock { get; set; }
        public string m_LockedAudio { get; set; }
        public string m_Pair { get; set; }

        public CustomLockSaveDataProxy(LockState lockState, ObjectGuid gUID, List<GearItem> itemsUsedToForceLock, string lockedAudio, string pair)
        {
            m_LockState = lockState;
            m_GUID = gUID;
            m_ItemsUsedToForceLock = itemsUsedToForceLock;
            m_LockedAudio = lockedAudio;
            m_Pair = pair;
        }
    }
}
