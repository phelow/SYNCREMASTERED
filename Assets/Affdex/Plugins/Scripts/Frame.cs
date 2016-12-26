using UnityEngine;

namespace Affdex
{
    /// <summary>
    /// A wrapper struct for images and their associated timestamps.
    /// </summary>
    public struct Frame
    {
        /// <summary>
        /// Representation of RGBA colors in 32 bit format.
        /// <para>
        /// Each color component is a byte value with a range from 0 to 255.
        /// </para><para>
        /// Components(r, g, b) define a color in RGB color space.Alpha component(a) defines transparency - alpha of 255 is completely opaque, alpha of zero is completely transparent.
        /// </para>
        /// </summary>
        public Color32[] rgba;

        /// <summary>
        /// The timestamp of the frame (in seconds). Can be used as an identifier of the frame.  If you use Time.timeScale to pause and use the same time units then you will not be able to process frames while paused.
        /// </summary>
        public float timestamp;

        /// <summary>
        /// Width of the frame. Value has to be greater than zero
        /// </summary>
        public int w;

        /// <summary>
        /// Height of the frame. Value has to be greater than zero
        /// </summary>
        public int h;

        /// <summary>
        /// Representation of RGBA colors in 32 bit format.
        /// <para>
        /// Each color component is a byte value with a range from 0 to 255.
        /// </para><para>
        /// Components(r, g, b) define a color in RGB color space.Alpha component(a) defines transparency - alpha of 255 is completely opaque, alpha of zero is completely transparent.
        /// </para>
        /// <param name="rgba">Representation of RGBA colors in 32 bit format.</param>
        /// <param name="width">Width of the frame. Value has to be greater than zero</param>
		/// <param name="height">Height of the frame. Value has to be greater than zero</param>
        /// <param name="timestamp">The timestamp of the frame (in seconds). Can be used as an identifier of the frame.  If you use Time.timeScale to pause and use the same time units then you will not be able to process frames while paused.</param>
        /// </summary>
        public Frame(Color32[] rgba, int width, int height, float timestamp)
        {
            this.w = width;
            this.h = height;
            this.rgba = rgba;
            this.timestamp = timestamp;
        }
    }
}
