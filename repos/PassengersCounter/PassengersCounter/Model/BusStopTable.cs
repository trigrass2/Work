//------------------------------------------------------------------------------
// <auto-generated>
//     Этот код создан по шаблону.
//
//     Изменения, вносимые в этот файл вручную, могут привести к непредвиденной работе приложения.
//     Изменения, вносимые в этот файл вручную, будут перезаписаны при повторном создании кода.
// </auto-generated>
//------------------------------------------------------------------------------

namespace PassengersCounter.Model
{
    using System;
    using System.Collections.Generic;
    [Serializable]
    public partial class BusStopTable
    {
        public int Id { get; set; }
        public string nameBusStop { get; set; }
        public string coordinateBusStop { get; set; }
        public int countPassengers { get; set; }
    }
}
