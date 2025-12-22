using UnityEngine;
using Unity.Cinemachine;


/// <summary>
/// An extension to force the Cinemachine camera to stay above a certain Y height.
/// </summary>
[ExecuteInEditMode]
[SaveDuringPlay]
public class CinemachineLockY : CinemachineExtension
{
    [Tooltip("The camera will strictly not go below this Y position.")]
    public float m_MinYPosition = 1.0f;

    protected override void PostPipelineStageCallback(
        CinemachineVirtualCameraBase vcam,
        CinemachineCore.Stage stage,
        ref CameraState state,
        float deltaTime)
    {
        // "Body" is the stage where the camera calculates its position.
        // We check this stage to override the position.
        if (stage == CinemachineCore.Stage.Body)
        {
            Vector3 pos = state.RawPosition;

            // Check if we are below the floor
            if (pos.y < m_MinYPosition)
            {
                // Force the Y to the minimum
                pos.y = m_MinYPosition;
                state.RawPosition = pos;
            }
        }
    }
}