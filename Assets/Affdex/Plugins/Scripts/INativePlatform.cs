using System;

namespace Affdex
{
    internal enum NativeEventType
    {
        ImageResults,
        FaceFound,
        FaceLost
    }

    internal struct NativeEvent
    {
        public NativeEventType type;
        public object eventData;

        public NativeEvent(NativeEventType t, object data)
        {

            type = t;
            eventData = data;
        }
    }

    internal interface INativePlatform
    {

        /// <summary>
        /// Initialize the detector.  Creates the instance for later calls
        /// </summary>
        /// <param name="discrete"></param>
        /// <param name="detector">Core detector object.  Handles all communicatoin with the native APIs.</param>
        void Initialize(Detector detector, int discrete);

        /// <summary>
        /// Start the detector
        /// </summary>
        /// <returns>Non-zero error code</returns>
        int Start();


        void Stop();

        /// <summary>
        /// Enable or disable an expression
        /// </summary>
        /// <param name="expression">ID of the expression to set the state of</param>
        /// <param name="state">ON/OFF state for the expression</param>
        void SetExpressionState(int expression, bool state);

        /// <summary>
        /// Get the ON/OFF state of the expression
        /// </summary>
        /// <param name="expression">ID of the expression</param>
        /// <returns>0/1 for OFF/ON state</returns>
        bool GetExpressionState(int expression);

        /// <summary>
        /// Enable or disable an emotion
        /// </summary>
        /// <param name="emotion">ID of the emotion to set the state of</param>
        /// <param name="state">ON/OFF state for the emotion</param>
        void SetEmotionState(int emotion, bool state);

        /// <summary>
        /// Get the ON/OFF state of the emotion
        /// </summary>
        /// <param name="emotion">emotion id to get the state of</param>
        /// <returns>0/1 for OFF/ON state</returns>
        bool GetEmotionState(int emotion);


        /// <summary>
        /// Process a single frame of data
        /// </summary>
        /// <param name="rgba">Representation of RGBA colors in 32 bit format.</param>
        /// <param name="width">Width of the frame. Value has to be greater than zero</param>
		/// <param name="height">Height of the frame. Value has to be greater than zero</param>
        /// <param name="timestamp">The timestamp of the frame (in seconds). Can be used as an identifier of the frame.  If you use Time.timeScale to pause and use the same time units then you will not be able to process frames while paused.</param>
        void ProcessFrame(byte[] rgba, int width, int height, float timestamp);


        /// <summary>
        /// Notify the native plugin to release memory and cleanup
        /// </summary>
        void Release();

    }

}
    