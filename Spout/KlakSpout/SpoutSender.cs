using LucHeart.Spout.KlakSpout.Internal;
using UnityEngine;
using UnityEngine.Rendering;

namespace LucHeart.Spout.KlakSpout;

public sealed class SpoutSender : MonoBehaviour
{
    private string _spoutName = "Spout Sender";

    public string SpoutName
    { get => _spoutName;
        set => ChangeSpoutName(value); }

    private void ChangeSpoutName(string spoutSenderName)
    {
        // Sender refresh on renaming
        if (_spoutName == spoutSenderName) return;
        _spoutName = spoutSenderName;
        ReleaseSender();
    }

    public bool KeepAlpha { get; set; }

    public CaptureMethods CaptureMethod { get; set; } = CaptureMethods.GameView;

    public Camera? SourceCamera { get; set; }

    public Texture? SourceTexture { get; set; }


    
    #region Sender plugin object

    private Sender? _sender;

    private void ReleaseSender()
    {
        _sender?.Dispose();
        _sender = null;
    }

    #endregion

    #region Buffer texture object

    private RenderTexture? _buffer;

    private void PrepareBuffer(int width, int height)
    {
        // If the buffer exists but has wrong dimensions, destroy it first.
        if (_buffer != null &&
            (_buffer.width != width || _buffer.height != height))
        {
            ReleaseSender();
            Utility.Destroy(_buffer);
            _buffer = null;
        }

        // Create a buffer if it hasn't been allocated yet.
        if (_buffer == null && width > 0 && height > 0)
        {
            _buffer = new RenderTexture(width, height, 0);
            _buffer.hideFlags = HideFlags.DontSave;
            _buffer.Create();
        }
    }

    #endregion

    #region Camera capture (SRP)

    private Camera? _attachedCamera;

    private void OnCameraCapture(RenderTargetIdentifier source, CommandBuffer cb)
    {
        if (_attachedCamera == null) return;
        SpoutBlitter.Blit(cb, source, _buffer, KeepAlpha);
    }

    private void PrepareCameraCapture(Camera? target)
    {
        // If it has been attached to another camera, detach it first.
        if (_attachedCamera != null && _attachedCamera != target)
        {
            CameraCaptureBridge.RemoveCaptureAction(_attachedCamera, OnCameraCapture);
            _attachedCamera = null;
        }
        
        // Attach to the target if it hasn't been attached yet.
        if (_attachedCamera == null && target != null)
        {
            CameraCaptureBridge.AddCaptureAction(target, OnCameraCapture);
            _attachedCamera = target;
        }
        
    }

    #endregion

    #region MonoBehaviour implementation

    private void OnDisable()
    {
        ReleaseSender();
        PrepareBuffer(0, 0);
        PrepareCameraCapture(null);
    }

    private void Update()
    {
        // GameView capture mode
        if (CaptureMethod == CaptureMethods.GameView)
        {
            PrepareBuffer(Screen.width, Screen.height);
            var temp = RenderTexture.GetTemporary(Screen.width, Screen.height, 0);
            ScreenCapture.CaptureScreenshotIntoRenderTexture(temp);
            SpoutBlitter.BlitVFlip(temp, _buffer!, KeepAlpha);
            RenderTexture.ReleaseTemporary(temp);
        }

        // Texture capture mode
        if (CaptureMethod == CaptureMethods.Texture)
        {
            if (SourceTexture == null) return;
            PrepareBuffer(SourceTexture.width, SourceTexture.height);
            SpoutBlitter.Blit(SourceTexture, _buffer!, KeepAlpha);
        }

        // Camera capture mode
        if (CaptureMethod == CaptureMethods.Camera)
        {
            PrepareCameraCapture(SourceCamera);
            if (SourceCamera == null) return;
            PrepareBuffer(SourceCamera.pixelWidth, SourceCamera.pixelHeight);
        }

        // Sender lazy initialization
        if (_sender == null) _sender = new Sender(_spoutName, _buffer!);

        // Sender plugin-side update
        _sender.Update();
    }

    #endregion
}