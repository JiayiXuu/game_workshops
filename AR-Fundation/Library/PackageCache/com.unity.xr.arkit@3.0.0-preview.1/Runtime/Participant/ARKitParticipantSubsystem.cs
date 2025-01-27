using System;
using System.Runtime.InteropServices;
using Unity.Collections;
using Unity.Collections.LowLevel.Unsafe;
using UnityEngine.Scripting;
using UnityEngine.XR.ARSubsystems;

#if !UNITY_2019_2_OR_NEWER
using UnityEngine.Experimental;
#endif

namespace UnityEngine.XR.ARKit
{
    /// <summary>
    /// ARKit implementation of the <c>XRParticipantSubsystem</c>. Do not create this using <c>new</c>. Instead, use the
    /// <a href="https://docs.unity3d.com/ScriptReference/SubsystemManager.GetSubsystemDescriptors.html">SubsystemManager</a>
    /// to enumerate the available <see cref="XRParticipantSubsystemDescriptor"/>s and get or create an instance from the descriptor.
    /// </summary>
    [Preserve]
    public sealed class ARKitParticipantSubsystem : XRParticipantSubsystem
    {
        protected override IProvider CreateProvider()
        {
            return new Provider();
        }

        class Provider : IProvider
        {
            IntPtr m_Ptr;

            bool created => m_Ptr != IntPtr.Zero;

            public Provider()
            {
                m_Ptr = UnityARKit_Participant_init();
            }

            public override void Start()
            {
                if (!created)
                    throw new InvalidOperationException($"The {typeof(ARKitParticipantSubsystem).Name} provider has not been created.");

                Api.UnityARKit_TrackableProvider_start(m_Ptr);
            }

            public override void Stop()
            {
                if (!created)
                    throw new InvalidOperationException($"The {typeof(ARKitParticipantSubsystem).Name} provider has not been created.");

                Api.UnityARKit_TrackableProvider_stop(m_Ptr);
            }

            public override void Destroy() => Api.CFRelease(ref m_Ptr);

            public unsafe override TrackableChanges<XRParticipant> GetChanges(
                XRParticipant defaultParticipant,
                Allocator allocator)
            {
                if (!created)
                    throw new InvalidOperationException($"The {typeof(ARKitParticipantSubsystem).Name} provider has not been created.");

                using (var nativeChanges = Api.UnityARKit_TrackableProvider_acquireChanges(m_Ptr))
                {
                    var changes = new TrackableChanges<XRParticipant>(
                        nativeChanges.addedLength,
                        nativeChanges.updatedLength,
                        nativeChanges.removedLength,
                        allocator, defaultParticipant);

                    Api.UnityARKit_TrackableProvider_copyChanges(
                        m_Ptr, nativeChanges,
                        UnsafeUtility.SizeOf<XRParticipant>(),
                        changes.added.GetUnsafePtr(),
                        changes.updated.GetUnsafePtr(),
                        changes.removed.GetUnsafePtr());

                    return changes;
                }
            }

            [DllImport("__Internal")]
            static extern IntPtr UnityARKit_Participant_init();
        }

#if UNITY_2019_2_OR_NEWER
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.SubsystemRegistration)]
#else
        [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
#endif
        static void RegisterDescriptor()
        {
#if UNITY_IOS && !UNITY_EDITOR
            XRParticipantSubsystemDescriptor.Register<ARKitParticipantSubsystem>(
                "ARKit-Participant",
                XRParticipantSubsystemDescriptor.Capabilities.None);
#endif
        }
    }
}
