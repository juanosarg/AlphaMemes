using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    
    public class RitualObligationTrigger_AlsoAddsDreadnoughtProps : RitualObligationTrigger_AlsoAddsProps
    {
        public RitualObligationTrigger_AlsoAddsDreadnoughtProps()
        {
            triggerClass = typeof(RitualObligationTrigger_AlsoAddsDreadnought);
        }



    }


}
