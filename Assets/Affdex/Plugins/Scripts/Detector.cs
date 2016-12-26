using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Affdex
{
    /// <summary>
    /// Core detector object.  Handles all communication with the native APIs.  
    /// </summary>
    public class Detector : MonoBehaviour
    {
        /// <summary>
        /// Boolean to determine if the detector should start on wake
        /// </summary>
        public bool startOnWake = true;

        /// <summary>
        /// True when the Detector is running
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// Initialization flag
        /// </summary>
        private bool _initialized = false;

        /// <summary>
        /// Native Platform handle
        /// </summary>
        private INativePlatform nativePlatform;

        /// <summary>
        /// If frames are processed as discrete images
        /// </summary>
        public bool discrete;

        /// <summary>
        /// Emotions for detector to look for (Contains Unity LayerMask values (pow2)) - dont let people use this programatically
        /// </summary>
        public Emotions emotions;

        /// <summary>
        /// Expressions for the detector to look for (Contains Unity LayerMask values (pow2)) - dont let people use this programatically
        /// </summary>
        public Expressions expressions;

        private ImageResultsListener _listener;

        /// <summary>
        /// Listener to receive callback events for the detector.  All events received in Update() call.
        /// </summary>
        public ImageResultsListener Listener {
            get {

                if (_listener == null) {
                    _listener = (ImageResultsListener)GameObject.FindObjectOfType (typeof(ImageResultsListener));
                    
                }

                return _listener;

            }
            set {
                _listener = value;
            }
        }

        /// <summary>
        /// Pointer to loaded library if it doesn't exist already
        /// </summary>
        private static IntPtr lib;

        internal static bool LoadNativeDll (string FileName)
        {

            if (lib != IntPtr.Zero) {
                return true;
            }
            lib = NativeMethods.LoadLibrary (FileName);
            if (lib == IntPtr.Zero) {
                Debug.LogError("Failed to load native library!");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Initialize the detector
        /// </summary>
        /// <param name="discrete">If the frames processed as discrete images</param>
        public void Initialize (bool discrete = false)
        {
            if (_initialized)
                return;

            // Libraries are expected in Assets/Affdex/Plugins
            String rootPath;
            if (RuntimePlatform.OSXPlayer == Application.platform || RuntimePlatform.WindowsPlayer == Application.platform)
                rootPath = Application.dataPath;
            else
                rootPath = Path.Combine (Application.dataPath, "Affdex");
            
            rootPath = Path.Combine (rootPath, "Plugins");

            Debug.Log ("Starting affdex SDK using (" + Application.platform + ") Platform");

            //use Application.platform to determine platform
            if (RuntimePlatform.WindowsEditor == Application.platform || RuntimePlatform.WindowsPlayer == Application.platform) {
                if (IntPtr.Size == 8 && RuntimePlatform.WindowsEditor == Application.platform)
                {
                    rootPath = Path.Combine(rootPath, "x86_64");
                }
                else if (RuntimePlatform.WindowsEditor == Application.platform)
                {
                    rootPath = Path.Combine(rootPath, "x86");
                }
                LoadNativeDll (Path.Combine (rootPath, "affdex-native.dll"));
                LoadNativeDll (Path.Combine (rootPath, "AffdexNativeWrapper.dll"));
                nativePlatform = new WindowsNativePlatform ();
            } else if (RuntimePlatform.OSXEditor == Application.platform || RuntimePlatform.OSXPlayer == Application.platform) {
                if (!LoadNativeDll (Path.Combine (rootPath, "affdex-native.bundle/Contents/MacOS/affdex-native")))
                    return;
                nativePlatform = new OSXNativePlatform ();
            }

            //todo: Handle INitialize failure here!
            nativePlatform.Initialize (this, discrete ? 1 : 0);
            
            //find all ON emotions and enable them!
            for (int i = 0; i < System.Enum.GetNames (typeof(Emotions)).Length; i++) {
                Emotions targetEmotion = (Emotions)i;
                if (emotions.On (targetEmotion)) {
                    //Debug.Log(targetEmotion + " is on");
                    nativePlatform.SetEmotionState (i, true);
                }
            }

            //find all ON emotions and enable them!
            for (int i = 0; i < System.Enum.GetNames (typeof(Expressions)).Length; i++) {
                Expressions targetExpression = (Expressions)i;
                if (expressions.On (targetExpression)) {
                    //Debug.Log(targetExpression + " is on");
                    nativePlatform.SetExpressionState (i, true);
                }
            }

            _initialized = true;

        }

        //start called via unity
        void Start ()
        {
            if (!AffdexUnityUtils.ValidPlatform ())
                return;

            if (startOnWake) {
                Initialize (discrete);
                if (_initialized)
                    StartDetector ();
            }
        }

        /// <summary>
        /// Unity onEnable callback for the detector component
        /// </summary>
        public void OnEnable ()
        {
            if (!AffdexUnityUtils.ValidPlatform ())
                return;

            StartCoroutine (ListenForEvents ());
        }


        /// <summary>
        /// Event callbacks
        /// </summary>
        private List<NativeEvent> nativeEvents;

        /// <summary>
        /// Events available flag
        /// </summary>
        volatile bool hasEvents = false;

        /// <summary>
        /// Synchronization lock
        /// </summary>
        object eventLock = new object ();

        /// <summary>
        /// Add Event to the queue for callback
        /// </summary>
        /// <param name="e"></param>
        internal void AddEvent (NativeEvent e)
        {
            lock (eventLock) {

                if (nativeEvents == null)
                    nativeEvents = new List<NativeEvent> ();

                nativeEvents.Add (e);
                hasEvents = true;
            }
            ;
            
        }

        /// <summary>
        /// Loops to listen for events received from native threads
        /// </summary>
        /// <returns></returns>
        IEnumerator ListenForEvents ()
        {

            nativeEvents = new List<NativeEvent> ();
            while (enabled) {
                yield return new WaitForEndOfFrame ();
               
                if (hasEvents) {
                   
                    //dispatch all events stored up!
                    lock (eventLock) {
                        while (nativeEvents.Count > 0) {
                            NativeEvent e = nativeEvents [0];
                            nativeEvents.RemoveAt (0);

                            if (e.type == NativeEventType.ImageResults) {
                                if (Listener != null)
                                    Listener.onImageResults ((Dictionary<int, Face>)e.eventData);
                            } else if (e.type == NativeEventType.FaceFound) {
                                if (Listener != null)
                                    Listener.onFaceFound (Time.realtimeSinceStartup, (int)e.eventData);
                            } else if (e.type == NativeEventType.FaceLost) {
                                if (Listener != null)
                                    Listener.onFaceLost (Time.realtimeSinceStartup, (int)e.eventData);
                            }
                        }

                        hasEvents = false;
                    }
                }
                  
            }
        }


        /// <summary>
        /// Single frame processing
        /// </summary>
        /// <param name="frame">Frame to process</param>
        public void ProcessFrame (Frame frame)
        {
            if (!IsRunning) {
                return;
            }


            int bytesPerPixel = 3;

            byte[] bytes = new byte[frame.rgba.Length * bytesPerPixel];

            // int stride = frame.w * bytesPerPixel;

            for (int y = 0; y < frame.h; y++) {
                for (int x = 0; x < frame.w; x++) {

                    int frameByteIndex = (y * (frame.w)) + x;
                    int idx = ((frame.h - y - 1) * (frame.w * bytesPerPixel)) + (x * bytesPerPixel);

                    bytes [idx] = frame.rgba [frameByteIndex].b;
                    bytes [idx + 1] = frame.rgba [frameByteIndex].g;
                    bytes [idx + 2] = frame.rgba [frameByteIndex].r;
                }

            }

            //Debug only saving of the image to a tmp file!
            //SampleImage(bytes, frame.w, frame.h);

            nativePlatform.ProcessFrame (bytes, frame.w, frame.h, frame.timestamp);
        }


        /// <summary>
        /// Create an image from the BGR bytes and save
        /// </summary>
        /// <param name="bytes">BGR bytes </param>
        /// <param name="w">image width</param>
        /// <param name="h">image height</param>
        private void SampleImage (byte[] bytes, int w, int h)
        {
            Texture2D t = new Texture2D (w, h, TextureFormat.RGB24, false);
            Color32[] colors = new Color32[w * h];
            for (int i = 0; i < colors.Length; i++) {
                colors [i].b = bytes [(i * 3)];
                colors [i].g = bytes [(i * 3) + 1];
                colors [i].r = bytes [(i * 3) + 2];
            }

            t.SetPixels32 (colors);

            byte[] png = t.EncodeToPNG ();
            File.WriteAllBytes ("tmp.png", png);

        }

        /// <summary>
        /// Method to start the detector if it is not already running
        /// </summary>
        public void StartDetector ()
        {

            if (IsRunning)
                return;

            if (!_initialized)
                Initialize (this.discrete);

            int success = nativePlatform.Start ();

            if (success == 0)
                throw new Exception ("Failed to start the detector");

            IsRunning = true;

        }

        /// <summary>
        /// Method to stop the detector
        /// </summary>
        public void Stop ()
        {
            if (IsRunning)
                nativePlatform.Stop ();

            IsRunning = false;
        }

        /// <summary>
        /// Method Unity calls when destroying objects.  This will release the detector and it's associated memory.
        /// </summary>
        public void OnDestroy ()
        {

            Stop ();
            if (nativePlatform != null)
                nativePlatform.Release ();
        }

        /// <summary>
        /// Set State to Off/On for an expression
        /// </summary>
        /// <param name="Expression">Expression to set the state of</param>
        /// <param name="State">Off/On state</param>
        public void SetExpressionState (Expressions Expression, bool State)
        {
            if (IsRunning)
                nativePlatform.SetExpressionState ((int)Expression, State);
        }

        /// <summary>
        /// Set State to Off/On for an emotion
        /// </summary>
        /// <param name="Emotion">Emotion to set the state of</param>
        /// <param name="State">Off/On state</param>
        public void SetEmotionState (Emotions Emotion, bool State)
        {
            if (IsRunning)
                nativePlatform.SetEmotionState ((int)Emotion, State);
        }

        //todo: Add GetExpressionState and GetEmotionState

    }

    /// <summary>
    /// Detector Input interface
    /// </summary>
    public interface IDetectorInput
    {
        /// <summary>
        /// Texture for the input interface
        /// </summary>
        Texture Texture { get; }
    }

//#if UNITY_EDITOR_WIN || UNITY_STANDALONE_WIN
    /// <summary>
    /// DLL loader helper
    /// </summary>
    internal static class NativeMethods
    {
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern IntPtr LoadLibrary(
            string lpFileName
            );
        
        
        [DllImport("kernel32", SetLastError = true, CharSet = CharSet.Unicode)]
        internal static extern int FreeLibrary(
            string lpFileName
            );
    }
    /*
#else
    /// <summary>
    /// DLL loader helper
    /// </summary>
    internal class NativeMethods
    {
        
        public static IntPtr LoadLibrary (string fileName)
        {
            IntPtr retVal = dlopen (fileName, RTLD_NOW);
            var errPtr = dlerror ();
            if (errPtr != IntPtr.Zero) {
                Debug.LogError (Marshal.PtrToStringAnsi (errPtr));
            }
            return retVal;
        }

        public static void FreeLibrary (IntPtr handle)
        {
            dlclose (handle);
        }

        const int RTLD_NOW = 2;

        [DllImport ("libdl.dylib")]
        private static extern IntPtr dlopen (String fileName, int flags);

        [DllImport ("libdl.dylib")]
        private static extern IntPtr dlsym (IntPtr handle, String symbol);

        [DllImport ("libdl.dylib")]
        private static extern int dlclose (IntPtr handle);

        [DllImport ("libdl.dylib")]
        private static extern IntPtr dlerror ();
    }
#endif
*/
}
