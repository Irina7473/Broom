using System;

namespace DataModel
{
    public class TypesOfEvent
    {
        public int Id { get; set; }
        public string TypeEvent { get; set; }

        public TypesOfEvent(int id, string typeevent)
        {
            Id = id;
            TypeEvent = typeevent;            
        }

        public TypesOfEvent() { }
    }
}
