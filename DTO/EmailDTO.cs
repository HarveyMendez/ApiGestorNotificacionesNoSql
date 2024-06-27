namespace Proyecto2Api.DTO
{
    public class EmailDTO
    {
        public int Id { get; set; }//para enviarlo a la api de correos
        public string IdFirebase { get; set; }//asi lo pone Firebase
        public string Subject { get; set; }

        public string Body { get; set; }

        public string AddressTo { get; set; }
    }
}
