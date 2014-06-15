using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Audio;

namespace PositronNova.Class
{
    static public class Manager
    {
        static public Texture2D littleCinetique_t, cinetique_t, laser_t, ion_t, plasma_t, missile_t;
        public static Texture2D bloodSting_t;
        public static Texture2D fumee_t, explosion_t, grosseExplosion_t, explosionLaser_t, explosionPlasma_t, explosionMissile_t;
        static public Texture2D lifeBrick_t;

        static public Texture2D chasseur_t, chasseurLourd_t, corvette_t, destroyer_t, croiseur_t, cuirasse_t;
        static public Texture2D virus_t, bacterie_t, neurone_t, phagosome_t, kraken_t, nyan_t;
        static public Texture2D virusAnim_t, bacterieAnim_t, neuroneAnim_t, phagosomeAnim_t, krakenAnim_t, nyanAnim_t;

        public static SoundEffect missileLaunch_s, missileHit_s, laserFire_s, laserHit_s, plasmaFire_s, plasmaHit_s;
        static public SoundEffect deathNoise_s;

        static public SoundEffect unitePerdu_s, unitePrete_s, actionImpossible_s, rechercheTerminee_s;

        static public SpriteFont font_t;

        static public void ContentLoad(ContentManager content)
        {
            littleCinetique_t = content.Load<Texture2D>("img\\bullets\\LittleBullet");
            cinetique_t = content.Load<Texture2D>("img\\bullets\\Bullet");
            laser_t = content.Load<Texture2D>("img\\bullets\\LaserBullet"); // Pour l'instant...
            ion_t = content.Load<Texture2D>("img\\bullets\\IonBullet");
            plasma_t = content.Load<Texture2D>("img\\bullets\\PlasmaBullet");
            missile_t = content.Load<Texture2D>("img\\bullets\\Missile");
            bloodSting_t = content.Load<Texture2D>("img\\bullets\\BloodSting");

            fumee_t = content.Load<Texture2D>("img\\effects\\MissileFumee");
            explosion_t = content.Load<Texture2D>("img\\effects\\Explosion");
            grosseExplosion_t = content.Load<Texture2D>("img\\effects\\ExplosionMk3");
            explosionLaser_t = content.Load<Texture2D>("img\\effects\\ExplosionLaser");
            explosionPlasma_t = content.Load<Texture2D>("img\\effects\\ExplosionPlasma");
            explosionMissile_t = content.Load<Texture2D>("img\\effects\\Explosion2");

            lifeBrick_t = content.Load<Texture2D>("img\\life");

            chasseur_t = content.Load<Texture2D>("img\\ships\\Chasseur2");
            chasseurLourd_t = content.Load<Texture2D>("img\\ships\\Bombardier");
            corvette_t = content.Load<Texture2D>("img\\ships\\Corvette");
            destroyer_t = content.Load<Texture2D>("img\\ships\\Destroyer");
            croiseur_t = content.Load<Texture2D>("img\\ships\\Croiseur");
            cuirasse_t = content.Load<Texture2D>("img\\ships\\Cuirasse");
            virus_t = content.Load<Texture2D>("img\\life");
            bacterie_t = content.Load<Texture2D>("img\\alienShips\\Bacterie");
            neurone_t = content.Load<Texture2D>("img\\alienShips\\Neurone");
            phagosome_t = content.Load<Texture2D>("img\\alienShips\\Phagosome");
            kraken_t = content.Load<Texture2D>("img\\alienShips\\Kraken");
            nyan_t = content.Load<Texture2D>("img\\life");

            virusAnim_t = content.Load<Texture2D>("img\\life");
            bacterieAnim_t = content.Load<Texture2D>("img\\alienShips\\BacterieSheet");
            neuroneAnim_t = content.Load<Texture2D>("img\\alienShips\\NeuroneSheet");
            phagosomeAnim_t = content.Load<Texture2D>("img\\alienShips\\PhagosomeSheet");
            krakenAnim_t = content.Load<Texture2D>("img\\alienShips\\KrakenSheet");
            nyanAnim_t = content.Load<Texture2D>("img\\life");

            laserFire_s = content.Load<SoundEffect>("sounds\\laserFire");
            laserHit_s = content.Load<SoundEffect>("sounds\\laserHit");
            plasmaFire_s = content.Load<SoundEffect>("sounds\\plasmaFire");
            plasmaHit_s = content.Load<SoundEffect>("sounds\\plasmaHit");
            missileLaunch_s = content.Load<SoundEffect>("sounds\\MissileLaunch");
            missileHit_s = content.Load<SoundEffect>("sounds\\missileHit");

            deathNoise_s = content.Load<SoundEffect>("sounds\\shipDeath");

            unitePerdu_s = content.Load<SoundEffect>("sounds\\voix\\unite perdu");
            unitePrete_s = content.Load<SoundEffect>("sounds\\voix\\unite prete");
            actionImpossible_s = content.Load<SoundEffect>("sounds\\voix\\action impossible1");
            rechercheTerminee_s = content.Load<SoundEffect>("sounds\\voix\\recherche terminee");

            font_t = content.Load<SpriteFont>("Affichage_mouse");
        }
    }
}
