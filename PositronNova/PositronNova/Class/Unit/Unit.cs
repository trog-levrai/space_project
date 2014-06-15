using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace PositronNova.Class.Unit
{
    public enum UnitSide
    {
        Humain, Alien
    };

    public enum UnitType
    {
        Chasseur, Bombardier, Corvette, Destroyer, Croiseur, Cuirasse,
        Virus, Bacterie, Neurone, Phagosome, Kraken, Nyan
    };

    public class Unit : sprite
    {
        // Sound et tir
        private int range;
        public int Range
        { get { return range; } }
        System.TimeSpan fireRate;
        System.TimeSpan last;

        SoundEffect deathNoise;
        Bullet localBullet;
        BulletType weaponType;

        // Caractéristiques unit
        private Color color;
        private SpriteFont _font;
        private string name;
        public string Name
        {
            get { return name; }
        }
        UnitType unitType;
        UnitSide side;
        public UnitSide Side { get { return side; } }

        private bool hasTarget;
        public bool HasTarget
        {
            get { return hasTarget; }
            set { hasTarget = value; }
        }

        int pv_max;
        public int Pv_max
        {
            get { return pv_max; }
        }
        int pv;
        public int Pv
        {
            get { return pv; }
            set { pv = value; }
        }
        HealthBar lifeBar;
        public HealthBar LifeBar
        {
            get { return lifeBar; }
        }

        int champDeVisionWidth, champDeVisionHeight;

        // Cible
        private Unit enn;
        public Unit Ennemy
        {
            get { return enn; }
            set { enn = value; }
        }

        private static Ressources requiredResources = new Ressources(0, 0);

        //////////////////////////////////// CONSTRUCTEURS /////////////////////////////////////

        public Unit(Vector2 position, UnitType type)
            : base(position)
        {
            unitType = type;
        }

        public Unit(String name, Vector2 position, UnitType type)
            : base(position)
        {
            this.name = name;
            unitType = type;
            direction = new Vector2(1, 0); // Important pour l'initialisation des hitboxes et CIV !! C'est pour qu'elles soient bien placées dès le départ
        }

        public override void Init()
        {
            if (side == UnitSide.Humain)
                color = Color.Aqua;
            else
                color = Color.IndianRed;
            //speed = 0;
            //direction = Vector2.One;
            destination = position;

            _font = Manager.font_t;
            deathNoise = Manager.deathNoise_s;

            switch (unitType)
            {
                case UnitType.Chasseur:
                    side = UnitSide.Humain;
                    texture = Manager.chasseur_t;
                    nbHitBoxes = 1;
                    decalageHitBoxes = 0;
                    hitBoxesWidth = 10; // Hitboxes ??? Je ne sais pas ce que c'est :o)
                    hitBoxesHeight = 10;
                    nbCIV = 1;
                    CIVWidth = 20; // CIV c'est le rectangle collisionInterVaisseau
                    CIVHeight = 20;
                    champDeVisionWidth = 100; // ça bah... c'est pour le champ de vision :o)
                    champDeVisionHeight = 100;
                    fireRate = new TimeSpan(0, 0, 0, 0, 600);
                    weaponType = BulletType.LittleCinetique;
                    pv_max = 10;
                    if (Universite.Changement_moteur)
                        speed = 2.8f;
                    else
                        speed = 2.2f;
                    range = 200;
                    requiredResources = new Ressources(10, 5);
                    break;
                case UnitType.Bombardier:
                    side = UnitSide.Humain;
                    texture = Manager.chasseurLourd_t;
                    nbHitBoxes = 3;
                    decalageHitBoxes = 14;
                    hitBoxesWidth = 16;
                    hitBoxesHeight = 16;
                    nbCIV = 3;
                    CIVWidth = 32;
                    CIVHeight = 32;
                    champDeVisionWidth = 150;
                    champDeVisionHeight = 150;
                    fireRate = new TimeSpan(0, 0, 0, 0, 900);
                    weaponType = BulletType.Cinetique;
                    pv_max = 40;
                    if (Universite.Changement_moteur)
                        speed = 2.5f;
                    else
                        speed = 1.9f;
                    range = 300;
                    requiredResources = new Ressources(40, 50);
                    break;
                case UnitType.Corvette:
                    side = UnitSide.Humain;
                    texture = Manager.corvette_t;
                    nbHitBoxes = 3;
                    decalageHitBoxes = 20;
                    hitBoxesWidth = 25;
                    hitBoxesHeight = 25;
                    nbCIV = 3;
                    CIVWidth = 50;
                    CIVHeight = 50;
                    champDeVisionWidth = 200;
                    champDeVisionHeight = 200;
                    fireRate = new TimeSpan(0, 0, 0, 0, 1500);
                    weaponType = BulletType.Laser;
                    pv_max = 60;
                    if (Universite.Changement_moteur)
                        speed = 2.2f;
                    else
                        speed = 1.6f;
                    range = 400;
                    requiredResources = new Ressources(15, 10);
                    break;
                case UnitType.Destroyer:
                    side = UnitSide.Humain;
                    texture = Manager.destroyer_t;
                    nbHitBoxes = 5;
                    decalageHitBoxes = 30;
                    hitBoxesWidth = 40;
                    hitBoxesHeight = 40;
                    nbCIV = 5;
                    CIVWidth = 80;
                    CIVHeight = 80;
                    champDeVisionWidth = 250;
                    champDeVisionHeight = 250;
                    fireRate = new TimeSpan(0, 0, 0, 0, 2000);
                    weaponType = BulletType.Ion;
                    pv_max = 90;
                    if (Universite.Changement_moteur)
                        speed = 2f;
                    else
                        speed = 1.4f;
                    range = 450;
                    requiredResources = new Ressources(20, 15);
                    break;
                case UnitType.Croiseur:
                    side = UnitSide.Humain;
                    texture = Manager.croiseur_t;
                    nbHitBoxes = 5;
                    decalageHitBoxes = 30;
                    hitBoxesWidth = 40;
                    hitBoxesHeight = 40;
                    nbCIV = 5;
                    CIVWidth = 80;
                    CIVHeight = 80;
                    champDeVisionWidth = 300;
                    champDeVisionHeight = 300;
                    fireRate = new TimeSpan(0, 0, 0, 0, 2500);
                    weaponType = BulletType.Plasma;
                    pv_max = 110;
                    if (Universite.Changement_moteur)
                        speed = 1.9f;
                    else
                        speed = 1.3f;
                    range = 500;
                    requiredResources = new Ressources(25, 20);
                    break;
                case UnitType.Cuirasse:
                    side = UnitSide.Humain;
                    texture = Manager.cuirasse_t;
                    nbHitBoxes = 5;
                    decalageHitBoxes = 40;
                    hitBoxesWidth = 50;
                    hitBoxesHeight = 50;
                    nbCIV = 5;
                    CIVWidth = 100;
                    CIVHeight = 100;
                    champDeVisionWidth = 350;
                    champDeVisionHeight = 350;
                    fireRate = new TimeSpan(0, 0, 0, 3);
                    weaponType = BulletType.Missile;
                    pv_max = 250;
                    if (Universite.Changement_moteur)
                        speed = 1.6f;
                    else
                        speed = 1f;
                    range = 600;
                    requiredResources = new Ressources(80, 110);
                    break;
                case UnitType.Virus:
                    side = UnitSide.Alien;
                    break;
                case UnitType.Bacterie:
                    side = UnitSide.Alien;
                    texture = Manager.bacterie_t;
                    textureAnime = Manager.bacterieAnim_t;
                    nbHitBoxes = 3;
                    decalageHitBoxes = 6;
                    hitBoxesWidth = 10;
                    hitBoxesHeight = 10;
                    nbCIV = 3;
                    CIVWidth = 20;
                    CIVHeight = 20;
                    nbFrame = 3;
                    frameWidth = 53;
                    frameHeight = 10;
                    timeToNextFrame = new TimeSpan(0, 0, 0, 0, 60);
                    fireRate = new TimeSpan(0, 0, 0, 0, 900);
                    weaponType = BulletType.BloodSting;
                    pv_max = 40;
                    speed = 1.9f;
                    range = 300;
                    break;
                case UnitType.Neurone:
                    side = UnitSide.Alien;
                    texture = Manager.neurone_t;
                    textureAnime = Manager.neuroneAnim_t;
                    nbHitBoxes = 1;
                    decalageHitBoxes = 0;
                    hitBoxesWidth = 20;
                    hitBoxesHeight = 20;
                    nbCIV = 1;
                    CIVWidth = 40;
                    CIVHeight = 40;
                    nbFrame = 3;
                    frameWidth = 40;
                    frameHeight = 40;
                    timeToNextFrame = new TimeSpan(0, 0, 0, 0, 60);
                    fireRate = new TimeSpan(0, 0, 0, 0, 400);
                    weaponType = BulletType.BloodSting;
                    pv_max = 60;
                    speed = 1.6f;
                    range = 400;
                    break;
                case UnitType.Phagosome:
                    side = UnitSide.Alien;
                    texture = Manager.phagosome_t;
                    textureAnime = Manager.phagosomeAnim_t;
                    nbHitBoxes = 1;
                    decalageHitBoxes = 0;
                    hitBoxesWidth = 75;
                    hitBoxesHeight = 75;
                    nbCIV = 1;
                    CIVWidth = 150;
                    CIVHeight = 150;
                    nbFrame = 5;
                    frameWidth = 150;
                    frameHeight = 150;
                    timeToNextFrame = new TimeSpan(0, 0, 0, 0, 120);
                    fireRate = new TimeSpan(0, 0, 0, 0, 2000);
                    weaponType = BulletType.BloodSting;
                    pv_max = 90;
                    speed = 1.4f;
                    range = 450;
                    break;
                case UnitType.Kraken:
                    side = UnitSide.Alien;
                    texture = Manager.kraken_t;
                    textureAnime = Manager.krakenAnim_t;
                    nbHitBoxes = 1;
                    decalageHitBoxes = 0;
                    hitBoxesWidth = 150;
                    hitBoxesHeight = 60;
                    nbCIV = 1;
                    CIVWidth = 220;
                    CIVHeight = 100;
                    nbFrame = 4;
                    frameWidth = 200;
                    frameHeight = 100;
                    timeToNextFrame = new TimeSpan(0, 0, 0, 0, 60);
                    fireRate = new TimeSpan(0, 0, 0, 0, 2000);
                    weaponType = BulletType.BloodSting;
                    pv_max = 300;
                    speed = 1;
                    range = 500;
                    break;
            }

            last = new TimeSpan(0);
            pv = pv_max;

            lifeBar = new HealthBar(pv, texture.Width);

            centre = new Vector2(texture.Width / 2, texture.Height / 2);
            // Creation des HitBoxes
            hitBoxes = new Rectangle[nbHitBoxes];
            for (int i = 0; i < nbHitBoxes; i++)
                hitBoxes[i] = new Rectangle((int)(position.X + (decalageHitBoxes * i)), (int)(position.Y + centre.Y - hitBoxesHeight / 2), hitBoxesWidth, hitBoxesHeight);
            // ----
            // Creation des rectangles CollisionInterVaisseau
            collisionInterVaisseau = new Rectangle[nbCIV];
            for (int i = 0; i < nbCIV; i++)
                collisionInterVaisseau[i] = new Rectangle((int)(position.X + (decalageHitBoxes * i)), (int)(position.Y + centre.Y - CIVHeight / 2), CIVWidth, CIVHeight);
            // ----

            // Placement des hitBoxes et et des CIV
            PlacementHitBoxes();

            champDeVision = new Rectangle((int)(position.X + centre.X - champDeVisionWidth / 2), (int)(position.Y + centre.Y - champDeVisionHeight / 2), champDeVisionWidth, champDeVisionHeight);

            base.Init();
        }

        public override void Update(GameTime gt)
        {
            last = last.Add(gt.ElapsedGameTime);
            if (hasTarget && last >= fireRate && enn != null && Math.Pow(position.X - enn.position.X, 2) + Math.Pow(position.Y - enn.position.Y, 2) <= Math.Pow(range, 2))
            {
                shoot();
                last = new TimeSpan(0);
            }

            deplacement(gt);
            if (Destruction())
                if (position.X + centre.X < PositronNova.winWidth + Camera2d.Origine.X &&
                        position.X + centre.X > Camera2d.Origine.X &&
                        position.Y + centre.Y < PositronNova.winWidth + Camera2d.Origine.Y &&
                        position.Y + centre.Y > Camera2d.Origine.Y)
                {
                    deathNoise.Play();
                }

            // Bords du background
            if (position.X <= 5 || position.X + texture.Width >= PositronNova.BackgroundTexture.Width - 5 ||
                position.Y <= 5 || position.Y + texture.Height >= PositronNova.BackgroundTexture.Height - 5)
                moving = false;

            //texture.GetData(textureData);
            lifeBar.Update(pv);

            // Update du changement de frame
            frameTimer = frameTimer.Add(gt.ElapsedGameTime);

            if (timeToNextFrame <= frameTimer)
            {
                frameTimer = new TimeSpan(0);
                if (frameSquare == 0)
                    reverseFrame = false;
                else if (frameSquare == nbFrame - 1)
                    reverseFrame = true;

                if (reverseFrame)
                    frameSquare--;
                else
                    frameSquare++;
            }

            base.Update(gt);
        }

        public override void Draw(SpriteBatch sb)
        {
            //Mettre "|| !friendly" pour tester l'effet de la methode attack
            if (side == UnitSide.Humain)
            {
                if (direction.X > 0)
                {
                    sb.Draw(texture, position + centre, null, Color.White, (float)Math.Atan(direction.Y / direction.X), centre, 1f, SpriteEffects.FlipHorizontally, 0);
                }
                else if (direction.X < 0)
                {
                    sb.Draw(texture, position + centre, null, Color.White, (float)Math.Atan(direction.Y / direction.X), centre, 1f, SpriteEffects.None, 0);
                }
                else
                {
                    sb.Draw(texture, position, Color.White);
                }

                sb.DrawString(_font, name, new Vector2(position.X - 3, position.Y - 25), color);
                //sb.Draw(Manager.lifeBrick_t, destination, Color.White);  // Dessine la destination pour voir comment elle évolue :o)
            }
            else
            {
                sb.Draw(textureAnime, position, new Rectangle(frameWidth * frameSquare, 0, frameWidth, frameHeight), Color.White);
            }

            //for (int i = 0; i < collisionInterVaisseau.Length; i++)
            //    sb.Draw(Manager.lifeBrick_t, hitBoxes[i], Color.White);

            base.Draw(sb);
        }

        /////////////////////////////////// METHODES //////////////////////////

        void deplacement(GameTime gameTime)
        {
            int stopPrecision = 2;

            if (moving)
            {
                destination = new Vector2((int)destination.X, (int)destination.Y);
                direction = new Vector2(destination.X - position.X, destination.Y - position.Y);
                direction.Normalize();
                position += direction * speed; // Silence ça pousse... ahem... bouge ! :o)
                CorrectionTrajectoire();
                PlacementHitBoxes();
                champDeVision.X = (int)(position.X + centre.X - champDeVisionWidth / 2);
                champDeVision.Y = (int)(position.Y + centre.Y - champDeVisionHeight / 2);
                if (Math.Abs(position.X - destination.X) <= stopPrecision && Math.Abs(position.Y - destination.Y) <= stopPrecision) // empêche le ship de tourner (vibrer?) autour de la destination avec stopPrecision
                    position = destination;
            }
            else
            {
                destination = position;
            }
            moving = (position != destination) || (hasTarget && last >= fireRate && enn != null && Math.Pow(position.X - enn.position.X, 2) + Math.Pow(position.Y - enn.position.Y, 2) <= Math.Pow(range, 2));
        }

        void shoot()
        {
            if (enn != null && enn.pv > 0)
            {
                localBullet = new Bullet(position + centre, enn, weaponType);
                PositronNova.AddBullet(localBullet);
                // Les kraken font apparaître des unités
                if (unitType == UnitType.Kraken)
                {
                    Unit localUnit = new Unit(position + new Vector2(0, 50), (UnitType)PositronNova.Rand.Next((int)UnitType.Bacterie, (int)UnitType.Neurone + 1));
                    localUnit.Init();
                    PositronNova.UnitList.Add(localUnit);
                }
                // Les sons à l'intérieur de l'écran
                if (position.X + centre.X < PositronNova.winWidth + Camera2d.Origine.X &&
                    position.X + centre.X > Camera2d.Origine.X &&
                    position.Y + centre.Y < PositronNova.winWidth + Camera2d.Origine.Y &&
                    position.Y + centre.Y > Camera2d.Origine.Y)
                {
                    if (weaponType == BulletType.Missile)
                        Manager.missileLaunch_s.Play();
                    else if (weaponType == BulletType.Laser)
                        Manager.laserFire_s.Play();
                    else if (weaponType == BulletType.Plasma)
                        Manager.plasmaFire_s.Play();
                }
            }
            else
                hasTarget = false;
        }

        public bool Destruction()
        {
            return pv <= 0;
        }

        public void PlacementHitBoxes()
        {
            if (unitType == UnitType.Chasseur || unitType == UnitType.Neurone || unitType == UnitType.Phagosome || unitType == UnitType.Kraken)
            {
                hitBoxes[0].X = (int)(position.X + centre.X - hitBoxes[0].Width / 2);
                hitBoxes[0].Y = (int)(position.Y + centre.Y - hitBoxes[0].Height / 2);

                collisionInterVaisseau[0].X = (int)(position.X + centre.X - collisionInterVaisseau[0].Width / 2);
                collisionInterVaisseau[0].Y = (int)(position.Y + centre.Y - collisionInterVaisseau[0].Height / 2);
            }
            else if (unitType == UnitType.Bombardier || unitType == UnitType.Corvette || unitType == UnitType.Bacterie)
            {
                hitBoxes[1].X = (int)(position.X + centre.X - hitBoxes[1].Width / 2);
                hitBoxes[1].Y = (int)(position.Y + centre.Y - hitBoxes[1].Height / 2);

                hitBoxes[0].X = (int)(decalageHitBoxes * Math.Cos(Math.Atan(direction.Y / direction.X))) + hitBoxes[1].X;
                hitBoxes[0].Y = (int)(decalageHitBoxes * Math.Sin(Math.Atan(direction.Y / direction.X))) + hitBoxes[1].Y;

                hitBoxes[2].X = (int)(decalageHitBoxes * -Math.Cos(Math.Atan(direction.Y / direction.X))) + hitBoxes[1].X;
                hitBoxes[2].Y = (int)(decalageHitBoxes * -Math.Sin(Math.Atan(direction.Y / direction.X))) + hitBoxes[1].Y;

                collisionInterVaisseau[1].X = (int)(position.X + centre.X - collisionInterVaisseau[1].Width / 2);
                collisionInterVaisseau[1].Y = (int)(position.Y + centre.Y - collisionInterVaisseau[1].Height / 2);

                collisionInterVaisseau[0].X = (int)(decalageHitBoxes * Math.Cos(Math.Atan(direction.Y / direction.X))) + collisionInterVaisseau[1].X;
                collisionInterVaisseau[0].Y = (int)(decalageHitBoxes * Math.Sin(Math.Atan(direction.Y / direction.X))) + collisionInterVaisseau[1].Y;

                collisionInterVaisseau[2].X = (int)(decalageHitBoxes * -Math.Cos(Math.Atan(direction.Y / direction.X))) + collisionInterVaisseau[1].X;
                collisionInterVaisseau[2].Y = (int)(decalageHitBoxes * -Math.Sin(Math.Atan(direction.Y / direction.X))) + collisionInterVaisseau[1].Y;
            }
            else if (unitType == UnitType.Destroyer || unitType == UnitType.Croiseur || unitType == UnitType.Cuirasse)
            {
                hitBoxes[2].X = (int)(position.X + centre.X - hitBoxes[2].Width / 2);
                hitBoxes[2].Y = (int)(position.Y + centre.Y - hitBoxes[2].Height / 2);

                hitBoxes[1].X = (int)(decalageHitBoxes * Math.Cos(Math.Atan(direction.Y / direction.X))) + hitBoxes[2].X;
                hitBoxes[1].Y = (int)(decalageHitBoxes * Math.Sin(Math.Atan(direction.Y / direction.X))) + hitBoxes[2].Y;

                hitBoxes[0].X = (int)(2 * decalageHitBoxes * Math.Cos(Math.Atan(direction.Y / direction.X))) + hitBoxes[2].X;
                hitBoxes[0].Y = (int)(2 * decalageHitBoxes * Math.Sin(Math.Atan(direction.Y / direction.X))) + hitBoxes[2].Y;

                hitBoxes[3].X = (int)(decalageHitBoxes * -Math.Cos(Math.Atan(direction.Y / direction.X))) + hitBoxes[2].X;
                hitBoxes[3].Y = (int)(decalageHitBoxes * -Math.Sin(Math.Atan(direction.Y / direction.X))) + hitBoxes[2].Y;

                hitBoxes[4].X = (int)(2 * decalageHitBoxes * -Math.Cos(Math.Atan(direction.Y / direction.X))) + hitBoxes[2].X;
                hitBoxes[4].Y = (int)(2 * decalageHitBoxes * -Math.Sin(Math.Atan(direction.Y / direction.X))) + hitBoxes[2].Y;

                collisionInterVaisseau[2].X = (int)(position.X + centre.X - collisionInterVaisseau[2].Width / 2);
                collisionInterVaisseau[2].Y = (int)(position.Y + centre.Y - collisionInterVaisseau[2].Height / 2);

                collisionInterVaisseau[1].X = (int)(decalageHitBoxes * Math.Cos(Math.Atan(direction.Y / direction.X))) + collisionInterVaisseau[2].X;
                collisionInterVaisseau[1].Y = (int)(decalageHitBoxes * Math.Sin(Math.Atan(direction.Y / direction.X))) + collisionInterVaisseau[2].Y;

                collisionInterVaisseau[0].X = (int)(2 * decalageHitBoxes * Math.Cos(Math.Atan(direction.Y / direction.X))) + collisionInterVaisseau[2].X;
                collisionInterVaisseau[0].Y = (int)(2 * decalageHitBoxes * Math.Sin(Math.Atan(direction.Y / direction.X))) + collisionInterVaisseau[2].Y;

                collisionInterVaisseau[3].X = (int)(decalageHitBoxes * -Math.Cos(Math.Atan(direction.Y / direction.X))) + collisionInterVaisseau[2].X;
                collisionInterVaisseau[3].Y = (int)(decalageHitBoxes * -Math.Sin(Math.Atan(direction.Y / direction.X))) + collisionInterVaisseau[2].Y;

                collisionInterVaisseau[4].X = (int)(2 * decalageHitBoxes * -Math.Cos(Math.Atan(direction.Y / direction.X))) + collisionInterVaisseau[2].X;
                collisionInterVaisseau[4].Y = (int)(2 * decalageHitBoxes * -Math.Sin(Math.Atan(direction.Y / direction.X))) + collisionInterVaisseau[2].Y;
            }
        }

        public void DrawHitboxes(SpriteBatch sb)
        {
            for (int i = 0; i < hitBoxes.Length; i++)
                sb.Draw(Manager.lifeBrick_t, hitBoxes[i], Color.White);
        }

        Vector2 CollisionInterVaisseau()
        {
            for (int i = 0; i < PositronNova.UnitList.Count; i++)
            {
                if (PositronNova.UnitList[i] != this)
                {
                    for (int j = 0; j < collisionInterVaisseau.Length; j++)
                        for (int k = 0; k < PositronNova.UnitList[i].collisionInterVaisseau.Length; k++)
                            if (collisionInterVaisseau[j].Intersects(PositronNova.UnitList[i].collisionInterVaisseau[k]))
                            {
                                return new Vector2(PositronNova.UnitList[i].position.X - position.X, PositronNova.UnitList[i].position.Y - position.Y);
                            }
                }
            }
            return Vector2.Zero;
        }

        void CorrectionTrajectoire()
        {
            if (CollisionInterVaisseau() != Vector2.Zero)
            {
                Vector2 temp = CollisionInterVaisseau();
                temp.Normalize();
                position -= temp * speed;
                destination -= temp * speed;
            }
        }
    }
}
