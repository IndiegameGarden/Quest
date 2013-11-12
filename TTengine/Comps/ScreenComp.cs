// (c) 2010-2013 TranceTrance.com. Distributed under the FreeBSD license in LICENSE.txt

using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Content;

namespace TTengine.Core
{
    /// <summary>
    /// Component to turn an Entity into a 'Screenlet', which acts as a screenComp
    /// (RenderBuffer) where other entities can draw themselves to.
    /// <seealso cref="ScreenletSystem"/>
    /// </summary>
    public class ScreenComp: Comp
    {
        /// <summary>create a Screenlet of given dimensions with optionally a RenderTarget</summary>
        public ScreenComp(bool hasRenderBuffer, int x, int y)
        {
            screenWidth = x;
            screenHeight = y;
            OnConstruction();
            if (hasRenderBuffer)
                InitRenderTarget();
            InitScreenDimensions();
        }

        /// <summary>create a Screenlet of full-screenletEntity dimensions with optionally a RenderTarget</summary>
        public ScreenComp(bool hasRenderBuffer): 
            this(hasRenderBuffer,TTGame.Instance.GraphicsDevice.Viewport.Width, TTGame.Instance.GraphicsDevice.Viewport.Height)
        {
        }

        public Color BackgroundColor = Color.Black;

        public bool Visible = true;

        /// <summary>The center pixel coordinate of the screen</summary>
        public Vector3 Center { get; private set; }

        /// <summary>The zoom-in factor, used for showing part of a screen and for translation of other coordinate systems to pixel coordinates.</summary>
        public float Zoom;

        /// <summary>The center coordinate, in either pixel or custom coordinates, for applying Zoom</summary>
        public Vector3 ZoomCenter;

        /// <summary>TODO</summary>
        public RenderTarget2D RenderTarget
        {
            get
            {
                return renderTarget;
            }
            set
            {
                renderTarget = value;
                InitScreenDimensions();
            }
        }

        /// <summary>Width of visible screenletEntity in relative coordinates</summary>
        public float Width { get { return aspectRatio; } }

        /// <summary>Height of visible screenletEntity in relative coordinates</summary>
        public float Height { get { return 1.0f; } }

        /// <summary>Width of visible screenletEntity in pixels</summary>
        public int WidthPixels { get { return screenWidth; } }

        /// <summary>Height of visible screenletEntity in pixels</summary>
        public int HeightPixels { get { return screenHeight; } }

        /// <summary>screenlet aspectratio</summary> 
        public float AspectRatio { get { return aspectRatio;  } }

        /// <summary>returns a single Rectangle instance with screenletEntity size/shape</summary>
        public Rectangle ScreenRectangle
        {
            get
            {
                return screenRect;
            }
        }

        /// <summary>The default spritebatch associated to this screen, for drawing to it</summary>
        public TTSpriteBatch SpriteBatch = null;

        #region Private and internal variables        
        //
        private int screenWidth = 0;
        private int screenHeight = 0;
        private float aspectRatio;
        private Rectangle screenRect;
        private RenderTarget2D renderTarget;
        #endregion

        /// <summary>
        /// translate a Vector2 relative coordinate to pixel coordinates
        /// </summary>
        /// <param name="pos">relative coordinate to translate</param>
        /// <returns>translated to pixels coordinate</returns>
        public Vector2 ToPixels(Vector3 pos)
        {
            var v = (pos - ZoomCenter) * Zoom + Center;
            return new Vector2(v.X, v.Y);
            //return pos * screen.screenHeight;
        }

        protected void OnConstruction()
        {
            // TODO spritebatch can be supplied from outside? optimize TTGame.Instance.GraphicsDevice access?
            SpriteBatch = new TTSpriteBatch(TTGame.Instance.GraphicsDevice);
        }

        protected void InitRenderTarget()
        {
            if (screenWidth > 0 && screenHeight > 0)  // init based on constructor parameters
                renderTarget = new RenderTarget2D(TTGame.Instance.GraphicsDevice, screenWidth, screenHeight);
            else
                renderTarget = null;            
        }

        protected void InitScreenDimensions()
        {
            if (renderTarget != null)
            {
                screenWidth = renderTarget.Width;
                screenHeight = renderTarget.Height;
            }
            else
            {
                screenWidth = TTGame.Instance.GraphicsDevice.Viewport.Width;
                screenHeight = TTGame.Instance.GraphicsDevice.Viewport.Height;
            }
            //scalingToNormalized = 1.0f / (float)screenHeight;
            screenRect = new Rectangle(0, 0, screenWidth, screenHeight);
            aspectRatio = (float)screenWidth / (float)screenHeight;
            Center = new Vector3(((float)screenWidth)/2.0f, ((float)screenHeight)/2.0f, 0f);
            Zoom = 1f;
            ZoomCenter = Center;
        }

    }
}
