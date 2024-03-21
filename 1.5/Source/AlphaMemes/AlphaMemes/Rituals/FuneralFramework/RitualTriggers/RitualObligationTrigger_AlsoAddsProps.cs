using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using RimWorld;
using Verse;
using System.Threading.Tasks;

namespace AlphaMemes
{    
    public class RitualObligationTrigger_AlsoAddsProps : RitualObligationTriggerProperties
    {
        public RitualObligationTrigger_AlsoAddsProps()
        {
            triggerClass = typeof(RitualObligationTrigger_AlsoAdds);

        }

        public RitualObligationTriggerProperties otherRitTriggerProps; //If you want to call an alternate trigger for the alsoAdd Ritual instead
        public RitualObligationTriggerProperties AltTriggerProps; //If you want to call an alternate trigger for the main Ritual instead
        public bool tryAddBoth = false;
        
    }


}
