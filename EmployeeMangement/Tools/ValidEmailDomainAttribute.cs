using System.ComponentModel.DataAnnotations;

namespace EmployeeMangement.Tools
{
    public class ValidEmailDomainAttribute : ValidationAttribute
    {
        private readonly string domain;

        public ValidEmailDomainAttribute(string domain)
        {
            this.domain = domain;
        }

        public override bool IsValid(object value)
        {
            string[] values = value.ToString().Split(new char[] { '@' });
            string[] domains = domain.Split(new char[] { ';' });

            foreach (string dom in domains)
            {
                if (values[1].ToLower() == dom.ToLower())
                {
                    return true;
                }
            }
            return false;
        }
    }
}
