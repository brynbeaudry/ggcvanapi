using System.ComponentModel.DataAnnotations.Schema;

namespace ggcvan.Models {
/* FirstName = googleApiTokenInfo.given_name,
            LastName = googleApiTokenInfo.family_name,
            Locale = googleApiTokenInfo.locale,
            Name = googleApiTokenInfo.name,
            ProviderUserId = googleApiTokenInfo.sub */
[NotMapped]
public class ProviderUserDetails{

    public string Name { get; set; }
    public string FirstName { get; set; }        

    public string LastName { get; set; }

    public string Locale { get; set; }

    public string ProviderUserId { get; set; }

    public string Email { get; set; }
}

}