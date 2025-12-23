using System.ComponentModel.DataAnnotations;

namespace Authentication.Domain.Options;

public class JwtOptions
{
	[Required]
	public required string Issuer { get; set; }

	[Required]
	public required string Audience { get; set; }

	[Required]
	public required string SecretKey { get; set; }
}
