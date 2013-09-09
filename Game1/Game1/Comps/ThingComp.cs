//
// Method IntersectPixels used under Microsoft Permissive License (Ms-PL). (http://create.msdn.com/downloads/?id=15)
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using TTengine.Core;
using TTengine.Comps;
using TTengine.Util;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Game1.Behaviors;
using Game1.Actors;
using TreeSharp;
using Artemis;
using Artemis.Interface;

namespace Game1.Comps
{
    public enum Faction { GOOD, EVIL };

    public class ThingComp: IComponent
    {
        public bool Active = true;
        public bool Visible = true;

        public Color Color = Color.White;

        public Faction Faction = Faction.GOOD;

        public double Health = 100;

        public List<ThingComp> Children = new List<ThingComp>();

        public Vector2 TargetMove = Vector2.Zero;

        /// <summary>
        /// this ThingComp can be attached to a parent ThingComp, if null then not attached.
        /// </summary>
        public ThingComp Parent = null;

        /// <summary>
        /// if true can pass anything
        /// </summary>
        public bool IsCollisionFree = true;

        /// <summary>
        /// Determines what intensity levels of background pixel color this ThingComp can pass.
        /// Intensity is the sum of R,G,B bytes of pixel. Any background pixel at threshold value or
        /// brighter is passable for this ThingComp.
        /// </summary>
        public int PassableIntensityThreshold;

        /// <summary>
        /// position in the level, in pixels, in sub-pixel resolution
        /// </summary>
        public Vector2 Position = Vector2.Zero;

        /// <summary>
        /// if attached to a parent, the position relative to parent where it is
        /// attached
        /// </summary>
        public Vector2 AttachmentPosition = Vector2.Zero;

        /// <summary>
        /// Position.X as a rounded integer
        /// </summary>
        public int PositionX
        {
            get { return (int)Math.Round(Position.X); }
            set { Position.X = (float)value; }
        }

        /// <summary>
        /// Position.Y as a rounded integer
        /// </summary>
        public int PositionY
        {
            get { return (int)Math.Round(Position.Y); }
            set { Position.Y = (float)value; }
        }


        /// <summary>
        /// Target.X as a rounded integer
        /// </summary>
        public int TargetX
        {
            get { return (int)Math.Round(Target.X); }
            set { Target.X = (float)value; }
        }

        /// <summary>
        /// Target.Y as a rounded integer
        /// </summary>
        public int TargetY
        {
            get { return (int)Math.Round(Target.Y); }
            set { Target.Y = (float)value; }
        }

        /// <summary>
        /// position in the level, in pixels, where the entity's Position should move towards in a smooth fashion
        /// </summary>
        public Vector2 Target = Vector2.Zero;

        /// <summary>
        /// a direction (if any) the Thing is facing towards e.g. up (0,-1), down (0,1) or right (1,0).
        /// </summary>
        public Vector2 FacingDirection = new Vector2(1f, 0f);

        /// <summary>
        /// to set both Position and Target in one go
        /// </summary>
        public Vector2 PositionAndTarget
        {
            set
            {
                Position = value;
                Target = value;
                TTutil.Round(ref Position);
                TTutil.Round(ref Target);
            }
        }

        private Rectangle boundingRectangle = new Rectangle();

        /// <summary>
        /// the bounding rectangle of the sprite of this Thing, based on Position
        /// </summary>
        protected Rectangle BoundingRectangle
        {
            get {
                boundingRectangle.X = PositionX; 
                boundingRectangle.Y = PositionY; 
                return boundingRectangle; 
            }
        }

        public void SetBoundingRectangleWidthHeight(int width, int height)
        {
            boundingRectangle.Width = width;
            boundingRectangle.Height = height;
        }

        /// <summary>
        /// a 'relative to normal' velocity-of-moving factor i.e. 1f == normal velocity
        /// </summary>
        public float Velocity = 1f;

        /// <summary>
        /// relative speed of the smooth motion for moving towards Target. Linear speed defined for Velocity==1.
        /// </summary>
        public float TargetSpeed = 10f;

        /// <summary>
        /// the Toy that is carried by and active for this ThingComp, or null if none
        /// </summary>
        public Toy ToyActive = null;

        // used for the collision detection per-pixel
        protected Color[] textureData;
        protected LevelBackground bg;

        public ThingComp()
        {
        }

        /// <summary>
        /// detect all collisions with other collidable Things (that have IsCollisionFree=false set)
        /// </summary>
        /// <param name="myPotentialMove">a potential move of this ThingComp, collision checked after applying potential move.</param>
        /// <returns></returns>
        public List<ThingComp> DetectCollisions(Vector2 myPotentialMove)
        {
            List<ThingComp> l = new List<ThingComp>();
            foreach (ThingComp t in allThingsList)
            {
                if (t == this) continue;
                if (!t.Active) continue;
                if (CollidesWhenThisMoves(t,myPotentialMove))
                {
                    l.Add(t);
                }
            }
            return l;
        }

        public ThingComp FindNearest(Type thingType)
        {
            ThingComp foundThing = null;
            float bestDist = 99999999f;
            foreach (ThingComp t in allThingsList)
            {
                if (t == this) continue;
                if (!t.Active) continue;
                if (t.GetType() == thingType)
                {
                    float dist = (t.Position - this.Position).Length();
                    if (dist < bestDist)
                    {
                        bestDist = dist;
                        foundThing = t;
                    }
                }
            }
            return foundThing;
        }

        public bool Collides(ThingComp other)
        {
            return IntersectPixels(BoundingRectangle, textureData, other.BoundingRectangle, other.textureData );
        }

        public bool CollidesWhenThisMoves(ThingComp other, Vector2 myPotentialMove)
        {
            Rectangle rectNow = BoundingRectangle;
            Rectangle rectMoved = new Rectangle(rectNow.X + (int) Math.Round(myPotentialMove.X) ,
                                            rectNow.Y + (int) Math.Round(myPotentialMove.Y) ,
                                            rectNow.Width,rectNow.Height);
            return IntersectPixels(rectMoved, textureData, other.BoundingRectangle, other.textureData);
        }

        public bool CollidesWhenOtherMoves(ThingComp other, Vector2 othersPotentialMove)
        {
            Rectangle rectOther = other.BoundingRectangle;
            Rectangle rectMoved = new Rectangle(rectOther.X + (int)Math.Round(othersPotentialMove.X),
                                            rectOther.Y + (int)Math.Round(othersPotentialMove.Y),
                                            rectOther.Width, rectOther.Height);
            return IntersectPixels(BoundingRectangle, textureData, rectMoved, other.textureData);
        }

        /// <summary>
        /// check whether this ThingComp would collide with anything (ThingComp or background), when attempting
        /// to do a given move
        /// </summary>
        /// <param name="potentialMove">the move to attempt for this ThingComp</param>
        /// <returns>true if ThingComp would collide with something after the potentialMove would have been made, false otherwise</returns>
        public bool CollidesWithSomething(Vector2 potentialMove)
        {
            if (CollidesWithBackground(potentialMove))
                return true;
            List<ThingComp> lt = DetectCollisions(potentialMove);
            return  lt.Count > 0 ;
        }

        /// <summary>
        /// check whether this ThingComp would collide with background, when attempting to do a given potential move
        /// </summary>
        /// <param name="potentialMove">the move vector, delta from current Target, that we would like to apply</param>
        /// <returns>true if ThingComp would collide with background after a potentialMove would have been made, false otherwise</returns>
        public bool CollidesWithBackground(Vector2 potentialMove)
        {
            int posX = TargetX + (int)Math.Round(potentialMove.X);
            int posY = TargetY + (int)Math.Round(potentialMove.Y);
            if (posX < 0) 
                return true;
            if (posY < 0) 
                return true;
            Rectangle bgSampleRect = new Rectangle(posX, posY, BoundingRectangle.Width, BoundingRectangle.Height);
            Rectangle thingRect = new Rectangle(posX, posY, BoundingRectangle.Width, BoundingRectangle.Height);
            if (bgSampleRect.Right >= bg.Texture.Width)
                return true;
            if (bgSampleRect.Bottom >= bg.Texture.Height)
                return true;
            int N = bgSampleRect.Width * bgSampleRect.Height;
            Color[] bgTextureData = new Color[N];
            bg.Texture.GetData<Color>(0, bgSampleRect, bgTextureData, 0, N);
            return IntersectPixelsBg(thingRect, textureData, bgSampleRect, bgTextureData);
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// between two sprites.
        /// Method code from http://create.msdn.com/en-US/education/catalog/tutorial/collision_2d_perpixel
        /// used under Microsoft Permissive License (Ms-PL). 
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the first sprite</param>
        /// <param name="dataA">Pixel data of the first sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the second sprite</param>
        /// <param name="dataB">Pixel data of the second sprite</param>
        /// <returns>True if non-transparent pixels overlap; false otherwise</returns>
        static bool IntersectPixels(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If both pixels are not completely transparent,
                    if (colorA.A != 0 && colorB.A != 0)
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }

        /// <summary>
        /// Determines if there is overlap of the non-transparent pixels
        /// of a sprite with non-passable colored pixels in the background.
        /// Derived from above IntersectPixels() method.
        /// </summary>
        /// <param name="rectangleA">Bounding rectangle of the sprite</param>
        /// <param name="dataA">Pixel data of the sprite</param>
        /// <param name="rectangleB">Bouding rectangle of the background snapshot</param>
        /// <param name="dataB">Pixel data of the background snapshot</param>
        /// <returns>True if collision with background; false otherwise</returns>
        bool IntersectPixelsBg(Rectangle rectangleA, Color[] dataA,
                                    Rectangle rectangleB, Color[] dataB)
        {
           

            // Find the bounds of the rectangle intersection
            int top = Math.Max(rectangleA.Top, rectangleB.Top);
            int bottom = Math.Min(rectangleA.Bottom, rectangleB.Bottom);
            int left = Math.Max(rectangleA.Left, rectangleB.Left);
            int right = Math.Min(rectangleA.Right, rectangleB.Right);

            // Check every point within the intersection bounds
            for (int y = top; y < bottom; y++)
            {
                for (int x = left; x < right; x++)
                {
                    // Get the color of both pixels at this point
                    Color colorA = dataA[(x - rectangleA.Left) +
                                         (y - rectangleA.Top) * rectangleA.Width];
                    Color colorB = dataB[(x - rectangleB.Left) +
                                         (y - rectangleB.Top) * rectangleB.Width];

                    // If pixel A not completely transparent,
                    // and BG pixel non-passable (by comparing intensity to threshold),
                    if (colorA.A != 0 && (colorB.R + colorB.G + colorB.B) < PassableIntensityThreshold )
                    {
                        // then an intersection has been found
                        return true;
                    }
                }
            }

            // No intersection found
            return false;
        }


    }
}
