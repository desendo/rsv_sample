using Game.Services;
using UnityEngine;

namespace Game
{
    public class CameraView : MonoBehaviour
    {
        private Camera _camera;
        private CameraService _cameraService;

        private void Awake()
        {
            _camera = GetComponent<Camera>();
            _cameraService = Di.Instance.Get<CameraService>();
            _cameraService.RegisterCamera(_camera);
            _cameraService.Position.Subscribe(vector3 => transform.position = vector3);
            _cameraService.Rotation.Subscribe(rotation => transform.rotation = rotation);
        }
    }
}