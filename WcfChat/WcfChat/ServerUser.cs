using System.ServiceModel;
using System.Runtime.Serialization;

namespace WcfChat
{
    // Класс DataContract для хранения и обмена информации с клиентом
    [DataContract]
    public class ServerUser
    {
        public int Id { get; set; }

        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public int Room { get; set; }

        [DataMember]
        public NameColor Color { get; set; }

        public OperationContext operationContext { get; set; }
    }

    // Перечисление цвета имени
    [DataContract(Name = "NameColor")]
    public enum NameColor
    {
        [EnumMember]
        Black,
        [EnumMember]
        Green,
        [EnumMember]
        Brue,
        [EnumMember]
        Red,
        [EnumMember]
        Yellow,
        [EnumMember]
        Gray,
        [EnumMember]
        Purple,
        [EnumMember]
        Pink,
        [EnumMember]
        Orange,
        [EnumMember]
        Brown
    }
}
