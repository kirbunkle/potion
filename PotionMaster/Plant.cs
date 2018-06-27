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

        public Plant(Rectangle loc, Item item)
        {
            posX = loc.X;
            posY = loc.Y;
            spriteFactory = Game1.CreateAnimationFactory(item.SpriteName, item.AnimationName);
            plantAnimationFrames = item.PlantAnimationFrames;
            plantSchedule = item.PlantSchedule;

            if (plantAnimationFrames.Count() != plantSchedule.Count())
            {
                throw new Exception(item.Name + " item has mismatching animation frames and schedule");
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
        }

        public override void Interact()
        {

        }

        public new void Update()
        {
            CheckForPlantGrowth();
            base.Update();
        }
    }
}
