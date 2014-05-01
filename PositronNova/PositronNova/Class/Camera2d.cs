using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PositronNova
{
    public class Camera2d
    {
        //GraphicsDevice _device;
        //private int _worldWidth;
        //private int _worldHeight;
        //private const float ZOOM_UPPER_LIMIT = 2.0f;
        //private const float ZOOM_LOWER_LIMIT = 0.2f;
        //private float _zoom = 1.0f;
        //private Vector2 _pos = Vector2.Zero;
        //private float _rotation = 0.0f;

        public Matrix transforme;
        Viewport view;
        float zoom;
        static Vector2 origine;
        static public Vector2 Origine
        {
            get { return origine; }
        }

        KeyboardState keyRepeat;

        public Camera2d(Viewport newView)
        {
            view = newView;
            zoom = 1;
        }

        public void Update1(GameTime gt, KeyboardState keyboard, MouseState mouse)
        {
            if ((keyboard.IsKeyDown(Keys.Up) || mouse.Y <= 0) && origine.Y > 0)
                origine = new Vector2(origine.X, origine.Y - 5);
            if ((keyboard.IsKeyDown(Keys.Down) || mouse.Y >= PositronNova.winHeight) && origine.Y + PositronNova.winHeight < PositronNova.BackgroundTexture.Height)
                origine = new Vector2(origine.X, origine.Y + 5);
            if ((keyboard.IsKeyDown(Keys.Left) || mouse.X <= 0) && origine.X > 0)
                origine = new Vector2(origine.X - 5, origine.Y);
            if ((keyboard.IsKeyDown(Keys.Right) || mouse.X >= PositronNova.winWidth) && origine.X + PositronNova.winWidth < PositronNova.BackgroundTexture.Width)
                origine = new Vector2(origine.X + 5, origine.Y);

            if (keyboard.IsKeyDown(Keys.PageUp) && keyRepeat.IsKeyUp(Keys.PageUp)) // ZOOM : Pas opérationnel !!! Mais ça fonctionne ;)
                zoom -= 0.2f;
            if (keyboard.IsKeyDown(Keys.PageDown) && keyRepeat.IsKeyUp(Keys.PageDown))
                zoom += 0.2f;

            keyRepeat = Keyboard.GetState();
        }

        public void Update2(GameTime gt, KeyboardState keyboard, MouseState mouse)
        {
            transforme = Matrix.CreateScale(new Vector3(zoom, zoom, 0)) *
                Matrix.CreateTranslation(new Vector3(-origine.X, -origine.Y, 0));
        }

        //public float Rotation
        //{

        //    get { return _rotation; }

        //    set { _rotation = value; }

        //}

        //public Vector2 Pos
        //{

        //    get { return _pos; }

        //    set
        //    {

        //        //Détermine les position à ne pas dépasser pour garder l'image dans la fenêtre.

        //        float leftBarrier = (float)_device.Viewport.Width * .5f / _zoom;

        //        float rightBarrier = _worldWidth - (float)_device.Viewport.Width * .5f / _zoom;

        //        float bottomBarrier = _worldHeight - (float)_device.Viewport.Height * .5f / _zoom;

        //        float topBarrier = (float)_device.Viewport.Height * .5f / _zoom;

        //        _pos = value;

        //        if (_pos.X < leftBarrier)

        //            _pos.X = leftBarrier;

        //        if (_pos.X > rightBarrier)

        //            _pos.X = rightBarrier;

        //        if (_pos.Y < topBarrier)

        //            _pos.Y = topBarrier;

        //        if (_pos.Y > bottomBarrier)

        //            _pos.Y = bottomBarrier;

        //    }

        //}

        //public float Zoom
        //{

        //    get { return _zoom; }

        //    set
        //    {

        //        _zoom = value;

        //        if (_zoom < ZOOM_LOWER_LIMIT)

        //            _zoom = ZOOM_LOWER_LIMIT;

        //        if (_zoom > ZOOM_UPPER_LIMIT)

        //            _zoom = ZOOM_UPPER_LIMIT;

        //        //Vérifie si zoom est trop petit

        //        float zoomMinX = (float)_device.Viewport.Width / _worldWidth;

        //        float zoomMinY = (float)_device.Viewport.Height / _worldHeight;

        //        float zoomMin = (zoomMinX < zoomMinY) ? zoomMinY : zoomMinX;

        //        if (_zoom < zoomMin)
        //        {

        //            _zoom = zoomMin;

        //        }

        //    }

        //}

        //public Matrix GetTransformation()
        //{

        //    //Centre de la fenêtre est le point d'origine.

        //    return

        //        Matrix.CreateTranslation(new Vector3(-_pos.X, -_pos.Y, 0)) *

        //        Matrix.CreateRotationZ(Rotation) *

        //        Matrix.CreateScale(new Vector3(Zoom, Zoom, 1)) *

        //        Matrix.CreateTranslation(new Vector3(_device.Viewport.Width * 0.5f, _device.Viewport.Height * 0.5f, 0));

        //}


    }
}
