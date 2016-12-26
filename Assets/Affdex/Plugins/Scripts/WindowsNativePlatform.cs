using System;
using System.Runtime.InteropServices;
using UnityEngine;
using System.IO;


namespace Affdex
{

    internal class WindowsNativePlatform : INativePlatform
    {

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void ImageResults(IntPtr i);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void FaceResults(Int32 i);

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        delegate void LogCallback(IntPtr l);
       
        private static  Detector detector;

        // This is a test that returns int 37 (see AffdexNativeWrapper.cpp for implementation)
        [DllImport("AffdexNativeWrapper")]
        private static extern int registerListeners([MarshalAs(UnmanagedType.FunctionPtr)] ImageResults imageCallback,
            [MarshalAs(UnmanagedType.FunctionPtr)] FaceResults foundCallback, 
            [MarshalAs(UnmanagedType.FunctionPtr)] FaceResults lostCallback);

        [DllImport("AffdexNativeWrapper")]
        private static extern int registerLog([MarshalAs(UnmanagedType.FunctionPtr)] LogCallback log);

        [DllImport("AffdexNativeWrapper")]
        private static extern int processFrame(IntPtr rgba, Int32 w, Int32 h, float timestamp);

        [DllImport("AffdexNativeWrapper")]
        private static extern int start();

        [DllImport("AffdexNativeWrapper")]
        private static extern void release();

        [DllImport("AffdexNativeWrapper")]
        private static extern int stop();

        [DllImport("AffdexNativeWrapper")]
        private static extern void setExpressionState(int expression, int state);

        [DllImport("AffdexNativeWrapper")]
        private static extern void setEmotionState(int emotion, int state);

        [DllImport("AffdexNativeWrapper")]
        private static extern int initialize(int discrete, string affdexDataPath);



        //Free these when platform closes!
        GCHandle h1, h2, h3; //handles to unmanaged function pointer callbacks


        public bool GetEmotionState(int emotion)
        {
            throw new NotImplementedException();
        }

        public bool GetExpressionState(int expression)
        {
            throw new NotImplementedException();
        }

        void onFaceFound(Int32 id)
        {
            detector.AddEvent(new NativeEvent(NativeEventType.FaceFound, id));
        }

        void onFaceLost(Int32 id)
        {

            detector.AddEvent(new NativeEvent(NativeEventType.FaceLost, id));
        }

        static void onLogCallback(IntPtr logPtr)
        {
            try
            {
                string lInfo = Marshal.PtrToStringAnsi(logPtr);
                Debug.Log("Native: " + lInfo);
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message + " " + e.StackTrace);
            }
        }


        static void handleOnImageResults(IntPtr faceData)
        {
            System.Collections.Generic.Dictionary<int, Face> faces = new System.Collections.Generic.Dictionary<int, Face>();

            if (faceData != IntPtr.Zero)
            {
                try
                {
                    //todo: Face ID might not always be zero, or there might be more faces!!!
                    FaceData f = (FaceData)Marshal.PtrToStructure(faceData, typeof(FaceData));
                    faces[0] = new Face(f);
                }
                catch (Exception e)
                {
                    Debug.Log(e.Message + " " + e.StackTrace);
                }
            }
            
            detector.AddEvent(new NativeEvent(NativeEventType.ImageResults, faces));
        }

        /// <summary>
        /// ImageResults callback from native plugin!
        /// </summary>
        /// <param name="ptr">Platform-specific pointer to image results</param>
        void onImageResults(IntPtr ptr)
        {
            handleOnImageResults(ptr);
        }

        public void Initialize(Detector detector, int discrete)
        {

            
            WindowsNativePlatform.detector = detector;

            //load our lib!

            String adP = Application.streamingAssetsPath;
            
            String affdexDataPath = Path.Combine(adP, "affdex-data"); // Application.streamingAssetsPath + "/affdex-data";
            //String affdexDataPath = Application.dataPath + "/affdex-data";
            affdexDataPath = affdexDataPath.Replace('/', '\\');
            int status = initialize(discrete, affdexDataPath);
            Debug.Log("Initialized detector: " + status);
             

            FaceResults faceFound = new FaceResults(this.onFaceFound);
            FaceResults faceLost = new FaceResults(this.onFaceLost);
            ImageResults imageResults = new ImageResults(this.onImageResults);

            h1 = GCHandle.Alloc(faceFound, GCHandleType.Pinned);
            h2 = GCHandle.Alloc(faceLost, GCHandleType.Pinned);
            h3 = GCHandle.Alloc(imageResults, GCHandleType.Pinned);

            status = registerListeners(imageResults, faceFound, faceLost);
            Debug.Log("Registered listeners: " + status);

        }


        public void ProcessFrame(byte[] rgba, int w, int h, float timestamp)
        {
            try
            {
                IntPtr addr = Marshal.AllocHGlobal(rgba.Length);

                Marshal.Copy(rgba, 0, addr, rgba.Length);

                processFrame(addr, w, h, Time.realtimeSinceStartup);
                
                Marshal.FreeHGlobal(addr);
                addr = IntPtr.Zero;
            }
            catch (Exception e)
            {
                Debug.LogError(e.Message + " " + e.StackTrace);
            }
		}

        public void SetExpressionState(int expression, bool state)
        {
            int intState = (state) ? 1 : 0;
            setExpressionState(expression, intState);
           // Debug.Log("Expression " + expression + " set to " + state);
        }

        public void SetEmotionState(int emotion, bool state)
        {
            int intState = (state) ? 1 : 0;
            setEmotionState(emotion, intState);
          //  Debug.Log("Emotion " + emotion + " set to " + state);
        }

        public int Start()
        {
            return start();
        }

        public void Stop()
        {
            stop();
        }

        public void Release()
        {
            release();
            h1.Free();
            h2.Free();
            h3.Free();
        }
    }
}
