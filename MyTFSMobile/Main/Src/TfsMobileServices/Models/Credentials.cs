namespace TfsMobileServices.Models
{
    public class Credentials
    {
        public string Username { get; set; }
        public string Password { get; set; }

        public string Domain { get; set; }

        public bool UseLocalDefault { get; set; }

        public Credentials()
        {
            UseLocalDefault = true;
        }

        public Credentials(string userName, string password)
        {
            
            Password = password;
            var tempString = userName.Replace('\\',':').Replace('/',':');
            var userNameAndDomain = tempString.Split(':');
            if (userNameAndDomain.Length > 0)
            {
                Username = userNameAndDomain[0]; 
                if (userNameAndDomain.Length > 1)
                {
                    Domain = userNameAndDomain[1]; 
                }
            }

        }
    }
}