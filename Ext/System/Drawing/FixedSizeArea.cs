using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Ext.System.Drawing {
    public class FixedSizeArea {

        public static float MoveSensitivity = 1f;


        private Bitmap _Picture = null;
        private Graphics _Drawer = null;
        private Color _Background = Color.White;
        private Brush _BackgroundBrush = new SolidBrush(Color.White);
        private EventArgs _EventCache = new EventArgs();
        private bool _MouseDown = false;
        private float _LastX = 0;
        private float _LastY = 0;
        private float _Zoom = 1;

        public int Width { get; protected set; } = 100;
        public int Height { get; protected set; } = 100;
        public Image SourceImage { get; protected set; } = null;
        public float ImagePositionX { get; protected set; } = 0;
        public float ImagePositionY { get; protected set; } = 0;

        public event EventHandler<EventArgs> SizeChanged;
        public event EventHandler<EventArgs> Updated;

        public Color Background
        {
            get
            {
                return _Background;
            }
            set
            {
                if(_Background == value)
                    return;
                _Background = value;
                _BackgroundBrush = new SolidBrush(_Background);
            }
        }

        public float Zoom
        {
            get
            {
                return _Zoom;
            }
            set
            {
                if(value < 0.05 || value > 10.0f)
                    return;
                _Zoom = value;
            }
        }

        private FixedSizeArea() {
            _Picture = new Bitmap(Width, Height);
            _Drawer = Graphics.FromImage(_Picture);
        }

        public FixedSizeArea(int Width, int Height) {
            this.Width = Width;
            this.Height = Height;
            _Picture = new Bitmap(Width, Height);
            _Drawer = Graphics.FromImage(_Picture);
        }

        public FixedSizeArea(PictureBox picBox) {
            this.Width = picBox.Width;
            this.Height = picBox.Height;
            _Picture = new Bitmap(picBox.Width, picBox.Height);
            _Drawer = Graphics.FromImage(_Picture);
            MergeControl(picBox);
        }

        public void Resize(int Width, int Height) {
            if(Width < 100)
                Width = 100;
            if(Height < 100)
                Height = 100;
            this.Width = Width;
            this.Height = Height;
            _Picture = new Bitmap(Width, Height);
            _Drawer = Graphics.FromImage(_Picture);
            ImagePositionX = 0;
            ImagePositionY = 0;
            Redraw();
            OnSizeChanged();
        }

        public void SetImage(Image img) {
            this.SourceImage = img;
            Redraw();
        }

        public void Redraw() {
            _Drawer.FillRectangle(_BackgroundBrush, 0, 0, Width, Height);
            if(this.SourceImage == null)
                return;
            _Drawer.DrawImage(SourceImage, ImagePositionX, ImagePositionY, SourceImage.Size.Width * _Zoom, SourceImage.Size.Height * _Zoom);
            OnUpdated();
        }

        public Image GetCurrentFrame() {
            return _Picture;
        }

        public void MouseDown(float X, float Y) {
            _LastX = X;
            _LastY = Y;
            _MouseDown = true;
        }

        public void MouseMove(float X, float Y) {
            if(!_MouseDown)
                return;
            ChangePosition(X, Y);
            _LastX = X;
            _LastY = Y;
            Redraw();
        }

        public void MouseUp(float X, float Y) {
            if(!_MouseDown)
                return;
            ChangePosition(X, Y);
            Redraw();
            _MouseDown = false;
        }

        private void ChangePosition(float X, float Y) {
            ImagePositionX -= (_LastX - X) * MoveSensitivity;
            ImagePositionY -= (_LastY - Y) * MoveSensitivity;
            ImprovePosition();
        }

        private void ImprovePosition() {
            if(SourceImage == null)
                return;
            if(ImagePositionX > 0)
                ImagePositionX = 0;
            if(ImagePositionY > 0)
                ImagePositionY = 0;
            if(Width > SourceImage.Width * _Zoom)
                ImagePositionX = (Width - SourceImage.Width * _Zoom) * (0.5f);
            else if(-ImagePositionX > SourceImage.Width * _Zoom - Width)
                ImagePositionX = Width - SourceImage.Width * _Zoom;
            if(Height > SourceImage.Height * _Zoom)
                ImagePositionY = 0;
            else if(-ImagePositionY > SourceImage.Height * _Zoom - Height)
                ImagePositionY = Height - SourceImage.Height * _Zoom;
        }

        public void ZoomIn() {
            ChangeZoom(0.1f);
        }

        public void ZoomOut() {
            ChangeZoom(-0.1f);
        }

        public void MergeControl(PictureBox pic) {
            pic.MouseDown += Frm_MouseDown;
            pic.MouseMove += Frm_MouseMove;
            pic.MouseUp += Frm_MouseUp;
            pic.MouseWheel += Frm_MouseWheel;
            pic.SizeChanged += (o, e) => {
                Resize(pic.Width, pic.Height);
                pic.Image = GetCurrentFrame();
            };
            Updated += (o, e) => { pic.Invalidate(); };
            pic.Image = GetCurrentFrame();
        }

        private void Frm_MouseWheel(object sender, MouseEventArgs e) {
            if(e.Delta > 0) {
                ZoomIn();
            } else {
                ZoomOut();
            }
        }

        private void Frm_MouseUp(object sender, MouseEventArgs e) {
            MouseUp(e.X, e.Y);
        }

        private void Frm_MouseMove(object sender, MouseEventArgs e) {
            MouseMove(e.X, e.Y);
        }

        private void Frm_MouseDown(object sender, MouseEventArgs e) {
            MouseDown(e.X, e.Y);
        }

        protected void ChangeZoom(float value) {
            if(SourceImage == null)
                return;
            var kX = ImagePositionX / (Width - SourceImage.Width * _Zoom);
            var kY = ImagePositionY / (Height - SourceImage.Height * _Zoom);
            Zoom += value;
            ImagePositionX = kX * (Width - SourceImage.Width * _Zoom);
            ImagePositionY = kY * (Height - SourceImage.Height * _Zoom);
            ImprovePosition();
            Redraw();
        }

        protected void OnSizeChanged() {
            SizeChanged?.Invoke(this, _EventCache);
        }

        protected void OnUpdated() {
            Updated?.Invoke(this, _EventCache);
        }

    }
}
