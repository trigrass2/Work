//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace CopyCAD.Model
{
    using System;
    using System.Collections.Generic;
    
    public partial class Stationszeiten
    {
        public int Stationsnummer { get; set; }
        public System.DateTime Zeit { get; set; }
        public short Zeittype { get; set; }
        public Nullable<int> PalettenID { get; set; }
        public Nullable<int> F_Palettennummer { get; set; }
    
        public virtual Palette Palette { get; set; }
    }
}
