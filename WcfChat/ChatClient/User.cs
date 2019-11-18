using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.Serialization;

namespace ChatClient
{
    [DataContract]
    class User
    {
        [DataMember]
        public string Name { get; set; }

        [DataMember]
        public string Password { get; set; }
    }
}
