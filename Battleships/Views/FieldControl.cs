using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Windows.Forms;
using Battleships.Domain;

namespace Battleships.Views
{
    public partial class FieldControl : UserControl
    {
        private IField field;
        private bool fogOfWar;
        private Dictionary<Domain.Point, Rectangle> pointToRectangle;
        private IShip selectedShip = null;
        private bool configured = false;

        public FieldControl()
        {
            InitializeComponent();
            DoubleBuffered = true;

            Resize += HandleResize;
            Click += HandleClick;
            DoubleClick += HandleClick;
        }

        public void Configure(IField field, bool fogOfWar)
        {
            if (configured)
                throw new InvalidOperationException();

            this.field = field;
            this.field.Updated += () => Invalidate();
            this.fogOfWar = fogOfWar;
            pointToRectangle = GeneratePointToRectangle(field, Width, Height);
            configured = true;
        }

        public event Action<Domain.Point, MouseEventArgs> ClickOnPoint;

        public void SetSelectedShip(IShip selectedShip)
        {
            this.selectedShip = selectedShip;
            Invalidate();
        }

        public IShip SelectedShip => selectedShip;

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            e.Graphics.SmoothingMode = SmoothingMode.AntiAlias;
            e.Graphics.DrawBackground(Width, Height);

            if (!configured)
                return;

            foreach (var pair in pointToRectangle)
                e.Graphics.DrawEmptyCell(pair.Value);

            var conflictingPoints = field.GetConflictingPoints();
            var shots = field.GetShots();
            foreach (var shot in shots)
                e.Graphics.DrawShotCell(pointToRectangle[shot]);

            foreach (var ship in field.GetShips().Where(it => !it.Equals(selectedShip)))
                PaintShip(e.Graphics, ship, conflictingPoints, shots, false);

            if (selectedShip != null)
                PaintShip(e.Graphics, selectedShip, conflictingPoints, shots, true);
        }

        private void PaintShip(Graphics graphics, IShip ship,
            ISet<Domain.Point> conflictingPoints, ISet<Domain.Point> shots, bool useLight)
        {
            foreach (var point in ship.GetPositionPoints())
            {
                if (!fogOfWar || shots.Contains(point))
                {
                    graphics.DrawShipCell(pointToRectangle[point],
                        useLight: useLight,
                        inConflict: conflictingPoints.Contains(point),
                        isShot: shots.Contains(point));
                }
            }
        }

        private void HandleResize(object sender, EventArgs e)
        {
            if (!configured)
                return;

            pointToRectangle = GeneratePointToRectangle(field, Width, Height);
            Invalidate();
        }

        private void HandleClick(object sender, EventArgs e)
        {
            if (!configured)
                return;

            var args = e as MouseEventArgs;
            var pairs = pointToRectangle.Where(it => it.Value.Contains(args.Location)).ToList();
            if (pairs.Count > 0)
                ClickOnPoint?.Invoke(pairs[0].Key, args);
        }

        private static Dictionary<Domain.Point, Rectangle> GeneratePointToRectangle(IField field, int width, int height)
        {
            var result = new Dictionary<Domain.Point, Rectangle>();
            for (var x = 0; x < field.Width; x++)
            {
                for (int y = 0; y < field.Height; y++)
                {
                    var left = (width - 2) * x / field.Width + 1;
                    var right = (width - 2) * (x + 1) / field.Width + 1;
                    var top = (height - 2) * y / field.Height + 1;
                    var bottom = (height - 2) * (y + 1) / field.Height + 1;
                    var rectangle = new Rectangle(left, top, right - left, bottom - top);
                    result.Add(new Domain.Point(x, y), rectangle);
                }
            }
            return result;
        }
    }
}
