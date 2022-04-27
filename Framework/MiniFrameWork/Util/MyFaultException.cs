using System;
using System.Text;
using System.Runtime.Serialization;


namespace MiniFrameWork.Util
{
    [DataContract]
    public class MyFaultException
    {


        public MyFaultException()
        {

            this.ErrorMsg = "Service encountered an error";

        }

        public MyFaultException(string message)
        {
            this.ErrorMsg = message;
            this.Description = message;

        }

        private int errorNumber;

        [DataMember(Order = 0)]

        public int ErrorNumber
        {

            get { return errorNumber; }

            set { errorNumber = value; }

        }

        private string errrorMsg;

        [DataMember(Order = 1)]

        public string ErrorMsg
        {

            get { return errrorMsg; }

            set { errrorMsg = value; }

        }

        private string description;

        [DataMember(Order = 2)]

        public string Description
        {

            get { return description; }

            set { description = value; }

        }
    }
}
