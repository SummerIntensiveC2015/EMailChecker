using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace netoaster
{
    public class Message
    {
        public string emailUser;
        public string emailSender;
        public string theme;
        public string textLetter;
        public string name;
        public Message(String _emailUser, String _emailSender, String _theme, String _textLetter, String _name)
        {
            emailUser = _emailUser;
            emailSender = _emailSender;
            theme = _theme;
            textLetter = _textLetter;
            name = _name;
        }
    }
}
