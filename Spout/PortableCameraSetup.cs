using ABI_RC.Systems.Camera;
using LucHeart.Spout.KlakSpout;
using UnityEngine;

namespace LucHeart.Spout;

public static class PortableCameraSetup
{
    private static SpoutSender? _spoutSender;
    
    public static void UpdateSpoutSender()
    {
        var portableCamera = PortableCamera.Instance;
        if(portableCamera == null) return;
        
        if (MelonConfig.EnablePortableCamera.Value)
        {
            if(_spoutSender != null) return;
            
            _spoutSender = portableCamera.gameObject.AddComponent<SpoutSender>();
            _spoutSender.CaptureMethod = CaptureMethods.Texture;
            _spoutSender.SourceTexture = portableCamera.processingTexture;
            _spoutSender.SpoutName = "ChilloutVR_PortableCamera";
            _spoutSender.enabled = true;
            return;
        }

        if (_spoutSender != null) Object.Destroy(_spoutSender);
        _spoutSender = null;
    }
}