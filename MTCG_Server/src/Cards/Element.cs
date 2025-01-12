namespace MTCG_Wiktoria.Cards
{
    public enum Element
    {
        FIRE,
        WATER,
        NORMAL,
        EARTH,
        LIGHTNING
    }
}

//  	        FIRE	WATER	NORMAL	EARTH	LIGHTNING
// FIRE	        1×	    0.5×	2×	    2×	    0.5×
// WATER	    2×	    1×	    0.5×	0.5×	2×
// NORMAL	    0.5×	2×	    1×	    2×	    0.5×
// EARTH        0.5×	2×	    0.5×	1×	    2×
// LIGHTNING	2×	    0.5×	2×	    0.5×	1×

// FIRE is strong against NORMAL and EARTH but weak against WATER and LIGHTNING.
// WATER is strong against FIRE and LIGHTNING but weak against NORMAL and EARTH.
// EARTH is strong against LIGHTNING and NORMAL but weak against FIRE and WATER.
// LIGHTNING is strong against EARTH and FIRE but weak against WATER and NORMAL.