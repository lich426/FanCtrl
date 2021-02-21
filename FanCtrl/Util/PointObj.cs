using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Runtime.Serialization;
using System.Security.Permissions;
using ZedGraph;

namespace ZedGraph
{
    /// <summary>
    /// A class that represents a marker at a point on the graph.  
    /// A list of PointObj objects is maintained by the <see cref="GraphObjList"/> collection class.
    /// Based on BoxObj
    /// </summary>
    [Serializable]
    public class PointObj : GraphObj, ICloneable, ISerializable
    {
        #region Fields
        /// <summary>
        /// Private field that stores the <see cref="ZedGraph.Fill"/> data for this
        /// <see cref="PointObj"/>.  Use the public property <see cref="Fill"/> to
        /// access this value.
        /// </summary>
        private Fill _fill;
        /// <summary>
        /// Private field that stores the <see cref="ZedGraph.Border"/> data for this
        /// <see cref="PointObj"/>.  Use the public property <see cref="Border"/> to
        /// access this value.
        /// </summary>
        private Border _border;
        /// <summary>
        /// Private field that stores the <see cref="SymbolType"/> for this
        /// <see cref="Symbol"/>.  Use the public
        /// property <see cref="Type"/> to access this value.
        /// </summary>
        private SymbolType _type;
        #endregion
        #region Defaults
        /// <summary>
        /// A simple struct that defines the
        /// default property values for the <see cref="ArrowObj"/> class.
        /// </summary>
        new public struct Default
        {
            /// <summary>
            /// The default pen width to be used for drawing curve symbols
            /// (<see cref="ZedGraph.LineBase.Width"/> property).  Units are points.
            /// </summary>
            public static float PenWidth = 1.0F;
            /// <summary>
            /// The default color for filling in this <see cref="Symbol"/>
            /// (<see cref="ZedGraph.Fill.Color"/> property).
            /// </summary>
            public static Color FillColor = Color.Red;
            /// <summary>
            /// The default custom brush for filling in this <see cref="Symbol"/>
            /// (<see cref="ZedGraph.Fill.Brush"/> property).
            /// </summary>
            public static Brush FillBrush = null;
            /// <summary>
            /// The default fill mode for the curve (<see cref="ZedGraph.Fill.Type"/> property).
            /// </summary>
            public static FillType FillType = FillType.None;
            /// <summary>
            /// The default for drawing frames around symbols (<see cref="ZedGraph.LineBase.IsVisible"/> property).
            /// true to display symbol frames, false to hide them.
            /// </summary>
            public static bool IsBorderVisible = true;
            /// <summary>
            /// The default color for drawing symbols (<see cref="ZedGraph.LineBase.Color"/> property).
            /// </summary>
            public static Color BorderColor = Color.Red;
            /// <summary>
            /// The default SymbolType used for the <see cref="PointObj"/> symboltype property.
            /// </summary>
            public static SymbolType Type = SymbolType.XCross;
        }
        #endregion
        #region Properties
		/// <summary>
		/// Gets or sets the <see cref="ZedGraph.Fill"/> data for this
		/// <see cref="Symbol"/>.
		/// </summary>
		public Fill	Fill
		{
			get { return _fill; }
			set { _fill = value; }
		}
		/// <summary>
		/// Gets or sets the <see cref="ZedGraph.Border"/> data for this
		/// <see cref="Symbol"/>, which controls the border outline of the symbol.
		/// </summary>
        public Border Border
        {
            get { return _border; }
            set { _border = value; }
        }

        /// <summary>
        /// Gets or sets the type (shape) of the <see cref="Symbol"/>
        /// </summary>
        /// <value>A <see cref="SymbolType"/> enum value indicating the shape</value>
        /// <seealso cref="Default.Type"/>
        public SymbolType Type
        {
            get { return _type; }
            set { _type = value; }
        }
        #endregion
        #region Constructors
        /// <overloads>Constructors for the <see cref="PointObj"/> object</overloads>
        /// <summary>
        /// A constructor that allows the position, size, and color of the <see cref="PointObj"/> to be specified.
        /// </summary>
        /// <param name="Color">An arbitrary <see cref="System.Drawing.Color"/> specification for the poit</param>
        /// <param name="type">A <see cref="SymbolType"/> enum value indicating the shape of the symbol</param>
        /// <param name="x">The x location for this <see cref="PointObj" />.  This will be in units determined by <see cref="ZedGraph.Location.CoordinateFrame" />.</param>
        /// <param name="y">The y location for this <see cref="PointObj" />.  This will be in units determined by <see cref="ZedGraph.Location.CoordinateFrame" />.</param>
        /// <param name="width">The width of this <see cref="PointObj" />.  This will be in Pixels.</param>
        /// <param name="height">The height of this <see cref="PointObj" />.  This will be in Pixels.</param>
        public PointObj(double x, double y, double width, double height, SymbolType type, Color Color)
            : base(x, y, width, height)
        {
            this.Border = new Border(Default.IsBorderVisible, Color, Default.PenWidth);
            this.Fill = new Fill(Color, Default.FillBrush, Default.FillType);
            this.Type = type;
        }

        /// <summary>
        /// A constructor that allows the position of the <see cref="PointObj"/> to be pre-specified.  Other properties are defaulted.
        /// </summary>
        /// <param name="x">The x location for this <see cref="PointObj" />.  This will be in units determined by <see cref="ZedGraph.Location.CoordinateFrame" />.</param>
        /// <param name="y">The y location for this <see cref="PointObj" />.  This will be in units determined by <see cref="ZedGraph.Location.CoordinateFrame" />.</param>
        /// <param name="width">The width of this <see cref="PointObj" />.  This will be in Pixels.</param>
        /// <param name="height">The height of this <see cref="PointObj" />.  This will be in Pixels.</param>
        public PointObj(double x, double y, double width, double height)
            : base(x, y, width, height)
        {
            this.Border = new Border(Default.IsBorderVisible, Default.BorderColor, Default.PenWidth);
            this.Fill = new Fill(Default.FillColor, Default.FillBrush, Default.FillType); 
            this.Type = Default.Type;
        }

        /// <summary>
        /// A default constructor that creates a <see cref="PointObj"/> using a location of (0,0),
        /// and a width,height of (1,1).  Other properties are defaulted.
        /// </summary>
        public PointObj()
            : this(0, 0, 1, 1)
        {
        }

        /// <summary>
        /// The Copy Constructor
        /// </summary>
        /// <param name="rhs">The <see cref="PointObj"/> object from which to copy</param>
        public PointObj(PointObj rhs)
            : base(rhs)
        {
            this.Border = rhs.Border.Clone();
            this.Fill = rhs.Fill.Clone();
            this.Type = rhs.Type;
        }

        /// <summary>
        /// Implement the <see cref="ICloneable" /> interface in a typesafe manner by just
        /// calling the typed version of <see cref="Clone" />
        /// </summary>
        /// <returns>A deep copy of this object</returns>
        object ICloneable.Clone()
        {
            return this.Clone();
        }

        /// <summary>
        /// Typesafe, deep-copy clone method.
        /// </summary>
        /// <returns>A new, independent copy of this class</returns>
        public PointObj Clone()
        {
            return new PointObj(this);
        }
        #endregion
        #region Serialization
        /// <summary>
        /// Current schema value that defines the version of the serialized file
        /// </summary>
        public const int schema2 = 10;

        /// <summary>
        /// Constructor for deserializing objects
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data
        /// </param>
        /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data
        /// </param>
        protected PointObj(SerializationInfo info, StreamingContext context)
            : base(info, context)
        {
            // The schema value is just a file version parameter.  You can use it to make future versions
            // backwards compatible as new member variables are added to classes
            int sch = info.GetInt32("schema2");

            _type = (SymbolType)info.GetValue("type", typeof(SymbolType));
            _fill = (Fill)info.GetValue("fill", typeof(Fill));
            _border = (Border)info.GetValue("border", typeof(Border));
        }

        /// <summary>
        /// Populates a <see cref="SerializationInfo"/> instance with the data needed to serialize the target object
        /// </summary>
        /// <param name="info">A <see cref="SerializationInfo"/> instance that defines the serialized data</param>
        /// <param name="context">A <see cref="StreamingContext"/> instance that contains the serialized data</param>
        [SecurityPermissionAttribute(SecurityAction.Demand, SerializationFormatter = true)]
        public override void GetObjectData(SerializationInfo info, StreamingContext context)
        {
            base.GetObjectData(info, context);
            info.AddValue("schema2", schema2);
            info.AddValue("type", _type);
            info.AddValue("fill", _fill);
            info.AddValue("border", _border);
        }
        #endregion
        #region Rendering Methods
        private RectangleF GetPointRect(PaneBase pane, float scaleFactor)
        {
            RectangleF pixRect = this.Location.TransformRect(pane);
            float w = ((float)this.Location.Width) * scaleFactor / 2.0f;
            float h = ((float)this.Location.Height) * scaleFactor / 2.0f;
            return new RectangleF(pixRect.Left - w, pixRect.Top - h, 2f * w, 2f * h);
        }

        /// <summary>
        /// Render this object to the specified <see cref="Graphics"/> device.
        /// </summary>
        /// <remarks>
        /// This method is normally only called by the Draw method
        /// of the parent <see cref="GraphObjList"/> collection object.
        /// </remarks>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="pane">
        /// A reference to the <see cref="PaneBase"/> object that is the parent or
        /// owner of this object.
        /// </param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="GraphPane"/> object using the
        /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        override public void Draw(Graphics g, PaneBase pane, float scaleFactor)
        {
            // Convert the arrow coordinates from the user coordinate system
            // to the screen coordinate system
            RectangleF pixRect = this.GetPointRect(pane, scaleFactor);

            // Clip the rect to just outside the PaneRect so we don't end up with wild coordinates.
            RectangleF tmpRect = pane.Rect;
            tmpRect.Inflate(20, 20);
            pixRect.Intersect(tmpRect);

            if (Math.Abs(pixRect.Left) < 100000 &&
                    Math.Abs(pixRect.Top) < 100000)
            {
                // If the box is to be filled, fill it                
                using (GraphicsPath path = new GraphicsPath())
                {
                    switch (this.Type)
                    {
                        case SymbolType.Square:
                            {
                                path.AddLine(pixRect.Left, pixRect.Bottom, pixRect.Right, pixRect.Bottom);
                                path.AddLine(pixRect.Right, pixRect.Bottom, pixRect.Right, pixRect.Top);
                                path.AddLine(pixRect.Right, pixRect.Top, pixRect.Left, pixRect.Top);
                                path.AddLine(pixRect.Left, pixRect.Top, pixRect.Left, pixRect.Bottom);
                                break;
                            }
                        case SymbolType.Diamond:
                            {
                                float midx = (pixRect.Left + pixRect.Right) / 2.0f;
                                float midy = (pixRect.Top + pixRect.Bottom) / 2.0f;
                                path.AddLine(pixRect.Left, midy, midx, pixRect.Bottom);
                                path.AddLine(midx, pixRect.Bottom, pixRect.Right, midy);
                                path.AddLine(pixRect.Right, midy, midx, pixRect.Top);
                                path.AddLine(midx, pixRect.Top, pixRect.Left, midy);
                                break;
                            }
                        case SymbolType.Triangle:
                            {
                                float midx = (pixRect.Left + pixRect.Right) / 2.0f;
                                path.AddLine(midx, pixRect.Top, pixRect.Right, pixRect.Bottom);
                                path.AddLine(pixRect.Right, pixRect.Bottom, pixRect.Left, pixRect.Bottom);
                                path.AddLine(pixRect.Left, pixRect.Bottom, midx, pixRect.Top);
                                break;
                            }
                        case SymbolType.Circle:
                            {
                                path.AddEllipse(pixRect);
                                break;
                            }
                        case SymbolType.Plus:
                            {
                                float midx = (pixRect.Left + pixRect.Right) / 2.0f;
                                float midy = (pixRect.Top + pixRect.Bottom) / 2.0f;
                                path.AddLine(midx, pixRect.Bottom, midx, pixRect.Top);
                                path.StartFigure();
                                path.AddLine(pixRect.Left, midy, pixRect.Right, midy);
                                break;
                            }
                        case SymbolType.Star:
                            {
                                float midx = (pixRect.Left + pixRect.Right) / 2.0f;
                                float midy = (pixRect.Top + pixRect.Bottom) / 2.0f;
                                path.AddLine(midx, pixRect.Bottom, midx, pixRect.Top);
                                path.StartFigure();
                                path.AddLine(pixRect.Left, midy, pixRect.Right, midy);
                                path.StartFigure();
                                path.AddLine(pixRect.Left, pixRect.Bottom, pixRect.Right, pixRect.Top);
                                path.StartFigure();
                                path.AddLine(pixRect.Left, pixRect.Top, pixRect.Right, pixRect.Bottom);
                                break;
                            }
                        case SymbolType.TriangleDown:
                            {
                                float midx = (pixRect.Left + pixRect.Right) / 2.0f;
                                path.AddLine(midx, pixRect.Bottom, pixRect.Right, pixRect.Top);
                                path.AddLine(pixRect.Right, pixRect.Top, pixRect.Left, pixRect.Top);
                                path.AddLine(pixRect.Left, pixRect.Top, midx, pixRect.Bottom);
                                break;
                            }
                        case SymbolType.HDash:
                            {
                                float midy = (pixRect.Top + pixRect.Bottom) / 2.0f;
                                path.AddLine(pixRect.Left, midy, pixRect.Right, midy);
                                break;
                            }
                        case SymbolType.VDash:
                            {
                                float midx = (pixRect.Left + pixRect.Right) / 2.0f;
                                path.AddLine(midx, pixRect.Bottom, midx, pixRect.Top);
                                break;
                            }
                        case SymbolType.XCross:
                        default:
                            {
                                path.AddLine(pixRect.Left, pixRect.Bottom, pixRect.Right, pixRect.Top);
                                path.StartFigure();
                                path.AddLine(pixRect.Left, pixRect.Top, pixRect.Right, pixRect.Bottom);
                                break;
                            }
                    }

                    if (_fill.IsVisible)
                        if (this.Type == SymbolType.Circle || this.Type == SymbolType.Diamond || this.Type == SymbolType.Square || this.Type == SymbolType.Triangle || this.Type == SymbolType.TriangleDown)
                            using (Brush brush = _fill.MakeBrush(pixRect))
                                g.FillPath(brush, path);

                    if (_border.IsVisible)
                        using (Pen pen = _border.GetPen(pane, scaleFactor))
                            g.DrawPath(pen, path);
                }
            }
        }

        /// <summary>
        /// Determine if the specified screen point lies inside the bounding box of this
        /// <see cref="BoxObj"/>.
        /// </summary>
        /// <param name="pt">The screen point, in pixels</param>
        /// <param name="pane">
        /// A reference to the <see cref="PaneBase"/> object that is the parent or
        /// owner of this object.
        /// </param>
        /// <param name="g">
        /// A graphic device object to be drawn into.  This is normally e.Graphics from the
        /// PaintEventArgs argument to the Paint() method.
        /// </param>
        /// <param name="scaleFactor">
        /// The scaling factor to be used for rendering objects.  This is calculated and
        /// passed down by the parent <see cref="GraphPane"/> object using the
        /// <see cref="PaneBase.CalcScaleFactor"/> method, and is used to proportionally adjust
        /// font sizes, etc. according to the actual size of the graph.
        /// </param>
        /// <returns>true if the point lies in the bounding box, false otherwise</returns>
        override public bool PointInBox(PointF pt, PaneBase pane, Graphics g, float scaleFactor)
        {
            if (!base.PointInBox(pt, pane, g, scaleFactor))
                return false;

            // transform the x,y location from the user-defined
            // coordinate frame to the screen pixel location
            RectangleF pixRect = this.GetPointRect(pane, scaleFactor);

            return pixRect.Contains(pt);
        }

        /// <summary>
        /// Determines the shape type and Coords values for this GraphObj
        /// </summary>
        override public void GetCoords(PaneBase pane, Graphics g, float scaleFactor,
                out string shape, out string coords)
        {
            // transform the x,y location from the user-defined
            // coordinate frame to the screen pixel location
            RectangleF pixRect = this.GetPointRect(pane, scaleFactor);

            shape = "rect";
            coords = String.Format("{0:f0},{1:f0},{2:f0},{3:f0}",
                        pixRect.Left, pixRect.Top, pixRect.Right, pixRect.Bottom);
        }
        #endregion
    }
}