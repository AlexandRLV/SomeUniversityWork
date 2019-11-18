using System;
using System.ComponentModel.DataAnnotations;

namespace WcfChat.Models
{
    public class Message
    {
        [Key]
        public int Id { get; set; }
        public DateTime DateTime { get; set; }
        public string Text { get; set; }

        public override string ToString()
        {
            return $"From data ({DateTime.ToShortDateString()} {DateTime.ToShortTimeString()}): {Text}";
        }
    }
}