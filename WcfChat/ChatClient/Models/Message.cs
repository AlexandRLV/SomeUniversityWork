using System;
using System.ComponentModel.DataAnnotations;

namespace ChatClient.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }
    }
}
