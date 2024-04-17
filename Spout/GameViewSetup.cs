using LucHeart.Spout.KlakSpout;
using UnityEngine;

namespace LucHeart.Spout;

public static class GameViewSetup
{
    private static GameObject? _gameViewGameObject;
    
    public static void SetupGameView()
    {
        if (MelonConfig.EnableGameView.Value)
        {
            if (_gameViewGameObject != null) return;

            _gameViewGameObject = new GameObject("Spout_GameView");
            var spoutSender = _gameViewGameObject.AddComponent<SpoutSender>();
            spoutSender.CaptureMethod = CaptureMethods.GameView;
            spoutSender.SpoutName = "ChilloutVR_GameView";
            spoutSender.enabled = true;

            Object.DontDestroyOnLoad(_gameViewGameObject);
            return;
        }
        
        if (_gameViewGameObject != null) Object.Destroy(_gameViewGameObject);
        _gameViewGameObject = null;
    }
}