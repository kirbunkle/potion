using Microsoft.Xna.Framework;
using MonoGame.Extended.Animations;
using MonoGame.Extended.Animations.SpriteSheets;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PotionMaster
{
    public class Plant : Interactable
    {
        private SpriteSheetAnimationFactory spriteFactory;
        private int curSprite;
        private int[] plantAnimationFrames;
        private int[] plantSchedule;
        private int datePlanted;
        private int curDate;
        private Location myLocation;
        private int cropYield;

        private void CheckForPlantGrowth() 
        {
            if (Game1.gameDateTime.DaysSinceStart != curDate)
            {
                curDate = Game1.gameDateTime.DaysSinceStart;
                int s = 0;
                for (int i = 0; i < plantSchedule.Count(); i++)
                {
                    if ((curDate - datePlanted) >= plantSchedule[i])
                        s = plantAnimationFrames[i];
                    else
                        break;
                }
                if (s != curSprite)
                {
                    curSprite = s;
                    sprite = Game1.CreateAnimatedSprite(spriteFactory, curSprite.ToString());
                }
            }
        }

        public Plant(Rectangle pos, Seed seed, Location location)
        {
            posX = pos.X;
            posY = pos.Y;
            myLocation = location;
            spriteFactory = Game1.CreateAnimationFactory(seed.SpriteName(), seed.AnimationName());
            plantAnimationFrames = seed.PlantAnimationFrames();
            plantSchedule = seed.PlantSchedule();

            if (plantAnimationFrames.Count() != plantSchedule.Count())
            {
                throw new Exception(seed.Name() + " item has mismatching animation frames and schedule");
            }

            for (int i = 0; i < plantAnimationFrames.Count(); i++)
            {
                spriteFactory.Add(i.ToString(), new SpriteSheetAnimationData(new[] { plantAnimationFrames[i] }, isLooping: false));
            }
            curSprite = 0;
            curDate = 0;
            sprite = Game1.CreateAnimatedSprite(spriteFactory, curSprite.ToString());
            collisionBox = MakeCollisionBoundingBox();
            datePlanted = Game1.gameDateTime.DaysSinceStart;
            cropYield = seed.CropYield();
        }

        public bool ReadyToHarvest()
        {
            return plantSchedule.Last() <= (curDate - datePlanted);
        }

        public override void Interact()
        {
            if (ReadyToHarvest())
            {
                myLocation.Harvest(this);
                Game1.inventory.AddItem(cropYield);
            }
        }

        public override void Collide(Collidable obj)
        {

        }

        public new void Update()
        {
            CheckForPlantGrowth();
            base.Update();
        }
    }
}
