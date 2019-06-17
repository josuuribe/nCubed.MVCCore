using System;
using System.Collections.Generic;
using System.Text;

namespace nCubed.MVCCore.HATEOAS
{
    public abstract class LinkedResourceBaseDto
    {
        public List<LinkDto> Links { get; set; }
        = new List<LinkDto>();
    }
}
