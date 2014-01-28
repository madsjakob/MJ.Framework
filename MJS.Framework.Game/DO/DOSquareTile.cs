using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MJS.Framework.Game.DO
{
    public class DOSquareTile : DOTile
    {
        protected override void Initialize()
        {
            _travelOptions = new DOTile[4];
        }

        public DOSquareTile North
        {
            get { return (DOSquareTile)GetTravelOption(0); }
            set { SetTravelOption(0, value); }
        }
        public DOSquareTile East
        {
            get { return (DOSquareTile)GetTravelOption(1); }
            set { SetTravelOption(1, value); }
        }
        public DOSquareTile South
        {
            get { return (DOSquareTile)GetTravelOption(2); }
            set { SetTravelOption(2, value); }
        }
        public DOSquareTile West
        {
            get { return (DOSquareTile)GetTravelOption(3); }
            set { SetTravelOption(3, value); }
        }

        
    }
}
