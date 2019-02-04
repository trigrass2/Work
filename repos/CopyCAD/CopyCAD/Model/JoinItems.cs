using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CopyCAD.Model
{
    public class JoinItems
    {
        public int ProdNr { get; set; }
        public string AuftragsNr { get; set; }
        public string Name { get; set; }
        public int? FK_PalettenId { get; set; }
    }
}
