using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace GroceryApp.Common
{
    public class ResponseMessage
    {
        public MessageType Type { get; set; }
        public string Message { get; set; }

        public ResponseMessage(MessageType messageType, string message)
        {
            this.Type = messageType;
            this.Message = message;
        }

        public enum MessageType
        {
            Success = 1,
            Info = 2,
            Warn = 3,
            Error = 4
        }
    }
}
