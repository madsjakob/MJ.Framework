using MJS.Framework.View.Types;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FrameWorkTest
{
    public class VOSag : ViewObject<DDSag>
    {
        public VOSag(Guid id) : base(id)
        {
        }
    }
}
